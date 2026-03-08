using AiLogAnalyzer.Domain.Models;

namespace AiLogAnalyzer.Application.Interfaces;

public interface ILogAnalyzerService
{
    Task<AnalysisResult> AnalyzeAsync(LogEntry logEntry);
}