using System.Text.Json.Serialization;

namespace PinotClient;

/// <summary>
/// Response is the data structure for broker response.
/// </summary>
public class Response
{
    public ResultTable? ResultTable { get; set; }

    [JsonPropertyName("SelectionResults")]
    public SelectionResults? SelectionResults { get; set; }

    public AggregationResult? AggregationResults { get; set; }

    public Dictionary<string, string>? TraceInfo { get; set; }

    public Exception[] Exceptions { get; set; } = [];

    public int NumSegmentsProcessed { get; set; }

    public int NumServersResponded { get; set; }

    public int NumSegmentsQueried { get; set; }

    public int NumServersQueried { get; set; }

    public int NumSegmentsMatched { get; set; }

    public int NumConsumingSegmentsQueried { get; set; }

    public long NumDocsScanned { get; set; }

    public long NumEntriesScannedInFilter { get; set; }

    public long NumEntriesScannedPostFilter { get; set; }

    public long TotalDocs { get; set; }

    public int TimeUsedMs { get; set; }

    public long MinConsumingFreshnessTimeMs { get; set; }

    public bool NumGroupsLimitReached { get; set; }
}

/// <summary>
/// AggregationResult is the data structure for PQL aggregation result
/// </summary>
public class AggregationResult
{
    public string Function { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string[] GroupByColumns { get; set; } = [];

    public GroupValue[] GroupByResult { get; set; } = [];
}

/// <summary>
/// GroupValue is the data structure for PQL aggregation GroupBy result
/// </summary>
public class GroupValue
{
    public string Value { get; set; } = string.Empty;

    public string[] Group { get; set; } = [];
}

/// <summary>
/// SelectionResults is the data structure for PQL selection result
/// </summary>
public class SelectionResults
{
    public string[] Columns { get; set; } = [];

    public string[][] Results { get; set; } = [];
}

/// <summary>
/// Exception is Pinot exceptions.
/// </summary>
public class Exception
{
    public string Message { get; set; } = string.Empty;

    public int ErrorCode { get; set; }
}

/// <summary>
/// DataSchema is response schema
/// </summary>
public class DataSchema
{
    public string[] ColumnDataTypes { get; init; } = [];

    public string[] ColumnNames { get; set; } = [];
}

/// <summary>
/// ResultTable is a ResultTable
/// </summary>
public class ResultTable
{
    public DataSchema DataSchema { get; set; } = null!;

    public object[][] Rows { get; set; } = [];

    /// <summary>
    /// GetRowCount returns how many rows in the ResultTable
    /// </summary>
    public int GetRowCount() => Rows.Length;

    /// <summary>
    /// GetColumnCount returns how many columns in the ResultTable
    /// </summary>
    public int GetColumnCount() => DataSchema.ColumnNames.Length;

    /// <summary>
    /// GetColumnName returns column name given column index
    /// </summary>
    public string GetColumnName(int columnIndex) => DataSchema.ColumnNames[columnIndex];

    /// <summary>
    /// GetColumnDataType returns column data type given column index
    /// </summary>
    public string GetColumnDataType(int columnIndex) => DataSchema.ColumnDataTypes[columnIndex];

    /// <summary>
    /// Get returns a ResultTable entry given row index and column index
    /// </summary>
    public object Get(int rowIndex, int columnIndex) => Rows[rowIndex][columnIndex];

    /// <summary>
    /// GetString returns a ResultTable string entry given row index and column index
    /// </summary>
    public string GetString(int rowIndex, int columnIndex) => (string)Rows[rowIndex][columnIndex];

    /// <summary>
    /// GetInt returns a ResultTable int entry given row index and column index
    /// </summary>
    public int GetInt(int rowIndex, int columnIndex) =>
        Convert.ToInt32(Rows[rowIndex][columnIndex]);

    /// <summary>
    /// GetLong returns a ResultTable long entry given row index and column index
    /// </summary>
    public long GetLong(int rowIndex, int columnIndex) =>
        Convert.ToInt64(Rows[rowIndex][columnIndex]);

    /// <summary>
    /// GetFloat returns a ResultTable float entry given row index and column index
    /// </summary>
    public float GetFloat(int rowIndex, int columnIndex) =>
        (float)Convert.ToDouble(Rows[rowIndex][columnIndex]);

    /// <summary>
    /// GetDouble returns a ResultTable double entry given row index and column index
    /// </summary>
    public double GetDouble(int rowIndex, int columnIndex) =>
        Convert.ToDouble(Rows[rowIndex][columnIndex]);
}
