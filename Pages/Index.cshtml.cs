using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishProj.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly FishContext _db;

    public IndexModel(ILogger<IndexModel> logger, FishContext db)
    {
        _logger = logger;
        _db = db;
    }


    public List<FishModel> FishList { get; private set; } = new();

    public async Task OnGetAsync()
    {
        FishList = await _db.Fish
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .ToListAsync();
    }
}
