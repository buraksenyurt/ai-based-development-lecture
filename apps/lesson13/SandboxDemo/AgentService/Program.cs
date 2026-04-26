using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion(
    modelId: "meta-llama-3-8b-instruct",
    apiKey: "ignore",
    endpoint: new Uri("http://localhost:1234/v1")
);
var kernel = kernelBuilder.Build();

var app = builder.Build();

app.MapPost("/api/agent/vulnerable", async (LogRequest request) =>
    await ProcessTask(request.LogContent, isSecure: false));

app.MapPost("/api/agent/secure", async (LogRequest request) =>
    await ProcessTask(request.LogContent, isSecure: true));

async Task<IResult> ProcessTask(string logContent, bool isSecure)
{
    var generatedCode = await CallAIForSolution(logContent, kernel);
    var cleanCode = ExtractPythonCode(generatedCode);

    if (string.IsNullOrEmpty(cleanCode))
        return Results.BadRequest("Does not create a valid Python script");

    string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "temp_agent_script.py");
    await File.WriteAllTextAsync(scriptPath, cleanCode);

    ProcessStartInfo psi = new()
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    if (isSecure)
    {
        psi.FileName = "docker";
        psi.Arguments = $"run --rm --network none -e PYTHONIOENCODING=utf-8 -v \"{scriptPath}:/app/script.py:ro\" --memory 128m --cpus 0.5 python:3.12-alpine python /app/script.py";
    }
    else
    {
        psi.FileName = "python";
        psi.Arguments = $"\"{scriptPath}\"";
        psi.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
    }

    using var process = Process.Start(psi);
    process!.WaitForExit(30000);

    string output = await process.StandardOutput.ReadToEndAsync();
    string error = await process.StandardError.ReadToEndAsync();

    if (File.Exists(scriptPath)) File.Delete(scriptPath);

    return Results.Ok(new
    {
        Strategy = isSecure ? "Sandbox (Secure)" : "Host Execution (Vulnerable)",
        LlmCode = cleanCode,
        Output = output,
        Error = error
    });
}

async Task<string> CallAIForSolution(string logContent, Kernel kernel)
{
    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    var chatHistory = new ChatHistory("You are an autonomous DevOps agent. Analyze the provided log and write a Python script to fix it. Output ONLY valid Python code inside ```python ``` blocks. Do not explain anything.");
    chatHistory.AddUserMessage($"Fix this server log issue:\n{logContent}");

    var executionSettings = new OpenAIPromptExecutionSettings
    {
        Temperature = 0.1
    };

    var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings, kernel);
    return response.Content ?? "";
}

string ExtractPythonCode(string llmResponse)
{
    var match = PythonRegex().Match(llmResponse);
    return match.Success ? match.Groups[1].Value.Trim() : llmResponse.Trim();
}

app.Run("http://localhost:6001");

record LogRequest(string LogContent);

partial class Program
{
    [GeneratedRegex(@"```python(.*?)```", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
    private static partial Regex PythonRegex();
}