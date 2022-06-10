using RangeCounting.Tree;
using System.Collections.Generic;
using System;

namespace RangeCounting.Query;
public class RangeQuery
{
    public RangeQuery(IRangeTree<RangeNode, double> tree)
    {
        this.tree = tree;
    }
    public IRangeTree<RangeNode, double> tree { get; private set; }

    public int Query(int minQueryInterval, int maxQueryInterval)
    {
        RangeNode splitNode = this.FindSplitNode(minQueryInterval, maxQueryInterval);
        if (!splitNode.isLeaf())
        { // is leaf
            double maxCount = FindMax(maxQueryInterval, splitNode.right);
            double minCount = FindMin(minQueryInterval, splitNode.left);
            return (int)Math.Round(minCount + maxCount);
        }
        else
        {
            return (int)Math.Round(splitNode.count);
        }
    }

    public double FindMax(int maxQueryInterval, RangeNode maxSubTree)
    {
        RangeNode node = maxSubTree;
        double count = 0;
        while (maxQueryInterval != node.max_interval)
        {
            if (node.max_cutoff <= maxQueryInterval)
            { // go right
                count += node.left.count;
                node = node.right;
            }
            else
            { // go left
                node = node.left;
            }
        }
        count += node.count;
        return count;
    }
    public double FindMin(int minQueryInterval, RangeNode minSubTree)
    {
        RangeNode node = minSubTree;
        double count = 0;
        while (minQueryInterval != node.min_interval)
        {
            if (node.min_cutoff >= minQueryInterval)
            { // go left
                count += node.right.count;
                node = node.left;
            }
            else
            { // go right
                node = node.right;
            }
        }
        count += node.count;
        return count;
    }

    public RangeNode FindSplitNode(int minQueryInterval, int maxQueryInterval)
    {
        RangeNode node = tree.root;
        while (!node.isLeaf())
        {
            RangeNode minIntervalPath = node.min_cutoff >= minQueryInterval ? node.left : node.right;
            RangeNode maxIntervalPath = node.max_cutoff <= maxQueryInterval ? node.right : node.left;
            if (minIntervalPath == maxIntervalPath)
            {
                node = minIntervalPath;
            }
            else
            {
                return node;
            }
        }
        return node;
    }


}