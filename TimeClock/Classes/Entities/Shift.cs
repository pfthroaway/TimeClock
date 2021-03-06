﻿using System;
using System.Globalization;

namespace TimeClock.Classes.Entities
{
    /// <summary>Represents a shift that was started or worked.</summary>
    internal class Shift : BaseINPC
    {
        private int _id;
        private string _role;
        private readonly string fullDateFormat = @"yyyy-MM-dd hh\:mm\:ss tt";
        private readonly string shiftWeekFormat = "dd':'hh':'mm':'ss";
        private readonly string shiftDayFormat = "hh':'mm':'ss";
        private readonly CultureInfo culture = new CultureInfo("en-US");
        private DateTime _shiftStart, _shiftEnd;
        private bool _edited;

        #region Modifying Properties

        /// <summary>User ID</summary>
        public int ID
        {
            get => _id;
            private set { _id = value; NotifyPropertyChanged(nameof(ID)); }
        }

        /// <summary>The <see cref="User"/>'s role this <see cref="Shift"/>.</summary>
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                NotifyPropertyChanged(nameof(Role));
            }
        }

        /// <summary>Time <see cref="Shift"/> started.</summary>
        public DateTime ShiftStart
        {
            get => _shiftStart;
            private set { _shiftStart = value; NotifyPropertyChanged(nameof(ShiftStart), nameof(ShiftLength), nameof(ShiftLengthToString)); }
        }

        /// <summary>Time <see cref="Shift"/> ended.</summary>
        public DateTime ShiftEnd
        {
            get => _shiftEnd;
            set
            {
                _shiftEnd = value;
                NotifyPropertyChanged(nameof(ShiftEnd), nameof(ShiftLength), nameof(ShiftLengthToString));
            }
        }

        /// <summary>Has this <see cref="Shift"/> been edited?</summary>
        public bool Edited
        {
            get => _edited;
            set
            {
                _edited = value;
                NotifyPropertyChanged(nameof(Edited));
            }
        }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>Time <see cref="Shift"/> started, formatted to string.</summary>
        public string ShiftStartToString => ShiftStart.ToString(fullDateFormat, culture);

        /// <summary>Time <see cref="Shift"/> ended, formatted to string.</summary>
        public string ShiftEndToString => ShiftEnd != DateTime.MinValue ? ShiftEnd.ToString(fullDateFormat, culture) : "";

        /// <summary>Length of <see cref="Shift"/>.</summary>
        public TimeSpan ShiftLength => ShiftEnd != DateTime.MinValue ? ShiftEnd - ShiftStart : DateTime.Now - ShiftStart;

        /// <summary>Length of <see cref="Shift"/>, formatted to string.</summary>
        public string ShiftLengthToString => ShiftEnd != DateTime.MinValue
            ? ShiftLength.Days > 0
            ? ShiftLength.ToString(shiftWeekFormat, culture)
            : ShiftLength.ToString(shiftDayFormat, culture)
            : "";

        #endregion Helper Properties

        #region Override Operators

        private static bool Equals(Shift left, Shift right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) ^ ReferenceEquals(null, right)) return false;
            return left.ID == right.ID && left.Role == right.Role && left.ShiftStart == right.ShiftStart && left.ShiftEnd == right.ShiftEnd && left.Edited == right.Edited;
        }

        public override bool Equals(object obj) => Equals(this, obj as Shift);

        public bool Equals(Shift otherShift) => Equals(this, otherShift);

        public static bool operator ==(Shift left, Shift right) => Equals(left, right);

        public static bool operator !=(Shift left, Shift right) => !Equals(left, right);

        public override int GetHashCode() => base.GetHashCode() ^ 17;

        public override string ToString() => $"{ID}: {ShiftStart}, {ShiftEnd}";

        #endregion Override Operators

        #region Constructors

        /// <summary>Initalizes a default instance of <see cref="Shift"/>.</summary>
        internal Shift()
        {
            ShiftStart = new DateTime();
            ShiftEnd = new DateTime();
        }

        /// <summary>Initializes a new instance of <see cref="Shift"/> by assigning only the ShiftStart Property.</summary>
        /// <param name="id">ID</param>
        /// <param name="role"></param>
        /// <param name="shiftStart">Start time of <see cref="Shift"/></param>
        internal Shift(int id, string role, DateTime shiftStart) : this(id, role, shiftStart, new DateTime(), false)
        {
        }

        /// <summary>Initializes a new instance of <see cref="Shift"/> by assigning Properties.</summary>
        /// <param name="id">ID</param>
        /// <param name="role">The <see cref="User"/>'s role this <see cref="Shift"/></param>
        /// <param name="shiftStart">Start of <see cref="Shift"/></param>
        /// <param name="shiftEnd">End of <see cref="Shift"/></param>
        internal Shift(int id, string role, DateTime shiftStart, DateTime shiftEnd, bool edited)
        {
            ID = id;
            Role = role;
            ShiftStart = shiftStart;
            ShiftEnd = shiftEnd;
            Edited = edited;
        }

        /// <summary>Replaces this instance of <see cref="Shift"/> with another instance.</summary>
        /// <param name="other">Instance of <see cref="Shift"/> to replace this instance</param>
        internal Shift(Shift other) : this(other.ID, other.Role, other.ShiftStart, other.ShiftEnd, other.Edited)
        {
        }

        #endregion Constructors
    }
}