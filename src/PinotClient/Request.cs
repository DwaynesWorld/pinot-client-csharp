namespace PinotClient;

public record Request(string QueryFormat, string Query, bool Trace, bool UseMultistageEngine);
