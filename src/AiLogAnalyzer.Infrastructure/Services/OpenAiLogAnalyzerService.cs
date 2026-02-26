using AiLogAnalyzer.Application.Interfaces;
using AiLogAnalyzer.Domain.Models;
using OpenAI.Chat;
using System.Text.Json;

namespace AiLogAnalyzer.Infrastructure.Services;

public class OpenAiLogAnalyzerService : ILogAnalyzerService
{
    private readonly string _apiKey;

    public OpenAiLogAnalyzerService()
    {
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                  ?? throw new Exception("OPENAI_API_KEY not set");
    }

    public async Task<AnalysisResult> AnalyzeAsync(LogEntry logEntry)
    {
        // Create the chat client with model & API key
        var client = new ChatClient(
            model: "gpt-4o-mini",
            apiKey: _apiKey
        );

        // Build prompt asking AI to return structured JSON
        string prompt = $@"Analyze this log:
						{logEntry.Message}

						Return JSON like:
						{{
						  ""summary"": ""..."",
						  ""severity"": ""Low|Medium|High"",
						  ""suggestedFix"": ""...""
						}}";

        // Make API call
        var completion = await client.CompleteChatAsync(prompt);

        // Extract the first AI response
        var aiText = completion.Content[0].Text.Trim();

        // Try parsing JSON into AnalysisResult
        AnalysisResult? result = null;
        try
        {
            result = JsonSerializer.Deserialize<AnalysisResult>(aiText);
        }
        catch
        {
            // If AI didn't return valid JSON, fallback
        }

        // Return parsed result or fallback
        return result ?? new AnalysisResult
        {
            Summary = aiText,
            Severity = "Unknown",
            SuggestedFix = aiText
        };
    }

    // Batch analysis
    public async Task<List<AnalysisResult>> AnalyzeBatchAsync(List<LogEntry> logEntries)
    {
        var results = new List<AnalysisResult>();

        foreach (var log in logEntries)
        {
            var analysis = await AnalyzeAsync(log);
            results.Add(analysis);
        }

        // sort by severity
        var severityOrder = new Dictionary<string, int> { { "High", 3 }, { "Medium", 2 }, { "Low", 1 }, { "Unknown", 0 } };
        results = results.OrderByDescending(r => severityOrder.GetValueOrDefault(r.Severity, 0)).ToList();

        return results;
    }
}