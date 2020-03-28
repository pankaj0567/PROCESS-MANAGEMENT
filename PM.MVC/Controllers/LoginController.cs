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
    public class LoginController : Controller
    {
        #region Fields
        private readonly IHttpClientHelper<UserDetails> httpClientHelper;
        #endregion

        #region Ctor
        public LoginController(IHttpClientHelper<UserDetails> httpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
        }
        #endregion

        #region Action Methods

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserDetails userDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetails ud = await httpClientHelper.PostRequest("api/login/Registration", userDetails, CancellationToken.None);
                    return RedirectToAction("Index", "User");
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        #endregion

    }
}