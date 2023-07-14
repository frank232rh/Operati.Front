using Prueba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Prueba.Data.Helpers
{
    public class Validate
    {
        public static ResponseViewModel ValidateForm(UserViewModel user)
        {
            ResponseViewModel response = new ResponseViewModel { Success = true, Message = string.Empty };
            string errMessage = string.Empty;
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Mail) || String.IsNullOrEmpty(user.Password) || (String.IsNullOrEmpty(user.NewPassword) && user.Id > 0))
            {
                errMessage += "You must fill in all the fields";
                response.Success = false;
            }

            if (!validateEmailRegex.IsMatch(user.Mail))
            {
                errMessage += " - Not a valid email";
                response.Success = false;
            }
            response.Message = errMessage;
            return response;
        }
    }
}
