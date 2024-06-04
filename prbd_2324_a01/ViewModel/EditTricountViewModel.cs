using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class EditTricountViewModel : ViewModelBase<User, PridContext>
{

    public ICommand SaveCommand { get; set; }
    private string _titleTextBox;

    public string TitleTextBox {
        get => _titleTextBox;
        set => SetProperty(ref _titleTextBox, value, () => Validate());
    }

    private string _descriptionTextBox;

    public string DescriptionTextBox {
        get => _descriptionTextBox;
        set => SetProperty(ref _descriptionTextBox, value, () => Validate());
    }

    private readonly Tricount _tricount;
    private readonly bool _isNew;

    public Tricount Tricount {
        get => _tricount;
    }

    public bool IsNew {
        get => _isNew;
    }

    public string Title => IsNew ? "<New Tricount>" : Tricount.Title;
    public string Description => StringBuilders.GetDescription(Tricount);
    public string Creator => IsNew ? CurrentUser.FullName : Context.Users.Find(Tricount.Creator).FullName;
    public string CreationDate => IsNew ? DateTime.Now.ToShortDateString() : Tricount.CreatedAt.ToShortDateString();

    public EditTricountViewModel(Tricount tricount, bool isNew) : base() {
        _tricount = tricount;
        _isNew = isNew;
        SaveCommand = new RelayCommand(SaveTricountAction, CanSave);
    }

    private void SaveTricountAction() {
        if (Validate()) {
            var tricount = new Tricount(TitleTextBox, DescriptionTextBox, App.CurrentUser.Id);
            tricount.Add();
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, tricount);
        }
    }

    private bool CanSave() {
        return !string.IsNullOrEmpty(TitleTextBox) &&
               !HasErrors;
    }

    public bool ValidateTitle() {
        ClearErrors();

        if (string.IsNullOrEmpty(TitleTextBox))
            AddError(nameof(TitleTextBox), "required");
        else if (TitleTextBox.Length < 3)
            AddError(nameof(TitleTextBox), "length minimum is 3");
        else if (!CurrentUser.IsTitleUnique(TitleTextBox))
            AddError(nameof(TitleTextBox), "Must be unique per user");

        return !HasErrors;
    }

    public bool ValidateDescription() {
        ClearErrors();

        if (DescriptionTextBox.Length == 0 || DescriptionTextBox.Length < 3)
            AddError(nameof (DescriptionTextBox), "Must be empty or at least 3 char");
        return !HasErrors;

    }

    public override bool Validate() {
       return ValidateTitle() && ValidateDescription();
    }
}