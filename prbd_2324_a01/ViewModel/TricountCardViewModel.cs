using prbd_2324_a01.Model;

namespace prbd_2324_a01.ViewModel;

public class TricountCardViewModel {
    private readonly Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
    }

    public string Title => Tricount.Title;

    public TricountCardViewModel(Tricount tricount) {
        _tricount = tricount;
    }
}
