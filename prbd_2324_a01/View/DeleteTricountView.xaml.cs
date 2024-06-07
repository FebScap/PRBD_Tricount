using prbd_2324_a01.Model;
using PRBD_Framework;
using prbd_2324_a01.ViewModel;



namespace prbd_2324_a01.View
{
    public partial class DeleteTricountView : DialogWindowBase
    {
        public DeleteTricountView(Tricount tricount) {
            InitializeComponent();
            DataContext = new DeleteTricountViewModel(tricount);
        }
    }
}
