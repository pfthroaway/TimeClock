using Extensions;
using Extensions.DataTypeHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TimeClock.Classes.Entities
{
    /// <summary>Represents someone who uses the Time Clock.</summary>
    internal class User : BaseINPC
    {
        private int _id;
        private string _username, _firstName, _lastName, _password;
        private readonly string format = @"hh\:mm\:ss";
        private readonly CultureInfo culture = new CultureInfo("en-US");
        private bool _loggedIn;
        private List<string> _roles = new List<string>();
        private List<Shift> _shifts = new List<Shift>();

        #region Modifying Properties

        /// <summary><see cref="User"/>'s ID</summary>
        public int ID
        {
            get => _id;
            set { _id = value; NotifyPropertyChanged(nameof(ID)); }
        }

        /// <summary><see cref="User"/>'s username</summary>
        public string Username
        {
            get => _username;
            set { _username = value; NotifyPropertyChanged(nameof(Username)); }
        }

        /// <summary><see cref="User"/>'s first name</summary>
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; NotifyPropertyChanged(nameof(FirstName), nameof(Names)); }
        }

        /// <summary><see cref="User"/>'s last name</summary>
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; NotifyPropertyChanged(nameof(LastName), nameof(Names)); }
        }

        /// <summary><see cref="User"/>'s Password</summary>
        public string Password
        {
            get => _password;
            set { _password = value; NotifyPropertyChanged(nameof(Password)); }
        }

        /// <summary>Is <see cref="User"/> logged in?</summary>
        public bool LoggedIn
        {
            get => _loggedIn;
            set { _loggedIn = value; NotifyPropertyChanged(nameof(LoggedIn), nameof(LoggedInText)); }
        }

        /// <summary>List of roles a <see cref="User"/> has available.</summary>
        public IEnumerable<string> Roles => _roles;

        /// <summary><see cref="Shift"/>s worked by <see cref="User"/>.</summary>
        public IEnumerable<Shift> Shifts => _shifts;

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Last name, first name</summary>
        public string Names => $"{LastName}, {FirstName}";

        /// <summary>Is <see cref="User"/> logged in? formatted to string.</summary>
        public string LoggedInText => LoggedIn ? "Clocked In" : "NOT Clocked In";

        /// <summary>List of roles a <see cref="User"/> has available, formatted.</summary>
        public string RolesToString => string.Join(", ", Roles);

        /// <summary>Total hours worked today.</summary>
        public TimeSpan TotalHoursToday
        {
            get
            {
                List<Shift> shifts = new List<Shift>(Shifts.Where(shift => shift.ShiftStart > DateTime.Today).ToList());
                return shifts.Count > 0 ? new TimeSpan(shifts.Sum(shift => shift.ShiftLength.Ticks)) : LoggedIn ? DateTime.Now - GetMostRecentShift().ShiftStart : new TimeSpan();
            }
        }

        // => LoggedIn ? new TimeSpan(Shifts.Where(shift => shift.ShiftStart > DateTime.Today).ToList().Sum(shift => shift.ShiftLength.Ticks));

        /// <summary>Total hours worked today, formatted.</summary>
        public string TotalHoursTodayToString => TotalHoursToday.ToString(format, culture);

        /// <summary>Total hours worked this week, formatted.</summary>
        public string TotalHoursTodayToStringWithText => $"Today: {TotalHoursTodayToString}";

        /// <summary>Total hours worked this week.</summary>
        public TimeSpan ThisWeekTotalHours
        {
            get
            {
                List<Shift> shifts = new List<Shift>(Shifts.Where(shift => shift.ShiftStart >= DateTime.Now.StartOfWeek(DayOfWeek.Sunday)).ToList());
                return shifts.Count > 0 ? new TimeSpan(shifts.Sum(shift => shift.ShiftLength.Ticks)) : LoggedIn ? DateTime.Now - GetMostRecentShift().ShiftStart : new TimeSpan();
            }
        }

        /// <summary>Total hours worked today, formatted with preceding text.</summary>
        public string ThisWeekTotalHoursToString => $"{Int32Helper.Parse(ThisWeekTotalHours.TotalHours).ToString(culture).PadLeft(2, '0')}:{ThisWeekTotalHours.Minutes.ToString(culture).PadLeft(2, '0')}:{ThisWeekTotalHours.Seconds.ToString(culture).PadLeft(2, '0')}";

        /// <summary>Total hours worked this week, formatted with preceding text.</summary>
        public string ThisWeekTotalHoursToStringWithText => $"This Week: {ThisWeekTotalHoursToString}";

        #endregion Helper Properties

        #region Shift Manipulation

        /// <summary>Adds a <see cref="Shift"/> to <see cref="User"/>'s <see cref="Shift"/>s.</summary>
        /// <param name="newShift"><see cref="Shift"/> to be added</param>
        internal void AddShift(Shift newShift)
        {
            _shifts.Add(newShift);
            UpdateShifts();
        }

        /// <summary>Gets the most recent <see cref="Shift"/>.</summary>
        /// <returns>Most recent <see cref="Shift"/></returns>
        internal Shift GetMostRecentShift() => _shifts[0];

        /// <summary>Modifies a <see cref="Shift"/> in <see cref="User"/>'s <see cref="Shift"/>s.</summary>
        /// <param name="oldShift"><see cref="Shift"/> to be replaced</param>
        /// <param name="newShift"><see cref="Shift"/> to be replace old <see cref="Shift"/></param>
        internal void ModifyShift(Shift oldShift, Shift newShift)
        {
            _shifts.Replace(oldShift, newShift);
            UpdateShifts();
        }

        /// <summary>Removes a <see cref="Shift"/> from <see cref="User"/>'s <see cref="Shift"/>s.</summary>
        /// <param name="newShift"><see cref="Shift"/> to be removed</param>
        internal void RemoveShift(Shift newShift)
        {
            _shifts.Remove(newShift);
            UpdateShifts();
        }

        /// <summary>Updates <see cref="Shift"/>s list.</summary>
        private void UpdateShifts()
        {
            if (Shifts.Any())
                _shifts = Shifts.OrderByDescending(shift => shift.ShiftStart).ToList();
            UpdateBindings();
        }

        #endregion Shift Manipulation

        #region Role Manipulation

        /// <summary>Adds a role to a <see cref="User"/>.</summary>
        /// <param name="role">Role to be added</param>
        internal void AddRole(string role)
        {
            _roles.Add(role);
            UpdateRoles();
        }

        /// <summary>Modifies a <see cref="User"/>'s role.</summary>
        /// <param name="oldRole">Role to be modified</param>
        /// <param name="newRole">Role to replace old role</param>
        internal void ModifyRole(string oldRole, string newRole)
        {
            _roles.Replace(oldRole, newRole);
            UpdateRoles();
        }

        /// <summary>Removes a role from a <see cref="User"/>.</summary>
        /// <param name="role"></param>
        internal void RemoveRole(string role)
        {
            _roles.Remove(role);
            UpdateRoles();
        }

        /// <summary>Updates a <see cref="User"/>'s roles.</summary>
        private void UpdateRoles()
        {
            if (_roles.Count > 0)
                _roles = _roles.OrderBy(role => role).ToList();
            NotifyPropertyChanged("Roles");
        }

        #endregion Role Manipulation

        /// <summary>Updates bindings.</summary>
        internal void UpdateBindings()
        {
            NotifyPropertyChanged("Shifts");
            NotifyPropertyChanged("TotalHoursToday");
            NotifyPropertyChanged("TotalHoursTodayToString");
            NotifyPropertyChanged("TotalHoursTodayToStringWithText");
            NotifyPropertyChanged("ThisWeekTotalHours");
            NotifyPropertyChanged("ThisWeekTotalHoursToString");
            NotifyPropertyChanged("ThisWeekTotalHoursToStringWithText");
        }

        #region Override Operators

        private static bool Equals(User left, User right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return left.ID == right.ID
                && string.Equals(left.Username, right.Username, StringComparison.OrdinalIgnoreCase)
                && string.Equals(left.FirstName, right.FirstName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(left.LastName, right.LastName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(left.Password, right.Password, StringComparison.OrdinalIgnoreCase)
                && left.LoggedIn == right.LoggedIn
                && !left.Roles.Except(right.Roles).Any()
                && !right.Roles.Except(left.Roles).Any()
                && !left.Shifts.Except(right.Shifts).Any()
                && !right.Shifts.Except(left.Shifts).Any();
        }

        public override bool Equals(object obj) => Equals(this, obj as User);

        public bool Equals(User other) => Equals(this, other);

        public static bool operator ==(User left, User right) => Equals(left, right);

        public static bool operator !=(User left, User right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => $"{Username}: {Names}";

        #endregion Override Operators

        #region Constructors

        /// <summary>Initializes a default instance of <see cref="User"/>.</summary>
        internal User()
        {
        }

        /// <summary>Initalizes an instance of <see cref="User"/> by assigning Properties.</summary>
        /// <param name="id">ID</param>
        /// <param name="username">Username</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="password">Password</param>
        /// <param name="loggedIn">Is the <see cref="User"/> logged in?</param>
        /// <param name="roles">List of roles a <see cref="User"/> has available.</param>
        /// <param name="shifts"><see cref="Shift"/>s worked by the <see cref="User"/></param>
        internal User(int id, string username, string firstName, string lastName, string password, bool loggedIn, IEnumerable<string> roles, IEnumerable<Shift> shifts)
        {
            ID = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            LoggedIn = loggedIn;

            List<string> allRoles = new List<string>();
            allRoles.AddRange(roles);
            _roles = allRoles;

            List<Shift> allShifts = new List<Shift>();
            allShifts.AddRange(shifts);
            _shifts = allShifts;
        }

        /// <summary>Replaces this instance of <see cref="User"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="User"/> to replace this one</param>
        internal User(User other) : this(other.ID, other.Username, other.FirstName, other.LastName, other.Password, other.LoggedIn, other.Roles, other.Shifts)
        {
        }

        #endregion Constructors
    }
}