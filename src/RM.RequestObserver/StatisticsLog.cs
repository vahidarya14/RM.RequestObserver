namespace RM.RequestObserver;

public record StatisticsLog
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string? User { get; set; }

    public string Path { get; set; }
    public string? Referer { get; set; }
    public string Ip { get; set; }
    public string Country { get; set; }

    //public string BrowserName { get; set; }
    //public string BrowserVersion { get; set; }
    //public string OsName { get; set; }
    //public string OsVersion { get; set; }
    //public string Device { get; set; }
}