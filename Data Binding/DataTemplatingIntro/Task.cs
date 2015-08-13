// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace DataTemplatingIntro
{
    public class Task : INotifyPropertyChanged
    {
        private string _description;
        private string _name;
        private int _priority;
        private TaskType _type;

        public Task()
        {
        }

        public Task(string name, string description, int priority, TaskType type)
        {
            _name = name;
            _description = description;
            _priority = priority;
            _type = type;
        }

        public string TaskName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("TaskName");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public TaskType TaskType
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("TaskType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => _name;

        protected void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}