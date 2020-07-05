using System;
using System.Numerics;

namespace SoundSimulator
{
    public class WhiteNoiseSource : ISoundSource
    {
        public Vector3 Position { get; }
        private Random rando;
        public double EvaluateSouceAtTime(double time)
        {
            return (2 * rando.NextDouble()) - 1;
        }

        public WhiteNoiseSource(Vector3 position)
        {
            rando = new Random();
            Position = position;
        }
    }


}
