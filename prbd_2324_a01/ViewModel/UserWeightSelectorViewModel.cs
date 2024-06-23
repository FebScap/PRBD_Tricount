using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Input;
using Operation = prbd_2324_a01.Model.Operation;

namespace prbd_2324_a01.ViewModel;

public class UserWeightSelectorViewModel : ViewModelBase<User, PridContext> {
    public event Action<int> NotifyBalance;
    public User user;
    private Tricount tricount;
    public int totalWeight { get; set; }
    public Operation Operation { get; set; }
    private Double operationAmount;

    private Double _amount;
    public Double Amount {
        get => _amount;
        set => SetProperty(ref _amount, value);
    }
    private bool _isChecked = true;
    public bool IsChecked {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    private int _weight = 1;
    public int Weight {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }

    public string Name => user.FullName;

    public ICommand CheckCommand {  get; set; }
    public ICommand UpCommand { get; set; }
    public ICommand DownCommand { get; set; }

    public UserWeightSelectorViewModel(User u, Tricount tricount) {
        this.user = u;
        this.tricount = tricount;

        OnRefreshData();

        CheckCommand = new RelayCommand(() => {
            if (IsChecked) {
                Weight = 1;
                NotifyBalance?.Invoke(1);
            } else {
                int x = Weight;
                Weight = 0;
                NotifyBalance?.Invoke(-x);
            }
           
        });

        UpCommand = new RelayCommand(() => {
            if (IsChecked) {
                Weight++;
                NotifyBalance?.Invoke(1);
            } else {
                Weight = 1;
                NotifyBalance?.Invoke(1);
                IsChecked = true;
            }
        });
        DownCommand = new RelayCommand(() => {
            if (IsChecked) {
                if (Weight > 1) {
                    Weight--;
                    NotifyBalance?.Invoke(-1);
                } else if (Weight == 1) {
                    Weight--;
                    NotifyBalance?.Invoke(-1);
                    IsChecked = false;
                }
            }
        });
    }

    public void doChangeAmount(Double x) {
        operationAmount = x;
        OnRefreshData();
    }

    public void doChangeTotal(int x) {
        totalWeight = x;
        OnRefreshData();
    }
    public void setOperation(Operation operation) {
        Operation = operation;
        Weight = Operation.getUserWeight(user.Id);
        NotifyBalance?.Invoke(Weight);
        if (Weight == 0) {
            IsChecked = false;
        }
    }

    private Double CalculateBalance() {
        if (totalWeight > 0) {
            return ((Double) Weight / (Double) totalWeight) * operationAmount;
        }
        return 0;
    }

    protected override void OnRefreshData() {
        Amount = CalculateBalance();
    }
}
