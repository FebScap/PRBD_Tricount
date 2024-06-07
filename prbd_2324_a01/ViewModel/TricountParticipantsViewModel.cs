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

        public TricountParticipantsViewModel(Tricount tricount) {
            _tricount = tricount;
            LoadParticipants();
            LoadAvailableUsers();
            DeleteCommand = new RelayCommand<User>(DeleteParticipant, CanDeleteParticipant);
            AddCommand = new RelayCommand(AddParticipant, CanAddParticipant);
            AddMyselfCommand = new RelayCommand(AddMyself);
            AddEverybodyCommand = new RelayCommand(AddEverybody);
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
            Participants = new ObservableCollection<User>(_tricount.Subscriptions.Select(s => s.User));
            OwnerId = _tricount.Creator;
        }

        private void LoadAvailableUsers() {
            AvailableUsers = new ObservableCollection<User>(Context.Users.Where(u => !_tricount.Subscriptions.Any(s => s.UserId == u.Id)).ToList());
        }

        private void DeleteParticipant(User participant) {
            if (participant.Id == OwnerId) {
                return;
            }

            var subscription = _tricount.Subscriptions.FirstOrDefault(s => s.UserId == participant.Id);
            if (subscription != null) {
                _tricount.Subscriptions.Remove(subscription);
                Context.SaveChanges();
                Participants.Remove(participant);
                AvailableUsers.Add(participant);
            }
        }

        private bool CanDeleteParticipant(User participant) {
            return participant.Id != OwnerId;
        }

        private void AddParticipant() {
            if (SelectedUser != null) {
                _tricount.Subscriptions.Add(new Subscription { User = SelectedUser, Tricount = _tricount });
                Context.SaveChanges();
                Participants.Add(SelectedUser);
                AvailableUsers.Remove(SelectedUser);
                SelectedUser = null;
            }
        }

        private bool CanAddParticipant() {
            return SelectedUser != null;
        }

        private void AddMyself() {
            var currentUser = Context.Users.Find(CurrentUser.Id);
            if (currentUser != null && !Participants.Contains(currentUser)) {
                _tricount.Subscriptions.Add(new Subscription { User = currentUser, Tricount = _tricount });
                Context.SaveChanges();
                Participants.Add(currentUser);
                AvailableUsers.Remove(currentUser);
            }
        }

        private void AddEverybody() {
            foreach (var user in AvailableUsers.ToList()) {
                _tricount.Subscriptions.Add(new Subscription { User = user, Tricount = _tricount });
                Participants.Add(user);
            }
            Context.SaveChanges();
            AvailableUsers.Clear();
        }
    }
}
