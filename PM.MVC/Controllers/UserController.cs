using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM.Model.Model;
using PM.MVC.Utility;

namespace PM.MVC.Controllers
{
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly IHttpClientHelper<UserDetails> httpClientHelper;
        private readonly IHttpClientHelper<UserProfile> userProfileDbContext;
        private readonly IHttpClientHelper<UserViewModel> userViewModelDbContext;
        private readonly IHttpClientHelper<MultipartFormDataContent> formFileDbContext;
        private readonly IAPIUrl aPIUrl;
        private readonly UserDetails userDetailsSession;
        #endregion

        #region Ctor
        public UserController(IHttpClientHelper<UserDetails> httpClientHelper,
                              IHttpClientHelper<UserProfile> UserProfileDbContext,
                              IHttpClientHelper<UserViewModel> userViewModelDbContext,
                              IHttpClientHelper<MultipartFormDataContent> formFileDbContext,
                              IAPIUrl aPIUrl,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientHelper = httpClientHelper;
            userProfileDbContext = UserProfileDbContext;
            this.userViewModelDbContext = userViewModelDbContext;
            this.formFileDbContext = formFileDbContext;
            this.aPIUrl = aPIUrl;
            userDetailsSession = httpContextAccessor.HttpContext.Session.GetObject<UserDetails>(nameof(UserDetails));
        }
        #endregion

        #region Action Methods

       
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserViewModel> userDetails = await userViewModelDbContext.GetMultipleItemsRequest("api/User/GetAll?Role=" + userDetailsSession.RoleMatrixId + "", CancellationToken.None);

            return View(userDetails);
        }

        public IActionResult UserProfile()
        {
            ViewBag.IsAdmin = userDetailsSession.RoleMatrixId == 1;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfile userProfile)
        {
            try
            {
                userProfile.UserName = userDetailsSession.UserName;
                UserProfile up = await userProfileDbContext.PostRequest("api/user/PostUserProfile", userProfile, CancellationToken.None);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult BulkInsert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BulkInsert(Model.Model.File file)
        {
            HttpClient client = new HttpClient();
            byte[] data;
            using (var br = new BinaryReader(file.FormFile.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.FormFile.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes, "FormFile", file.FormFile.FileName);
            var response = await client.PutAsync("" + aPIUrl.BaseAddress + "api/user/PutBulkInsert", multiContent);
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}