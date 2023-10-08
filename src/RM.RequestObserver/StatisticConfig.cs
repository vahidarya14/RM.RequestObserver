using UAParser;

namespace RM.RequestObserver;

public class StatisticConfig
{
    public List<string> IgnoreUserNames { get; set; } = new();
    public List<string> IgnorePaths { get; set; } = new();
    public List<string> RequestTypes { get; set; } = new() { "GET" };
    public List<int> IgnoreResponseStatusCodes { get; set; } = new();
    public Action<StatisticsLog, ClientInfo, int>? CallBack { get; set; }
}
