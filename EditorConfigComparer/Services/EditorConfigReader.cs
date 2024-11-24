using System.Data;
using System.IO;

using EditorConfigComparer.Models;

using EditorConfigComparer.Logging;

namespace EditorConfigComparer.Services
{
    internal class EditorConfigReader : IEditorConfigReader
    {
        private static readonly ILogger _logger = new Logger<EditorConfigReader>();

        public EditorConfig Read(string filePath)
        {
            var config = new EditorConfig();

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"EditorConfig file not found: {filePath}");
            }

            using (var reader = new StreamReader(filePath))
            {
                string? line;
                string[] scopes = [];
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    {
                        continue; // Skip comments and empty lines
                    }

                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        scopes = line.Substring(1, line.Length - 2).Split(',').Select(s => s.Trim()).ToArray();
                        continue;
                    }

                    var parts = line.Split('=', 2);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    string ruleId = parts[0].Trim();
                    bool hasValue = true;
                    string value = parts[1].Trim();
                    bool hasSeverity = false;
                    string severity = string.Empty;

                    if (ruleId.EndsWith(".severity"))
                    {
                        severity = value;
                        ruleId = ruleId.Substring(0, ruleId.Length - 9);
                        hasValue = false;
                        hasSeverity = true;
                    }

                    if (value.Contains(":"))
                    {
                        string[] valueParts = value.Split(":", 2);
                        value = valueParts[0].Trim();
                        severity = valueParts[1].Trim();
                        hasSeverity = true;
                    }

                    EditorConfigScopedRule scopedRule = CreateOrGetScopedRule(ruleId, scopes, config);

                    if (hasValue)
                        scopedRule.Value = value;

                    if (hasSeverity)
                        scopedRule.Severity = severity;
                }
            }

            MergeScopedRules(config);

            return config;
        }

        private EditorConfigScopedRule CreateOrGetScopedRule(string ruleId, string[] scopes, EditorConfig config)
        {
            EditorConfigScopedRule scopedRule = new EditorConfigScopedRule(ruleId, scopes);

            if (config.Rules.TryGetValue(ruleId, out EditorConfigRule? rule))
            {
                if (rule.TryGetScopedRuleWithScope(scopes, out EditorConfigScopedRule? scopedRuleOut))
                {
                    _logger.LogWarning($"Rule with same scope is defined multiple times. Rule: {ruleId}, Scope: {scopedRuleOut.FormattedScopes}");
                    scopedRule = scopedRuleOut;
                }
                else
                {
                    rule.AddScopedRule(scopedRule);
                }
            }
            else
            {
                EditorConfigRule newRule = new EditorConfigRule(ruleId);
                newRule.AddScopedRule(scopedRule);
                config.Rules[ruleId] = newRule;
            }

            return scopedRule;
        }

        private void MergeScopedRules(EditorConfig config)
        {
            foreach (EditorConfigRule rule in config.Rules.Values)
            {
                MergeScopedRules(rule);
            }
        }

        private void MergeScopedRules(EditorConfigRule rule)
        {
            IList<EditorConfigScopedRule> newScopedRules = new List<EditorConfigScopedRule>();
            IList<EditorConfigScopedRule> scopedRulesToRemove = new List<EditorConfigScopedRule>();
            foreach(EditorConfigScopedRule scopedRule in rule.ScopedRules)
            {
                EditorConfigScopedRule? ruleWithSameValueAndSeverity = newScopedRules.FirstOrDefault(
                    sr => sr.Value == scopedRule.Value && sr.Severity == scopedRule.Severity);
                if (ruleWithSameValueAndSeverity != null)
                {
                    foreach (string scope in scopedRule.Scopes)
                    {
                        if (!ruleWithSameValueAndSeverity.Scopes.Contains(scope))
                        {
                            ruleWithSameValueAndSeverity.AddScope(scope);
                        }
                    }

                    scopedRulesToRemove.Add(scopedRule);
                }
                else
                {
                    newScopedRules.Add(scopedRule);
                }
            }

            foreach(EditorConfigScopedRule scopedRule in scopedRulesToRemove)
            {
                rule.RemoveScopedRule(scopedRule);
            }
        }
    }
}
