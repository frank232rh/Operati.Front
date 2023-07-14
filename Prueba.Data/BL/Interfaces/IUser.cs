using Prueba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.Data.BL.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>ResponseViewModel{true/false, message}</returns>
        Task<ResponseViewModel> CreateUser(UserViewModel user);

        /// <summary>
        /// Get users list
        /// </summary>
        /// <returns>List<UserViewModel></returns>
        Task<List<UserViewModel>> GetUsers();

        /// <summary>
        /// Modify a user data
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>ResponseViewModel{true/false, message}</returns>
        Task<ResponseViewModel> ModifyUser(UserViewModel user);
    }
}
