using prbd_2324_a01.Model;
using prbd_2324_a01.ViewModel;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class TricountDetailView : UserControlBase
{
    private Tricount tricount;
    private bool isNew;

    public TricountDetailView(Tricount tricount, bool isNew) {
        InitializeComponent();
        
        DataContext = new TricountDetailViewModel(tricount, isNew);

        this.tricount = tricount;
        this.isNew = isNew;
    }
}