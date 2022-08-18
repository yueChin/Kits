using System;
using System.Collections.Generic;
using System.IO;
using Kits.ClientKit.Handlers.Audio.Codec;
using Kits.DevlpKit.Helpers.CompressHelpers;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Audio
{
    public static class AudioClipHandler
    {
        /// <summary>
        /// 将Unity的AudioClip数据转化为PCM格式16bit数据
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static byte[] AudioClipToPCM16(AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            short[] samples_int16 = new short[samples.Length];

            for (int index = 0; index < samples.Length; index++)
            {
                float f = samples[index];
                samples_int16[index] = (short)(f * short.MaxValue);
            }

            byte[] byteArray = new byte[samples_int16.Length * 2];
            Buffer.BlockCopy(samples_int16, 0, byteArray, 0, byteArray.Length);

            return byteArray;
        }

        public static float[] GetData(this AudioClip clip, int offset = 0)
        {
            float[] data = new float[clip.samples * clip.channels];

            clip.GetData(data, offset);
            return data;
        }

        public static byte[] GetData(this AudioClip clip)
        {
            float[] data = new float[clip.samples * clip.channels];

            clip.GetData(data, 0);

            byte[] bytes = new byte[data.Length * 4];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static void SetData(this AudioClip clip, byte[] bytes)
        {
            float[] data = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);

            clip.SetData(data, 0);
        }

        public static byte[] GetData16(this AudioClip clip)
        {
            float[] data = new float[clip.samples * clip.channels];

            clip.GetData(data, 0);

            byte[] bytes = new byte[data.Length * 2];

            int rescaleFactor = 32767;

            for (int i = 0; i < data.Length; i++)
            {
                short value = (short) (data[i] * rescaleFactor);
                BitConverter.GetBytes(value).CopyTo(bytes, i * 2);
            }

            return bytes;
        }

        public static ICodec Codec = new ADPCMCodec();
        private static readonly List<byte> s_DataList = new List<byte>();
        private static readonly List<short> s_TmpList = new List<short>();

        public static byte[] CompressAudioClip(this AudioClip clip, out int samples, BandMode mode, float gain = 1.0f)
        {
            s_DataList.Clear();
            samples = 0;

            short[] b = AudioClipToShorts(clip, gain);

            int num = 0;
            for (int i = 0; i < b.Length; i++)
            {
                //identify "quiet" samples, set them to exactly 1 so that a row of these "near-zero" samples becomes a row of exactly-one samples and achieves better Deflate compression
                if (b[i] <= 5 && b[i] >= -5 && b[i] != 0)
                {
                    b[i] = 1;
                    num++;
                }
            }

            byte[] mlaw = Codec.Encode(b);

            s_DataList.AddRange(mlaw);

            return ZIPCompressHelper.Zip(s_DataList.ToArray());
        }

        public static AudioClip DecompressAudioClip(byte[] data, int samples, int channels, BandMode mode, float gain)
        {
            int frequency = 4000;
            if (mode == BandMode.Narrow)
            {
                frequency = 8000;
            }
            else if (mode == BandMode.Wide)
            {
                frequency = 16000;
            }
#if UNITY_EDITOR
            Debug.Log(data.Length + "解压前");
#endif
            byte[] d;
            d = ZIPCompressHelper.Unzip(data);

            short[] pcm = Codec.Decode(d);
#if UNITY_EDITOR
            Debug.Log(pcm.Length + "解压后");
#endif

            s_TmpList.Clear();
            s_TmpList.AddRange(pcm);

            //while( tmp.Count > 1 && Mathf.Abs( tmp[ tmp.Count - 1 ] ) <= 10 )
            //{
            //    tmp.RemoveAt( tmp.Count - 1 );
            //}

            //while( tmp.Count > 1 && Mathf.Abs( tmp[ 0 ] ) <= 10 )
            //{
            //    tmp.RemoveAt( 0 );
            //}

            return ShortsToAudioClip(s_TmpList.ToArray(), channels, frequency, gain);
        }

        public static byte[] AudioClipToBytes(this AudioClip clip)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);

            byte[] data = new byte[clip.samples * clip.channels];
            for (int i = 0; i < samples.Length; i++)
            {
                //convert to the -128 to +128 range
                float conv = samples[i] * 128.0f;
                int c = Mathf.RoundToInt(conv);
                c += 127;
                if (c < 0)
                    c = 0;
                if (c > 255)
                    c = 255;

                data[i] = (byte) c;
            }

            return data;
        }

        public static short[] AudioClipToShorts(this AudioClip clip, float gain = 1.0f)
        {
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
#if UNITY_EDITOR
            Debug.Log(samples.Length + "压缩前");
#endif
            short[] data = new short[clip.samples * clip.channels];
            for (int i = 0; i < samples.Length; i++)
            {
                //convert to the -3267 to +3267 range
                float g = samples[i] * gain;
                if (Mathf.Abs(g) > 1.0f)
                {
                    if (g > 0)
                        g = 1.0f;
                    else
                        g = -1.0f;
                }

                float conv = g * 3267.0f;
                //int c = Mathf.RoundToInt( conv );

                data[i] = (short) conv;
            }

            return data;
        }

        public static AudioClip BytesToAudioClip(byte[] data, int channels, int frequency, float gain)
        {
            float[] samples = new float[data.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                //convert to integer in -128 to +128 range
                int c = (int) data[i];
                c -= 127;
                samples[i] = ((float) c / 128.0f) * gain;
            }

            AudioClip clip = AudioClip.Create("clip", data.Length / channels, channels, frequency, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip ShortsToAudioClip(short[] data, int channels, int frequency, float gain)
        {
            float[] samples = new float[data.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                //convert to float in the -1 to 1 range
                int c = (int) data[i];
                samples[i] = ((float) c / 3267.0f) * gain;
            }

            AudioClip clip = AudioClip.Create("clip", data.Length / channels, channels, frequency, false);
            clip.SetData(samples, 0);
            return clip;
        }

        /// <summary>
        /// clip转wav
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static byte[] EncodeToWAV(this AudioClip clip)
        {
            //byte[] bytes = clip.GetData();
            //WAV wav = new WAV(bytes);
            //return wav.ToBytes();
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(new byte[44], 0, 44); //预留44字节头部信息

                byte[] bytesData = clip.GetData16();

                memoryStream.Write(bytesData, 0, bytesData.Length);

                memoryStream.Seek(0, SeekOrigin.Begin);

                byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
                memoryStream.Write(riff, 0, 4);

                byte[] chunkSize = BitConverter.GetBytes(memoryStream.Length - 8);
                memoryStream.Write(chunkSize, 0, 4);

                byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
                memoryStream.Write(wave, 0, 4);

                byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
                memoryStream.Write(fmt, 0, 4);

                byte[] subChunk1 = BitConverter.GetBytes(16);
                memoryStream.Write(subChunk1, 0, 4);

                // UInt16 two = 2;
                UInt16 one = 1;

                byte[] audioFormat = BitConverter.GetBytes(one);
                memoryStream.Write(audioFormat, 0, 2);

                byte[] numChannels = BitConverter.GetBytes(clip.channels);
                memoryStream.Write(numChannels, 0, 2);

                byte[] sampleRate = BitConverter.GetBytes(clip.frequency);
                memoryStream.Write(sampleRate, 0, 4);

                byte[] byteRate =
                    BitConverter.GetBytes(clip.frequency * clip.channels *
                                          2); // sampleRate * bytesPerSample*number of channels
                memoryStream.Write(byteRate, 0, 4);

                UInt16 blockAlign = (ushort) (clip.channels * 2);
                memoryStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

                UInt16 bps = 16;
                byte[] bitsPerSample = BitConverter.GetBytes(bps);
                memoryStream.Write(bitsPerSample, 0, 2);

                byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
                memoryStream.Write(datastring, 0, 4);

                byte[] subChunk2 = BitConverter.GetBytes(clip.samples * clip.channels * 2);
                memoryStream.Write(subChunk2, 0, 4);

                bytes = memoryStream.ToArray();
            }

            return bytes;
        }
    }
   
}

