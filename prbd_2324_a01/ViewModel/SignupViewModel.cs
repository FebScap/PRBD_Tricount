using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using prbd_2324_a01.Model;
using PRBD_Framework;
using prbd_2324_a01.Utils;

namespace prbd_2324_a01.ViewModel;

public class SignupViewModel : ViewModelBase<User, PridContext>
{
    public ICommand SignupCommand { get; set; }

    private string _mail;
    public string Mail {
        get => _mail;
        set => SetProperty(ref _mail, value, () => Validate());
    }

    private string _fullname;
    public string Fullname {
        get => _fullname;
        set => SetProperty(ref _fullname, value, () => ValidateFullname());
    }

    private string _password;
    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => ValidatePassword());
    }

    private string _passwordConfirm;
    public string PasswordConfirm {
        get => _passwordConfirm;
        set => SetProperty(ref _passwordConfirm, value, () => ValidatePasswordConfirm());
    }

    public SignupViewModel() {
        SignupCommand = new RelayCommand(SignupAction, CanSignup);
    }

    private void SignupAction() {
        if (Validate() && ValidateFullname() && ValidatePassword() && ValidatePasswordConfirm()) {
            var user = new User(Mail, Password, Fullname, 0);
            user.Add();
            NotifyColleagues(App.Messages.MSG_LOGIN, user);
        }
    }

    private bool CanSignup() {
        return !string.IsNullOrEmpty(Mail) &&
               !string.IsNullOrEmpty(Fullname) &&
               !string.IsNullOrEmpty(Password) &&
               !string.IsNullOrEmpty(PasswordConfirm) &&
               !HasErrors;
    }

    public override bool Validate() {
        ClearErrors();

        var user = Context.Users.FirstOrDefault(user => user.Mail == Mail);

        if (string.IsNullOrEmpty(Mail))
            AddError(nameof(Mail), "required");
        else if (!Validations.Mail(Mail))
            AddError(nameof(Mail), "must be valid");
        else if (user != null)
            AddError(nameof(Mail), "does already exist");

        return !HasErrors;
    }

    public bool ValidateFullname() {
        ClearErrors();

        if (string.IsNullOrEmpty(Fullname))
            AddError(nameof(Fullname), "required");
        else if (Fullname.Length < 3)
            AddError(nameof(Fullname), "length minimum is 3");

        return !HasErrors;
    }

    public bool ValidatePassword() {
        ClearErrors();

        if (string.IsNullOrEmpty(Password))
            AddError(nameof(Password), "required");
        else if (Password.Length < 8)
            AddError(nameof(Password), "length minimum is 8");
        else if (!Password.Any(char.IsUpper))
            AddError(nameof(Password), "Must contain one uppercase");
        else if (!Password.Any(char.IsNumber))
            AddError(nameof(Password), "Must contain one number");

        return !HasErrors;
    }

    public bool ValidatePasswordConfirm() {
        ClearErrors();

        if (string.IsNullOrEmpty(PasswordConfirm))
            AddError(nameof(PasswordConfirm), "required");
        else if (PasswordConfirm != Password)
            AddError(nameof(PasswordConfirm), "Password does not match");

        return !HasErrors;
    }

    protected override void OnRefreshData() {
    }
}
