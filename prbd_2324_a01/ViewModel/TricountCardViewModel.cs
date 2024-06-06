using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System.Text;

namespace prbd_2324_a01.ViewModel;

public class TricountCardViewModel : ViewModelBase<User, PridContext> {
    private readonly Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
    }

    public string BgColor => GetBackgroundColor();
    public string BalanceColor => "Black";
    public string Title => Tricount.Title;
    public string Description => StringBuilders.GetDescription(Tricount);
    public string Creator => Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => Tricount.CreatedAt.ToShortDateString();
    public string LastOperation => GetLastOperationDate();
    public string FriendsNumber => GetFriendsNumberToString();
    public string HasOperations => OperationDateVisibility();
    public string NumberOfOperations => NumberOfOperationsToString();
    public string TotalExpenses => string.Format("{0:0.00 €}", Math.Round(Tricount.GetTotalExpenses(), 2));
    public string MyExpenses => string.Format("{0:0.00 €}",Math.Round((Tricount.GetMyExpenses(CurrentUser.Id) - GetMyBalance()), 2));
    public string MyBalance => string.Format("{0:0.00 €}", Math.Round(GetMyBalance(), 2));

    public TricountCardViewModel(Tricount tricount) : base() {
        _tricount = tricount;
    }

    private string GetBackgroundColor() {
        if (GetMyBalance() < 0) {
            return "LightPink";
        } else if (GetMyBalance() > 0) {
            return "DarkSeaGreen";
        }
        return "LightGray";
    }

    private string GetFriendsNumberToString() {
        int number = Tricount.GetAllParticipantsExpectCurrent(CurrentUser).Count;
        if (number == 0) {
            return "no friend";
        } else if (number == 1) {
            return number + " friend";
        } else {
            return number + " friends";
        }
    }

    private string OperationDateVisibility() {
        if (Tricount.GetAllOperations().Any()) return "Visible";
        return "Collapsed";
    }

    private string NumberOfOperationsToString() {
        int number = Tricount.GetAllOperations().Count();
        if (number == 0) {
            return "No operation";
        } else if (number == 1) {
            return number + " operation";
        } else {
            return number + " operations";
        }
    }

    private string GetLastOperationDate() {
        if (Tricount.GetLastOperation() == null) return null;  
        return Tricount.GetLastOperation().OperationDate.ToShortDateString();
    }

    private double GetMyBalance() {
        var balances = Tricount.CalculateBalances();

        if (balances.TryGetValue(CurrentUser.Id, out double balance)) {
            return balance;
        } else {
            return 0.00; // Retourne zéro si l'utilisateur n'a pas de solde calculé
        }
    }

}
