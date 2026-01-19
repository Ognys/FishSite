using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishProj.Pages.Admin;

public class AddNewsModel : PageModel
{
    private readonly FishContext _db;
    private readonly IWebHostEnvironment _env;

    public AddNewsModel(FishContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [BindProperty]
    public NewsModel News { get; set; }
    [BindProperty]
    public List<IFormFile> Upload { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (Upload is not null && Upload.Count > 0)
        {
            foreach (var file in Upload)
            {
                if (file is null || file.Length == 0)
                    continue;

                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError(nameof(Upload), "Можно загружать только изображения");
                    return Page();
                }
            }
        }

        News.CreateDate = DateTime.Now;
        _db.News.Add(News);
        await _db.SaveChangesAsync();

        if (Upload is null || Upload.Count == 0)
            return RedirectToPage("/Admin/Home");

        var uploadDir = Path.Combine(_env.WebRootPath, "img", "news");
        Directory.CreateDirectory(uploadDir);

        foreach (var file in Upload)
        {
            if (file is null || file.Length == 0)
                continue;

            var ext = Path.GetExtension(file.FileName);
            var fn = $"{Guid.NewGuid():N}{ext}";
            var savePath = Path.Combine(uploadDir, fn);

            using (var stream = new FileStream(savePath, FileMode.Create))
                await file.CopyToAsync(stream);

            _db.ImageNews.Add(new NewsImgModel
            {
                ImagePath = $"/img/news/{fn}",
                NewsModelId = News.Id
            });
        }

        await _db.SaveChangesAsync();

        return RedirectToPage("/Admin/Home");
    }


}