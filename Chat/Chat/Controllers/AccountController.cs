using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Chat.Filters;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
using Entities.Models;
using WebMatrix.WebData;

namespace Chat.Controllers
{
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public PartialViewResult UserFastMenuPartial()
        {
            if (WebSecurity.IsAuthenticated)
                ViewBag.Login = User.Identity.Name;
            return PartialView();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistration userRegistration, HttpPostedFileBase avatar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    authorizationService.Register(userRegistration.Login, userRegistration.Password);
                    authorizationService.Login(userRegistration.Login, userRegistration.Password);

                    if (avatar != null)
                    {
                        var currentUser = authorizationService.GetCurrentUser();
                        var image = new Image { ImageMimeType = avatar.ContentType, User = currentUser};
                        authorizationService.SaveImage(image);
                        authorizationService.Commit();

                        var fileName = Path.Combine("~/Image/",
                                                      String.Concat(image.Id, Path.GetExtension(avatar.FileName)));
                        var filePath = Server.MapPath(image.FileName);
                        avatar.SaveAs(filePath);

                        image.FileName = fileName.Substring(1);
                        authorizationService.Commit();
                    }
                    return RedirectToAction("List", "Room");
                }
                catch (MembershipCreateUserException)
                {
                    ModelState.AddModelError("", "Login is already in use");
                }
            }
            return View();
        }

        [HttpGet]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin userLogin, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authorizationService.Login(userLogin.Login, userLogin.Password))
                    return Redirect(returnUrl ?? "~/");
                ModelState.AddModelError("", "Login or password is invalid");
            }
            return View();
        }

        [HttpGet]
        public RedirectToRouteResult Logout(string login)
        {
            authorizationService.Logout();
            return RedirectToAction("List", "Room");
        }
    }
}
