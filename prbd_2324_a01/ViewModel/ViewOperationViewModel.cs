using Azure;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class ViewOperationViewModel : DialogViewModelBase<User, PridContext>
{

    private ObservableCollection<UserWeightSelectorViewModel> _users;

    public ObservableCollection<UserWeightSelectorViewModel> Users {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    private Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    private Model.Operation _operation = null;
    public Model.Operation Operation {
        get => _operation;
        set => SetProperty(ref _operation, value);
    }

    public ICommand DeleteCommand { get; set; }
    public ICommand AddSaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public string WindowTitle => Operation == null ? "Add Operation" : "Edit Operation";
    public string EditVisibility => Operation != null ? "Visible" : "Collapsed";
    public string AddSaveButtonContent => Operation == null ? "Add" : "Save";

    public ViewOperationViewModel(Tricount tricount) {
        Tricount = tricount;
        OnRefreshData();
        RegisterCommands();
    }

    public ViewOperationViewModel(Model.Operation operation) {
        _operation = operation;
        Tricount = Context.Tricounts.Find(operation.Tricount);
        OnRefreshData();
        RegisterCommands();

    }

    private void RegisterCommands() {
        DeleteCommand = new RelayCommand(() => {
            DialogResult = Operation;
            NotifyColleagues(App.Messages.MSG_DELETE_OPERATION, new Tricount());
        });
        AddSaveCommand = new RelayCommand(() => {
            DialogResult = Operation;
            NotifyColleagues(App.Messages.MSG_SAVE_OPERATION, new Tricount());
        });
        CancelCommand = new RelayCommand(() => {
            DialogResult = null;
        });
    }

    protected override void OnRefreshData() {
        List<User> users = Tricount.GetAllUsers();

        Users = new ObservableCollection<UserWeightSelectorViewModel>(users.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
    }
}

