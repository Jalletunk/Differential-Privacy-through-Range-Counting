using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RangeCounting.Utils;

public class DataParser
{
    public DataParser(string filename)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        if (dir.Name == "RangeCounting" || dir.Name == "App")
        {
            this.path = Path.Combine("..", "Data", filename);
        }
        else
        {
            this.path = Path.Combine("..", "..", "..", "..", "Data", filename);
        }
        this.countList = new List<double>();
        this.parseData();
    }
    public string path { get; set; }
    public List<double> countList { get; set; }
    private void parseData()
    {
        int previousIndex = -1;
        foreach (string line in File.ReadLines(path))
        {
            string[] parsedLine = line.Split(',');
            int index = Int32.Parse(parsedLine[0]) - 1;
            double count = Double.Parse(parsedLine[1]);
            while (previousIndex + 1 != index)
            { // <
                countList.Add(0);
                previousIndex++;
            }
            countList.Add(count);
            previousIndex = index;
        }

        double log2 = Math.Log2(countList.Count);
        if (Math.Ceiling(log2) == Math.Floor(log2))
        {
            return;
        }
        else
        {
            int neededLength = (int)Math.Pow(2, (Math.Floor(log2) + 1));
            while (countList.Count != neededLength)
            {
                countList.Add(0);
            }
        }
    }

    private bool isPowerOfTwo(int n)
    {
        return (Math.Ceiling(Math.Log2(n)) == Math.Floor(Math.Log2(n)));
    }
}

