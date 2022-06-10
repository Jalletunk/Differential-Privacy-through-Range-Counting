using NUnit.Framework;
using RangeCounting;
using System.Collections.Generic;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;
using RangeCounting.Noise;
using System;

namespace RangeCountingTests.NoiseTests;
public class SimpleNoiseTests
{
    SimpleNoise testNoiseLaplace;
    SimpleNoise testNoiseGaussian;
    RangeTree2DSimpleNoise testTree;
    RangeQuery2D testQuery;

    [SetUp]
    public void Setup()
    {
        testNoiseLaplace = new SimpleNoise(0.5, "Laplace", 32);
        testNoiseGaussian = new SimpleNoise(0.5, "Gaussian", 32);

        List<double> a = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> b = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> c = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> d = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> e = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> f = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> g = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> h = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<List<double>> countList2D = new List<List<double>>() { a, b, c, d, e, f, g, h };
        testTree = new RangeTree2DSimpleNoise(countList2D, testNoiseLaplace);
        testQuery = new RangeQuery2D(testTree);
    }

    [Test]
    public void LaplaceTest()
    {
        Assert.That(Math.Abs(-0.23 - testNoiseLaplace.drawProbability()), Is.LessThan(0.01M));
    }

    [Test]
    public void GaussianTest()
    {
        Assert.That(Math.Abs(-3.14 - testNoiseGaussian.drawProbability()), Is.LessThan(0.01M));
    }

    [TestCase(2, 1, 1, 1, 8)]
    [TestCase(1, 2, 2, 1, 8)]
    [TestCase(-1, 3, 3, 1, 8)]
    [TestCase(5, 4, 4, 1, 8)]
    [TestCase(5, 5, 5, 1, 8)]
    [TestCase(7, 6, 6, 1, 8)]
    [TestCase(6, 7, 7, 1, 8)]
    [TestCase(-4, 8, 8, 1, 8)]
    [TestCase(3, 1, 2, 1, 8)]
    [TestCase(4, 3, 4, 1, 8)]
    [TestCase(29, 1, 4, 1, 8)]
    [TestCase(19, 1, 8, 1, 8)]
    [TestCase(12, 5, 8, 1, 8)]
    [TestCase(2, 7, 8, 1, 8)]
    [TestCase(12, 5, 6, 1, 8)]
    public void TestLeafPrivateNoise(int expected, int minX, int maxX, int minY, int maxY)
    {
        Assert.AreEqual(expected, testQuery.Query2D(minX, maxX, minY, maxY));
    }
}