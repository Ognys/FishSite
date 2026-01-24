using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class HomenewsModel : PageModel
{
    private readonly FishContext _db;

    public HomenewsModel(FishContext db)
    {
        _db = db;
    }


    public List<NewsModel> News { get; set; }

    async public Task OnGetAsync()
    {
        News = await _db.News
        .Include(n => n.ImagesNews)
        .OrderByDescending(n => n.CreateDate)
        .ToListAsync();
    }
}