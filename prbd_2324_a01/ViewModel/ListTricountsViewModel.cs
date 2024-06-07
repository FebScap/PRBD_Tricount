using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;

namespace prbd_2324_a01.ViewModel;

public class ListTricountsViewModel : ViewModelBase<User, PridContext>
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
    public ICommand DisplayTricountDetail { get; set; }

    public ListTricountsViewModel() : base() {
        OnRefreshData();

        ClearFilter = new RelayCommand(() => Filter = "");

        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT);
        });

        DisplayTricountDetail = new RelayCommand<TricountCardViewModel>(vm => {
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, vm.Tricount);
        });

        Register<Tricount>(App.Messages.MSG_TRICOUNT_CHANGED, tricount => OnRefreshData());

        Register(App.Messages.MSG_SAVE_OPERATION,
         () => OnRefreshData());
    }

    protected override void OnRefreshData() {
        if (CurrentUser.Role != 1) {
            List<Tricount> tricounts = string.IsNullOrEmpty(Filter) ? CurrentUser.GetAllTricount() : CurrentUser.GetAllTricountFiltered(Filter);
            Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(t => new TricountCardViewModel(t)));
        } else {
            IQueryable<Tricount> tricounts = string.IsNullOrEmpty(Filter) ? Tricount.GetAll() : Tricount.GetAllFiltered(Filter);
            Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(t => new TricountCardViewModel(t)));
        }
    }
}
