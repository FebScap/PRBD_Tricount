using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class AddTricountViewModel : ViewModelBase<User, PridContext>
{

    public ICommand SaveCommand { get; set; }
    private string _title;

    public string Title {
        get => _title;
        set => SetProperty(ref _title, value, () => ValidateTitle());
    }

    private string _description;

    public string Description {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public AddTricountViewModel() {
        SaveCommand = new RelayCommand(SaveTricountAction, CanSave);
    }

    private void SaveTricountAction() {
        if (Validate()) {
            var tricount = new Tricount(Title, Description, App.CurrentUser.Id);
            tricount.AddTricount();
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, tricount);
        }
    }

    private bool CanSave() {
        return !string.IsNullOrEmpty(Title) &&
               !HasErrors;
    }


    public bool ValidateTitle() {
        ClearErrors();

        if (string.IsNullOrEmpty(Title))
            AddError(nameof(Title), "required");
        else if (Title.Length < 3)
            AddError(nameof(Title), "length minimum is 3");

        //Ajouter titre unique par user

        return !HasErrors;
    }
}