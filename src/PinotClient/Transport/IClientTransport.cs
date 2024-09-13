using FluentResults;

namespace PinotClient.Transport;

public interface IClientTransport
{
    Task<Result<Response>> Execute(string brokerAddress, Request request);
}
