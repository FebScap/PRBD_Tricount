using Microsoft.Extensions.Options;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class ViewOperationViewModel : DialogViewModelBase<User, PridContext>
{
    private ObservableCollection<UserWeightSelectorViewModel> _users;
    public ObservableCollection<UserWeightSelectorViewModel> Users {
        get => _users;
        set => SetProperty(ref _users, value, () => Validate());
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

    public string ErrorBalance { get; set; }

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

    private DateTime _creationDate;
    public DateTime CreationDate {
        get => _creationDate;
        set => SetProperty(ref _creationDate, value, () => Validate());
    }

    public ICommand DeleteCommand { get; set; }
    public ICommand AddSaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public string WindowTitle => Operation == null ? "Add Operation" : "Edit Operation";
    public string EditVisibility => Operation != null ? "Visible" : "Collapsed";
    public string AddSaveButtonContent => Operation == null ? "Add" : "Save";

    public ViewOperationViewModel(Tricount tricount) {
        Tricount = tricount;
        Participants = Tricount.GetAllUsers();
        Users = new ObservableCollection<UserWeightSelectorViewModel>(Participants.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
        AmountTextBox = "0";        
        TotalWeight = Participants.Count;
        CreationDate = DateTime.Now;

        foreach (var u in Users) {
            u.doChangeTotal(TotalWeight);
            //Listener des enfants pour voir si leurs balances ont changées
            u.NotifyBalance += w => {
                TotalWeight += w;
                Validate();
                OnRefreshData();
            };
        }
        OnRefreshData();
        RegisterCommands();
    }

    public ViewOperationViewModel(Model.Operation operation) {
        Operation = operation;
        Tricount = Context.Tricounts.Find(operation.Tricount);
        Participants = Tricount.GetAllUsers();
        Users = new ObservableCollection<UserWeightSelectorViewModel>(Participants.Select(u => new UserWeightSelectorViewModel(u, Tricount)));
        TitleTextBox = Operation.Title;
        CreationDate = Operation.OperationDate;
        ThisUser = CurrentUser;
        TotalWeight = 0;
       
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
        AddSaveCommand = new RelayCommand(() => SaveOperation(), CanSave);
        CancelCommand = new RelayCommand(() => {
            DialogResult = null;
        });
    }
    
    private bool CanSave() {
        return !string.IsNullOrEmpty(TitleTextBox) &&
               !HasErrors;
    }

    public override bool Validate() {
        ClearErrors();
        bool isValid = true;
        if (!ValidateDate()) isValid = false;
        if (!ValidateTitle()) isValid = false;
        if (!ValidateUser()) isValid = false;
        if (!ValidateAmount()) isValid = false;
        if (!ValidateBalance()) isValid = false;
        return isValid;
    }

    public bool ValidateAmount() {
        try {
            Double a = Double.Parse(AmountTextBox);
            if (a < 0.01) {
                AddError(nameof(AmountTextBox), "Must be 0,01€ at least");
            } else {
                // Verif pour le premier affichage d'une nouvelle operation
                if (Users != null) {
                    // Mise à jour du total dans les vm pour les calculs de balance
                    foreach (var u in Users) {
                        u.doChangeAmount(a);
                    }
                }
            }
        } catch (FormatException) {
            AddError(nameof(AmountTextBox), "Must be a number");
        }
        return !HasErrors;
    }

    public bool ValidateUser() {
        if (ThisUser == null)
            AddError(nameof(ThisUser), "Cannot be empty");
        return !HasErrors;
    }

    public bool ValidateTitle() {
        if (string.IsNullOrEmpty(TitleTextBox))
            AddError(nameof(TitleTextBox), "required");
        else if (TitleTextBox.Length < 3)
            AddError(nameof(TitleTextBox), "length minimum is 3");

        return !HasErrors;
    }

    public bool ValidateDate() {
        if (CreationDate > DateTime.Now)
            AddError(nameof(CreationDate), "Cannot be in the future");
        else if (CreationDate < Tricount.CreatedAt)
            AddError(nameof(CreationDate), "Must be after tricount creation date");

        return !HasErrors;
    }

    public bool ValidateBalance() {
        bool oneChecked = false;
        foreach (var user in Users) {
            if (user.IsChecked) oneChecked = true;
        }

        if (!oneChecked)
            AddError(nameof(ErrorBalance), "You must check at least one participant!");

        return !HasErrors;
    }

    public void SaveOperation() {
        DialogResult = Operation;
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
        NotifyColleagues(App.Messages.MSG_OPERATION_CHANGED);
        NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
    }

    protected override void OnRefreshData() {
        foreach (var u in Users) {
            u.doChangeTotal(TotalWeight);
            Validate();
        }
    }

    public override void Dispose() {
        base.Dispose();
        foreach (var user in Users) {
            user.Dispose();
        }
    }
}

