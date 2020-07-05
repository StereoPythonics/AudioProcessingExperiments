using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using NAudio.Wave;
using Xunit;

namespace SoundSimulator
{
    public class WavSource : ISoundSource
    {
        public Vector3 Position { get; set; }
        private List<double> wavSamples = new List<double>();
        private int rate;

        public double EvaluateSouceAtTime(double evaluationTime)
        {
            var sampleN = (int)Math.Round(evaluationTime * (double)rate);
            if(sampleN < 0 | sampleN > (wavSamples.Count - 1))
            {
                return 0.0;
            }
            return wavSamples[sampleN];
        }

        public WavSource(Vector3 pos, string wavPath)
        {

            Position = pos;
            using (WaveFileReader reader = new WaveFileReader(wavPath))
            {
                Assert.Equal(16, reader.WaveFormat.BitsPerSample);
                
                rate = reader.WaveFormat.SampleRate;

                byte[] bytesBuffer = new byte[reader.Length];
                int read = reader.Read(bytesBuffer, 0, (int)reader.Length);

                for (int sampleIndex = 0; sampleIndex < read / 2; sampleIndex++)
                {
                    var intSampleValue = BitConverter.ToInt16(bytesBuffer, sampleIndex * 2);
                    wavSamples.Add(intSampleValue / 32768.0);
                }
                wavSamples = wavSamples.ScaleSoundSource().ToList();
            }
        }
    }


}
