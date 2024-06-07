using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Controls;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();

        Register<Tricount>(App.Messages.MSG_EDIT_TRICOUNT,
           tricount => DoEditTricount(tricount));

        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT,
           tricount => DoAddTricount());

        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT,
            tricount => DoDisplayTricount(tricount));

        Register<Tricount>(App.Messages.MSG_CLOSE_TAB,
           tricount => DoCloseTab(tricount));
    }

    private void DoCloseTab(Tricount tricount) {
        tabControl.CloseByTag(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title);
    }

    private void DoEditTricount(Tricount tricount) {
            OpenTab(tricount.Title, () => new EditTricountView(tricount));
    }
    private void DoAddTricount() {
        OpenTab("New Tricount", () => new EditTricountView());
    }

    private void DoDisplayTricount(Tricount tricount) {
        if (tricount != null)
            OpenTab(tricount.Title, () => new DisplayTricountView(tricount));

    }

    private void OpenTab(string header, Func<UserControlBase> createView) {
        var tab = tabControl.FindByTag(header);
        if (tab == null)
            tabControl.Add(createView(), header, header);
        else
            tabControl.SetFocus(tab);
    }

    private void MenuLogout_Click(object sender, System.Windows.RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}