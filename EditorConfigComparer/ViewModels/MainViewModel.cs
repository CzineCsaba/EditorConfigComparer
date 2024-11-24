using System.Collections.ObjectModel;
using System.IO;

using EditorconfigComparer.Models;
using EditorconfigComparer.Services;

using Microsoft.Win32;

namespace EditorconfigComparer.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IEditorConfigReader _configReader;
    private readonly IEditorConfigService _configService;

    private string _leftFilePath = string.Empty;
    private string _rightFilePath = string.Empty;
    private ObservableCollection<EditorConfigRulePair> _rulePairs = new();
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

    public ObservableCollection<EditorConfigRulePair> RulePairs
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
            foreach (EditorConfigRulePair rulePair in rulePairs)
            {
                RulePairs.Add(rulePair);
            }
        }
    }
}
