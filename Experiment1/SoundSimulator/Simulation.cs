using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SoundSimulator
{
    public class Simulation
    {
        MicrophoneConfiguration MicrophoneConfiguration;
        SourceCollection SourceCollection;

        public List<double> GetIntensityResult(double startTime, double endTime, double samplingStep, Vector3? listeningTarget = null)
        {
            List<double> returnable = new List<double>();
            int count = (int)Math.Floor((endTime - startTime) / samplingStep);
            for (int i = 0; i < count; i++)
            {
                double currentTime = 0;
                IEnumerable<double> test = MicrophoneConfiguration.PhaseCorrectionsForPoint(listeningTarget)
                    .SelectMany(pc =>
                        SourceCollection.SoundSources
                        .Select(ss => ss.EvaluateSouceAtTime(currentTime - pc.DelayCorrection))
                    );
                returnable.Add(test.Average());
            }
            return returnable;
        }
        public Simulation(SourceCollection sourceCollection, MicrophoneConfiguration microphoneConfiguration)
        {

            SourceCollection = sourceCollection;
            
            
            
            MicrophoneConfiguration = microphoneConfiguration; 
        }
    }

    public static class Constants
    {
        public const double SOS = 343;
    }


}
