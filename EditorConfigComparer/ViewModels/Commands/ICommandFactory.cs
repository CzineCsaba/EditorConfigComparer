namespace EditorConfigComparer.ViewModels.Commands
{
    internal interface ICommandFactory
    {
        RelayCommand CreateOrGet();
    }
}