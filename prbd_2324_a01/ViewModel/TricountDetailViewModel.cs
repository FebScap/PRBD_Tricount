using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class TricountDetailViewModel : ViewModelBase<User, PridContext>
{
    private readonly Tricount _tricount;
    private readonly bool _isNew;

    public Tricount Tricount {
        get => _tricount;
    }

    public bool IsNew {
        get => _isNew;
    }

    public string Title => IsNew ? "<New Tricount>" : Tricount.Title;
    public string Description => StringBuilders.GetDescription(Tricount);
    public string Creator => IsNew ? CurrentUser.FullName : Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => IsNew ? DateTime.Now.ToShortDateString() : Tricount.CreatedAt.ToShortDateString();

    public TricountDetailViewModel(Tricount tricount, bool isNew) : base() {
        _tricount = tricount;
        _isNew = isNew;
    }

}