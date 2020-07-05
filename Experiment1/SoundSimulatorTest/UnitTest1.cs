using NAudio.Wave;
using SoundSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace SoundSimulatorTest
{
    public class UnitTest1
    {
        public static void ShowData(IEnumerable<double> result, string identifier="")
        {
            var subsample = result.Take(1000);
            var plt = new ScottPlot.Plot(800, 800);
            plt.PlotScatter(Enumerable.Range(0, subsample.Count()).Select(i => (double)i).ToArray(), subsample.ToArray());
            plt.SaveFig(@$"C:\Users\nigel\Pictures\{DateTime.Now.ToFileTimeUtc()}{identifier}.PNG");
        }

        public static void ShowFFTData(IEnumerable<double> result, int samplerate)
        {
            double[] resultArray = result.Take(512*2).ToArray();
            // Window your signal
            double[] window = FftSharp.Window.Hanning(resultArray.Length);
            
            FftSharp.Window.ApplyInPlace(window, resultArray);
            // For audio we typically want the FFT amplitude (in dB)
            double[] fftPower = FftSharp.Transform.FFTpower(resultArray);

            // Create an array of frequencies for each point of the FFT
            double[] freqs = FftSharp.Transform.FFTfreq(samplerate, fftPower.Length);
            var plt = new ScottPlot.Plot(800, 800);
            plt.PlotScatter(freqs, fftPower);
            plt.SaveFig(@$"C:\Users\nigel\Pictures\{DateTime.Now.ToFileTimeUtc()}fft.PNG");
        }
        public static void WriteWav(IEnumerable<double> result, int rate = 44000, int bitDepth = 16, string identifier="")
        {
            byte[] buffer = result.SelectMany(r => BitConverter.GetBytes(Convert.ToInt16(Math.Max(Math.Min(1, r), -1) * 0.5 * 32768))).ToArray();

            using (WaveFileWriter writer = new WaveFileWriter(@$"C:\Users\Rob\Pictures\{DateTime.Now.ToFileTimeUtc()}{identifier}.wav", new WaveFormat(rate, bitDepth, 1)))
            {
                //int bytesRead;
                //while ((bytesRead = wavReader.Read(buffer, 0, buffer.Length)) > 0)
                //{
                writer.Write(buffer, 0, buffer.Length/*bytesRead*/);
                //}
            }
        }
        [Fact]
        public void TestPhaseCancel()
        {
            MicrophoneConfiguration microphoneConfiguration = new MicrophoneConfiguration()
            {
                Microphones = new List<IMicrophone>()
                {
                    new Microphone(){ Position = new Vector3(0,0,0)},
                    //new Microphone(){ Position = new Vector3(-1,0,0)}
                }
            };

            SourceCollection sourceCollection = new SourceCollection()
            {
                SoundSources = new List<ISoundSource>()
                {
                    new SteadySinSource(new Vector3(1,0,0),440),
                    new SteadySinSource(new Vector3(0,1+(float)(Constants.SOS/880),0),440),
                }
            };
            var test = Constants.SOS;
            Simulation sim = new Simulation(sourceCollection, microphoneConfiguration);
            var result = sim.GetIntensityResult(0, 0.1, 0.0001);
            Assert.True(result.All(i => i < 0.0001));
        }

        [Fact]
        public void TestSoundSelection()
        {
            Random randoCalrissian = new Random(123456789);
            float magnitude = 10;
            MicrophoneConfiguration microphoneConfiguration = new MicrophoneConfiguration()
            {
                Microphones = new List<IMicrophone>(Enumerable.Range(0, 1)
                .Select(i =>
                new Microphone()
                {
                    Position = new Vector3() { X = magnitude*(float)randoCalrissian.NextDouble(), Y = magnitude*(float)randoCalrissian.NextDouble(), Z = magnitude*(float)randoCalrissian.NextDouble() }
                }))
                //    new Microphone(){ Position = new Vector3(0,0,0)},
                //    new Microphone(){ Position = new Vector3(1,0,0)},
                //    new Microphone(){ Position = new Vector3(0,0,1)},
                //    new Microphone(){ Position = new Vector3(0,1,0)},
                //    new Microphone(){ Position = new Vector3(-1,0,0)}
                //}
            };

            SourceCollection sourceCollection = new SourceCollection()
            {
                SoundSources = new List<ISoundSource>()
                {
                    //new SteadySinSource(new Vector3(1,0,-3),190),
                    new WhiteNoiseSource(new Vector3(0,2,1)),
                    new WhiteNoiseSource(new Vector3(0,-2,1)),
                    //new SteadySinSource(new Vector3(0,1,15),720),
                    new SteadySinSource(new Vector3(1,1,0),1000),
                    //new SteadySinSource(new Vector3(2,1,1),1100),
                }
            };
            var test = Constants.SOS;
            int SampleRate = 44000;
            Simulation sim = new Simulation(sourceCollection, microphoneConfiguration);
            var result = sim.GetIntensityResult(0, 1, 1.0/SampleRate,new Vector3(1,1,0));
            double scaler = 0.999 / result.Max(r => Math.Abs(r));
            double[] scaledResult = result.Select(r => r * scaler).ToArray();



            //Assert.True(result.All(i => i < 0.0001));
            string identifier = "Select1kHz_1mic_1source";
            ShowFFTData(scaledResult, SampleRate);
            ShowData(scaledResult, identifier: identifier);
            WriteWav(scaledResult, identifier: identifier);

        }

        [Fact]
        public void TestWavSource()
        {
            System.Diagnostics.Debug.WriteLine(Environment.CurrentDirectory);
            Random randoCalrissian = new Random(123456789);
            float magnitude = 10;
            MicrophoneConfiguration microphoneConfiguration = new MicrophoneConfiguration()
            {
                Microphones = new List<IMicrophone>(Enumerable.Range(0, 45)
                .Select(i =>
                new Microphone()
                {
                    Position = new Vector3() { X = magnitude * (float)randoCalrissian.NextDouble(), Y = magnitude * (float)randoCalrissian.NextDouble(), Z = magnitude * (float)randoCalrissian.NextDouble() }
                }))
                //    new Microphone(){ Position = new Vector3(0,0,0)},
                //    new Microphone(){ Position = new Vector3(1,0,0)},
                //    new Microphone(){ Position = new Vector3(0,0,1)},
                //    new Microphone(){ Position = new Vector3(0,1,0)},
                //    new Microphone(){ Position = new Vector3(-1,0,0)}
                //}
            };

            SourceCollection sourceCollection = new SourceCollection()
            {
                SoundSources = new List<ISoundSource>()
                {
                    new SteadySinSource(new Vector3(0,1,1),1000),
                    new SteadySinSource(new Vector3(0,1,1),750),
                    new SteadySinSource(new Vector3(1,1,0),500),
                    new WavSource(new Vector3(1,1,1), $"source1.wav")
                }
            };
            //var test = Constants.SOS;
            Simulation sim = new Simulation(sourceCollection, microphoneConfiguration);
            var result = sim.GetIntensityResult(0, 5, 1.0 / 44000, new Vector3(1, 1, 1));
            //Assert.True(result.All(i => i < 0.0001));
            string identifier = "WavTest_45mic_1source";
            ShowData(result, identifier: identifier);
            WriteWav(result, identifier: identifier);

        }



    }
}
