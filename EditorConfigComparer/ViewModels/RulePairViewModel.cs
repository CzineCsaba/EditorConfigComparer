using EditorConfigComparer.Models;

namespace EditorConfigComparer.ViewModels;

public class RulePairViewModel : ViewModelBase
{
    private readonly EditorConfigRulePair _rulePair;
    private bool _isLeftSelected;
    private bool _isRightSelected;

    public RulePairViewModel(EditorConfigRulePair rulePair)
    {
        _rulePair = rulePair;
    }

    public EditorConfigRulePair RulePair => _rulePair;

    public EditorConfigRule? LeftRule => _rulePair.LeftRule;

    public EditorConfigRule? RightRule => _rulePair.RightRule;

    public bool IsLeftSelected
    {
        get => _isLeftSelected;
        set
        {
            if (_isLeftSelected != value)
            {
                _isLeftSelected = value;
                RaisePropertyChanged();
                if (AreEqual)
                {
                    IsRightSelected = value;
                }
            }
        }
    }

    public bool IsRightSelected
    {
        get => _isRightSelected;
        set
        {
            if (_isRightSelected != value)
            {
                _isRightSelected = value;
                RaisePropertyChanged();
                if (AreEqual)
                {
                    IsLeftSelected = value;
                }
            }
        }
    }

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
