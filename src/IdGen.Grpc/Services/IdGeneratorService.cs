using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

namespace IdGen.Grpc.Services;

/// <summary>
/// Service implementation for the IdGenerator service.
/// </summary>
/// <param name="logger"></param>
/// <param name="idGenerator"></param>
public sealed partial class IdGeneratorService(
    ILogger<IdGeneratorService> logger,
    IIdGenerator<long> idGenerator)
    : IdGenerator.IdGeneratorBase
{
    private readonly ILogger<IdGeneratorService> _logger = logger;

    /// <summary>
    /// Generates a new Snowflake ID of type int64
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task<SnowflakeIdResponse> GetSnowflakeId(Empty request, ServerCallContext context)
    {
        long newId = idGenerator.CreateId();
        TraceLogNewSnowflakeID(newId);
        return Task.FromResult(new SnowflakeIdResponse() { Id = newId });
    }

    [LoggerMessage("Created new Snowflake ID: {newSnowflakeId}", Level = LogLevel.Trace)]
    private partial void TraceLogNewSnowflakeID(long newSnowflakeId);
}
