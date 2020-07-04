using System;
using System.Numerics;

namespace SoundSimulator
{
    public class SteadySinSource : ISoundSource
    {
        public Vector3 Position { get; set; }
        private double frequency;
        private double startingPhase;
        public double EvaluateSouceAtTime(double evaluationTime)
        {
            return Math.Sin(this.frequency * evaluationTime + this.startingPhase);
        }

        public SteadySinSource(Vector3 pos, double frequency, double startingPhase = 0)
        {
            
            Position = pos;
            this.frequency = frequency;
            this.startingPhase = startingPhase;
        }
    }


}
