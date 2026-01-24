using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace FishProj.Pages.Admin;


public class UpdateNewsModel : PageModel
{
    private readonly FishContext _db;
    private readonly IWebHostEnvironment _env;

    public UpdateNewsModel(FishContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [BindProperty]
    public NewsModel UpdateModelNews { get; set; }
    [BindProperty]
    public IFormFile Upload { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        NewsModel? news = await _db.News.FirstOrDefaultAsync(n => n.Id == id);
        if (news == null) return NotFound();

        UpdateModelNews = news;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (Upload == null || Upload.Length == 0)
        {
            ModelState.AddModelError(nameof(Upload), "Выберите файл изображения.");
            return Page();
        }

        if (!Upload.ContentType.StartsWith("image/"))
        {
            ModelState.AddModelError(nameof(Upload), "Файл не является изображением");
            return Page();
        }

        string uploadDir = Path.Combine(_env.WebRootPath, "img");
        Directory.CreateDirectory(uploadDir);

        FishModel? fisshDb = await _db.Fish.FirstOrDefaultAsync(f => f.Id == UpdateModelFish.Id);
        if (fisshDb == null) return NotFound();

        fisshDb.Name = UpdateModelFish.Name;
        fisshDb.Description = UpdateModelFish.Description;

        var oldFileName = Path.GetFileName(fisshDb.ImagePath ?? "");
        var ext = Path.GetExtension(Upload.FileName).ToLowerInvariant();
        var fn = $"{Guid.NewGuid():N}{ext}";
        var newPath = Path.Combine(uploadDir, fn);


        await using (var fs = new FileStream(newPath, FileMode.Create))
        {
            await Upload.CopyToAsync(fs);
        }

        fisshDb.ImagePath = "/img/" + fn;

        await _db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(oldFileName))
        {
            var oldPath = Path.Combine(uploadDir, oldFileName);
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
        }

        return RedirectToPage("./Home");
    }
}