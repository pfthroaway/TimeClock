using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeClock
{
    internal interface IDatabaseInteraction
    {
        void VerifyDatabaseIntegrity();

        Task<bool> LogIn(User loginUser);

        Task<bool> LogOut(User logOutUser, Shift logOutShift);

        Task<bool> NewUser(User newUser);

        Task<bool> ChangeUserPassword(User user, string newHashedPassword);

        Task<bool> ChangeAdminPassword(string hashedAdminPassword);

        Task<List<Shift>> LoadShifts(User user);
    }
}