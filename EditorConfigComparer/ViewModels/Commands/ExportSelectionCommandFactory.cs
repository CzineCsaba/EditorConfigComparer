
using EditorConfigComparer.Models;
using EditorConfigComparer.Services;

using Microsoft.Win32;

namespace EditorConfigComparer.ViewModels.Commands;

internal class ExportSelectionCommandFactory : ICommandFactory
{
    private readonly IMainViewModel _mainViewModel;
    private RelayCommand? _command;
    private IEditorConfigService _editorConfigService;

    public ExportSelectionCommandFactory(IMainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _mainViewModel.PropertyChanged += OnPropertyChanged;
        _editorConfigService = EditorConfigService.Instance;
    }

    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (_command != null && e.PropertyName == nameof(_mainViewModel.RulePairs))
        {
            _command.RaiseCanExecuteChanged();
        }
    }

    public RelayCommand CreateOrGet()
    {
        _command ??= new RelayCommand(Execute, CanExecute);
        return _command;
    }

    public bool CanExecute(object? parameter)
    {
        return _mainViewModel.RulePairs.Any();
    }

    public void Execute(object? parameter)
    {
        string? filePath = SelectFilePath();

        if (filePath == null)
            return;

        IList<EditorConfigRule> selectedRules = ReadSelectedRules();
        _editorConfigService.Export(selectedRules, filePath);
    }

    private IList<EditorConfigRule> ReadSelectedRules()
    {
        List<EditorConfigRule> selecedRules = new List<EditorConfigRule>();

        foreach(RulePairViewModel rulePair in _mainViewModel.RulePairs)
        {
            if (rulePair.LeftRule != null && rulePair.IsLeftSelected)
            {
                selecedRules.Add(rulePair.LeftRule);

                if (rulePair.AreEqual)
                {
                    continue;
                }
            }

            if (rulePair.RightRule != null && rulePair.IsRightSelected)
            {
                selecedRules.Add(rulePair.RightRule);
            }
        }

        return selecedRules;
    }

    private string? SelectFilePath()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            FileName = ".editorconfig",
            DefaultExt = ""
        };

        bool? result = saveFileDialog.ShowDialog();

        if (result == true)
            return saveFileDialog.FileName;

        return null;
    }
}
