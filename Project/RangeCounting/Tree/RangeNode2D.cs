using System;


namespace RangeCounting.Tree;

public class RangeNode2D : IRangeNode<RangeNode2D>
{
    public RangeNode2D(int min_interval, int max_interval)
    {
        this.left = null;
        this.right = null;
        this.min_interval = min_interval;
        this.max_interval = max_interval;
        this.parent = null;
        this.count = 0;
        this.max_cutoff = 0;
        this.min_cutoff = 0;
        this.Ytree = null;
    }
    public RangeNode2D left { get; set; }
    public RangeNode2D right { get; set; }
    public RangeNode2D parent { get; set; }
    public IRangeTree<RangeNode, double> Ytree { get; set; }
    public int min_interval { get; set; }
    public int max_interval { get; set; }
    public int max_cutoff { get; set; }
    public int min_cutoff { get; set; }
    public double count { get; set; }
    public bool isLeaf()
    {
        if (this.left == null && this.right == null)
        {
            return true;
        }
        return false;
    }
}
