using System;
using System.Collections.Generic;
using System.Linq;
using RangeCounting.Utils;
using MathNet.Numerics.Distributions;

namespace RangeCounting.Noise;

public class SimpleNoise : INoise
{
    public SimpleNoise(double epsilon, string distributionName)
    {
        this.epsilon = epsilon;
        this.DistributionName = distributionName;
        this.seed = null;
        switch (DistributionName)
        {
            case "Laplace":
                double b = 1 / epsilon;
                this.Distribution = new Laplace(0, b);
                break;
            case "Gaussian":
                double sensitivity = 1;
                double delta = 0.00001;
                double sigma2 = (2 * sensitivity * Math.Log2(1.25 / delta)) / Math.Pow(epsilon, 2);
                this.Distribution = new Normal(0, Math.Sqrt(sigma2));
                break;
            default:
                throw new ArgumentException("Wrong argument, only Laplace and Gaussian distributions are allowed");
        }
    }
    public SimpleNoise(double epsilon, string distributionName, int seed)
    {
        this.epsilon = epsilon;
        this.DistributionName = distributionName;
        var randomSource = new System.Random(seed);
        this.seed = seed;
        switch (DistributionName)
        {
            case "Laplace":
                double b = 1 / epsilon;
                this.Distribution = new Laplace(0, b, randomSource);
                break;
            case "Gaussian":
                double sensitivity = 1;
                double delta = 0.00001;
                double sigma2 = (2 * sensitivity * Math.Log2(1.25 / delta)) / Math.Pow(epsilon, 2);
                this.Distribution = new Normal(0, Math.Sqrt(sigma2), randomSource);
                break;
            default:
                throw new ArgumentException("Wrong argument, only Laplace and Gaussian distributions are allowed");
        }
    }
    public string DistributionName { get; set; }
    public double epsilon { get; set; }
    public int? seed { get; set; }
    public IContinuousDistribution Distribution { get; set; }

    public double drawProbability()
    {
        return Distribution.Sample();
    }
}