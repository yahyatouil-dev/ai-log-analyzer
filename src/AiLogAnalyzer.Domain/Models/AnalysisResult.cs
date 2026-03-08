namespace AiLogAnalyzer.Domain.Models;

public class AnalysisResult
{
    public string Summary { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string SuggestedFix { get; set; } = string.Empty;
}