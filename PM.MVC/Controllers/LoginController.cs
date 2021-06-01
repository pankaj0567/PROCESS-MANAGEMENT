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
        private readonly IHttpClientHelper<UserProfile> userProfilehttpClientHelper;
        #endregion

        #region Ctor
        public LoginController(IHttpClientHelper<UserDetails> httpClientHelper,
                                IHttpClientHelper<UserProfile> UserProfilehttpClientHelper)
        {
            this.httpClientHelper = httpClientHelper;
            userProfilehttpClientHelper = UserProfilehttpClientHelper;
        }
        #endregion

        #region Action Methods

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDetails userDetails)
        {
           try
            {
                if (ModelState.IsValid)
                {
                    UserDetails ud = await httpClientHelper.PostRequest("api/login/Login", userDetails, CancellationToken.None);
                    if (userDetails.UserName.Trim() == ud.UserName.Trim() && userDetails.UserPassword.Trim() == ud.UserPassword.Trim())
                    {
                        HttpContext.Session.SetObject(nameof(UserDetails), ud);
                        return RedirectToAction("Index", "User");
                    }
                }

                return View(userDetails);
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserProfile userProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserProfile ud = await userProfilehttpClientHelper.PostRequest("api/login/Registration", userProfile, CancellationToken.None);
                    ViewBag.Message = "Your registration has been successful. Your User Name and password sent to your email";
                    return RedirectToAction(nameof(Login));
                }

                return View(userProfile);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        #endregion

    }
}