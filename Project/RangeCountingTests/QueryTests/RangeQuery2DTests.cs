using NUnit.Framework;
using RangeCounting;
using System.Collections.Generic;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;
using RangeCounting.Noise;

namespace RangeCountingTests.QueryTests;
public class RangeQuery2DTests
{
    NoNoise dummyNoise;
    RangeTree2DRangeNoise testTree;
    RangeQuery2D testQuery;

    DataParser2D testDataParser;
    RangeTree2DRangeNoise testTreeBig;
    RangeQuery2D testQueryBig;

    [SetUp]
    public void Setup()
    {
        dummyNoise = new NoNoise();
        List<double> a = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> b = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> c = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> d = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> e = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> f = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> g = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<double> h = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        List<List<double>> countList2D = new List<List<double>>() { a, b, c, d, e, f, g, h };
        testTree = new RangeTree2DRangeNoise(countList2D, dummyNoise);
        testQuery = new RangeQuery2D(testTree);

        testDataParser = new DataParser2D("lat_lon.csv");
        testTreeBig = new RangeTree2DRangeNoise(testDataParser.countList, dummyNoise);
        testQueryBig = new RangeQuery2D(testTreeBig);
    }

    [TestCase(8, 1, 1, 1, 8)]
    [TestCase(16, 1, 2, 1, 8)]
    [TestCase(24, 1, 3, 1, 8)]
    [TestCase(32, 1, 4, 1, 8)]
    [TestCase(40, 1, 5, 1, 8)]
    [TestCase(48, 1, 6, 1, 8)]
    [TestCase(56, 1, 7, 1, 8)]
    [TestCase(64, 1, 8, 1, 8)]
    [TestCase(48, 2, 7, 1, 8)]
    [TestCase(4, 1, 1, 1, 4)]
    [TestCase(8, 1, 2, 1, 4)]
    [TestCase(12, 1, 3, 1, 4)]
    [TestCase(16, 1, 4, 1, 4)]
    [TestCase(20, 1, 5, 1, 4)]
    [TestCase(24, 1, 6, 1, 4)]
    [TestCase(28, 1, 7, 1, 4)]
    [TestCase(32, 1, 8, 1, 4)]
    [TestCase(24, 2, 7, 1, 4)]
    [TestCase(8, 1, 8, 1, 1)]
    [TestCase(16, 1, 8, 1, 2)]
    [TestCase(24, 1, 8, 1, 3)]
    [TestCase(40, 1, 8, 1, 5)]
    [TestCase(48, 1, 8, 1, 6)]
    [TestCase(56, 1, 8, 1, 7)]
    [TestCase(48, 1, 8, 2, 7)]
    [TestCase(36, 2, 7, 2, 7)]
    [TestCase(12, 3, 6, 5, 7)]
    [TestCase(8, 2, 5, 4, 5)]
    [TestCase(12, 1, 3, 3, 6)]
    [TestCase(6, 5, 6, 2, 4)]
    [TestCase(2, 7, 8, 8, 8)]
    [TestCase(3, 8, 8, 3, 5)]
    [TestCase(15, 6, 8, 4, 8)]
    [TestCase(6, 2, 4, 3, 4)]
    public void RangeQuerySimpleDataTest(int expected, int minX, int maxX, int minY, int maxY)
    {
        Assert.AreEqual(expected, testQuery.Query2D(minX, maxX, minY, maxY));
    }

    [TestCase(1, 872, 872, 542, 542)]
    [TestCase(1, 149, 149, 459, 459)]
    [TestCase(39, 149, 433, 1, 621)]
    [TestCase(36, 149, 433, 1, 521)]
    [TestCase(14, 432, 433, 491, 524)]
    [TestCase(4925, 2, 1023, 2, 1023)]
    [TestCase(4923, 2, 871, 2, 928)]
    [TestCase(4925, 1, 1024, 1, 1024)]
    public void RangeQueryActualDataTest(int expected, int minX, int maxX, int minY, int maxY)
    {
        Assert.AreEqual(expected, testQueryBig.Query2D(minX, maxX, minY, maxY));
    }
}