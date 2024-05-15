using System;
using System.Net.Mail;
using System.Windows.Input;
using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class LoginViewModel : ViewModelBase<User, PridContext>
{
    public ICommand LoginCommand { get; set; }
    public ICommand BenoitCommand { get; set; }
    public ICommand BorisCommand { get; set; }
    public ICommand XavierCommand { get; set; }
    public ICommand AdminCommand { get; set; }

    private string _mail;

    public string Mail {
        get => _mail;
        set => SetProperty(ref _mail, value, () => Validate());
    }

    private string _password;

    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => Validate());
    }

    public LoginViewModel() {
        LoginCommand = new RelayCommand(LoginAction,
            () => { return _mail != null && _password != null && !HasErrors; });

        BenoitCommand = new RelayCommand(LogBenoit);
        BorisCommand = new RelayCommand(LogBoris);
        XavierCommand = new RelayCommand(LogXavier);
        AdminCommand = new RelayCommand(LogAdmin);
    }


    private void LogBenoit() {
        NotifyColleagues(App.Messages.MSG_LOGIN, Context.Users.Find(2));
    }
    private void LogBoris() {
        NotifyColleagues(App.Messages.MSG_LOGIN, Context.Users.Find(1));
    }
    private void LogXavier() {
        NotifyColleagues(App.Messages.MSG_LOGIN, Context.Users.Find(3));
    }
    private void LogAdmin() {
        NotifyColleagues(App.Messages.MSG_LOGIN, Context.Users.Find(5));
    }

    private void LoginAction() {
        if (Validate()) {
            var user = Context.Users.Where(user => user.Mail.Equals(Mail)).FirstOrDefault();
            if (user != null && !SecretHasher.Verify(Password, user.HashedPassword)) {
                AddError(nameof(Password), "wrong password");
            } else {
                NotifyColleagues(App.Messages.MSG_LOGIN, user);
            }
        }
    }

    public override bool Validate() {
        ClearErrors();

        var user = Context.Users.Where(user => user.Mail.Equals(Mail)).FirstOrDefault();


        if (string.IsNullOrEmpty(Mail))
            AddError(nameof(Mail), "required");
        else if (!IsValidMail(Mail))
            AddError(nameof(Mail), "must be valid");
        else if (user == null)
            AddError(nameof(Mail), "does not exist");
        else {
            if (string.IsNullOrEmpty(Password))
                AddError(nameof(Password), "required");
        }

        return !HasErrors;
    }

    private static bool IsValidMail(string email) {
        var valid = true;

        try {
            var emailAddress = new MailAddress(email);
        } catch {
            valid = false;
        }
        return valid;
    }


    protected override void OnRefreshData() {
    }
}
