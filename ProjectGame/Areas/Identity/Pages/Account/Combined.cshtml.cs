using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectGame.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectGame.Areas.Identity.Pages.Account;

public class CombinedAuthModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly ILogger<CombinedAuthModel> _logger;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;

    public CombinedAuthModel(
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        ILogger<CombinedAuthModel> logger,
        IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<ApplicationUser>?)GetEmailStore();
        _logger = logger;
        _emailSender = emailSender;
    }

    [BindProperty]
    public LoginInputModel Login { get; set; }

    [BindProperty]
    public RegisterInputModel Register { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }



    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


    public class RegisterInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }



    // GET
    public async Task OnGetAsync(string handler)
    {
        ReturnUrl ??= Url.Content("~/");

        _logger.LogInformation("OnGetAsync() handler = {Handler}, ReturnUrl = {ReturnUrl}", handler, ReturnUrl);

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        // 登入時清除外部 cookie（註冊時不一定要清）
        if (string.Equals(handler, "Login", StringComparison.OrdinalIgnoreCase))
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }



    // POST: Login
    public async Task<IActionResult> OnPostLoginAsync()
    {
        _logger.LogInformation(">>> 進入 OnPostLoginAsync");

        ReturnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ClearNonLoginErrors();


        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Login.Email, Login.Password, Login.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                // Home/Index
                return RedirectToAction("Index", "Home");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl, RememberMe = Login.RememberMe });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return Page();

    }



    // POST: Register
    public async Task<IActionResult> OnPostRegisterAsync()
    {
        _logger.LogInformation(">>> 進入 OnPostRegisterAsync");

        ReturnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ClearNonRegisterErrors();

        if (!ModelState.IsValid)
        {
            foreach (var kvp in ModelState)
            {
                _logger.LogError("Key: {Key}, AttemptedValue: {Value}, Errors: {Errors}",
                    kvp.Key,
                    kvp.Value?.AttemptedValue,
                    string.Join(", ", kvp.Value?.Errors.Select(e => e.ErrorMessage)));
            }
        }

        if (ModelState.IsValid)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Register.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Register.Email, CancellationToken.None);

            user.Name = Register.Name;

            var result = await _userManager.CreateAsync(user, Register.Password);

            if (result.Succeeded)
            {
                
                _logger.LogInformation("User created a new account with password.");

                // 如果是公司信箱，加到 Staff；否則加入 Customer
                if (Register.Email.EndsWith("@gamebox.com", StringComparison.OrdinalIgnoreCase))
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Staff);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }

                // 產生 Email 驗證連結
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = ReturnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(Register.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity", email = Register.Email });
                }
                else
                {
                    _logger.LogInformation("Error");
                    return RedirectToPage("Combined", new { registered = "true" });
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }


    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }


    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }


    private void ClearNonRegisterErrors()
    {
        foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("Register.") && k != nameof(ReturnUrl)).ToList())
        {
            ModelState.Remove(key);
        }
    }

    private void ClearNonLoginErrors()
    {
        foreach (var key in ModelState.Keys.Where(k => !k.StartsWith("Login.") && k != nameof(ReturnUrl)).ToList())
        {
            ModelState.Remove(key);
        }
    }

}