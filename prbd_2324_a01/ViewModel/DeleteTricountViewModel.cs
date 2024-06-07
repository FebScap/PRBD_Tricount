using Azure;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class DeleteTricountViewModel : DialogViewModelBase<User, PridContext>
{
    private Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    public ICommand DeleteCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public DeleteTricountViewModel(Tricount tricount) {
        Tricount = tricount;
        RegisterCommands();
    }
    private void RegisterCommands() {
        DeleteCommand = new RelayCommand(() => {
            DialogResult = Tricount;
        });
        CancelCommand = new RelayCommand(() => {
            DialogResult = null;
        });
    }
}