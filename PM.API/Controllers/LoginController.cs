using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.API.DAL.DAL;
using PM.Model.Model;

namespace PM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Fields
        private readonly IUserDetailsDbManager userDetailsDbManager;
        private readonly IUserProfileDbManager userProfileDbManager;

        #endregion

        #region Ctor
        public LoginController(IUserDetailsDbManager userDetailsDbManager,
                                IUserProfileDbManager userProfileDbManager)
        {
            this.userDetailsDbManager = userDetailsDbManager ?? throw new ArgumentNullException(nameof(userDetailsDbManager));
            this.userProfileDbManager = userProfileDbManager;
        }
        #endregion

        #region API Methods

        [HttpPost("Registration")]
        public IActionResult Registration(UserProfile userProfile)
        {
            try
            {
                if (this.userProfileDbManager.Create(userProfile,1))
                    return Created("Created successfully", userProfile);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserDetails userDetails)
        {
            try
            {
                return Ok(this.userDetailsDbManager.ValidateUser(userDetails));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}