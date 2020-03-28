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

        #endregion

        #region Ctor
        public LoginController(IUserDetailsDbManager userDetailsDbManager)
        {
            this.userDetailsDbManager = userDetailsDbManager ?? throw new ArgumentNullException(nameof(userDetailsDbManager));
        }
        #endregion

        #region API Methods

        [HttpPost("Registration")]
        public IActionResult Registration(UserDetails userDetails)
        {
            try
            {
                if (this.userDetailsDbManager.Create(userDetails))
                    return Created("Created successfully", userDetails);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        } 
        #endregion
    }
}