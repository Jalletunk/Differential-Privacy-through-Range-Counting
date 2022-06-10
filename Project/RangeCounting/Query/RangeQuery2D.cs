using RangeCounting.Tree;
using System.Collections.Generic;
using System;

namespace RangeCounting.Query;
public class RangeQuery2D
{
    public RangeQuery2D(IRangeTree<RangeNode2D, List<double>> tree)
    {
        this.tree = tree;
    }
    public IRangeTree<RangeNode2D, List<double>> tree { get; private set; }

    public int Query2D(int XminQueryInterval, int XmaxQueryInterval, int YminQueryInterval, int YmaxQueryInterval)
    {
        RangeNode2D splitNode = this.FindSplitNode(XminQueryInterval, XmaxQueryInterval);
        if (!splitNode.isLeaf())
        { // is leaf
            double maxCount = FindMax(XmaxQueryInterval, splitNode.right, YminQueryInterval, YmaxQueryInterval);
            double minCount = FindMin(XminQueryInterval, splitNode.left, YminQueryInterval, YmaxQueryInterval);
            return (int)Math.Round(minCount + maxCount);
        }
        else
        {
            RangeQuery Yquery = new RangeQuery(splitNode.Ytree);
            int YMaxQuery = splitNode.Ytree.max_interval < YmaxQueryInterval ? splitNode.Ytree.max_interval : YmaxQueryInterval;
            return Yquery.Query(YminQueryInterval, YMaxQuery);
        }
    }
    public double FindMax(int XmaxQueryInterval, RangeNode2D maxSubTree, int YminQueryInterval, int YmaxQueryInterval)
    {
        RangeNode2D node = maxSubTree;
        double count = 0;
        while (XmaxQueryInterval != node.max_interval)
        {
            if (node.max_cutoff <= XmaxQueryInterval)
            { // go right
                RangeQuery Yquery = new RangeQuery(node.left.Ytree);
                int YMaxQuery = node.left.Ytree.max_interval < YmaxQueryInterval ? node.left.Ytree.max_interval : YmaxQueryInterval;
                count += Yquery.Query(YminQueryInterval, YMaxQuery);
                node = node.right;
            }
            else
            { // go left
                node = node.left;
            }
        }
        RangeQuery Yquery2 = new RangeQuery(node.Ytree);
        int YMaxQuery2 = node.Ytree.max_interval < YmaxQueryInterval ? node.Ytree.max_interval : YmaxQueryInterval;
        count += Yquery2.Query(YminQueryInterval, YMaxQuery2);
        return count;
    }
    public double FindMin(int XminQueryInterval, RangeNode2D minSubTree, int YminQueryInterval, int YmaxQueryInterval)
    {
        RangeNode2D node = minSubTree;
        double count = 0;
        while (XminQueryInterval != node.min_interval)
        {
            if (node.min_cutoff >= XminQueryInterval)
            { // go left
                RangeQuery Yquery = new RangeQuery(node.right.Ytree);
                int YMaxQuery = node.right.Ytree.max_interval < YmaxQueryInterval ? node.right.Ytree.max_interval : YmaxQueryInterval;
                count += Yquery.Query(YminQueryInterval, YMaxQuery);
                node = node.left;
            }
            else
            { // go right
                node = node.right;
            }
        }
        RangeQuery Yquery2 = new RangeQuery(node.Ytree);
        int YMaxQuery2 = node.Ytree.max_interval < YmaxQueryInterval ? node.Ytree.max_interval : YmaxQueryInterval;
        count += Yquery2.Query(YminQueryInterval, YMaxQuery2);
        return count;
    }
    public RangeNode2D FindSplitNode(int XminQueryInterval, int XmaxQueryInterval)
    {
        RangeNode2D node = tree.root;
        while (!node.isLeaf())
        {
            RangeNode2D XminIntervalPath = node.min_cutoff >= XminQueryInterval ? node.left : node.right;
            RangeNode2D XmaxIntervalPath = node.max_cutoff <= XmaxQueryInterval ? node.right : node.left;
            if (XminIntervalPath == XmaxIntervalPath)
            {
                node = XminIntervalPath;
            }
            else
            {
                return node;
            }
        }
        return node;
    }


}