namespace EditorconfigComparer.Models
{
    public class EditorConfigScopedRule
    {
        public EditorConfigScopedRule(string ruleId, IList<string> scopes)
        {
            RuleId = ruleId;
            Scopes = scopes.Order().ToList();

            FormattedScopes = $"{string.Join(",", Scopes)}";
            Id = $"{RuleId}-{FormattedScopes}";
        }

        public string Id { get; }
        public string RuleId { get; }
        public IList<string> Scopes { get; }

        public string FormattedScopes { get; }

        public string Value { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EditorConfigScopedRule other)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return Id == other.Id && Value == other.Value && Severity == other.Severity;
        }

        public bool HasSameScopes(EditorConfigScopedRule otherScopedRule)
        {
            return Scopes.SequenceEqual(otherScopedRule.Scopes);
        }

        public bool HasSameScopes(IList<string> otherScopes)
        {
            IList<string> orderedScopes = otherScopes.Order().ToList();
            return Scopes.SequenceEqual(orderedScopes);
        }
    }
}
