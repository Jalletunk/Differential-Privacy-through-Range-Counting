using NUnit.Framework;
using RangeCounting;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;
using System.IO;

namespace RangeCountingTests.UtilsTests;
public class DataParser2DTest
{
    DataParser2D testDataParser;
    [SetUp]
    public void Setup(){
      testDataParser = new DataParser2D("fake_data.csv");
    }
    [Test]
    public void parseData2DTest(){
      Assert.AreEqual(8,testDataParser.countList.Count);
      Assert.AreEqual(3,testDataParser.countList[0][4]);
    }
    [Test]
    public void ActualDataTest(){
      DataParser2D testParser = new DataParser2D("lat_lon.csv");
      Assert.AreEqual(5, testParser.countList[835][363]);
    }
}