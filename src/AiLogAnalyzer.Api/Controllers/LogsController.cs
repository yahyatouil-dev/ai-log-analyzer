using AiLogAnalyzer.Application.Interfaces;
using AiLogAnalyzer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiLogAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogAnalyzerController : ControllerBase
{
    private readonly ILogAnalyzerService _logAnalyzerService;

    public LogAnalyzerController(ILogAnalyzerService logAnalyzerService)
    {
        _logAnalyzerService = logAnalyzerService;
    }

    [HttpPost("analyze")]
    public async Task<AnalysisResult> Analyze([FromBody] LogEntry logEntry)
    {
        return await _logAnalyzerService.AnalyzeAsync(logEntry);
    }
	
	[HttpPost("analyze/batch")]
	public async Task<ActionResult<List<AnalysisResult>>> AnalyzeBatch([FromBody] List<LogEntry> logEntries)
{
    var results = new List<AnalysisResult>();

    foreach (var log in logEntries)
    {
        var analysis = await _logAnalyzerService.AnalyzeAsync(log);
        results.Add(analysis);
    }

    // sort by severity (High > Medium > Low)
    var severityOrder = new Dictionary<string, int> { { "High", 3 }, { "Medium", 2 }, { "Low", 1 }, { "Unknown", 0 } };
    results = results.OrderByDescending(r => severityOrder.GetValueOrDefault(r.Severity, 0)).ToList();

    return Ok(results);
}
}