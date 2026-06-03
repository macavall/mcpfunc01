using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace Proj1.Tools;

/// <summary>
/// A minimal MCP tool that returns a greeting. Use this to confirm an MCP
/// client (for example, GitHub Copilot agent mode) can discover and invoke
/// tools exposed by this function app.
/// </summary>
public class HelloTool
{
    private readonly ILogger<HelloTool> _logger;

    public HelloTool(ILogger<HelloTool> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SayHello))]
    public string SayHello(
        [McpToolTrigger(
            "say_hello",
            "Returns a friendly greeting. Useful for verifying the MCP server is reachable.")]
            ToolInvocationContext context,
        [McpToolProperty(
            "name",
            "The name of the person to greet. Defaults to 'world' when omitted.")]
            string? name)
    {
        _logger.LogInformation("SayHello invoked for session {SessionId}", context.SessionId);
        return $"Hello, {(string.IsNullOrWhiteSpace(name) ? "world" : name)}! 👋 Your Azure Functions MCP server is working.";
    }
}
