using System;
using System.ComponentModel;

namespace TimeClock.Classes.Entities
{
    /// <summary>Represents a shift that was started or worked.</summary>
    internal class Shift : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _shiftStart, _shiftEnd;
        private bool _edited;

        #region Modifying Properties

        /// <summary>User ID</summary>
        public int ID
        {
            get => _id;
            private set { _id = value; OnPropertyChanged("ID"); }
        }

        /// <summary>Time Shift started.</summary>
        public DateTime ShiftStart
        {
            get => _shiftStart;
            private set { _shiftStart = value; OnPropertyChanged("ShiftStart"); OnPropertyChanged("ShiftLength"); OnPropertyChanged("ShiftLengthToString"); }
        }

        /// <summary>Time Shift ended.</summary>
        public DateTime ShiftEnd
        {
            get => _shiftEnd;
            set
            {
                _shiftEnd = value;
                OnPropertyChanged("ShiftEnd");
                OnPropertyChanged("ShiftLength");
                OnPropertyChanged("ShiftLengthToString");
            }
        }

        /// <summary>Has this Shift been edited?</summary>
        public bool Edited
        {
            get => _edited;
            set
            {
                _edited = value;
                OnPropertyChanged("Edited");
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Time Shift started, formatted to string.</summary>
        public string ShiftStartToString => ShiftStart.ToString(@"yyyy\/MM\/dd hh\:mm\:ss tt");

        /// <summary>Time Shift ended, formatted to string.</summary>
        public string ShiftEndToString => ShiftEnd != DateTime.MinValue ? ShiftEnd.ToString(@"yyyy\/MM\/dd hh\:mm\:ss tt") : "";

        /// <summary>Length of Shift.</summary>
        public TimeSpan ShiftLength => ShiftEnd != DateTime.MinValue ? ShiftEnd - ShiftStart : DateTime.Now - ShiftStart;

        /// <summary>Length of Shift, formatted to string.</summary>
        public string ShiftLengthToString => ShiftEnd != DateTime.MinValue
            ? ShiftLength.Days > 0
                ? ShiftLength.ToString(@"d\:hh\:mm\:ss")
                : ShiftLength.ToString(@"hh\:mm\:ss")
            : "";

        #endregion Helper Properties

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        #region Override Operators

        private static bool Equals(Shift left, Shift right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return left.ID == right.ID && left.ShiftStart == right.ShiftStart && left.ShiftEnd == right.ShiftEnd && left.Edited == right.Edited;
        }

        public override bool Equals(object obj) => Equals(this, obj as Shift);

        public bool Equals(Shift otherShift) => Equals(this, otherShift);

        public static bool operator ==(Shift left, Shift right) => Equals(left, right);

        public static bool operator !=(Shift left, Shift right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => $"{ID}: {ShiftStart}, {ShiftEnd}";

        #endregion Override Operators

        #region Constructors

        /// <summary>Initalizes a default instance of Shift.</summary>
        internal Shift()
        {
            ShiftStart = new DateTime();
            ShiftEnd = new DateTime();
        }

        /// <summary>Initializes a new instance of Shift by assigning only the ShiftStart Property.</summary>
        /// <param name="id">ID</param>
        /// <param name="shiftStart">Start of Shift</param>
        internal Shift(int id, DateTime shiftStart) : this(id, shiftStart, new DateTime())
        {
        }

        /// <summary>Initializes a new instance of Shift by assigning Properties.</summary>
        /// <param name="id">ID</param>
        /// <param name="shiftStart">Start of Shift</param>
        /// <param name="shiftEnd">End of Shift</param>
        internal Shift(int id, DateTime shiftStart, DateTime shiftEnd)
        {
            ID = id;
            ShiftStart = shiftStart;
            ShiftEnd = shiftEnd;
        }

        /// <summary>Replaces this instance of Shift with another instance.S</summary>
        /// <param name="other">Instance of Shift to replace this instance</param>
        internal Shift(Shift other) : this(other.ID, other.ShiftStart, other.ShiftEnd)
        {
        }

        #endregion Constructors
    }
}