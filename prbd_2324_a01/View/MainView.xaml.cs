﻿using Azure;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Windows.Controls;

namespace prbd_2324_a01.View;

public partial class MainView : WindowBase
{
    public MainView() {
        InitializeComponent();

        Register<Tricount>(App.Messages.MSG_EDIT_TRICOUNT,
           tricount => DoEditTricount(tricount, tricount.IsNew));

        Register<Tricount>(App.Messages.MSG_DELETE_TRICOUNT,
        tricount => DoDeleteTricount(tricount));

        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT,
            tricount => DoDisplayTricount(tricount));

        Register<Tricount>(App.Messages.MSG_CLOSE_TAB,
           tricount => DoCloseTab(tricount));

        Register<Tricount>(App.Messages.MSG_ADD_OPERATION,
        tricount => DoAddOperation(tricount));

        Register<Model.Operation>(App.Messages.MSG_EDIT_OPERATION,
         operation => DoEditOperation(operation));
    }

    private void DoCloseTab(Tricount tricount) {
        tabControl.CloseByTag(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title);
    }

    private void DoEditTricount(Tricount tricount, bool isNew) {
        if (tricount != null)
            OpenTab(isNew ? "<New Tricount>" : tricount.Title, () => new EditTricountView(tricount));
    }

    private void DoDeleteTricount(Tricount tricount) {
        throw new NotImplementedException();
    }

    private void DoDisplayTricount(Tricount tricount) {
        if (tricount != null)
            OpenTab(tricount.Title, () => new DisplayTricountView(tricount));

    }

    private void DoAddOperation(Tricount tricount) {
        new AddOperationView(tricount).ShowDialog();
    }
    private void DoEditOperation(Model.Operation operation) {
        new EditOperationView(operation).ShowDialog();
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