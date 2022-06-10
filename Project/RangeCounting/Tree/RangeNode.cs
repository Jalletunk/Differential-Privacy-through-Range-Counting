using System;


namespace RangeCounting.Tree;

public class RangeNode : IRangeNode<RangeNode>{
  public RangeNode(int min_interval, int max_interval){
      this.left = null;
      this.right = null;
      this.min_interval = min_interval;
      this.max_interval = max_interval;
      this.parent = null;
      this.count = 0;
      this.max_cutoff = 0;
      this.min_cutoff = 0;
      
  }
  public RangeNode left {get; set;}
  public RangeNode right {get; set;}

  public RangeNode parent {get; set;}
  public int min_interval {get; set;}
  public int max_interval {get; set;}
  public int max_cutoff {get; set;}
  public int min_cutoff {get; set;}
  public double count {get; set;}
  public bool isLeaf(){
    if (this.left == null && this.right == null) {
      return true;
    }
    return false;
  }
}
