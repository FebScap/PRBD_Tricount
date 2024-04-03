using System;
using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class LoginViewModel : ViewModelBase<User, PridContext>
{
    protected override void OnRefreshData() {
    }
}
