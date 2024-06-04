using prbd_2324_a01.Model;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class TricountDetailView : UserControlBase
{
    private Tricount tricount;

    public TricountDetailView(Tricount tricount) {
        InitializeComponent();

        this.tricount = tricount;
    }
}