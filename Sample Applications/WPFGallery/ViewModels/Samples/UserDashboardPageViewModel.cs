using System.Windows.Threading;
using WPFGallery.Models;

namespace WPFGallery.ViewModels.Samples
{
    public partial class UserDashboardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> _users;

        [ObservableProperty]
        private User? _selectedUser;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private User? _editableUser;

        [ObservableProperty]
        private bool _isReadOnly = true;

        [ObservableProperty]
        private bool _isSaved;

        [ObservableProperty]
        private string _deletedName = string.Empty;
        partial void OnSelectedUserChanged(User? oldValue, User? newValue)
        {
            if (SelectedUser != null && SelectedUser != EditableUser)
            {
                EditableUser = new User(SelectedUser);
                IsReadOnly = true;
                IsEditing = false;
            }
        }

        [RelayCommand]
        private void AddUser()
        {
            Users.Add(new User("New User", ""));
            SelectedUser = Users.Last();
           
            IsReadOnly = false;
            IsEditing = true;
        }

        private CancellationTokenSource cancelTokenSource = new();
        private Task displayMessageTask = Task.CompletedTask;
        partial void OnDeletedNameChanged(string? oldValue, string newValue)
        {
            if(string.IsNullOrEmpty(newValue))
            {               
                return;
            }

            cancelTokenSource = new();          
            displayMessageTask = Task.Delay(2000, cancelTokenSource.Token).ContinueWith(_ =>
            {
                DeletedName = string.Empty;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        [RelayCommand]
        private async Task RemoveUser(User selectedUser)
        {

            cancelTokenSource.Cancel();
            await displayMessageTask;

            DeletedName = selectedUser.Name;

            int index = Users.Last().Equals(selectedUser) ?
                Users.IndexOf(selectedUser) - 1 :
                Users.IndexOf(selectedUser) + 1;

            SelectedUser = index >= 0 ?
                           Users[index] :
                           null;

            Users.Remove(selectedUser);
            IsReadOnly = true;
            IsEditing = false;
        }

        [RelayCommand]
        private void EditUserStart()
        {           
            if (SelectedUser != null)
            {
                EditableUser = new User(SelectedUser);
                IsReadOnly = false;
                IsEditing = true;
            }
        }


        [RelayCommand]
        private void EditUserCommit()
        {
            if (EditableUser != null && SelectedUser != null)
            {
                int index = Users.IndexOf(SelectedUser);
                Users.RemoveAt(index);
                Users.Insert(index, EditableUser);
                SelectedUser = Users[index];
                IsReadOnly = true;
                IsEditing = false;
                IsSaved = true;

                Task.Delay(2000).ContinueWith(_ => IsSaved = false, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }


        [RelayCommand]
        private void EditUserCancel()
        {
            EditableUser = null;
            EditableUser = new User(SelectedUser);
            IsReadOnly = true;
            IsEditing = false;
        }

        public UserDashboardPageViewModel()
        {
            _users = GenerateUsers();
        }

        private static ObservableCollection<User> GenerateUsers()
        {
            var random = new Random();
            var users = new ObservableCollection<User>();

            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime endDate = DateTime.Now.Date;
            int range = (endDate - startDate).Days;


            var imageids = new[] { "64","65", "91", "103", "177", "334", "338", "342", "349", "366", "367", "373",
                                    "375", "378", "399", "447", "453", "473", "469", "505"};
            var names = new[]
            {
                "John",
                "Winston",
                "Adrianna",
                "Spencer",
                "Phoebe",
                "Lucas",
                "Carl",
                "Marissa",
                "Brandon",
                "Antoine",
                "Arielle",
            };

            var surnames = new[]
            {
                "Doe",
                "Tapia",
                "Cisneros",
                "Lynch",
                "Munoz",
                "Marsh",
                "Hudson",
                "Bartlett",
                "Gregory",
                "Banks",
                "Hood",
                "Fry",
                "Carroll"
            };

            var companies = new[]
            {
                "Luminary Nexus",
                "CrestWave Dynamics",
                "Horizon Ventures",
                "Sapphire Pulse Technologies",
                "EmberLight Industries",
                "StellarEdge Ventrues",
            };

            var addresses = new[]
            {
                "Room 1450, 9819 Rutledge Parkway, Saint Louis, Missouri, United States",
                "18th Floor, 3631 Manitowish Point, Mobile, Alabama, United States",
                "Apt 1145, Kansas, United States",
                "PO Box 54647, 252 Derek Way, Flushing, New York, United States",
                "Apt 687, 47182 Superior Avenue, Kansas City, Missouri, ",
                "20th Floor, 5524 Badeau Pass, Glendale, Arizona, United States",
                "Room 1121, 9 Kipling Terrace, Winston Salem, North Carolina, United States",
                "16th Floor, Odessa, Texas, United States",
                "Suite 82, 44 Shasta Terrace, Las Cruces, United States",
                "Room 1930, 45779 Anhalt Junction, Detroit, Michigan, United States",
                "PO Box 54206, 14 Waubesa Street, Greenville, South Carolina, United States",
                "1st Floor, 78 Barby Park, South Dakota, United States",
                "Room 1426, 7394 Welch Alley, Huntsville, Alabama, United States",
                "20th Floor, 11 Eastwood Road, El Paso, Texas, United States",
                "Suite 92, 9 Hermina Point, Bakersfield, United States",
                "",
            };


            for (int i = 0; i < 20; i++)
            {
                int randomDays = random.Next(range + 1);
                users.Add(

                    new User(
                        imageids[random.Next(0, imageids.Length)],
                        names[random.Next(0, names.Length)],
                        surnames[random.Next(0, surnames.Length)],
                        companies[random.Next(0, companies.Length)],
                        addresses[random.Next(0, addresses.Length)],
                        random.Next(21, 63),
                        startDate.AddDays(randomDays),
                        random.Next(2) == 1

                    )
                );
            }


            return users;
        }
    }
}
