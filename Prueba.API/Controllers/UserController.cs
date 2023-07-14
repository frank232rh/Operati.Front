using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba.Data.BL.Interfaces;
using Prueba.Data.Helpers;
using Prueba.Data.Models;

namespace Prueba.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Constructor

        private readonly IUser _userService;

        public UserController(IUser userService)
        {
            _userService = userService;
        }

        #endregion

        #region Metodos

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
            try
            {
                ResponseViewModel response = Validate.ValidateForm(user);
                if (response.Success)
                {
                    response = await _userService.CreateUser(user);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var response = await _userService.GetUsers();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("ModifyUser")]
        public async Task<IActionResult> ModifyUser(UserViewModel user)
        {
            try
            {
                ResponseViewModel response = Validate.ValidateForm(user);
                if (response.Success)
                {
                    response = await _userService.ModifyUser(user);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        #endregion

    }
}
