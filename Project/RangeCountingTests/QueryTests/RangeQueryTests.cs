using NUnit.Framework;
using RangeCounting;
using System.Collections.Generic;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;
using RangeCounting.Noise;

namespace RangeCountingTests.QueryTests;
public class RangeQueryTests
{
    RangeQuery testQuery;
    RangeTreeSimpleNoise testTree;
    RangeTreeSimpleNoise testTreeBig;
    RangeTreeSimpleNoise testTreeNoise;
    RangeQuery testQueryBig;
    RangeQuery testQueryNoise;
    NoNoise dummyNoise;
    SimpleNoise eventNoise;
    DataParser testDataParser;


    [SetUp]
    public void Setup()
    {
        dummyNoise = new NoNoise();
        List<double> countList = new List<double>() { 1, 1, 1, 1, 1, 1, 1, 1 };
        testTree = new RangeTreeSimpleNoise(countList, dummyNoise);
        testQuery = new RangeQuery(testTree);
        testDataParser = new DataParser("day_count.csv");
        testTreeBig = new RangeTreeSimpleNoise(testDataParser.countList, dummyNoise);
        testQueryBig = new RangeQuery(testTreeBig);
        eventNoise = new SimpleNoise(32, "Laplace");

        testTreeNoise = new RangeTreeSimpleNoise(testDataParser.countList, eventNoise);
        testQueryNoise = new RangeQuery(testTreeNoise);

    }

    [Test]
    public void RangeQuerySimpleTest()
    {
        int actualCount1 = testQuery.Query(1, 8);
        Assert.AreEqual(8, actualCount1);
        int actualCount2 = testQuery.Query(2, 3); // two leafs test
        Assert.AreEqual(2, actualCount2);
        int actualCount3 = testQuery.Query(3, 6);
        Assert.AreEqual(4, actualCount3);
        int actualCount4 = testQuery.Query(2, 7);
        Assert.AreEqual(6, actualCount4);
        int actualCount5 = testQuery.Query(1, 1);
        Assert.AreEqual(1, actualCount5);
    }

    [Test]
    public void RangeQueryBigTest()
    {
        int actualCount1 = testQueryBig.Query(1, 2);
        Assert.AreEqual(704333, actualCount1);
        int actualCount2 = testQueryBig.Query(1, 32);
        Assert.AreEqual(14092413, actualCount2);
        int actualCount3 = testQueryBig.Query(10, 25);
        Assert.AreEqual(7477812, actualCount3);
    }
}
