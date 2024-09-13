using FluentResults;

namespace PinotClient.Brokers;

public interface IBrokerSelector
{
    /// <summary>
    /// Returns the broker address in the form host:port
    /// </summary>
    Task<Result<string>> Select(string table);
}
