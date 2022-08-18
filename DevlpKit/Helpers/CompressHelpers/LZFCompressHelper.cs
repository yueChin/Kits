/*
 * Copyright (c) 2005 Oren J. Maurice <oymaurice@hazorea.org.il>
 * 
 * Redistribution and use in source and binary forms, with or without modifica-
 * tion, are permitted provided that the following conditions are met:
 * 
 *   1.  Redistributions of source code must retain the above copyright notice,
 *       this list of conditions and the following disclaimer.
 * 
 *   2.  Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 * 
 *   3.  The name of the author may not be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MER-
 * CHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO
 * EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPE-
 * CIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTH-
 * ERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * Alternatively, the contents of this file may be used under the terms of
 * the GNU General Public License version 2 (the "GPL"), in which case the
 * provisions of the GPL are applicable instead of the above. If you wish to
 * allow the use of your version of this file only under the terms of the
 * GPL and not to allow others to use your version of this file under the
 * BSD license, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the GPL. If
 * you do not delete the provisions above, a recipient may use your version
 * of this file under either the BSD or the GPL.
 */

using System;

namespace Kits.DevlpKit.Helpers.CompressHelpers 
{
    /// <summary>
    /// Summary description for CLZF.
    /// </summary>
    public class LzfCompressHelper 
    {
        /// <summary>
        /// LZF Compressor
        /// </summary>
        UInt32 m_Hlog = 14;
        UInt32 m_Hsize = (1 << 14);
        /*
		* don't play with this unless you benchmark!
		* decompression is not dependent on the hash function
		* the hashing function might seem strange, just believe me
		* it works ;)
		*/
        UInt32 m_MAXLit = (1 << 5);
        UInt32 m_MAXOff = (1 << 13);
        UInt32 m_MAXRef = ((1 << 8) + (1 << 3));
        UInt32 Frst(byte[] array, UInt32 ptr)
        {
            return (UInt32)(((array[ptr]) << 8) | array[ptr + 1]);
        }

        UInt32 Next(UInt32 v, byte[] array, UInt32 ptr)
        {
            return ((v) << 8) | array[ptr + 2];
        }

        UInt32 Idx(UInt32 h)
        {
            return ((((h ^ (h << 5)) >> (int)(3 * 8 - m_Hlog)) - h * 5) & (m_Hsize - 1));
        }

        /*
		* compressed format
		*
		* 000LLLLL <L+1>    ; literal
		* LLLOOOOO oooooooo ; backref L
		* 111OOOOO LLLLLLLL oooooooo ; backref L+7
		*
		*/
        public int lzf_compress(byte[] inData, int inLen, byte[] outData, int outLen)
        {
            int c;
            long[] htab = new long[1 << 14];
            for (c = 0; c < 1 << 14; c++)
            {
                htab[c] = 0;
            }

            long hslot;
            UInt32 iidx = 0;
            UInt32 oidx = 0;
            //byte *in_end  = ip + in_len;
            //byte *out_end = op + out_len;
            long reference;
            UInt32 hval = Frst(inData, iidx);
            long off;
            int lit = 0;
            for (;;)
            {
                if (iidx < inLen - 2)
                {
                    hval = Next(hval, inData, iidx);
                    hslot = Idx(hval);
                    reference = htab[hslot];
                    htab[hslot] = (long)iidx;
                    if ((off = iidx - reference - 1) < m_MAXOff && iidx + 4 < inLen && reference > 0 && inData[reference + 0] == inData[iidx + 0] && inData[reference + 1] == inData[iidx + 1] && inData[reference + 2] == inData[iidx + 2])
                    {
                        /* match found at *reference++ */
                        UInt32 len = 2;
                        UInt32 maxlen = (UInt32)inLen - iidx - len;
                        maxlen = maxlen > m_MAXRef ? m_MAXRef : maxlen;
                        if (oidx + lit + 1 + 3 >= outLen)
                            return 0;
                        do len++;
                        while (len < maxlen && inData[reference + len] == inData[iidx + len]);
                        if (lit != 0)
                        {
                            outData[oidx++] = (byte)(lit - 1);
                            lit = -lit;
                            do outData[oidx++] = inData[iidx + lit];
                            while ((++lit) != 0);
                        }

                        len -= 2;
                        iidx++;
                        if (len < 7)
                        {
                            outData[oidx++] = (byte)((off >> 8) + (len << 5));
                        }
                        else
                        {
                            outData[oidx++] = (byte)((off >> 8) + (7 << 5));
                            outData[oidx++] = (byte)(len - 7);
                        }

                        outData[oidx++] = (byte)off;
                        iidx += len - 1;
                        hval = Frst(inData, iidx);
                        hval = Next(hval, inData, iidx);
                        htab[Idx(hval)] = iidx;
                        iidx++;
                        hval = Next(hval, inData, iidx);
                        htab[Idx(hval)] = iidx;
                        iidx++;
                        continue;
                    }
                }
                else if (iidx == inLen)
                    break;
                /* one more literal byte we must copy */
                lit++;
                iidx++;
                if (lit == m_MAXLit)
                {
                    if (oidx + 1 + m_MAXLit >= outLen)
                        return 0;
                    outData[oidx++] = (byte)(m_MAXLit - 1);
                    lit = -lit;
                    do outData[oidx++] = inData[iidx + lit];
                    while ((++lit) != 0);
                }
            }

            if (lit != 0)
            {
                if (oidx + lit + 1 >= outLen)
                    return 0;
                outData[oidx++] = (byte)(lit - 1);
                lit = -lit;
                do outData[oidx++] = inData[iidx + lit];
                while ((++lit) != 0);
            }

            return (int)oidx;
        }

        /// <summary>
        /// LZF Decompressor
        /// </summary>
        public int lzf_decompress(byte[] inData, int offset, int inLen, byte[] outData, int outLen)
        {
            UInt32 iidx = (UInt32)offset;
            UInt32 oidx = 0;
            UInt32 eidx = (UInt32)(offset + inLen);
            do
            {
                UInt32 ctrl = inData[iidx++];
                if (ctrl < (1 << 5)) /* literal run */
                {
                    ctrl++;
                    if (oidx + ctrl > outLen)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    do outData[oidx++] = inData[iidx++];
                    while ((--ctrl) != 0);
                }
                else /* back reference */
                {
                    UInt32 len = ctrl >> 5;
                    int reference = (int)(oidx - ((ctrl & 0x1f) << 8) - 1);
                    if (len == 7)
                        len += inData[iidx++];
                    reference -= inData[iidx++];
                    if (oidx + len + 2 > outLen)
                    {
                        //SET_ERRNO (E2BIG);
                        return 0;
                    }

                    if (reference < 0)
                    {
                        //SET_ERRNO (EINVAL);
                        return 0;
                    }

                    outData[oidx++] = outData[reference++];
                    outData[oidx++] = outData[reference++];
                    do outData[oidx++] = outData[reference++];
                    while ((--len) != 0);
                }
            }
            while (iidx < eidx);
            return (int)oidx;
        }

        public LzfCompressHelper()
        {
        //
        // TODO: Add ructor logic here
        //
        }
    }
}