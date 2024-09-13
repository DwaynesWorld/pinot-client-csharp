using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Logging;
using PinotClient.Brokers;
using PinotClient.Transport;

namespace PinotClient;

public class Connection(
    ILogger<Connection> logger,
    IClientTransport transport,
    IBrokerSelector brokerSelector
)
{
    private const string ISO8601Format = "yyyy-MM-dd'T'HH:mm:ss.fff";
    private bool _trace;
    private bool _useMultistageEngine;

    /// <summary>
    /// Use Multistage Engine for the connection
    /// </summary>
    public void UseMultistageEngine(bool val) => _useMultistageEngine = val;

    /// <summary>
    /// Open trace for the connection
    /// </summary>
    public void OpenTrace() => _trace = true;

    /// <summary>
    /// Close trace for the connection
    /// </summary>
    public void CloseTrace() => _trace = false;

    /// <summary>
    /// Execute SQL for a given table
    /// </summary>
    public async Task<Result<Response>> ExecuteSQL(string table, string query)
    {
        var addressResult = await brokerSelector.Select(table);
        if (addressResult.IsFailed)
        {
            logger.LogError(
                "Unable to find an available broker for table {table}, Error: {err}",
                table,
                addressResult.Errors
            );
            return addressResult.ToResult();
        }

        var responseResult = await transport.Execute(
            brokerAddress: addressResult.Value,
            request: new(
                QueryFormat: "sql",
                Query: query,
                Trace: _trace,
                UseMultistageEngine: _useMultistageEngine
            )
        );

        if (responseResult.IsFailed)
        {
            logger.LogError(
                "Caught exception to execute SQL query {query}, Error: {err}\n",
                query,
                addressResult.Errors
            );
            return addressResult.ToResult();
        }

        return responseResult.Value;
    }

    /// <summary>
    /// ExecuteSQLWithParams executes an SQL query with parameters for a given table
    /// </summary>
    public async Task<Result<Response>> ExecuteSQLWithParams(
        string table,
        string queryPattern,
        params object[] parameters
    )
    {
        var result = FormatQuery(queryPattern, parameters);
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return await ExecuteSQL(table, result.Value);
    }

    internal static Result<string> FormatQuery(string queryPattern, params object[] parameters)
    {
        // Count the number of placeholders in queryPattern
        var numPlaceholders = queryPattern.Where(c => c == '?').Count();

        if (numPlaceholders != parameters.Length)
        {
            return Result.Fail(
                $"Number of placeholders in queryPattern ({queryPattern}) does not match number of parameters ({parameters.Length})"
            );
        }

        // Split the query by '?' and incrementally build the new query
        var parts = queryPattern.Split("?");
        var @new = new StringBuilder();

        for (int i = 0; i < parts.Length - 1; i++)
        {
            @new.Append(parts[i]);
            @new.Append(FormatArg(parameters[i]));
        }

        // Add the last part of the query, which does not follow a '?'
        @new.Append(parts[^1]);
        return @new.ToString();
    }

    internal static string FormatArg(object value)
    {
        return value switch
        {
            // For pinot types - STRING, BIG_DECIMAL - enclose in single quotes
            string s => $"'{s}'",

            // For pinot type - BYTES - convert to Hex string and enclose in single quotes
            byte[] b => $"'{Convert.ToHexString(b)}'",

            // For pinot type - TIMESTAMP - convert to ISO8601 format and enclose in single quotes
            DateTimeOffset dto => $"'{dto.ToString(ISO8601Format, CultureInfo.InvariantCulture)}'",

            // For pinot types - INT, LONG, FLOAT, DOUBLE and BOOLEAN use as-is
            bool
            or byte
            or short
            or int
            or long
            or ushort
            or uint
            or ulong
            or float
            or double
            or decimal
                => $"{value}",

            // Throw error for unsupported types
            _ => throw new NotSupportedException("")
        };
    }
}
