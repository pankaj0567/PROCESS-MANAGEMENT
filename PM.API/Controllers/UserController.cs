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
        private readonly IUserProfileDbManager userProfileDbContext;

        #endregion

        #region Ctor
        public UserController(IUserDetailsDbManager userDetailsDbManager, IUserProfileDbManager userProfileDbContext)
        {
            this.userDetailsDbManager = userDetailsDbManager ?? throw new ArgumentNullException(nameof(userDetailsDbManager));
            this.userProfileDbContext = userProfileDbContext ?? throw new ArgumentNullException(nameof(userProfileDbContext));
        }
        #endregion

        #region API Methods

        [HttpGet("GetAll")]
        public IEnumerable<UserViewModel> GetAll(int Role)
        {
            try
            {
                return userDetailsDbManager.GetAll(Role);
            }
            catch (Exception ex)
            {
                return new List<UserViewModel>();
            }
        }

        [HttpPost("PostUserProfile")]
        public IActionResult PostUserProfile(UserProfile userProfile)
        {
            try
            {
                if (this.userProfileDbContext.Create(userProfile))
                    return Created("Created successfully", userProfile);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPut("PutBulkInsert")]
        public IActionResult PutBulkInsert([FromForm]Model.Model.File file)
        {
            try
            {
                if (this.userDetailsDbManager.BulkInsertFile(file))
                    return Ok();

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