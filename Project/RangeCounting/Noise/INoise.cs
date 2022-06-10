
namespace RangeCounting.Noise;
public interface INoise
{
    double drawProbability();

    double epsilon {get; set;}
    string DistributionName {get; set;}
    int? seed {get; set;}
}
