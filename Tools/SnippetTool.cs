using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace Proj1.Tools;

/// <summary>
/// A pair of MCP tools that persist and retrieve text snippets in Azure Blob
/// Storage. This demonstrates tools that take arguments and maintain state,
/// the canonical Azure Functions MCP sample.
/// Blobs are keyed by snippet name: snippets/{name}.json.
/// </summary>
public class SnippetTool
{
    private const string BlobPath = "snippets/{mcptoolargs.name}.json";

    private readonly ILogger<SnippetTool> _logger;

    public SnippetTool(ILogger<SnippetTool> logger)
    {
        _logger = logger;
    }

    [Function(nameof(GetSnippet))]
    public string GetSnippet(
        [McpToolTrigger(
            "get_snippet",
            "Gets a previously saved text snippet by its name.")]
            ToolInvocationContext context,
        [McpToolProperty(
            "name",
            "The unique name of the snippet to retrieve.")]
            string name,
        [BlobInput(BlobPath)] string? snippetContent)
    {
        _logger.LogInformation("GetSnippet '{Name}' (session {SessionId})", name, context.SessionId);
        return snippetContent ?? $"No snippet found with the name '{name}'.";
    }

    [Function(nameof(SaveSnippet))]
    [BlobOutput(BlobPath)]
    public string SaveSnippet(
        [McpToolTrigger(
            "save_snippet",
            "Saves a text snippet under a given name so it can be retrieved later.")]
            ToolInvocationContext context,
        [McpToolProperty(
            "name",
            "The unique name to store the snippet under.")]
            string name,
        [McpToolProperty(
            "snippet",
            "The text content of the snippet to save.")]
            string snippet)
    {
        _logger.LogInformation("SaveSnippet '{Name}' (session {SessionId})", name, context.SessionId);
        return snippet;
    }
}
