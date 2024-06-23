using Microsoft.IdentityModel.Tokens;
using prbd_2324_a01.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_2324_a01.ViewModel
{
    public class TricountParticipantsViewModel : ViewModelBase<User, PridContext>
    {
        private Tricount _tricount;
        public Tricount Tricount {
            get => _tricount;
            set => _tricount = value;
        }

        private bool _isNew;
        public bool IsNew {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        public TricountParticipantsViewModel(Tricount tricount, bool isNew) {
            Tricount = tricount;
            IsNew = isNew;
            LoadParticipants();
            LoadAvailableUsers();
            if (IsNew) {
                Participants.Add(CurrentUser);
                AvailableUsers.Remove(CurrentUser);
                OwnerId = CurrentUser.Id;
                NotifyColleagues(App.Messages.MSG_PARTICIPANTS_CHANGED, Participants);
            } else {
                OwnerId = Tricount.Creator;
            }
            DeleteCommand = new RelayCommand<User>(DeleteParticipant, CanDeleteParticipant);
            AddCommand = new RelayCommand(AddParticipant, CanAddParticipant);
            AddMyselfCommand = new RelayCommand(AddMyself, CanAddMyself);
            AddEverybodyCommand = new RelayCommand(AddEverybody, CanAddEverybody);
        }

        private ObservableCollection<User> _participants;
        public ObservableCollection<User> Participants {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        private ObservableCollection<User> _availableUsers;
        public ObservableCollection<User> AvailableUsers {
            get => _availableUsers;
            set => SetProperty(ref _availableUsers, value);
        }

        private User _selectedUser;
        public User SelectedUser {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        private int _ownerId;
        public int OwnerId {
            get => _ownerId;
            set => SetProperty(ref _ownerId, value);
        }

        public ICommand DeleteCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand AddMyselfCommand { get; private set; }
        public ICommand AddEverybodyCommand { get; private set; }

        private void LoadParticipants() {
            Participants = new ObservableCollection<User>(Tricount.Participants);
        }

        private void LoadAvailableUsers() {
            var participantIds = Participants.Select(p => p.Id).ToList();
            AvailableUsers = new ObservableCollection<User>(
                Context.Users.Where(u => !participantIds.Contains(u.Id)).ToList()
            );
        }

        private void DeleteParticipant(User participant) {
            if (participant.Id != OwnerId) {
                Participants.Remove(participant);
                AvailableUsers.Add(participant);
                NotifyColleagues(App.Messages.MSG_PARTICIPANTS_CHANGED, Participants);
            }
        }
        private bool CanDeleteParticipant(User participant) {
            return participant.Id != OwnerId && Tricount.GetMyExpenses(participant.Id) == 0;
        }

        private void AddParticipant() {
            if (SelectedUser != null) {
                Participants.Add(SelectedUser);
                AvailableUsers.Remove(SelectedUser);
                SelectedUser = null;
            }
            NotifyColleagues(App.Messages.MSG_PARTICIPANTS_CHANGED, Participants);
        }
        private bool CanAddParticipant() {
            return SelectedUser != null;
        }

        private void AddMyself() {
            var currentUser = Context.Users.Find(CurrentUser.Id);
            if (currentUser != null && !Participants.Contains(currentUser)) {
                Participants.Add(currentUser);
                AvailableUsers.Remove(currentUser);
            }
            NotifyColleagues(App.Messages.MSG_PARTICIPANTS_CHANGED, Participants);
        }

        private bool CanAddMyself() {
            return !Participants.Contains(CurrentUser);
        }

        private void AddEverybody() {
            foreach (var user in AvailableUsers.ToList()) {
                Participants.Add(user);
            }
            AvailableUsers.Clear();
            NotifyColleagues(App.Messages.MSG_PARTICIPANTS_CHANGED, Participants);

        }

        private bool CanAddEverybody() {
            return !AvailableUsers.IsNullOrEmpty();
        }
    }
}
