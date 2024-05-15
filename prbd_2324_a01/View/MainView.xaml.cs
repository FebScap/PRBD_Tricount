using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();

        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT,
           tricount => DoDisplayTricount(tricount, true));

        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT,
            tricount => DoDisplayTricount(tricount, false));
    }

    private void DoDisplayTricount(Tricount tricount, bool isNew) {
        if (tricount != null)
            OpenTab(isNew ? "<New Tricount>" : tricount.Title, tricount.Description, () => new TricountDetailView(tricount, isNew));
    }

    private void OpenTab(string header, string tag, Func<UserControlBase> createView) {
        var tab = tabControl.FindByTag(tag);
        if (tab == null)
            tabControl.Add(createView(), header, tag);
        else
            tabControl.SetFocus(tab);
    }

    private void MenuLogout_Click(object sender, System.Windows.RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}