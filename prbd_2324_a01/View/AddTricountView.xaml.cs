using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class AddTricountView : UserControlBase
{

    public AddTricountView () {

        InitializeComponent();
    }

    private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_CLOSE_TAB);
    }
}