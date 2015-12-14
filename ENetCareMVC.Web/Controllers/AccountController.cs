using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;
using ENetCareMVC.Web.Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.BusinessService;
using Microsoft.AspNet.Identity.Owin;
using ENetCareMVC.Repository.Repository;

namespace ENetCareMVC.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register        
        public ActionResult Register()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);            

            RegisterViewModel model = new RegisterViewModel();

            model.DistributionCentres = context.DistributionCentre;
            model.EmployeeType = EmployeeType.Agent;            

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]       
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            if (ModelState.IsValid)
            {
                DistributionCentre locationCentre =
                    context.DistributionCentre.FirstOrDefault(d => d.CentreId == model.LocationCentreId);
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Fullname = model.FullName,                   
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, model.EmployeeType.ToString());
                }

                int written = 0;
                if (result.Succeeded)
                {
                    Employee employee = new Employee();
                    employee.UserId = new Guid(user.Id);
                    employee.UserName = user.UserName;
                    employee.FullName = model.FullName;
                    employee.LocationCentreId = model.LocationCentreId;
                    employee.EmployeeType = model.EmployeeType;
                    employee.EmailAddress = model.Email;

                    context.Employee.Add(employee);
                    written = context.SaveChanges();                    
                }

                if (result.Succeeded && written > 0)
                {
                    await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                else if (!result.Succeeded)
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            model.DistributionCentres = context.DistributionCentre;
            return View(model);
        }

        public ActionResult EditEmployee()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            Employee employee = GetCurrentEmployee();

            var employeeService = GetEmployeeService();

            var model = new EditEmployeeViewModel();

            model.Username = employee.UserName;
            model.EmailAddress = employee.EmailAddress;
            model.FullName = employee.FullName;
            model.EmployeeType = employee.EmployeeType;
            model.LocationCentreId = employee.LocationCentreId;
          
            model.DistributionCentres = employeeService.GetAllDistributionCentres();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(EditEmployeeViewModel model)
        {
            var employeeService = GetEmployeeService();
            var distributionCentres = employeeService.GetAllDistributionCentres();

            if (ModelState.IsValid)
            {
                DistributionCentre locationCentre =
                    distributionCentres.FirstOrDefault(d => d.CentreId == model.LocationCentreId);

                var user = UserManager.FindById(User.Identity.GetUserId());
                Employee employee = GetCurrentEmployee();

                user.Fullname = model.FullName;

                var result = UserManager.Update(user);

                int written = 0;
                if (result.Succeeded)
                {
                    var employeeResult = employeeService.Update(employee.UserName,
                        model.FullName,
                        model.EmailAddress,
                        locationCentre,
                        employee.EmployeeType);

                    if (employeeResult.Success)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", employeeResult.ErrorMessage);
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }

            model.DistributionCentres = distributionCentres;
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private EmployeeService GetEmployeeService()
        {
            IEmployeeRepository repository = new EmployeeRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new EmployeeService(repository);
        }

        private Employee GetCurrentEmployee()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            var employeeService = GetEmployeeService();

            return employeeService.Retrieve(username);
        }

        #endregion
    }
}