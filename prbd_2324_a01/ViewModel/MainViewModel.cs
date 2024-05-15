using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.ViewModel;

public class MainViewModel : ViewModelBase<User, PridContext>
{
    public static string Title {
        get => $"My Tricount ({CurrentUser?.Mail})";
    }

    public MainViewModel() {
    }
}