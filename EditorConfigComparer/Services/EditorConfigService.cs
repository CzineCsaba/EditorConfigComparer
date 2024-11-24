using System.IO;

using EditorConfigComparer.Models;

namespace EditorConfigComparer.Services;

internal class EditorConfigService : IEditorConfigService
{
    public static IEditorConfigService Instance { get; private set; } = new EditorConfigService();

    private EditorConfigService()
    {
    }

    public IEnumerable<EditorConfigRulePair> CreatePairs(EditorConfig leftConfig, EditorConfig rightConfig)
    {
        foreach (EditorConfigRule leftRule in leftConfig.Rules.Values)
        {
            rightConfig.Rules.TryGetValue(leftRule.RuleId, out EditorConfigRule? rightRule);
            yield return new EditorConfigRulePair(leftRule, rightRule);
        }

        foreach (EditorConfigRule rightRule in rightConfig.Rules.Values)
        {
            if (leftConfig.Rules.ContainsKey(rightRule.RuleId))
            {
                continue;
            }

            yield return new EditorConfigRulePair(null, rightRule);
        }
    }

    public void Export(IEnumerable<EditorConfigRule> editorConfigRules, string filePath)
    {
        if (File.Exists(filePath))
        {
            throw new IOException(filePath + " already exists");
        }

        Dictionary<string, IList<EditorConfigScopedRule>> rulesMappedToScopes =
            new Dictionary<string, IList<EditorConfigScopedRule>>();

        foreach (var rule in editorConfigRules)
        {
            foreach(var scopedRule in rule.ScopedRules)
            {
                if (rulesMappedToScopes.TryGetValue(scopedRule.FormattedScopes, out IList<EditorConfigScopedRule>? scopedRules))
                {
                    scopedRules.Add(scopedRule);
                }
                else
                {
                    scopedRules = [ scopedRule ];
                    rulesMappedToScopes[scopedRule.FormattedScopes] = scopedRules;
                }
            }
        }

        List<string> editorConfigLines = new List<string>();
        foreach(KeyValuePair<string, IList<EditorConfigScopedRule>> scopeAndRules in rulesMappedToScopes)
        {
            string scope = scopeAndRules.Key;
            if (!string.IsNullOrEmpty(scope))
            {
                editorConfigLines.Add($"[{scope}]");
                editorConfigLines.Add(string.Empty);
            }

            foreach(EditorConfigScopedRule rule in scopeAndRules.Value)
            {
                string ruleLine = $"{rule.RuleId} = {rule.Value}";

                if (!string.IsNullOrEmpty(rule.Severity))
                    ruleLine += $":{rule.Severity}";

                editorConfigLines.Add(ruleLine);
            }

            editorConfigLines.Add(string.Empty);
        }

        File.WriteAllLines(filePath, editorConfigLines);
    }
}