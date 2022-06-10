using System.Collections.Generic;
using NUnit.Framework;
using RangeCounting.Tree;
using RangeCounting.Utils;
using RangeCounting.Noise;
namespace RangeCountingTests.TreeTests;
public class RangeTree2DSimpleNoiseTests
{
    IRangeTree<RangeNode2D, List<double>> testTree;
    IRangeTree<RangeNode2D, List<double>> testTreeActualData;
    IRangeTree<RangeNode2D, List<double>> testTreeLeafNoiseActualData;

    NoNoise dummyNoise;
    SimpleNoise leafNoise;
    DataParser2D testDataParser;
    DataParser2D testDataParserActualData;

    [SetUp]
    public void Setup()
    {
        dummyNoise = new NoNoise();
        testDataParser = new DataParser2D("fake_data.csv");
        testDataParserActualData = new DataParser2D("lat_lon.csv");
        testTree = new RangeTree2DSimpleNoise(testDataParser.countList, dummyNoise);
        testTreeActualData = new RangeTree2DSimpleNoise(testDataParserActualData.countList, dummyNoise);

        leafNoise = new SimpleNoise(2.0, "Laplace");
        testTreeLeafNoiseActualData = new RangeTree2DSimpleNoise(testDataParserActualData.countList, leafNoise);
    }

    [Test]
    public void TreeConstructionTest()
    {
        Assert.AreEqual(8, testTree.max_interval);
        Assert.AreEqual(16, testTree.root.left.left.left.count);
        Assert.IsNull(testTree.root.left.left.right.Ytree.root.left.left.left.left);
        Assert.AreEqual(1, testTree.root.left.left.right.count);
        Assert.AreEqual(21, testTree.root.count);
        Assert.AreEqual(16, testTree.root.left.left.left.Ytree.root.count);
        Assert.AreEqual(17, testTree.root.left.left.Ytree.root.count);
        Assert.AreEqual(20, testTree.root.left.Ytree.root.count);
    }

    [Test]
    public void IntervalTest()
    {
        var node = testTreeActualData.root.right.right.left.right;
        Assert.AreEqual(1024, testTreeActualData.max_interval);
        Assert.AreEqual((833, 896), (node.min_interval, node.max_interval));
        Assert.AreEqual(24, node.count);
    }

    [Test]
    public void CountTest()
    {
        Assert.AreEqual(4925, testTreeActualData.root.count);
        Assert.AreEqual(2, testTreeActualData.root.left.left.count);
    }
    [Test]
    public void IntervalLeafNoiseTest()
    {
        var node = testTreeLeafNoiseActualData.root.right.right.left.right;
        Assert.AreEqual(1024, testTreeLeafNoiseActualData.max_interval);
        Assert.AreEqual((833, 896), (node.min_interval, node.max_interval));
        Assert.AreNotEqual(24, node.count);
    }

    [Test]
    public void CountLeafNoiseTest()
    {
        Assert.AreNotEqual(4925, testTreeLeafNoiseActualData.root.count);
        Assert.AreNotEqual(2, testTreeLeafNoiseActualData.root.left.left.count);
    }
}