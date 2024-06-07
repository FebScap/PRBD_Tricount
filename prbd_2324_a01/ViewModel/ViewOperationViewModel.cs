using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class ViewOperationViewModel : DialogViewModelBase<User, PridContext>
{
    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    private readonly Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
    }

    private readonly Operation _operation = null;
    public Operation Operation {
        get => _operation;
    }

    public ICommand DeleteCommand { get; set; }
    public ICommand AddSaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public string WindowTitle => Operation == null ? "Add Operation" : "Edit Operation";
    public string EditVisibility => Operation != null ? "Visible" : "Collapsed";
    public string AddSaveButtonContent => Operation == null ? "Add" : "Save";

    public ViewOperationViewModel(Tricount tricount) {
        _tricount = tricount;
        OnRefreshData();
        RegisterCommands();

    }

    public ViewOperationViewModel(Operation operation) {
        _operation = operation;
        _tricount = Tricount.GetTricountById(operation.Tricount);
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
       Users = new ObservableCollection<User>(Tricount.GetAllUsers());        
    }
}

