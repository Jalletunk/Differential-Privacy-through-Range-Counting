namespace RangeCounting.Tree;
public interface IRangeNode<T>
{
  T left {get; set;}
  T right {get; set;}

  T parent {get; set;}
  bool isLeaf();

}