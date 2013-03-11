using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Chat.Filters;
using Chat.Infrastructure.Abstract;
using Chat.ViewModels;
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

        public PartialViewResult LoginPartial()
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
        public ActionResult Register(UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    authorizationService.Register(userRegistration.Login, userRegistration.Password);
                    authorizationService.Login(userRegistration.Login, userRegistration.Password);
                    return RedirectToAction("List", "Chat");
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
            return RedirectToAction("List", "Chat");
        }
    }
}
