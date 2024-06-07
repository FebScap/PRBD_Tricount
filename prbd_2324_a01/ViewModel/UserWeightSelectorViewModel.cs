using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Input;
using Operation = prbd_2324_a01.Model.Operation;

namespace prbd_2324_a01.ViewModel;

public class UserWeightSelectorViewModel : ViewModelBase<User, PridContext> {
    private User user;
    private Tricount tricount;

    private Double _amount;
    public Double Amount {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }

    private int _weight;
    public int Weight {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }

    public string Name => user.FullName;

    public ICommand UpCommand { get; set; }
    public ICommand DownCommand { get; set; }

    public UserWeightSelectorViewModel(User u, Tricount tricount) {
        this.user = u;
        this.tricount = tricount;

        OnRefreshData();

        UpCommand = new RelayCommand(() => {
            Weight++;
            OnRefreshData();
        });
        DownCommand = new RelayCommand(() => {
            if (Weight > 0) {
                Weight--;
                OnRefreshData();
            }
        });
    }

    private Double CalculateBalance() {
        return 3.1654;
        // (MyWeight/TotalWeight)*OperationAmount
    }

    protected override void OnRefreshData() {
        Amount = CalculateBalance();
    }
}
