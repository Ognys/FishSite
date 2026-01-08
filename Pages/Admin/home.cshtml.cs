using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishProj.Pages.Admin;


public class HomeModel : PageModel
{
    private readonly FishContext _db;

    public HomeModel(FishContext db)
    {
        _db = db;
    }

    public List<FishModel> fishList { get; private set; }

    async public Task OnGetAsync()
    {
        fishList = await _db.Fish
        .AsNoTracking()
        .ToListAsync();
    }
}