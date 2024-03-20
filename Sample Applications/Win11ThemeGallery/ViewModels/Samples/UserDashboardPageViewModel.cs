using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win11ThemeGallery.Models;

namespace Win11ThemeGallery.ViewModels.Samples
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

        partial void OnSelectedUserChanged(User? oldValue, User? newValue)
        {
            EditableUser = null;
        }

        [RelayCommand]
        private void AddUser()
        { 
            Users.Add(new User("New User", ""));
            SelectedUser = Users.Last();
            EditableUser = new User(SelectedUser);
        }

        [RelayCommand]
        private void RemoveUser(object selectedUser)
        {
            if(selectedUser is User user)
            {
                Users.Remove(user);
                SelectedUser = null;
            }
        }

        [RelayCommand]
        private void EditUserStart()
        {
            EditableUser = new User(SelectedUser);
        }


        [RelayCommand]
        private void EditUserCommit()
        {
            if(EditableUser != null && SelectedUser != null)
            {
                int index = Users.IndexOf(SelectedUser);
                Users[index] = EditableUser;
                SelectedUser = Users[index];
                EditableUser = null;
            }
        }

        [RelayCommand]
        private void EditUserCancel()
        {
            EditableUser = null;
        }

        public UserDashboardPageViewModel()
        {
            _users = GenerateUsers();
        }

        private ObservableCollection<User> GenerateUsers()
        {
            var random = new Random();
            var users = new ObservableCollection<User>();

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
                "Pineapple Inc.",
                "Microsoft Redmond",
                "Amazing Basics Ltd",
                "Megabyte Computers Inc",
                "Roude Mics",
                "XD Projekt Red S.A.",
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

            int ncolors = Enum.GetNames<AvatarColor>().Length;
            for (int i = 0; i < 20; i++)
                users.Add(
                    new User(
                        imageids[random.Next(0, imageids.Length)],
                        names[random.Next(0, names.Length)],
                        surnames[random.Next(0, surnames.Length)],
                        companies[random.Next(0, companies.Length)],
                        addresses[random.Next(0, addresses.Length)],
                        random.Next(2) == 1 ? true : false
                    )
                );

            return users;
        }
    }
}
