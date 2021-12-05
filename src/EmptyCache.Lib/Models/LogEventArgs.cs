namespace EmptyCache.Lib.Models
{
    public class LogEventArgs : EventArgs
    {
        public string? FilePath { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public override string ToString()
        {
            if (Success)
                return $"{FilePath} rimosso";
            return $"{FilePath} NON rimosso\r\n{ErrorMessage}";
        }
    }
}