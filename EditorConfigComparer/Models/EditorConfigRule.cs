using System.Diagnostics.CodeAnalysis;

namespace EditorconfigComparer.Models
{
    public class EditorConfigRule : IEquatable<EditorConfigRule>
    {
        private IList<EditorConfigScopedRule> _scopedRules = new List<EditorConfigScopedRule>();

        public EditorConfigRule(string key)
        {
            RuleId = key;
        }

        public string RuleId { get; }

        public IReadOnlyList<EditorConfigScopedRule> ScopedRules
        {
            get => (IReadOnlyList<EditorConfigScopedRule>)_scopedRules;
        }

        public bool Equals(EditorConfigRule? other)
        {
            if (null == other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return RuleId == other.RuleId && ScopedRules.SequenceEqual(other.ScopedRules);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not EditorConfigRule)
                return false;

            return Equals((EditorConfigRule)obj);
        }

        public override int GetHashCode()
        {
            return RuleId.GetHashCode();
        }

        public void AddScopedRule(EditorConfigScopedRule scopedRule)
        {
            _scopedRules.Add(scopedRule);
        }

        public bool TryGetScopedRuleWithScope(IList<string> scopes, [NotNullWhen(true)] out EditorConfigScopedRule? scopedRule)
        {
            scopedRule = ScopedRules.FirstOrDefault(sr => sr.HasSameScopes(scopes));
            return scopedRule != null;
        }
    }
}
