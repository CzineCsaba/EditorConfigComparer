namespace EditorconfigComparer.Models
{
    internal class EditorConfig
    {
        public string FilePath { get; set; } = string.Empty;
        public Dictionary<string, EditorConfigRule> Rules { get; set; } = new();
    }
}
