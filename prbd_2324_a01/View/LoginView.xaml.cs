using System.Windows;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class LoginView : WindowBase
{
    public LoginView() {
        InitializeComponent();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
        Close();
    }
}
