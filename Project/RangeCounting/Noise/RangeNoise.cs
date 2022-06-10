using System;
using System.Collections.Generic;
using System.Linq;
using RangeCounting.Utils;
using MathNet.Numerics.Distributions;

namespace RangeCounting.Noise;

public class RangeNoise : INoise
{
    public RangeNoise(double epsilon, int rho, string distributionName)
    {
      this.epsilon = epsilon;
      this.Rho = rho;
      this.DistributionName = distributionName;
      this.seed = null;
      switch (DistributionName)
      { 
        case "Laplace":
          var b = this.Rho/epsilon;
          this.Distribution = new Laplace(0,b);
          break;
        case "Gaussian":
          double sensitivity = Math.Sqrt(Rho);
          double delta = 0.00001;
          double sigma2 = ( 2*rho*Math.Log2(1.25/delta) ) / Math.Pow(epsilon, 2);
          this.Distribution = new Normal(0,Math.Sqrt(sigma2));
          break;
        default:
          throw new ArgumentException("Wrong argument, only Laplace and Gaussian distributions are allowed");
      }
    }
    public RangeNoise(double epsilon, int rho, string distributionName, int seed)
    {
      this.epsilon = epsilon;
      this.Rho = rho;
      this.DistributionName = distributionName;
      var randomSource = new System.Random(seed);
      this.seed = seed;
      switch (DistributionName)
      {
        case "Laplace":
          var b = this.Rho/epsilon;
          this.Distribution = new Laplace(0,b, randomSource);
          break;
        case "Gaussian":
          double sensitivity = Math.Sqrt(Rho);
          double delta = 0.00001;
          double sigma2 = ( 2*rho*Math.Log2(1.25/delta) ) / Math.Pow(epsilon, 2);
          this.Distribution = new Normal(0,Math.Sqrt(sigma2), randomSource);
          break;
        default:
          throw new ArgumentException("Wrong argument, only Laplace and Gaussian distributions are allowed");
      }
    }
    public string DistributionName {get; set;}
    public double epsilon {get; set;}
    public int Rho {get; private set;}

    public int? seed {get; set;}
    public IContinuousDistribution Distribution {get; set;}

    public double drawProbability(){
      return Distribution.Sample();
    }
}