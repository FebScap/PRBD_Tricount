using PRBD_Framework;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace prbd_2324_a01.View;

public partial class UserWeightSelectorView : UserControlBase {
    public UserWeightSelectorView() {
        InitializeComponent();
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }
}

