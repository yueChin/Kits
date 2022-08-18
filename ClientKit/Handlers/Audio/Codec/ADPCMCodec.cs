
namespace Kits.ClientKit.Handlers.Audio.Codec
{
    internal class ADPCMCodec : ICodec
    {
        private static int[] s_IndexTable =
        {
            -1, -1, -1, -1, 2, 4, 6, 8,
            -1, -1, -1, -1, 2, 4, 6, 8
        };

        private static int[] s_StepsizeTable =
        {
            7, 8, 9, 10, 11, 12, 14,
            16, 17, 19, 21, 23, 25, 28,
            31, 34, 37, 41, 45, 50, 55,
            60, 66, 73, 80, 88, 97, 107,
            118, 130, 143, 157, 173, 190, 209,
            230, 253, 279, 307, 337, 371, 408,
            449, 494, 544, 598, 658, 724, 796,
            876, 963, 1060, 1166, 1282, 1411, 1522,
            1707, 1876, 2066, 2272, 2499, 2749, 3024, 3327, 3660, 4026,
            4428, 4871, 5358, 5894, 6484, 7132, 7845, 8630,
            9493, 10442, 11487, 12635, 13899, 15289, 16818,
            18500, 203500, 22385, 24623, 27086, 29794, 32767
        };

        private int m_PredictedSample = 0;
        private int m_StepSize = 7;
        private int m_Index = 0;
        private int m_NewSample = 0;

        //reset initial values prior to encoding/decoding
        private void Init()
        {
            m_PredictedSample = 0;
            m_StepSize = 7;
            m_Index = 0;
            m_NewSample = 0;
        }

        private short ADPCM_Decode(byte originalSample)
        {
            int diff = 0;
            diff = (m_StepSize * originalSample / 4) + (m_StepSize / 8);
            if ((originalSample & 4) == 4)
                diff += m_StepSize;
            if ((originalSample & 2) == 2)
                diff += m_StepSize >> 1;
            if ((originalSample & 1) == 1)
                diff += m_StepSize >> 2;
            diff += m_StepSize >> 3;

            if ((originalSample & 8) == 8)
                diff = -diff;

            m_NewSample = diff;

            if (m_NewSample > short.MaxValue)
                m_NewSample = short.MaxValue;
            else if (m_NewSample < short.MinValue)
                m_NewSample = short.MinValue;

            m_Index += s_IndexTable[originalSample];
            if (m_Index < 0)
                m_Index = 0;
            if (m_Index > 88)
                m_Index = 88;

            m_StepSize = s_StepsizeTable[m_Index];

            return (short)m_NewSample;
        }

        private byte ADPCM_Encode(short originalSample)
        {
            int diff = (originalSample - m_PredictedSample);
            if (diff >= 0)
                m_NewSample = 0;
            else
            {
                m_NewSample = 8;
                diff = -diff;
            }

            byte mask = 4;
            int tempStepSize = m_StepSize;
            for (int i = 0; i < 3; i++)
            {
                if (diff >= tempStepSize)
                {
                    m_NewSample |= mask;
                    diff -= tempStepSize;
                }

                tempStepSize >>= 1;
                mask >>= 1;
            }

            //diff = 0;
            diff = m_StepSize >> 3;
            if ((m_NewSample & 4) != 0)
                diff += m_StepSize;
            if ((m_NewSample & 2) != 0)
                diff += m_StepSize >> 1;
            if ((m_NewSample & 1) != 0)
                diff += m_StepSize >> 2;

            if ((m_NewSample & 8) != 0)
                diff = -diff;

            m_PredictedSample += diff;

            if (m_PredictedSample > short.MaxValue)
                m_PredictedSample = short.MaxValue;
            if (m_PredictedSample < short.MinValue)
                m_PredictedSample = short.MinValue;

            m_Index += s_IndexTable[m_NewSample];
            if (m_Index < 0)
                m_Index = 0;
            else if (m_Index > 88)
                m_Index = 88;

            m_StepSize = s_StepsizeTable[m_Index];

            return (byte)(m_NewSample);
        }

        #region ICodec Members

        public byte[] Encode(short[] data)
        {
            Init();
            int len = data.Length / 2;
            if (len % 2 != 0)
                len++;
            byte[] temp = new byte[len];
            for (int i = 0; i < temp.Length; i++)
            {
                if ((i * 2) >= data.Length)
                    break;

                byte a = ADPCM_Encode(data[i * 2]);
                byte b = 0;
                if (((i * 2) + 1) < data.Length)
                {
                    b = ADPCM_Encode(data[(i * 2) + 1]);
                }

                byte c = (byte)((b << 4) | a);

                temp[i] = c;
            }

            return temp;
        }

        public short[] Decode(byte[] data)
        {
            Init();
            short[] temp = new short[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                byte c = data[i];
                byte d = (byte)(c & 0x0f);
                byte e = (byte)(c >> 4);
                temp[i * 2] = ADPCM_Decode(d);
                temp[(i * 2) + 1] = ADPCM_Decode(e);
            }

            return temp;
        }

        #endregion
    }

}
