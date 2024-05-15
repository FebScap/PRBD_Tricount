using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class TricountCardViewModel : ViewModelBase<User, PridContext> {
    private readonly Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
    }

    public string BgColor => GetBalanceColor();
    public string Title => Tricount.Title;
    public string Description => Tricount.Description;
    public string Creator => Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => Tricount.CreatedAt.ToString();
    public string LastOperation => "DATE";
    public string Members => "MEMEBRS";



    public TricountCardViewModel(Tricount tricount) {
        _tricount = tricount;
    }

    private string GetBalanceColor() {
        return "Red";
    }
}
