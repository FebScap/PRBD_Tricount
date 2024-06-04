using System.Windows;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using prbd_2324_a01.Model;
using prbd_2324_a01.ViewModel;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class SignupView : WindowBase
{
    public SignupView() {
        InitializeComponent();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}
