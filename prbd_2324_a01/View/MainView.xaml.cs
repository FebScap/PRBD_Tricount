using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();
    }

    private void MenuLogout_Click(object sender, System.Windows.RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}