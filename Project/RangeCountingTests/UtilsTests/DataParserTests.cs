using NUnit.Framework;
using RangeCounting;
using RangeCounting.Query;
using RangeCounting.Tree;
using RangeCounting.Utils;

namespace RangeCountingTests.UtilsTests;
public class DataParserTest
{
    DataParser testDataParser;
    [SetUp]
    public void Setup(){
      testDataParser = new DataParser("day_count.csv");
    }
    [Test]
    public void parseDataTest(){
      Assert.AreEqual(327625, testDataParser.countList[0]);
      Assert.AreEqual(32,testDataParser.countList.Count);
      Assert.AreEqual(0,testDataParser.countList[31]);
      Assert.AreEqual(419962, testDataParser.countList[17]);
    }
}