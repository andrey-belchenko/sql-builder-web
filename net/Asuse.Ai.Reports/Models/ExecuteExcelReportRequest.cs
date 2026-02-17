namespace Asuse.Ai.Reports.Models
{
    public class ExecuteExcelReportRequest
    {
        public string ReportName { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
        public string FileId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
