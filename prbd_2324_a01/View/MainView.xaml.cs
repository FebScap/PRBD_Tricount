using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Controls;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();

        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT,
           tricount => DoDisplayTricount(tricount, true));

        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT,
            tricount => DoDisplayTricount(tricount, false));

        Register<Tricount>(App.Messages.MSG_CLOSE_TAB,
           tricount => DoCloseTab(tricount));
    }

    private void DoCloseTab(Tricount tricount) {
        tabControl.CloseByTag(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title);
    }

    private void DoDisplayTricount(Tricount tricount, bool isNew) {
        if (tricount != null)
            OpenTab(isNew ? "<New Tricount>" : tricount.Title, () => new TricountDetailView(tricount, isNew));
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