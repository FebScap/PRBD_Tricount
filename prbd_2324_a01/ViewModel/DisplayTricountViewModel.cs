using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel; 

public class DisplayTricountViewModel : ViewModelBase<User, PridContext>
{
    private ObservableCollection<OperationCardViewModel> _operations;
    public ObservableCollection<OperationCardViewModel> Operations {
        get => _operations;
        set => SetProperty(ref _operations, value);
    }
    private ObservableCollection<UserBalanceCardViewModel> _users;
    public ObservableCollection<UserBalanceCardViewModel> Users {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    private readonly Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
    }
    
    public ICommand DisplayEditOperation { get; set; }
    public ICommand AddOperationCommand { get; set; }
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public string Title => Tricount.Title;
    public string Description => StringBuilders.GetDescription(Tricount);
    public string Creator => Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => Tricount.CreatedAt.ToShortDateString();

    public DisplayTricountViewModel(Tricount tricount) {
        _tricount = tricount;

        OnRefreshData();

        DisplayEditOperation = new RelayCommand<OperationCardViewModel>(vm => {
            NotifyColleagues(App.Messages.MSG_EDIT_OPERATION, vm.Operation);
        });

        AddOperationCommand = new RelayCommand<Tricount>(tricount => {
            NotifyColleagues(App.Messages.MSG_ADD_OPERATION, tricount);
        });

        EditCommand = new RelayCommand<Tricount>(tricount => {
            NotifyColleagues(App.Messages.MSG_EDIT_TRICOUNT, tricount);
        });

        DeleteCommand = new RelayCommand<Tricount>(tricount => {
            NotifyColleagues(App.Messages.MSG_DELETE_TRICOUNT, tricount);
        });
    }

    protected override void OnRefreshData() {
        IQueryable<Operation> operations = Tricount.GetAllOperations();
        List<User> users = Tricount.GetAllUsers();

        Operations = new ObservableCollection<OperationCardViewModel>(operations.Select(o => new OperationCardViewModel(o)));
        Users = new ObservableCollection<UserBalanceCardViewModel>(users.Select(u => new UserBalanceCardViewModel(u, Tricount)));
    }
}

