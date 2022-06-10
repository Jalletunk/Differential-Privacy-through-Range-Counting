using RangeCounting.Noise;
using System.Collections.Generic;
namespace RangeCounting.Tree;
public interface IRangeTree<T, W>
{
    List<W> countList { get; set; }
    T root { get; set; }
    T createTree(int min, int max, T parent);
    int min_interval { get; set; }
    int max_interval { get; set; }
    INoise noise { get; set; }
}