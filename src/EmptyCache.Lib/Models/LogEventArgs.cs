namespace EmptyCache.Lib.Models
{
    public class LogEventArgs : EventArgs
    {
        public string? FilePath { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int Total { get; set; }
        public int index { get; set; }
        public int Percent => Total > 0 ? index / Total * 100 : 100;
        public override string ToString()
        {
            if (Success)
                return $"{FilePath} rimosso";
            return $"{FilePath} NON rimosso\r\n{ErrorMessage}";
        }
    }
}