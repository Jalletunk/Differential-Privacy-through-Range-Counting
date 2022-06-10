using System;
using System.Collections.Generic;
using System.IO;

namespace RangeCounting.Utils;

public class DataParser2D
{
    public DataParser2D(string filename)
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
        this.countList = new List<List<double>>();
        this.parseData();
    }
    public string path { get; set; }
    public int universe_max_x { get; set; }
    public int universe_max_y { get; set; }
    public List<List<double>> countList { get; set; }
    private void parseData()
    {
        bool firstLine = true;
        foreach (string line in File.ReadLines(path))
        {
            string[] parsedLine = line.Split(',');

            int xIndex = Int32.Parse(parsedLine[0]) - 1;
            int yIndex = Int32.Parse(parsedLine[1]) - 1;
            double count = Double.Parse(parsedLine[2]);
            if (firstLine)
            {
                universe_max_x = (int)Math.Pow(2, (Math.Floor(Math.Log2(xIndex)) + 1));
                universe_max_y = (int)Math.Pow(2, (Math.Floor(Math.Log2(yIndex)) + 1));
                firstLine = false;

                // Fill out Initial List and subList
                for (int i = 0; i < universe_max_x; i++)
                {
                    countList.Add(new List<double>());
                    for (int j = 0; j < universe_max_y; j++)
                    {
                        countList[i].Add(0);
                    }
                }
            }
            else
            {
                countList[xIndex][yIndex] = count;
            }
        }
    }
}
