using prbd_2324_a01.Model;
using prbd_2324_a01.ViewModel;
using PRBD_Framework;

namespace prbd_2324_a01.View;

public partial class EditTricountView : UserControlBase
{
    private Tricount tricount;

    public EditTricountView(Tricount tricount) {
        InitializeComponent();
        
        DataContext = new EditTricountViewModel(tricount, true);

        this.tricount = tricount;
    }
}