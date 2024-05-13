using System;
using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class LoginViewModel : ViewModelBase<User, PridContext>
{
    private string _pseudo;

    public string Pseudo {
        get => _pseudo;
        set => SetProperty(ref _pseudo, value, () => Validate());
    }

    public override bool Validate() {
        ClearErrors();

        var member = Context.Users.Find(Pseudo);

        if (string.IsNullOrEmpty(Pseudo))
            AddError(nameof(Pseudo), "required");
        else if (Pseudo.Length < 3)
            AddError(nameof(Pseudo), "length must be >= 3");
        else if (member == null)
            AddError(nameof(Pseudo), "does not exist");

        return !HasErrors;
    }

    protected override void OnRefreshData() {
    }
}
