using System.Collections.Generic;
using NUnit.Framework;
using RangeCounting.Tree;
using RangeCounting.Noise;
namespace RangeCountingTests.TreeTests;
public class RangeTreeSimpleNoiseTests
{
  IRangeTree<RangeNode,double> testTree;
  INoise dummyNoise;


  [SetUp]
  public void Setup()
  {
    dummyNoise = new NoNoise();
    List<double> countList = new List<double>(){1,1,1,1,1,1,1,1};
    testTree = new RangeTreeSimpleNoise(countList, dummyNoise);
  }

  [Test]
  public void TreeConstructionTest()
  {
    Assert.AreEqual(1, testTree.root.left.left.left.count);
    Assert.AreEqual(2, testTree.root.left.left.count);
    Assert.AreEqual(4, testTree.root.left.count);
    Assert.AreEqual(8, testTree.root.count);
  }
}