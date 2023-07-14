using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Prueba.Data.BL.Interfaces;
using Prueba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.Data.BL.Services
{
    public class UserService : IUser
    {
        public async Task<ResponseViewModel> CreateUser(UserViewModel user)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                using (var context = new PruebaDbContext())
                {
                    context.Database.OpenConnection();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "[dbo].[SP_User]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@option", 2));
                    command.Parameters.Add(new SqlParameter("@name", user.Name));
                    command.Parameters.Add(new SqlParameter("@password", user.Password));
                    command.Parameters.Add(new SqlParameter("@mail", user.Mail));

                    var lectura = await command.ExecuteReaderAsync();
                    while (lectura.Read())
                    {
                        response.Success = lectura.GetBoolean(lectura.GetOrdinal("Success"));
                        response.Message = lectura.GetString(lectura.GetOrdinal("Message"));
                    }
                }
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel { Success = false, Message = ex.Message };
            }
            return response;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                using (var context = new PruebaDbContext())
                {
                    context.Database.OpenConnection();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "[dbo].[SP_User]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@option", 1));

                    var lectura = await command.ExecuteReaderAsync();
                    while (lectura.Read())
                    {
                        users.Add(new UserViewModel()
                        {
                            Id = lectura.GetInt32(lectura.GetOrdinal("Id")),
                            Name = lectura.GetString(lectura.GetOrdinal("Name")),
                            Mail = lectura.GetString(lectura.GetOrdinal("Mail"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return users;
        }

        public async Task<ResponseViewModel> ModifyUser(UserViewModel user)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                using (var context = new PruebaDbContext())
                {
                    context.Database.OpenConnection();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText = "[dbo].[SP_User]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@option", 3));
                    command.Parameters.Add(new SqlParameter("@name", user.Name));
                    command.Parameters.Add(new SqlParameter("@password", user.Password));
                    command.Parameters.Add(new SqlParameter("@newPassword", user.NewPassword));
                    command.Parameters.Add(new SqlParameter("@mail", user.Mail));
                    command.Parameters.Add(new SqlParameter("@id", user.Id));

                    var lectura = await command.ExecuteReaderAsync();
                    while (lectura.Read())
                    {
                        response.Success = lectura.GetBoolean(lectura.GetOrdinal("Success"));
                        response.Message = lectura.GetString(lectura.GetOrdinal("Message"));
                    }
                }
            }
            catch (Exception ex)
            {
                response = new ResponseViewModel { Success = false, Message = ex.Message };
            }
            return response;
        }
    }
}
