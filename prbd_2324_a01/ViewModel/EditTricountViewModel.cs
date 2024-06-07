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

    private string _descriptionTextBox = "";

    public string DescriptionTextBox {
        get => _descriptionTextBox;
        set => SetProperty(ref _descriptionTextBox, value, () => Validate());
    }

    private Tricount _tricount;

    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Creator { get; set; }
    public DateTime CreationDate { get; set; }
    public TricountParticipantsViewModel TricountParticipants { get; set; }

    public EditTricountViewModel(Tricount tricount) : base() {
        OnRefreshData();
        _tricount = tricount;
        Title = tricount.Title;
        Description = tricount.Description; 
        Creator = User.GetUserById(tricount.Creator).FullName;
        TitleTextBox = tricount.Title;
        DescriptionTextBox = tricount.Description;
        CreationDate = tricount.CreatedAt;

        TricountParticipants = new TricountParticipantsViewModel(tricount);
        SaveCommand = new RelayCommand(SaveTricountAction, CanSave);
        CancelCommand = new RelayCommand(CancelButtonAction);
    }

    public EditTricountViewModel() : base() {
        OnRefreshData();
        Tricount tricount = new Tricount();
        Title = "<New Tricount>";
        Description = "No Description";
        Creator = CurrentUser.FullName;
        CreationDate = DateTime.Now;

        TricountParticipants = new TricountParticipantsViewModel(tricount);
        SaveCommand = new RelayCommand(SaveTricountAction, CanSave);
        CancelCommand = new RelayCommand(CancelButtonAction);
    }

    private void SaveTricountAction() {
        if (Validate()) {
            Tricount tricount;
            if (Tricount == null) {
                tricount = new Tricount(TitleTextBox, DescriptionTextBox, App.CurrentUser.Id);
            } else {
                tricount = Tricount;
                Tricount.Title = TitleTextBox;
                Tricount.Description = DescriptionTextBox;
                //Tricount.CreatedAt = Modif date ici
            }
            foreach (var p in TricountParticipants.Participants) {
                if (!tricount.Participants.Contains(p))
                    Context.Subscriptions.Add(new Subscription(p.Id, tricount.Id));
            }
            foreach (var p in Tricount.Participants) {
                if (!TricountParticipants.Participants.Contains(p)) {
                    Subscription sub = Context.Subscriptions.Find(p.Id, tricount.Id);
                    Context.Subscriptions.Remove(sub);
                }
            }
            if (Tricount == null)
                tricount.Add();
            else
                Tricount.Update();
            Context.SaveChanges();
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
            AddError(nameof (DescriptionTextBox), "Must be empty or at least 3 char");
        return !HasErrors;

    }

    public override bool Validate() {
       return ValidateTitle() && ValidateDescription();
    }

    protected override void OnRefreshData() {
        
    }
}