using Microsoft.Extensions.Options;
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

    private List<User> _participants;

    public List<User> Participants {
        get => _participants;
        set => SetProperty(ref _participants, value);
    }
    private int _totalWeight;

    public int TotalWeight {
        get => _totalWeight;
        set => SetProperty(ref _totalWeight, value);
    }

    private string _titleTextBox;
    public string TitleTextBox {
        get => _titleTextBox;
        set => SetProperty(ref _titleTextBox, value, () => Validate());
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

    public DateTime CreationDate { get; set; }

    private string _amountTextBox = "";

    public string AmountTextBox {
        get => _amountTextBox;
        set => SetProperty(ref _amountTextBox, value, () => Validate());
    }

    private User _thisUser;
    public User ThisUser {
        get => _thisUser;
        set => SetProperty(ref _thisUser, value, () => Validate());
    }

    public ICommand DeleteCommand { get; set; }
    public ICommand AddSaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public string WindowTitle => Operation == null ? "Add Operation" : "Edit Operation";
    public string EditVisibility => Operation != null ? "Visible" : "Collapsed";
    public string AddSaveButtonContent => Operation == null ? "Add" : "Save";

    public ViewOperationViewModel(Tricount tricount) {
        Tricount = tricount;
        AmountTextBox = "0";
        OnRefreshData();
        RegisterCommands();
        foreach (var item in Tricount.GetAllUsers()) {
            DoChangeWeight(1);
        }
        Participants = Tricount.GetAllUsers();
    }

    public ViewOperationViewModel(Model.Operation operation) {
        _operation = operation;
        Tricount = Context.Tricounts.Find(operation.Tricount);
        TitleTextBox = Operation.Title;
        CreationDate = Operation.OperationDate;
        AmountTextBox = Operation.Amount.ToString();
        ThisUser = CurrentUser;
        OnRefreshData();
        RegisterCommands();
        foreach (var item in Tricount.GetAllUsers()) {
            DoChangeWeight(1);
        }
        Participants = Tricount.GetAllUsers();

    }

    private void RegisterCommands() {
        DeleteCommand = new RelayCommand(() => {
            DialogResult = Operation;
            NotifyColleagues(App.Messages.MSG_DELETE_OPERATION, new Tricount());
        });
        AddSaveCommand = new RelayCommand(() => {
            DialogResult = Operation;
            NotifyColleagues(App.Messages.MSG_SAVE_OPERATION, new Tricount());
        }, Validate);
        CancelCommand = new RelayCommand(() => {
            DialogResult = null;
        });

        Register(App.Messages.MSG_WEIGHT_INCREASED,
          () => DoChangeWeight(1));

        Register(App.Messages.MSG_WEIGHT_DECREASED,
          () => DoChangeWeight(-1));

        Register<int>(App.Messages.MSG_WEIGHT_REMOVED,
          (x) => DoChangeWeight(-x));
    }

    private void DoChangeWeight(int x) {
        TotalWeight += x;

        NotifyColleagues(App.Messages.MSG_TOTALWEIGHT_CHANGED, TotalWeight);
    }

    private bool AmountChanged() {
        try {
            NotifyColleagues(App.Messages.MSG_OPERATION_AMOUNT_CHANGED, Double.Parse(AmountTextBox));
            Console.WriteLine(AmountTextBox);
            if (Double.Parse(AmountTextBox) < 0) AddError(nameof(AmountTextBox), "Must be positive");
        } catch (FormatException) {
                AddError(nameof(AmountTextBox), "Must be a number");
        }

        return !HasErrors;

    }

    public bool ValidateTitle() {
        if (string.IsNullOrEmpty(TitleTextBox))
            AddError(nameof(TitleTextBox), "required");
        else if (TitleTextBox.Length < 3)
            AddError(nameof(TitleTextBox), "length minimum is 3");

        return !HasErrors;
    }

    public override bool Validate() {
        ClearErrors();
        return AmountChanged() && ValidateTitle() && ThisUser != null;
    }

    protected override void OnRefreshData() {
        List<User> users = Tricount.GetAllUsers();

        Users = new ObservableCollection<UserWeightSelectorViewModel>(users.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
    }

}

