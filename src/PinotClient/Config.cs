namespace PinotClient;

/// <summary>
/// ClientConfig configs to create a PinotDbConnection
/// </summary>
public class ClientConfig
{
    /// <summary>
    /// Additional HTTP headers to include in broker query API requests
    /// </summary>
    public Dictionary<string, string> ExtraHTTPHeader { get; set; } = [];

    /// <summary>
    /// Zookeeper Configs
    /// </summary>
    public ZookeeperConfig? ZkConfig { get; set; }

    /// <summary>
    /// Controller Config
    /// </summary>
    public ControllerConfig? ControllerConfig { get; set; }

    /// <summary>
    /// BrokerList
    /// </summary>
    public string[] BrokerList { get; set; } = [];

    /// <summary>
    /// HTTP request timeout in your broker query for API requests
    /// </summary>
    public int HttpTimeoutMs { get; set; }

    /// <summary>
    /// UseMultistageEngine is a flag to enable multistage query execution engine
    /// </summary>
    public bool UseMultistageEngine { get; set; }
}

/// <summary>
/// ZookeeperConfig describes how to config Pinot Zookeeper connection
/// </summary>
public class ZookeeperConfig
{
    /// <summary>
    ///
    /// </summary>
    public string PathPrefix { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public string[] ZookeeperPath { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    public int SessionTimeoutMs { get; set; }
}

/// <summary>
/// ControllerConfig describes connection of a controller-based selector that
/// periodically fetches table-to-broker mapping via the controller API
/// </summary>
public class ControllerConfig
{
    /// <summary>
    /// Additional HTTP headers to include in the controller API request
    /// </summary>
    public Dictionary<string, string> ExtraControllerAPIHeaders { get; set; } = [];

    /// <summary>
    ///
    /// </summary>
    public string ControllerAddress { get; set; } = string.Empty;

    /// <summary>
    /// Frequency of broker data refresh in milliseconds via controller API - defaults to 1000ms
    /// </summary>
    public int UpdateFrequencyMs { get; set; } = 1000;
}
