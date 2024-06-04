using prbd_2324_a01.Model;
using prbd_2324_a01.ViewModel;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class TricountDetailView : UserControlBase
{
    private Tricount tricount;

    public TricountDetailView(Tricount tricount) {
        InitializeComponent();
        
        DataContext = new TricountDetailViewModel(tricount, isNew);

        this.tricount = tricount;
    }
}