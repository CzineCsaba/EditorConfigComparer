namespace EditorConfigComparer.Models;

public class EditorConfigRulePair
{
    public EditorConfigRulePair(EditorConfigRule? leftRule, EditorConfigRule? rightRule)
    {
        LeftRule = leftRule;
        RightRule = rightRule;
    }

    public EditorConfigRule? LeftRule { get; }
    public EditorConfigRule? RightRule { get; }
}
