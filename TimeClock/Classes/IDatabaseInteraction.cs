using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeClock
{
    internal interface IDatabaseInteraction
    {
        bool VerifyDatabaseIntegrity();

        Task<bool> LogIn(User loginUser);

        Task<bool> LogOut(User logOutUser, Shift logOutShift);

        Task<bool> NewUser(User newUser);

        Task<bool> ChangeUserPassword(User user, string newHashedPassword);

        Task<bool> ChangeAdminPassword(string newHashedPassword);

        Task<List<Shift>> LoadShifts(User user);
    }
}