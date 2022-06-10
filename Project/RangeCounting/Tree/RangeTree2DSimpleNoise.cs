using System;
using System.Collections.Generic;
using System.Linq;
using RangeCounting.Utils;
using RangeCounting.Noise;

namespace RangeCounting.Tree;

public class RangeTree2DSimpleNoise : IRangeTree<RangeNode2D, List<double>>
{
    public RangeTree2DSimpleNoise(List<List<double>> countList, INoise noise)
    {
        this.noise = noise;
        this.countList = countList;
        if (noise.GetType() == typeof(NoNoise))
        {
            this.yTreeNoise = noise;
        }
        else if (noise.seed == null)
        {
            this.yTreeNoise = new SimpleNoise(noise.epsilon, noise.DistributionName);
        }
        else
        {
            this.yTreeNoise = new SimpleNoise(noise.epsilon, noise.DistributionName, (int)noise.seed);
        }
        this.min_interval = 1;
        this.max_interval = countList.Count;
        this.root = createTree(min_interval, max_interval, null);
    }
    public RangeNode2D root { get; set; }
    public INoise yTreeNoise { get; set; }
    public List<List<double>> countList { get; set; }
    public INoise noise { get; set; }
    public int min_interval { get; set; }
    public int max_interval { get; set; }

    public RangeNode2D createTree(int min, int max, RangeNode2D parent)
    {
        RangeNode2D node = new RangeNode2D(min, max);
        // case if tree is just one element
        if (min == max && parent == null)
        {
            node.min_cutoff = min;
            node.max_cutoff = max;
            node.Ytree = new RangeTreeSimpleNoise(countList[min - 1], yTreeNoise);
            node.count = node.Ytree.root.count;
        }
        // base case if node is the root of the tree
        else if (parent == null)
        {
            int diff = max - min;
            node.min_cutoff = (int)Math.Floor(diff / 2.0) + min;
            node.max_cutoff = (int)Math.Ceiling(diff / 2.0) + min;
            node.left = createTree(min, node.min_cutoff, node);
            node.right = createTree(node.max_cutoff, max, node);
            node.Ytree = UnionTrees(node.left.Ytree, node.right.Ytree);
            node.count = node.Ytree.root.count;
        }
        // recursive stop case if node is a leaf
        else if (min == max)
        {
            node.parent = parent;
            node.min_cutoff = min;
            node.max_cutoff = max;
            node.Ytree = new RangeTreeSimpleNoise(countList[min - 1], yTreeNoise);
            node.count = node.Ytree.root.count;
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
            node.Ytree = UnionTrees(node.left.Ytree, node.right.Ytree);
            node.count = node.Ytree.root.count;
        }
        return node;
    }

    private IRangeTree<RangeNode, double> UnionTrees(IRangeTree<RangeNode, double> leftTree, IRangeTree<RangeNode, double> rightTree)
    {
        IRangeTree<RangeNode, double> newTree; // newTree starts as a copy of the left tree, and then the right trees counts is added.
        newTree = new RangeTreeSimpleNoise(leftTree.countList, leftTree.noise);
        TraverseTreeCounts(rightTree.root, newTree.root, newTree.countList);
        return newTree;
    }
    private void TraverseTreeCounts(RangeNode rightTreeNode, RangeNode newTreeNode, List<double> newTreeCountlist)
    {
        if (rightTreeNode.isLeaf())
        {

            newTreeNode.count += rightTreeNode.count;
            newTreeCountlist[newTreeNode.min_interval - 1] = newTreeNode.count;
        }
        else
        {
            newTreeNode.count += rightTreeNode.count;
            TraverseTreeCounts(rightTreeNode.right, newTreeNode.right, newTreeCountlist);
            TraverseTreeCounts(rightTreeNode.left, newTreeNode.left, newTreeCountlist);
        }
    }
}



