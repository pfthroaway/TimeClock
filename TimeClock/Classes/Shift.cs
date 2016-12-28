using System;
using System.ComponentModel;

namespace TimeClock
{
    /// <summary>Represents a shift that was started or worked.</summary>
    internal class Shift : INotifyPropertyChanged
    {
        private string _id;
        private DateTime _shiftStart, _shiftEnd;

        #region Properties

        public string ID
        {
            get { return _id; }
            private set { _id = value; OnPropertyChanged("ID"); }
        }

        public DateTime ShiftStart
        {
            get { return _shiftStart; }
            private set { _shiftStart = value; OnPropertyChanged("ShiftStart"); }
        }

        public string ShiftStartToString => ShiftStart.ToString(@"yyyy\/MM\/dd hh\:mm\:ss tt");

        public DateTime ShiftEnd
        {
            get
            { return _shiftEnd; }
            set { _shiftEnd = value; OnPropertyChanged("ShiftEnd"); OnPropertyChanged("ShiftLength"); OnPropertyChanged("ShiftLengthToString"); }
        }

        public string ShiftEndToString => ShiftEnd.ToString(@"yyyy\/MM\/dd hh\:mm\:ss tt");

        public TimeSpan ShiftLength
        {
            get
            {
                if (ShiftEnd != DateTime.MinValue)
                    return ShiftEnd - ShiftStart;
                else
                    return DateTime.Now - ShiftStart;
            }
        }

        public string ShiftLengthToString => ShiftEnd != DateTime.MinValue ? ShiftLength.ToString(@"d\:hh\:mm\:ss") : (DateTime.Now - ShiftStart).ToString("d:hh:mm:ss");

        #endregion Properties

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
            return string.Equals(left.ID, right.ID, StringComparison.OrdinalIgnoreCase) && left.ShiftStart == right.ShiftStart && left.ShiftEnd == right.ShiftEnd;
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj as Shift);
        }

        public bool Equals(Shift otherShift)
        {
            return Equals(this, otherShift);
        }

        public static bool operator ==(Shift left, Shift right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Shift left, Shift right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ 17;
        }

        public override string ToString()
        {
            return ID + ": " + ShiftStart + ", " + ShiftEnd;
        }

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
        internal Shift(string id, DateTime shiftStart)
        {
            ID = id;
            ShiftStart = shiftStart;
            ShiftEnd = new DateTime();
        }

        /// <summary>Initializes a new instance of Shift by assigning Properties.</summary>
        /// <param name="id">ID</param>
        /// <param name="shiftStart">Start of Shift</param>
        /// <param name="shiftEnd">End of Shift</param>
        internal Shift(string id, DateTime shiftStart, DateTime shiftEnd)
        {
            ID = id;
            ShiftStart = shiftStart;
            ShiftEnd = shiftEnd;
        }

        /// <summary>Replaces this instance of Shift with another instance.S</summary>
        /// <param name="otherShift">Instance of Shift to replace this instance</param>
        internal Shift(Shift otherShift)
        {
            ID = otherShift.ID;
            ShiftStart = otherShift.ShiftStart;
            ShiftEnd = otherShift.ShiftEnd;
        }

        #endregion Constructors
    }
}