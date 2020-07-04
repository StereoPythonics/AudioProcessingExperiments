using System;
using System.Numerics;

namespace SoundSimulator
{
    public class AMModulatedSinSource : ISoundSource
    {
        public Vector3 position { get; }

        public Func<double, double> EvaluateSouceAtTime => throw new NotImplementedException();

        public AMModulatedSinSource(Vector3 pos)
        {

        }
    }


}
