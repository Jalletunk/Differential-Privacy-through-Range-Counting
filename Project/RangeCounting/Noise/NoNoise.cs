using System;
using System.Collections.Generic;
using System.Linq;
using RangeCounting.Utils;
using MathNet.Numerics.Distributions;

namespace RangeCounting.Noise;

public class NoNoise : INoise
{
    public NoNoise()
    {
      this.epsilon = -1.0;
      this.DistributionName = "";
    }
    public double drawProbability(){
      return 0;
    }
    public double epsilon{get; set;}
    public int? seed {get; set;}
    public string DistributionName {get; set;}

}