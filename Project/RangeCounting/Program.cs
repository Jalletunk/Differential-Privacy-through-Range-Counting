using System;
using System.IO;
using RangeCounting.Tree;
using RangeCounting.Query;
using RangeCounting.Noise;
using RangeCounting.Utils;
using System.Globalization;
using System.Collections.Generic;

namespace RangeCounting;
#pragma warning disable 8602, 8604, 162, 8618
internal class Program
{
    static string Mechanism { get; set; }
    static string Dimension { get; set; }
    static string Range { get; set; }
    static int RangeInt { get; set; }
    static double Epsilon { get; set; }
    static string Distribution { get; set; }
    static RangeTreeSimpleNoise SimpleTree { get; set; }
    static RangeTreeRangeNoise RangeTree { get; set; }
    static RangeTree2DSimpleNoise SimpleTree2D { get; set; }
    static RangeTree2DRangeNoise RangeTree2D { get; set; }
    static RangeTree2DSimpleNoise NoNoiseTree2D { get; set; }
    static RangeTreeSimpleNoise NoNoiseTree { get; set; }

    static int minimumRange { get; set; }
    static int maximumRange { get; set; }

    static int minimumRange2D { get; set; }
    static int maximumRange2D { get; set; }


    static void Main(string[] args)
    {


        bool isRunning = true;
        while (isRunning)
        {
            Mechanism = "";
            Range = "";
            Epsilon = 0.0;
            Dimension = "";
            Distribution = "";
            Console.WriteLine("Welcome to the interactive range counting program!. You must give some information about the mechanism, dimension, range and epsilon. Note, that delta is static at 10^-5");
            while (!ChooseMechanism())
            {
            }
            while (!ChooseDistribution())
            {
            }
            while (!ChooseDimension())
            {
            }
            while (!ChooseRange())
            {
            }
            while (!ChooseEpsilon())
            {
            }
            Console.WriteLine($"Building a {Dimension}-dimeensional range tree with T={Range}, using the {Mechanism} counting mechanism with the {Distribution} distribution using epsilon = {Epsilon}.");
            var isAskingQueries = true;
            switch (Dimension)
            {
                case "1":
                    Build1DTree();
                    while (!Choose1DQueryRange())
                    {
                    }
                    MakeQuery1D();
                    while (isAskingQueries)
                    {
                        Console.WriteLine("Press Q to make another query on this tree. Press E to create same tree with different epsilon value, and make a query on it. Press R to start from the beginning");
                        var answer = Console.ReadLine().ToLowerInvariant();
                        switch (answer)
                        {
                            case "e":
                                while (!ChooseEpsilon())
                                {
                                }
                                Build1DTree();
                                while (!Choose1DQueryRange())
                                {
                                }
                                MakeQuery1D();
                                break;
                            case "q":
                                while (!Choose1DQueryRange())
                                {
                                }
                                MakeQuery1D();
                                break;
                            case "r":
                                isAskingQueries = false;
                                break;
                        }
                    }
                    break;
                case "2":
                    Build2DTree();
                    while (!Choose2DQueryRange())
                    {
                    }
                    MakeQuery2D();
                    while (isAskingQueries)
                    {
                        Console.WriteLine("Press Q to make another query on this tree. Press E to create same tree with different epsilon value, and make a query on it. Press R to start from the beginning");
                        var answer = Console.ReadLine().ToLowerInvariant();
                        switch (answer)
                        {
                            case "e":
                                while (!ChooseEpsilon())
                                {
                                }
                                Build2DTree();
                                while (!Choose2DQueryRange())
                                {
                                }
                                MakeQuery2D();
                                break;
                            case "q":
                                while (!Choose2DQueryRange())
                                {
                                }
                                MakeQuery2D();
                                break;
                            case "r":
                                isAskingQueries = false;
                                break;
                        }
                    }
                    break;
            }
        }
    }
    private static bool Choose1DQueryRange()
    {
        Console.WriteLine("To make a range query on the 1-dimensional tree, enter the query range \"x_min,x_max\":");
        var intervalString = Console.ReadLine();
        var stringArray = intervalString.Split(',');
        var minString = stringArray[0];
        var maxString = stringArray[1];
        if (stringArray.Length != 2)
        {
            throw new ArgumentException("invalid query range.");
        }
        if (Int32.TryParse(minString, out var temp) && temp >= 0 && temp <= RangeInt)
        {
            minimumRange = temp;
            if (Int32.TryParse(maxString, out var maxTemp) && maxTemp >= minimumRange && maxTemp <= RangeInt)
            {
                maximumRange = maxTemp;
                return true;
            }
        }
        Console.WriteLine("Invalid range input. Try Again.");
        return false;
    }
    private static bool Choose2DQueryRange()
    {
        Console.WriteLine("To make a range query on the 2-dimensional tree. Choose query range \"x_min,x_max,y_min,y_max\":");
        var intervalString = Console.ReadLine();
        var stringArray = intervalString.Split(',');
        if (stringArray.Length != 4)
        {
            throw new ArgumentException("invalid query range.");
        }
        var minString = stringArray[0];
        var maxString = stringArray[1];
        if (Int32.TryParse(minString, out var temp) && temp >= 0 && temp <= RangeInt)
        {
            minimumRange = temp;
            if (Int32.TryParse(maxString, out var maxTemp) && maxTemp >= minimumRange && maxTemp <= RangeInt)
            {
                maximumRange = maxTemp;
            }
            else
            {
                Console.WriteLine("Invalid range input. Try Again.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid range input. Try Again.");
            return false;
        }
        var minString2D = stringArray[2];
        var maxString2D = stringArray[3];
        if (Int32.TryParse(minString2D, out var temp2D) && temp2D >= 0 && temp2D <= RangeInt)
        {
            minimumRange2D = temp2D;
            if (Int32.TryParse(maxString2D, out var maxTemp2D) && maxTemp2D >= minimumRange2D && maxTemp2D <= RangeInt)
            {
                maximumRange2D = maxTemp2D;
                return true;
            }
        }
        Console.WriteLine("Invalid range input. Try Again.");
        return false;
    }
    private static void MakeQuery1D()
    {
        RangeQuery query;
        var actualQueryObject = new RangeQuery(NoNoiseTree);
        switch (Mechanism)
        {
            case "simple":
                query = new RangeQuery(SimpleTree);
                break;
            case "range":
                query = new RangeQuery(RangeTree);
                break;
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
        Console.WriteLine($"The query over the range [{minimumRange}, {maximumRange}]:");
        var noisedQuery = query.Query(minimumRange, maximumRange);
        var actualQuery = actualQueryObject.Query(minimumRange, maximumRange);
        var absError = (double)(Math.Abs(actualQuery - noisedQuery));
        Console.WriteLine($"The noised result is: {noisedQuery}");
        Console.WriteLine($"The actual result is: {actualQuery}");
        Console.WriteLine($"The absolute error is: {absError}");
        if (actualQuery != 0)
        {
            var relError = ((double)(Math.Abs(actualQuery - noisedQuery)) / (double)actualQuery) * 100;
            Console.WriteLine($"The relative error is: {relError}%");
        }
        else
        {
            Console.WriteLine($"The relative error is: invalid, since the actual result is 0.");
        }

    }
    private static void MakeQuery2D()
    {
        RangeQuery2D query;
        var actualQueryObject = new RangeQuery2D(NoNoiseTree2D);
        switch (Mechanism)
        {
            case "simple":
                query = new RangeQuery2D(SimpleTree2D);
                break;
            case "range":
                query = new RangeQuery2D(RangeTree2D);
                break;
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
        Console.WriteLine($"The query over the range [{minimumRange}, {maximumRange}, {minimumRange2D}, {maximumRange2D}]:");
        var noisedQuery = query.Query2D(minimumRange, maximumRange, minimumRange2D, maximumRange2D);
        var actualQuery = actualQueryObject.Query2D(minimumRange, maximumRange, minimumRange2D, maximumRange2D);
        var absError = (double)(Math.Abs(actualQuery - noisedQuery));
        Console.WriteLine($"The noised result is: {noisedQuery}");
        Console.WriteLine($"The actual result is: {actualQuery}");
        Console.WriteLine($"The absolute error is: {absError}");
        if (actualQuery != 0)
        {
            var relError = ((double)(Math.Abs(actualQuery - noisedQuery)) / (double)actualQuery) * 100;
            Console.WriteLine($"The relative error is: {relError}%");
        }
        else
        {
            Console.WriteLine($"The relative error is: invalid, since the actual result is 0.");
        }

    }
    private static bool ChooseMechanism()
    {
        Console.WriteLine("Press R to use the range counting mechanism. Press S to use the simple counting mechanism");
        var mechanism = Console.ReadLine().ToLowerInvariant();
        if (mechanism == "r")
        {
            Mechanism = "range";
            return true;
        }
        else if (mechanism == "s")
        {
            Mechanism = "simple";
            return true;
        }
        Console.WriteLine("Invalid mechanism input. Try Again.");
        return false;
    }
    private static bool ChooseDistribution()
    {
        Console.WriteLine("Press L to use the Laplace distribution. Press G to use the Gaussian distribution.");
        var distribution = Console.ReadLine().ToLowerInvariant();
        if (distribution == "l")
        {
            Distribution = "Laplace";
            return true;
        }
        else if (distribution == "g")
        {
            Distribution = "Gaussian";
            return true;
        }
        Console.WriteLine("Invalid mechanism input. Try Again.");
        return false;
    }
    private static bool ChooseDimension()
    {
        Console.WriteLine("Press 1 to use 1-dimensional range counting. Press 2 to use 2-dimensional range counting.");
        var dimension = Console.ReadLine();
        if (dimension == "1" || dimension == "2")
        {
            Dimension = dimension;
            return true;
        }
        Console.WriteLine("Invalid dimension input. Try Again.");
        return false;
    }
    private static bool ChooseRange()
    {
        Console.WriteLine("Choose between the ranges T = {128, 256, 512, 1024, 2048}.");
        var range = Console.ReadLine();
        var rangeList = new List<string>() { "128", "256", "512", "1024", "2048" };
        if (rangeList.Contains(range))
        {
            Range = range;
            return true;
        }
        Console.WriteLine("Invalid range input. Try Again.");
        return false;
    }
    private static bool ChooseEpsilon()
    {
        Console.WriteLine("Choose a decimal value for epsilon. Epsilon between 0 and 1 recommended.");
        var epsilon = Console.ReadLine();
        if (Double.TryParse(epsilon, out var temp))
        {
            Epsilon = temp;
            return true;
        }
        Console.WriteLine("Invalid epsilon input. Try Again.");
        return false;
    }
    private static void Build1DTree()
    {
        var filePath = FindCountList();
        DataParser data = new DataParser(filePath);
        DataParser noNoiseData = new DataParser(filePath);
        INoise noise = Create1DNoise(Mechanism, Distribution, Epsilon, data.countList);
        NoNoiseTree = new RangeTreeSimpleNoise(noNoiseData.countList, new NoNoise());
        Create1DTree(Mechanism, noise, data.countList);
    }
    private static void Build2DTree()
    {
        var filePath = FindCountList();
        DataParser2D data = new DataParser2D(filePath);
        DataParser2D noNoiseData = new DataParser2D(filePath);
        INoise noise = Create2DNoise(Mechanism, Distribution, Epsilon, data.countList);
        NoNoiseTree2D = new RangeTree2DSimpleNoise(noNoiseData.countList, new NoNoise());
        Create2DTree(Mechanism, noise, data.countList);
    }
    public static INoise Create2DNoise(string noiseType, string distributionName, double epsilon, List<List<double>> countList)
    {
        switch (noiseType)
        {
            case "simple":
                return new SimpleNoise(epsilon, distributionName);
            case "range":
                var rho = ((int)Math.Log2(countList.Count) + 1) * ((int)Math.Log2(countList[0].Count) + 1);
                return new RangeNoise(epsilon, rho, distributionName);
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
    }
    public static void Create2DTree(string treeType, INoise noise, List<List<double>> countList)
    {
        switch (treeType)
        {
            case "simple":
                SimpleTree2D = new RangeTree2DSimpleNoise(countList, noise);
                break;
            case "range":
                RangeTree2D = new RangeTree2DRangeNoise(countList, noise);
                break;
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
    }
    public static INoise Create1DNoise(string noiseType, string distributionName, double epsilon, List<double> countList)
    {
        switch (noiseType)
        {
            case "simple":
                return new SimpleNoise(epsilon, distributionName);
            case "range":
                return new RangeNoise(epsilon, (int)Math.Log2(countList.Count) + 1, distributionName);
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
    }
    public static void Create1DTree(string treeType, INoise noise, List<double> countList)
    {
        switch (treeType)
        {
            case "simple":
                SimpleTree = new RangeTreeSimpleNoise(countList, noise);
                break;
            case "range":
                RangeTree = new RangeTreeRangeNoise(countList, noise);
                break;
            default:
                throw new ArgumentException("wrong mechanism argument");
        }
    }
    private static string FindCountList()
    {
        switch (Dimension)
        {
            case "1":
                switch (Range)
                {
                    case "128":
                        RangeInt = 128;
                        return "lon_125.csv";
                    case "256":
                        RangeInt = 256;
                        return "lon_250.csv";
                    case "512":
                        RangeInt = 512;
                        return "lon_500.csv";
                    case "1024":
                        RangeInt = 1024;
                        return "lon_1000.csv";
                    case "2048":
                        RangeInt = 2048;
                        return "lon_2000.csv";
                        break;
                }
                break;
            case "2":
                switch (Range)
                {
                    case "128":
                        RangeInt = 128;
                        return "lat_lon_125.csv";
                    case "256":
                        RangeInt = 256;
                        return "lat_lon_250.csv";
                    case "512":
                        RangeInt = 512;
                        return "lat_lon_500.csv";
                    case "1024":
                        RangeInt = 1024;
                        return "lat_lon_1000.csv";
                    case "2048":
                        RangeInt = 2048;
                        return "lat_lon_2000.csv";
                        break;
                }
                break;
        }
        throw new ArgumentException(" Wrong Dimenesion argument");
    }
}
