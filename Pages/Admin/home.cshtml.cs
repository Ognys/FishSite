using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishProj.Pages.Admin;


public class HomeModel : PageModel
{
    private readonly FishContext _db;
    private readonly IWebHostEnvironment _env;

    public HomeModel(FishContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public List<FishModel> fishList { get; private set; } = new();

    async public Task OnGetAsync()
    {
        fishList = await _db.Fish
        .AsNoTracking()
        .ToListAsync();
    }

    async public Task<IActionResult> OnPostDeleteAsync(int id)
    {
        FishModel? fish = await _db.Fish.FirstOrDefaultAsync(f => f.Id == id);
        if(fish == null) return NotFound();

        string fileName = Path.GetFileName(fish.ImagePath ?? "");
        if(!string.IsNullOrWhiteSpace(fileName)) 
        {
            string path = Path.Combine(_env.WebRootPath,"img",fileName);
            if(System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        _db.Fish.Remove(fish);
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }
}