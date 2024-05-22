using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using prbd_2324_a01.Model;
using PRBD_Framework;
using prbd_2324_a01.Utils;

namespace prbd_2324_a01.ViewModel;

public class SignupViewModel : ViewModelBase<User, PridContext>
{
    public ICommand LoginCommand { get; set; }
    
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

    public SignupViewModel() {
        LoginCommand = new RelayCommand(LoginAction,
            () => { return _mail != null && _password != null && !HasErrors; });

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
        else if (!Validations.Mail(Mail))
            AddError(nameof(Mail), "must be valid");
        else if (user == null)
            AddError(nameof(Mail), "does not exist");
        else {
            if (string.IsNullOrEmpty(Password))
                AddError(nameof(Password), "required");
        }

        return !HasErrors;
    }

    protected override void OnRefreshData() {
    }
}
