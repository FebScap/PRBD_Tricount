using Microsoft.Identity.Client;
using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using SQLitePCL;
using System.Text;
using System.Windows.Navigation;

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
    public string Amount => string.Format("{0:0.00 €}", GetBalance(User, Tricount));
    public string NegativeVisibility => GetBalance(User, Tricount) < 0 ? "Visible" : "Collapsed";
    public string PositiveVisibility => GetBalance(User, Tricount) >= 0 ? "Visible" : "Collapsed";
    public string isSelfUserVisibility => User == CurrentUser ? "Visible" : "Collapsed";
    public string isNotSelfUserVisibility => User != CurrentUser ? "Visible" : "Collapsed";
    public int BorderWidth => (int) GetAmountBgWidth(Tricount, GetBalance(User, Tricount));



    public UserBalanceCardViewModel(User user, Tricount tricount) : base() {
        _user = user;
        _tricount = tricount;
    }

    public static Double GetAmountBgWidth(Tricount t, Double curentValue) {
        Double maxValue = t.CalculateBalances().MaxBy(k => k.Value).Value;
        Double division = (curentValue / maxValue) * 200;
        if (division < 0) {
            return -division;
        } else {
            return division;
        }

    }

    public static Double GetBalance(User u, Tricount t) {
        return Math.Round(t.CalculateBalances().GetValueOrDefault(u), 2);
    }
}
