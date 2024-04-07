using autorent.ViewModels;

namespace autorent.Services
{
    public interface INavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        void Navigate();
    }
}