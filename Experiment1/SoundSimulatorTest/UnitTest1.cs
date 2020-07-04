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
        public static void ShowData(IEnumerable<double> result)
        {
            var plt = new ScottPlot.Plot(800, 800);
            plt.PlotScatter(Enumerable.Range(0, result.Count()).Select(i => (double)i).ToArray(), result.ToArray());
            plt.SaveFig(@$"C:\Users\nigel\Pictures\{DateTime.Now.ToFileTimeUtc()}.PNG");
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
                Microphones = new List<IMicrophone>(Enumerable.Range(0, 16)
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
                    new SteadySinSource(new Vector3(1,0,0),250),
                    new SteadySinSource(new Vector3(0,1,0),1500),
                    new SteadySinSource(new Vector3(0,1,1),750),
                    new SteadySinSource(new Vector3(1,1,0),500),
                    new SteadySinSource(new Vector3(1,1,1),1000),
                }
            };
            var test = Constants.SOS;
            Simulation sim = new Simulation(sourceCollection, microphoneConfiguration);
            var result = sim.GetIntensityResult(0, 5, 1.0/44000,new Vector3(1,1,1));
            //Assert.True(result.All(i => i < 0.0001));
            ShowData(result);

            byte[] buffer = result.SelectMany(r => BitConverter.GetBytes(Convert.ToInt16(Math.Max(Math.Min(1,r),-1)*0.5*32768))).ToArray();

            using (WaveFileWriter writer = new WaveFileWriter(@$"C:\Users\nigel\Pictures\{DateTime.Now.ToFileTimeUtc()}.wav", new WaveFormat(44000, 16, 1)))
            {
                //int bytesRead;
                //while ((bytesRead = wavReader.Read(buffer, 0, buffer.Length)) > 0)
                //{
                writer.Write(buffer, 0, buffer.Length/*bytesRead*/);
                //}
            }
        }
    }
}
