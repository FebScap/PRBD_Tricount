using Microsoft.Identity.Client;
using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System.Text;

namespace prbd_2324_a01.ViewModel;

public class UserBalanceCardViewModel : ViewModelBase<User, PridContext> {
    private readonly User _user;
    public User User {
        get => _user;
    }
    private readonly Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
    }


    public string UserName => User.FullName;
    public string Amount => string.Format("{0:0.00 €}", Math.Round(Tricount.GetMyExpenses(User.Id), 2));
    public string NegativeVisibility => Tricount.GetMyExpenses(User.Id) < 0 ? "Visible" : "Collapse";
    public string PositiveVisibility => Tricount.GetMyExpenses(User.Id) >= 0 ? "Visible" : "Collapse";
    public string isSelfUserVisibility => User == CurrentUser ? "Visible" : "Collapse";
    public string isNotSelfUserVisibility => User != CurrentUser ? "Visible" : "Collapse";



    public UserBalanceCardViewModel(User user, Tricount tricount) : base() {
        _user = user;
        _tricount = tricount;
    }
}
