using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System.Text;

namespace prbd_2324_a01.ViewModel;

public class OperationCardViewModel : ViewModelBase<User, PridContext> {
    private readonly Operation _operation;

    public Operation Operation {
        get => _operation;
    }

    public string Title => Operation.Title;
    public string Amount => string.Format("{0:0.00 €}", Math.Round(Operation.Amount, 2));
    public string PaidBy => "Paid by " + Operation.GetInitiator().FullName;
    public string Date => Operation.OperationDate.ToShortDateString();


    public OperationCardViewModel(Operation operation) : base() {
        _operation = operation;
    }
}
