using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishProj.Pages;

public class NewsPageModel : PageModel
{
    private readonly FishContext _db;

    public NewsPageModel (FishContext db)
    {
        _db = db;
    }

    public List<NewsModel> News { get; set; }

    public async Task OnGetAsync()
    {
               News = await _db.News
            .Include(n => n.ImagesNews)
            .OrderByDescending(n => n.CreateDate)
            .ToListAsync();
    }
}