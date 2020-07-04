using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SoundSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!"); //hello rob
            // hey barney
            Console.ReadKey();
            
        }
    }

    public class MicrophoneConfiguration
    { 
        public List<IMicrophone> Microphones { get; set; }
        public MicrophoneConfiguration(List<IMicrophone> microphones)
        {
            Microphones = microphones;
        }

        public MicrophoneConfiguration()
        {
        }

        public List<MicrophoneDelayCorrection> PhaseCorrectionsForPoint(Vector3? target)
        {
            if (target.HasValue) return Microphones.Select(m => new MicrophoneDelayCorrection() { Microphone = m, DelayCorrection = (m.Position - target.Value).Length() / Constants.SOS }).ToList();
            else
            {
                return Microphones.Select(m => new MicrophoneDelayCorrection()
                {
                    Microphone = m,
                    DelayCorrection = 0
                }).ToList();
            }
        }
    }

    public class MicrophoneDelayCorrection
    {
        public IMicrophone Microphone { get; set; }
        public double DelayCorrection { get; set; }
    }
    
    public class SourceCollection
    { 
        public List<ISoundSource> SoundSources { get; set; }
    }


}
