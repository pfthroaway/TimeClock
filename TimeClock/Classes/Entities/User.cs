using Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TimeClock.Classes.Entities
{
    /// <summary>Represents someone who uses the Time Clock.</summary>
    internal class User : INotifyPropertyChanged, IEquatable<User>
    {
        private int _id;
        private string _username, _firstName, _lastName, _password;
        private bool _loggedIn;
        private List<Shift> _shifts = new List<Shift>();

        #region Modifying Properties

        /// <summary>User ID</summary>
        public int ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged("ID"); }
        }

        /// <summary>User ID</summary>
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged("Username"); }
        }

        /// <summary>First name</summary>
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged("FirstName"); OnPropertyChanged("Names"); }
        }

        /// <summary>Last name</summary>
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged("LastName"); OnPropertyChanged("Names"); }
        }

        /// <summary>Last name, first name</summary>
        public string Names => $"{LastName}, {FirstName}";

        /// <summary>User Password</summary>
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged("Password"); }
        }

        /// <summary>Is User logged in?</summary>
        public bool LoggedIn
        {
            get => _loggedIn;
            set { _loggedIn = value; OnPropertyChanged("LoggedIn"); OnPropertyChanged("LoggedInText"); }
        }

        /// <summary>Shifts worked by User</summary>
        public IEnumerable<Shift> Shifts => _shifts;

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Is User logged in? formatted to string.</summary>
        public string LoggedInText => LoggedIn ? "Clocked In" : "NOT Clocked In";

        #endregion Helper Properties

        #region Shift Manipulation

        /// <summary>Adds a Shift to User's Shifts.</summary>
        /// <param name="newShift">Shift to be added</param>
        internal void AddShift(Shift newShift)
        {
            _shifts.Add(newShift);
            UpdateShifts();
        }

        /// <summary>Gets the most recent Shift.</summary>
        /// <returns>Most recent Shift</returns>
        internal Shift GetMostRecentShift() => _shifts[0];

        /// <summary>Modifies a Shift in User's Shifts.</summary>
        /// <param name="oldShift">Shift to be replaced</param>
        /// <param name="newShift">Shift to be replace old Shift</param>
        internal void ModifyShift(Shift oldShift, Shift newShift)
        {
            _shifts.Replace(oldShift, newShift);
            UpdateShifts();
        }

        /// <summary>Removes a Shift from User's Shifts.</summary>
        /// <param name="newShift">Shift to be removed</param>
        internal void RemoveShift(Shift newShift)
        {
            _shifts.Remove(newShift);
            UpdateShifts();
        }

        /// <summary>Updates Shifts list.</summary>
        private void UpdateShifts()
        {
            OnPropertyChanged("Shifts");
            if (Shifts.Any())
                _shifts = Shifts.OrderByDescending(shift => shift.ShiftStart).ToList();
        }

        #endregion Shift Manipulation

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
            return left.ID == right.ID && string.Equals(left.Username, right.Username, StringComparison.OrdinalIgnoreCase) && string.Equals(left.FirstName, right.FirstName, StringComparison.OrdinalIgnoreCase) && string.Equals(left.LastName, right.LastName, StringComparison.OrdinalIgnoreCase) && string.Equals(left.Password, right.Password, StringComparison.OrdinalIgnoreCase) && left.LoggedIn == right.LoggedIn && !left.Shifts.Except(right.Shifts).Any();
        }

        public override bool Equals(object obj) => Equals(this, obj as User);

        public bool Equals(User otherUser) => Equals(this, otherUser);

        public static bool operator ==(User left, User right) => Equals(left, right);

        public static bool operator !=(User left, User right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => $"{Username}: {LastName}, {FirstName}";

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of User.</summary>
        internal User()
        {
        }

        /// <summary>Initalizes an instance of User by assigning Properties.</summary>
        /// <param name="id">ID</param>
        /// <param name="username">Username</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="password">Password</param>
        /// <param name="loggedIn">Is the User logged in?</param>
        /// <param name="shifts">Shifts worked by User</param>
        internal User(int id, string username, string firstName, string lastName, string password, bool loggedIn, IEnumerable<Shift> shifts)
        {
            ID = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            LoggedIn = loggedIn;

            List<Shift> allShifts = new List<Shift>();
            allShifts.AddRange(shifts);
            _shifts = allShifts;
        }

        /// <summary>Replaces this instance of User with another instance.</summary>
        /// <param name="other">Instance of User to replace this one</param>
        internal User(User other) : this(other.ID, other.Username, other.FirstName, other.LastName, other.Password, other.LoggedIn, other.Shifts)
        {
        }

        #endregion Constructors
    }
}