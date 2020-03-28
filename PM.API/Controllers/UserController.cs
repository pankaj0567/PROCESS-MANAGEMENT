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
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly IUserDetailsDbManager userDetailsDbManager;

        #endregion

        #region Ctor
        public UserController(IUserDetailsDbManager userDetailsDbManager)
        {
            this.userDetailsDbManager = userDetailsDbManager ?? throw new ArgumentNullException(nameof(userDetailsDbManager));
        }
        #endregion

        #region API Methods

        [HttpGet("GetAll")]
        public IEnumerable<UserDetails> GetAll()
        {
            try
            {
                return userDetailsDbManager.GetAll();
            }
            catch (Exception ex)
            {
                return new List<UserDetails>();
            }
        }
        #endregion
    }
}