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
                    //new SteadySinSource(new Vector3(0,1,0),440),
                }
            };
            var test = Constants.SOS;
            Simulation sim = new Simulation(sourceCollection, microphoneConfiguration);
            var result = sim.GetIntensityResult(0, 10, 0.0001);
            Assert.True(result.All(i => i < 0.0001));
        }
    }
}
