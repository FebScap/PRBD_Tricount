using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Text;

namespace prbd_2324_a01.ViewModel;

public class TricountCardViewModel : ViewModelBase<User, PridContext> {
    private readonly Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
    }

    public string BgColor => GetBalanceColor();
    public string Title => Tricount.Title;
    public string Description => GetDescription();
    public string Creator => Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => Tricount.CreatedAt.ToShortDateString();
    public string LastOperation => GetLastOperationDate();
    public string FriendsNumber => GetFriendsNumberToString();
    public string HasOperations => OperationDateVisibility();

    public string NumberOfOperations => NumberOfOperationsToString();

    public TricountCardViewModel(Tricount tricount) {
        _tricount = tricount;
    }

    private string GetBalanceColor() {
        if (Tricount.Title.Contains("Vacances")) {
            return "LightPink";
        } else if (Tricount.Title.Contains("Resto")) {
            return "DarkSeaGreen";
        }
        return "LightGray";
    }

    private string GetDescription() {
        if (Tricount.Description == null) {
            return "No Description";
        }
        return Tricount.Description;
    }

    private string GetFriendsNumberToString() {
        int number = Subscription.GetAllUserByTricountIdExeptCurent(Tricount.Id, CurrentUser).Count();
        if (number == 0) {
            return "no friend";
        } else if (number == 1) {
            return "1 friend";
        } else {
            return number + " friends";
        }

    }

    private string OperationDateVisibility() {
        if (Operation.GetAllByTricountId(Tricount.Id).Any()) return "Visible";
        return "Collapsed";
    }

    private string NumberOfOperationsToString() {
        int number = Operation.GetAllByTricountId(Tricount.Id).Count();
        if (number == 0) {
            return "No operation";
        } else if (number == 1) {
            return "1 operation";
        } else {
            return number + " operations";
        }
    }

    private string GetLastOperationDate() {
        if (Operation.GetLastOperationByTricountId(Tricount.Id) == null) return null;  
        return Operation.GetLastOperationByTricountId(Tricount.Id).OperationDate.ToShortDateString();
    }

}
