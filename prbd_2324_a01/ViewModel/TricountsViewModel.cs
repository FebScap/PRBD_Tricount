using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class TricountsViewModel : ViewModelBase<User, PridContext>
{

    private ObservableCollection<TricountCardViewModel> _tricounts;
    public ObservableCollection<TricountCardViewModel> Tricounts {
        get => _tricounts;
        set => SetProperty(ref _tricounts, value);
    }

    private string _filter;
    public string Filter {
        get => _filter;
        set => SetProperty(ref _filter, value, OnRefreshData);
    }

    public ICommand ClearFilter { get; set; }
    public ICommand NewTricount { get; set; }
    public ICommand DisplayTricountDetails { get; set; }

    public TricountsViewModel() : base() {
        OnRefreshData();

        ClearFilter = new RelayCommand(() => Filter = "");

        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricount());
        });

        DisplayTricountDetails = new RelayCommand<TricountCardViewModel>(vm => {
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, vm.Tricount);
        });
    }

    protected override void OnRefreshData() {   
        IQueryable<Tricount> tricounts = string.IsNullOrEmpty(Filter) ? Tricount.GetAllById(CurrentUser.Id) : Tricount.GetFilteredById(CurrentUser.Id, Filter);

        Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(t => new TricountCardViewModel(t)));
    }
}