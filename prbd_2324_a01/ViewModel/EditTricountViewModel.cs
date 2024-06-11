using prbd_2324_a01.Model;
using prbd_2324_a01.Utils;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel;

public class EditTricountViewModel : ViewModelBase<User, PridContext>
{

    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

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

    private DateTime _cretationDateTextBox;
    public DateTime CreationDateTextBox {
        get => _cretationDateTextBox;
        set => SetProperty(ref _cretationDateTextBox, value, () => Validate());
    }

    private Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    private bool _isNew;
    public bool IsNew {
        get => _isNew;
        set => SetProperty(ref _isNew, value);
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Creator { get; set; }
    public DateTime CreationDate { get; set; }
    public TricountParticipantsViewModel TricountParticipants { get; set; }

    public EditTricountViewModel(Tricount tricount, bool isNew) : base() {
        
        _tricount = tricount;
        IsNew = isNew;

        if(isNew) {
            Title = "<New Tricount>";
            Description = "No Description";
            Creator = CurrentUser.FullName;
            CreationDate = DateTime.Now;
            CreationDateTextBox = DateTime.Now;
        } else {
            Title = tricount.Title;
            Description = tricount.Description;
            Creator = User.GetUserById(tricount.Creator).FullName;
            CreationDate = tricount.CreatedAt;
            TitleTextBox = tricount.Title;
            DescriptionTextBox = tricount.Description;
            CreationDateTextBox = tricount.CreatedAt;
        }

        TricountParticipants = new TricountParticipantsViewModel(tricount);
        SaveCommand = new RelayCommand(SaveTricountAction, CanSave);
        CancelCommand = new RelayCommand(CancelButtonAction);

        RaisePropertyChanged();
    }

    private void SaveTricountAction() {
        if (Validate()) {
            var tricount = new Tricount(TitleTextBox, DescriptionTextBox, App.CurrentUser.Id);
            tricount.Add();
            foreach (var p in TricountParticipants.Participants) {
                Context.Subscriptions.Add(new Subscription(p.Id, tricount.Id));
                Context.SaveChanges();
            }
            RaisePropertyChanged();
            NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, tricount);
            NotifyColleagues(App.Messages.MSG_CLOSE_TAB, new Tricount());
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, tricount);
        }
    }

    private void CancelButtonAction() {
        NotifyColleagues(App.Messages.MSG_CLOSE_TAB, new Tricount());
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

        if (!string.IsNullOrEmpty(DescriptionTextBox) && DescriptionTextBox.Length < 3)
            AddError(nameof(DescriptionTextBox), "Must be empty or at least 3 char");
        
        return !HasErrors;
    }

    public bool ValidateDate() {
        ClearErrors();

        if (CreationDateTextBox > DateTime.Now)
            AddError(nameof(CreationDateTextBox), "Cannot be in the future");

        return !HasErrors;
    }

    public override bool Validate() {
       return ValidateTitle() && ValidateDescription() && ValidateDate();
    }

    protected override void OnRefreshData() {
    }
}