using System;
using System.Numerics;

namespace SoundSimulator
{
    public interface ISoundSource
    { 
        Vector3 Position { get; }
        double EvaluateSouceAtTime(double time);
    }
}
