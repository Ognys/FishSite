using System.Dynamic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


namespace FishProj.Pages;

public class LoginModel : PageModel
{
    private readonly IConfiguration _conf;

    public LoginModel(IConfiguration conf)
    {
        _conf = conf;
    }

    [BindProperty]
    public string Name { get; set; }
    [BindProperty]
    public string Password { get; set; }

    public void OnGet() {}

    public async Task<IActionResult> OnPostAsync()
    {
        var AdminUser = _conf["Admin:UserName"];
        var AdminPass = _conf["Admin:Password"];

        if(string.IsNullOrWhiteSpace(AdminUser) || string.IsNullOrWhiteSpace(AdminPass))
        {
            ModelState.AddModelError("","Конфигурация админ-панели не настроена");
            return Page();
        }

        if(Name != AdminUser || Password != AdminPass)
        {
            ModelState.AddModelError("","Неверные учетные данные");
            return Page();
        }

        var Claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, Name),  
        };

        var identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

            return RedirectToPage("/Admin/Add");
    }
}