using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Controls;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();

        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT,
           tricount => DoCreateNewTricount(tricount));

        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT,
            tricount => DoDisplayTricount(tricount, false));

        Register<Tricount>(App.Messages.MSG_CLOSE_TAB,
           tricount => DoCloseTab(tricount));
    }

    private void DoCloseTab(Tricount tricount) {
        tabControl.CloseByTag(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title);
    }

    private void DoDisplayTricount(Tricount tricount, bool isNew) {
        Console.Write(tricount.Title.ToString());
        if (tricount != null)
            OpenTab(isNew ? "<New Tricount>" : tricount.Title, () => new TricountDetailView(tricount));
    }

    private void DoCreateNewTricount(Tricount tricount) {
        OpenTab("<New Tricount>", () => new AddTricountView());
    }

    private void OpenTab(string header, Func<UserControlBase> createView) {
        var tab = tabControl.FindByTag(header);
        if (tab == null)
            tabControl.Add(createView(), header, header);
        else
            tabControl.SetFocus(tab);
        Console.Write(tab?.Tag?.ToString());
    }

    private void MenuLogout_Click(object sender, System.Windows.RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}