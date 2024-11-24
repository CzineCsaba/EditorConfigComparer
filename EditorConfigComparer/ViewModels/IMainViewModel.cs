using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EditorConfigComparer.ViewModels
{
    internal interface IMainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<RulePairViewModel> RulePairs { get; }
    }
}