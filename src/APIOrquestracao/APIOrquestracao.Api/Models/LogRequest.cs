namespace APIOrquestracao.Api.Models
{
    public class LogRequest
    {
        public LogType LogType { get; set; }
        public string? Message { get; set; }
    }
}
