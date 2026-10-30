using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovaStore.Models;
using NovaStore.Services;
namespace NovaStore.Controllers;

public class AccountController(StoreService store) : Controller
{
    private const string EmailKey = "CustomerEmail";
    private const string NameKey = "CustomerName";
    private readonly PasswordHasher<CustomerAccount> _hasher = new();
    private string? CurrentEmail => HttpContext.Session.GetString(EmailKey);
    private CustomerAccount? CurrentAccount => CurrentEmail is null ? null : store.FindAccount(CurrentEmail);

    [HttpGet] public IActionResult Register() => CurrentAccount is not null ? RedirectToAction(nameof(Profile)) : View(new RegisterInput());
    [HttpPost, ValidateAntiForgeryToken] public IActionResult Register(RegisterInput input)
    {
        if(!ModelState.IsValid) return View(input);
        var account = new CustomerAccount { FullName=input.FullName.Trim(), Email=input.Email.Trim(), Phone=input.Phone.Trim() };
        account.PasswordHash = _hasher.HashPassword(account, input.Password);
        if(!store.AddAccount(account)){ModelState.AddModelError(nameof(input.Email), "An account with this email already exists.");return View(input);}
        SignIn(account, false); TempData["Toast"]="Welcome to NOVA! Your account has been created."; return RedirectToAction(nameof(Profile));
    }
    [HttpGet] public IActionResult Login(string? returnUrl=null) { if(CurrentAccount is not null)return RedirectToAction(nameof(Profile)); ViewBag.ReturnUrl=returnUrl; return View(new LoginInput()); }
    [HttpPost, ValidateAntiForgeryToken] public IActionResult Login(LoginInput input,string? returnUrl=null)
    {
        ViewBag.ReturnUrl=returnUrl; if(!ModelState.IsValid)return View(input); var account=store.FindAccount(input.Email.Trim());
        if(account is null || _hasher.VerifyHashedPassword(account,account.PasswordHash,input.Password)==PasswordVerificationResult.Failed){ModelState.AddModelError("", "The email or password is incorrect.");return View(input);}
        SignIn(account,input.RememberMe); TempData["Toast"]="You are now signed in."; if(!string.IsNullOrWhiteSpace(returnUrl)&&Url.IsLocalUrl(returnUrl))return LocalRedirect(returnUrl); return RedirectToAction(nameof(Profile));
    }
    [HttpGet] public IActionResult Profile(){var account=CurrentAccount;if(account is null)return RedirectToAction(nameof(Login));return View(new ProfileInput{FullName=account.FullName,Email=account.Email,Phone=account.Phone});}
    [HttpPost,ValidateAntiForgeryToken] public IActionResult Profile(ProfileInput input){var account=CurrentAccount;if(account is null)return RedirectToAction(nameof(Login));if(!ModelState.IsValid)return View(input);account.FullName=input.FullName.Trim();account.Email=input.Email.Trim();account.Phone=input.Phone.Trim();if(!store.UpdateAccount(account)){ModelState.AddModelError(nameof(input.Email),"That email is already used by another account.");return View(input);}SignIn(account,true);TempData["Toast"]="Your profile has been updated.";return RedirectToAction(nameof(Profile));}
    public IActionResult MyOrders(){var account=CurrentAccount;if(account is null)return RedirectToAction(nameof(Login),new{returnUrl="/Account/MyOrders"});return View(store.Orders().Where(x=>string.Equals(x.Customer.Email,account.Email,StringComparison.OrdinalIgnoreCase)).ToList());}
    [HttpPost,ValidateAntiForgeryToken] public IActionResult Logout(){HttpContext.Session.Remove(EmailKey);HttpContext.Session.Remove(NameKey);TempData["Toast"]="You have been signed out.";return RedirectToAction("Index","Home");}
    private void SignIn(CustomerAccount account,bool remember){HttpContext.Session.SetString(EmailKey,account.Email);HttpContext.Session.SetString(NameKey,account.FullName);HttpContext.Session.SetString("AccountId",account.Id.ToString());if(remember)HttpContext.Session.SetString("RememberMe","true");}
}
