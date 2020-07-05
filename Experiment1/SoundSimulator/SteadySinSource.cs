using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace SoundSimulator
{
    public static class SoundSourceUtils
    {
        public static double[] ScaleSoundSource(this IEnumerable<double> source)
        {
            double scaler = 0.999 / source.Max(r => Math.Abs(r));
            double[] scaledResult = source.Select(r => r * scaler).ToArray();
            return scaledResult;
        }
    }
    public class SteadySinSource : ISoundSource
    {
        public Vector3 Position { get; set; }
        private double frequency;
        private double startingPhase;
        public double EvaluateSouceAtTime(double evaluationTime)
        {
            return Math.Sin((2*Math.PI*this.frequency * evaluationTime) + this.startingPhase);
        }

        public SteadySinSource(Vector3 pos, double frequency, double startingPhase = 0)
        {
            
            Position = pos;
            this.frequency = frequency;
            this.startingPhase = startingPhase;
        }
    }
    //public class WavSource : ISoundSource
    //{
    //    public Vector3 Position { get; set; }
    //    double[] Source;
    //    public double EvaluateSouceAtTime(double time)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    WaveFormat format;
    //    public WavSource(Vector3 pos, string filepath)
    //    {
    //        using WaveFileReader reader = new WaveFileReader(filepath);
    //        format = reader.WaveFormat;
    //        using MemoryStream ms = new MemoryStream();
    //        reader.CopyTo(ms);
    //        Source = ms.ToArray().;
    //        Position = pos;
    //    }
    //}


}
