using prbd_2324_a01.Model;
using prbd_2324_a01.ViewModel;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class DisplayTricountView : UserControlBase
{
    private Tricount tricount;

    public DisplayTricountView(Tricount tricount) {
        InitializeComponent();

        DataContext = new DisplayTricountViewModel(tricount);

        this.tricount = tricount;
    }
}