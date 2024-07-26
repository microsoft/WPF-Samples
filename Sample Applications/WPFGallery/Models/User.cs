using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WPFGallery.Models
{
    public class User : INotifyPropertyChanged
    {
        private string? _firstName;
        private string? _lastName;
        private string? _company;
        private string? _address;
        private bool _isNewGraduate;
        private string _imageId = "91";
        private int _age;
        private string _deletedname;
        private DateTime _dateOfJoining;

        public string Deletedname
        {
            get => _deletedname;
            set
            {
                _deletedname = value;
                OnPropertyChanged(nameof(Deletedname));
            }
        }

        public string? FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    OnPropertyChanged(nameof(Name)); // Update Name whenever FirstName changes
                }
            }
        }

        public string? LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged(nameof(Name)); // Update Name whenever LastName changes
                }
            }
        }

        public string Name => $"{FirstName} {LastName}";

        public string ImageId
        {
            get => _imageId;
            set
            {
                if (_imageId != value)
                {
                    _imageId = value;
                    OnPropertyChanged(nameof(ImageId));
                    OnPropertyChanged(nameof(ImageKey)); // Update ImageKey whenever ImageId changes
                }
            }
        }

        public string ImageKey => $"p{ImageId}";

        public string? Company
        {
            get => _company;
            set
            {
                if (_company != value)
                {
                    _company = value;
                    OnPropertyChanged(nameof(Company));
                }
            }
        }

        public string? Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public DateTime DateOfJoining
        {
            get => _dateOfJoining;
            set
            {
                if (_dateOfJoining != value)
                {
                    _dateOfJoining = value;
                    OnPropertyChanged(nameof(DateOfJoining));
                }
            }
        }

        public bool IsNewGraduate
        {
            get => _isNewGraduate;
            set
            {
                if (_isNewGraduate != value)
                {
                    _isNewGraduate = value;
                    OnPropertyChanged(nameof(IsNewGraduate));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        //public User()
        //{
        //}

        public User(User user)
        {
            
            ImageId = user.ImageId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Company = user.Company;
            Address = user.Address;
            Age = user.Age;
            DateOfJoining = user.DateOfJoining;
            IsNewGraduate = user.IsNewGraduate;
        }

        public User(string imageID, string? firstName, string? lastName, string? company, string? address, int age, DateTime doj, bool isNewGraduate = false)
        {
            ImageId = imageID;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Address = address;
            IsNewGraduate = isNewGraduate;
            Age = age;
            DateOfJoining = doj;
            
        }
    }
}
