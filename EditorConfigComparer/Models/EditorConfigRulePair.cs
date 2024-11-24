namespace EditorconfigComparer.Models
{
    public class EditorConfigRulePair
    {
        public EditorConfigRulePair(EditorConfigRule? leftRule, EditorConfigRule? rightRule)
        {
            LeftRule = leftRule;
            RightRule = rightRule;
        }

        public EditorConfigRule? LeftRule { get; }
        public EditorConfigRule? RightRule { get; }

        public string RuleName
        {
            get
            {
                return LeftRule?.RuleId ?? RightRule?.RuleId ?? "Undefined";
            }
        }

        public bool AreEqual
        {
            get
            {
                return LeftRule == null && RightRule == null || LeftRule != null && LeftRule.Equals(RightRule);
            }
        }
    }
}
