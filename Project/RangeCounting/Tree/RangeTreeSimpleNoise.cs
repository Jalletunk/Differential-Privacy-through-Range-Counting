using System;
using System.Collections.Generic;
using System.Linq;
using RangeCounting.Utils;
using RangeCounting.Noise;

namespace RangeCounting.Tree;

public class RangeTreeSimpleNoise : IRangeTree<RangeNode, double>
{
    public RangeTreeSimpleNoise(List<double> countList, INoise noise)
    {
        this.noise = noise;
        this.countList = countList;
        this.min_interval = 1;
        this.max_interval = countList.Count;
        this.root = createTree(min_interval, max_interval, null);
    }
    public RangeNode root { get; set; }
    public List<double> countList { get; set; }
    public INoise noise { get; set; }
    public int min_interval { get; set; }
    public int max_interval { get; set; }

    private double findIntervalCount(int min)
    {
        return noise.drawProbability() + this.countList[min - 1];
    }

    public RangeNode createTree(int min, int max, RangeNode parent)
    {
        RangeNode node = new RangeNode(min, max);
        // case if tree is just one element
        if (min == max && parent == null)
        {
            int diff = max - min;
            node.min_cutoff = (int)Math.Floor(diff / 2.0) + min;
            node.max_cutoff = (int)Math.Ceiling(diff / 2.0) + min;
            node.left = null;
            node.right = null;
            node.count = findIntervalCount(min);
        }
        // base case if node is the root of the tree
        else if (parent == null)
        {
            int diff = max - min;
            node.min_cutoff = (int)Math.Floor(diff / 2.0) + min;
            node.max_cutoff = (int)Math.Ceiling(diff / 2.0) + min;
            node.left = createTree(min, node.min_cutoff, node);
            node.right = createTree(node.max_cutoff, max, node);
            node.count = node.left.count + node.right.count;
        }
        // recursive stop case if node is a leaf
        else if (min == max)
        {
            node.parent = parent;
            node.min_cutoff = min;
            node.max_cutoff = max;
            node.count = findIntervalCount(min);
        }
        // recursive case
        else
        {
            node.parent = parent;
            int diff = max - min;
            node.min_cutoff = (int)Math.Floor(diff / 2.0) + min;
            node.max_cutoff = (int)Math.Ceiling(diff / 2.0) + min;
            node.left = createTree(min, node.min_cutoff, node);
            node.right = createTree(node.max_cutoff, max, node);
            node.count = node.left.count + node.right.count;
        }
        return node;
    }
}



