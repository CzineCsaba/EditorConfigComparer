using System.Collections.ObjectModel;
using System.IO;

using EditorConfigComparer.Models;
using EditorConfigComparer.Services;

using Microsoft.Win32;

namespace EditorConfigComparer.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IEditorConfigReader _configReader;
    private readonly IEditorConfigService _configService;

    private string _leftFilePath = string.Empty;
    private string _rightFilePath = string.Empty;
    private ObservableCollection<RulePairViewModel> _rulePairs = new();
    private EditorConfig? _leftEditorConfig;
    private EditorConfig? _rightEditorConfig;

    public MainViewModel()
    {
        _configReader = new EditorConfigReader();
        _configService = new EditorConfigService();
    }

    public string LeftFilePath
    {
        get => _leftFilePath;
        set
        {
            _leftFilePath = value;
            RaisePropertyChanged();
        }
    }

    public string RightFilePath
    {
        get => _rightFilePath;
        set
        {
            _rightFilePath = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<RulePairViewModel> RulePairs
    {
        get => _rulePairs;
        private set
        {
            _rulePairs = value;
            RaisePropertyChanged();
        }
    }

    public void ProcessFiles()
    {
        EditorConfig leftConfig = _configReader.Read(LeftFilePath);
        EditorConfig rightConfig = _configReader.Read(RightFilePath);

        UpdateRulePairs();
    }

    internal void LoadLeftFile()
    {
        if (string.IsNullOrWhiteSpace(LeftFilePath))
            SelectFile(newPath => LeftFilePath = newPath);

        if (File.Exists(LeftFilePath))
        {
            _leftEditorConfig = _configReader.Read(LeftFilePath);
            UpdateRulePairs();
        }
    }

    internal void LoadRightFile()
    {
        if (string.IsNullOrWhiteSpace(RightFilePath))
            SelectFile(newPath => RightFilePath = newPath);

        if (File.Exists(RightFilePath))
        {
            _rightEditorConfig = _configReader.Read(RightFilePath);
            UpdateRulePairs();
        }
    }

    public bool IsSelectAllRulesEnabled
    {
        get => RulePairs.Count > 0;
    }

    internal void SelectAllRulesOnTheLeft()
    {
        foreach (var rulePair in RulePairs)
        {
            rulePair.IsRightSelected = false;
            if (rulePair.LeftRule != null)
            {
                rulePair.IsLeftSelected = true;
            }
        }
    }

    internal void SelectAllRulesOnTheRight()
    {
        foreach (var rulePair in RulePairs)
        {
            rulePair.IsLeftSelected = false;
            if (rulePair.RightRule != null)
            {
                rulePair.IsRightSelected = true;
            }
        }
    }

    private void SelectFile(Action<string> setFilePath)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Multiselect = false;
        dialog.Title = "Select editorconfig file";
        bool? dialogResult = dialog.ShowDialog();

        if (dialogResult == true)
        {
            setFilePath(dialog.FileName);
        }
    }

    private void UpdateRulePairs()
    {
        if (_leftEditorConfig != null && _rightEditorConfig != null)
        {
            RulePairs.Clear();

            IEnumerable<EditorConfigRulePair> rulePairs =
                _configService.CreatePairs(_leftEditorConfig, _rightEditorConfig);
            foreach (RulePairViewModel rulePair in rulePairs.Select(rp => new RulePairViewModel(rp)))
            {
                RulePairs.Add(rulePair);
            }

            RaisePropertyChanged(nameof(IsSelectAllRulesEnabled));
        }
    }
}
