using Microsoft.AspNetCore.Mvc;

namespace AiLogAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/logs/summary")]
    public class LogSummaryController : ControllerBase
    {
        [HttpPost]
        public IActionResult Summary([FromBody] string[] logs)
        {
            var summary = new LogSummary();

            foreach (var line in logs)
            {
                if (line.Contains("ERROR")) summary.ErrorCount++;
                else if (line.Contains("WARNING")) summary.WarningCount++;
                else if (line.Contains("INFO")) summary.InfoCount++;
                else if (line.Contains("DEBUG")) summary.DebugCount++;
            }

            return Ok(summary);
        }
    }

    public class LogSummary
    {
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int DebugCount { get; set; }
    }
}