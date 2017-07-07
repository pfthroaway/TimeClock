using System.Collections.Generic;
using System.Threading.Tasks;
using TimeClock.Classes.Entities;

namespace TimeClock.Classes.Database
{
    internal interface IDatabaseInteraction
    {
        void VerifyDatabaseIntegrity();

        #region Administrator Management

        /// <summary>Changes the Admin password in the database.</summary>
        /// <param name="hashedAdminPassword">New hashed admin password</param>
        /// <returns>Whether the admin password was updated in the database</returns>
        Task<bool> ChangeAdminPassword(string hashedAdminPassword);

        #endregion Administrator Management

        #region Audit

        /// <summary>Creates a database entry for actions taken in modifying data in the database.</summary>
        /// <param name="editor">Who modified the data?</param>
        /// <param name="action">Was the action an update, deletion, etc.?</param>
        /// <param name="originalItem">Original item</param>
        /// <param name="alteredItem">Altered item</param>
        /// <returns></returns>
        Task<bool> InsertAudit(string editor, string action, string originalItem, string alteredItem);

        #endregion Audit

        #region Load

        /// <summary>Gets the next User ID autoincrement value in the database for the Users table.</summary>
        /// <returns>Next User ID value</returns>
        Task<int> GetNextUserIndex();

        /// <summary>Loads the administrator password from the database.</summary>
        /// <returns>Administrator password</returns>
        Task<string> LoadAdminPassword();

        /// <summary>Loads all Users currently logged in</summary>
        /// <returns>List of all Users currently logged in</returns>
        Task<List<Shift>> LoadLoggedInUsers();

        /// <summary>Loads all the selected User's Shifts from the database.</summary>
        /// <param name="userID"></param>
        /// <returns>Returns the list of Shifts</returns>
        Task<List<Shift>> LoadShifts(int userID);

        /// <summary>Loads a User from the database.</summary>
        /// <returns>User</returns>
        Task<User> LoadUser(string username);

        /// <summary>Loads all Users from the database.</summary>
        /// <returns>List of all Users</returns>
        Task<List<User>> LoadUsers(bool loggedIn = false);

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginShift">Shift started by User</param>
        Task<bool> LogIn(Shift loginShift);

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutShift">Shift to be created on logout</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        Task<bool> LogOut(Shift logOutShift);

        #endregion Log In/Out

        #region User Management

        /// <summary>Changes a User's details in the database.</summary>
        /// <param name="oldUser">User whose details needs to be changed</param>
        /// <param name="newUser">User with new details</param>
        /// <returns>True if successfully updated in the database</returns>
        Task<bool> ChangeUserDetails(User oldUser, User newUser);

        /// <summary>Deletes a User and all their Shifts from the database.</summary>
        /// <param name="user">User to be deleted</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteUser(User user);

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        Task<bool> NewUser(User newUser);

        #endregion User Management
    }
}