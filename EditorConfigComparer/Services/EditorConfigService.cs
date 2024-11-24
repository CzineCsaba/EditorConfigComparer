using EditorconfigComparer.Models;

namespace EditorconfigComparer.Services
{
    internal class EditorConfigService : IEditorConfigService
    {
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
    }
}