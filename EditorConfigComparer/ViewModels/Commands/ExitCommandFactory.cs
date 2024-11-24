using System.Windows;

namespace EditorConfigComparer.ViewModels.Commands;

internal class ExitCommandFactory : ICommandFactory
{
    private RelayCommand? _exitCommand;

    public RelayCommand CreateOrGet()
    {
        _exitCommand ??= new RelayCommand(Execute);
        return _exitCommand;
    }

    private void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }
}
