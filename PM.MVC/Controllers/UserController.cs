using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PM.Model.Model;
using PM.MVC.Utility;

namespace PM.MVC.Controllers
{
    public class UserController : Controller
    {
        #region Fields
        private readonly IHttpClientHelper<UserDetails> httpClientHelper;
        #endregion

        #region Ctor
        public UserController(IHttpClientHelper<UserDetails> httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }
        #endregion

        #region Action Methods
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserDetails> userDetails = await httpClientHelper.GetMultipleItemsRequest("api/User/GetAll", CancellationToken.None);
            return View(userDetails);
        }
        #endregion

    }
}