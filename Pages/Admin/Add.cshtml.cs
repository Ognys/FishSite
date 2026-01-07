using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishProj.Pages.Admin;


public class AddModel : PageModel
{
    private readonly FishContext _db;
    private readonly IWebHostEnvironment _env;
    public AddModel(FishContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [BindProperty]
    public FishModel Fish { get; set; } = new();
    [BindProperty]
    public IFormFile? Upload {get; set; }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        if(!ModelState.IsValid)
            return Page();
        
        if(Upload != null && Upload.Length > 0)
        {
            if(!Upload.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError(nameof(Upload),"Файл не является изображением");
                return Page();
            }

            string uploadDir = Path.Combine(_env.WebRootPath,"img");
            Directory.CreateDirectory(uploadDir);

            string ext = Path.GetExtension(Upload.FileName);
            string fn = $"{Guid.NewGuid():N}{ext}";
            string savePath = Path.Combine(uploadDir, fn);

            await using (var fs = new FileStream(savePath, FileMode.Create))
            {
                await Upload.CopyToAsync(fs);
            }

            Fish.ImagePath = $"/img/{fn}";
        }

        _db.Fish.Add(Fish);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}

