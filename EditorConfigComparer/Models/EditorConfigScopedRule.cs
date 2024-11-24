namespace EditorConfigComparer.Models;

public class EditorConfigScopedRule
{
    private readonly IList<string> _scopes;

    public EditorConfigScopedRule(string ruleId, IList<string> scopes)
    {
        RuleId = ruleId;
        _scopes = scopes.Order().ToList();

        FormattedScopes = $"{string.Join(",", Scopes)}";
        Id = $"{RuleId}-{FormattedScopes}";
    }

    public string Id { get; private set; }
    public string RuleId { get; }
    public IReadOnlyList<string> Scopes => (IReadOnlyList<string>)_scopes;
    public string FormattedScopes { get; private set; }

    public string Value { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;

    public void AddScope(string newScope)
    {
        _scopes.Add(newScope);
        FormattedScopes = $"{string.Join(",", Scopes)}";
        Id = $"{RuleId}-{FormattedScopes}";
    }

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
