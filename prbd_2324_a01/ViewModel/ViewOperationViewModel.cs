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
    public DateTime CreationDate { get; set; }

    public ViewOperationViewModel(Tricount tricount) {
        Tricount = tricount;
        AmountTextBox = "0";
        Participants = Tricount.GetAllUsers();
        TotalWeight = Participants.Count;

        Users = new ObservableCollection<UserWeightSelectorViewModel>(Participants.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
        foreach (var u in Users) {
            u.doChangeTotal(TotalWeight);
            //Listener des enfants pour voir si leurs balances ont changées
            u.NotifyBalance += w => {
                TotalWeight += w;
                OnRefreshData();
            };
        }
        OnRefreshData();
        RegisterCommands();
    }

    public ViewOperationViewModel(Model.Operation operation) {
        Operation = operation;
        Tricount = Context.Tricounts.Find(operation.Tricount);
        TitleTextBox = Operation.Title;
        CreationDate = Operation.OperationDate;
        ThisUser = CurrentUser;
        Participants = Tricount.GetAllUsers();
        TotalWeight = 0;

        Users = new ObservableCollection<UserWeightSelectorViewModel>(Participants.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
        foreach (var u in Users) {
            u.doChangeTotal(TotalWeight);
            //Listener des enfants pour voir si leurs balances ont changées
            u.NotifyBalance += w => {
                TotalWeight += w;
                OnRefreshData();
            };
            u.setOperation(Operation);
        }

        AmountTextBox = Operation.Amount.ToString();
        OnRefreshData();
        RegisterCommands();
    }

    private void RegisterCommands() {
        DeleteCommand = new RelayCommand(() => {
            DialogResult = Operation;
            NotifyColleagues(App.Messages.MSG_DELETE_OPERATION, Operation);
        });
        AddSaveCommand = new RelayCommand(() => SaveOperation(), Validate);
        CancelCommand = new RelayCommand(() => {
            DialogResult = null;
        });
    }

    private bool AmountChanged() {
        try {
            Double a = Double.Parse(AmountTextBox);
            foreach (var u in Users) {
                u.doChangeAmount(a);
            }
            if (a < 0) AddError(nameof(AmountTextBox), "Must be positive");
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

    public void SaveOperation() {
        DialogResult = Operation;
        NotifyColleagues(App.Messages.MSG_SAVE_OPERATION, new Tricount());
        if (Operation != null) {
            Operation.Title = TitleTextBox;
            Operation.Amount = Double.Parse(AmountTextBox);
            Operation.OperationDate = CreationDate;
            Operation.Update();

        } else {
            Operation = new Operation();
            Operation.Title = TitleTextBox;
            Operation.Amount = Double.Parse(AmountTextBox);
            Operation.OperationDate = CreationDate;
            Operation.Add();
        }
    }

    public override bool Validate() {
        ClearErrors();
        return AmountChanged() && ValidateTitle() && ThisUser != null;
    }

    protected override void OnRefreshData() {
        foreach (var u in Users) {
            u.doChangeTotal(TotalWeight);
        }
    }

    public override void Dispose() {
        base.Dispose();
        foreach (var user in Users) {
            user.Dispose();
        }
    }
}

