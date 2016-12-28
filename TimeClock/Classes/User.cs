using System;
using System.ComponentModel;

namespace TimeClock
{
    internal class User : INotifyPropertyChanged, IEquatable<User>
    {
        private string _id, _firstName, _lastName, _password;
        private bool _loggedIn;

        #region Properties

        public string ID
        {
            get { return _id; }
            private set { _id = value; OnPropertyChanged("ID"); }
        }

        public string FirstName
        {
            get { return _firstName; }
            private set { _firstName = value; OnPropertyChanged("FirstName"); OnPropertyChanged("Names"); }
        }

        public string LastName
        {
            get { return _lastName; }
            private set { _lastName = value; OnPropertyChanged("LastName"); OnPropertyChanged("Names"); }
        }

        public string Names => LastName + ", " + FirstName;

        public string Password
        {
            get { return _password; }
            private set { _password = value; OnPropertyChanged("Password"); }
        }

        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; OnPropertyChanged("LoggedIn"); OnPropertyChanged("LoggedInText"); }
        }

        public string LoggedInText
        {
            get
            {
                if (LoggedIn)
                    return "Clocked In";
                else
                    return "NOT Clocked In";
            }
        }

        #endregion Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Override Operators

        private static bool Equals(User left, User right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return string.Equals(left.ID, right.ID, StringComparison.OrdinalIgnoreCase) && string.Equals(left.FirstName, right.FirstName, StringComparison.OrdinalIgnoreCase) && string.Equals(left.LastName, right.LastName, StringComparison.OrdinalIgnoreCase) && string.Equals(left.Password, right.Password, StringComparison.OrdinalIgnoreCase) && left.LoggedIn == right.LoggedIn;
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as User);
        }

        public bool Equals(User otherUser)
        {
            return Equals(this, otherUser);
        }

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ 17;
        }

        public override string ToString()
        {
            return ID + ": " + LastName + ", " + FirstName;
        }

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of User.</summary>
        internal User()
        {
        }

        /// <summary>Initalizes an instance of User by assigning Properties.</summary>
        /// <param name="id">User's ID</param>
        /// <param name="firstName">User's First name</param>
        /// <param name="lastName">User's Last name</param>
        /// <param name="password">User's Password</param>
        /// <param name="loggedIn">Is the User logged in?</param>
        internal User(string id, string firstName, string lastName, string password, bool loggedIn)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            LoggedIn = loggedIn;
        }

        /// <summary>Replaces this instance of User with another instance.</summary>
        /// <param name="otherUser">Instance of User to replace this one</param>
        internal User(User otherUser)
        {
            ID = otherUser.ID;
            FirstName = otherUser.FirstName;
            LastName = otherUser.LastName;
            Password = otherUser.Password;
            LoggedIn = otherUser.LoggedIn;
        }

        #endregion Constructors
    }
}