using NUnit.Framework;
using RangeCounting;
using System.Collections.Generic;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;
using RangeCounting.Noise;
using System;

namespace RangeCountingTests.NoiseTests;
public class RangeNoiseTests
{
    RangeNoise testNoiseLaplace;
    int rho;
    RangeTree2DRangeNoise testTree;
    RangeQuery2D testQuery;

    [SetUp]
    public void Setup()
    {
        List<double> a = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> b = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> c = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> d = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> e = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> f = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> g = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> h = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<List<double>> countList2D = new List<List<double>>() { a, b, c, d, e, f, g, h };
        rho = ((int)Math.Log2(countList2D.Count) + 1) * ((int)Math.Log2(countList2D[0].Count) + 1);
        testNoiseLaplace = new RangeNoise(0.5, rho, "Laplace", 10);
        testTree = new RangeTree2DRangeNoise(countList2D, testNoiseLaplace);
        testQuery = new RangeQuery2D(testTree);
    }

    [TestCase(-12, 1, 1, 1, 8)]
    [TestCase(71, 2, 2, 1, 8)]
    [TestCase(11, 3, 3, 1, 8)]
    [TestCase(-25, 4, 4, 1, 8)]
    [TestCase(-6, 5, 5, 1, 8)]
    [TestCase(19, 6, 6, 1, 8)]
    [TestCase(32, 7, 7, 1, 8)]
    [TestCase(84, 8, 8, 1, 8)]
    [TestCase(59, 1, 2, 1, 8)]
    [TestCase(-14, 3, 4, 1, 8)]
    [TestCase(13, 1, 4, 1, 8)]
    [TestCase(195, 1, 8, 1, 8)]
    [TestCase(16, 5, 8, 1, 8)]
    [TestCase(116, 7, 8, 1, 8)]
    [TestCase(13, 5, 6, 1, 8)]
    public void TestEventPrivateNoise(int expected, int minX, int maxX, int minY, int maxY)
    {
        Assert.AreEqual(expected, testQuery.Query2D(minX, maxX, minY, maxY));
    }
}