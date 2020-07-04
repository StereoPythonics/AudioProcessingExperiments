using System;
using System.Numerics;

namespace SoundSimulator
{
    public class AMModulatedSinSource : ISoundSource
    {
        public Vector3 position { get; }

        public Vector3 Position => throw new NotImplementedException();

        public AMModulatedSinSource(Vector3 pos)
        {

        }

        public double EvaluateSouceAtTime(double time)
        {
            throw new NotImplementedException();
        }
    }


}
