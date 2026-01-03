using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishProj.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly FishContext _db;
    public FishModel RandomFish { get; private set; }

    public IndexModel(ILogger<IndexModel> logger, FishContext db)
    {
        _logger = logger;
        _db = db;
    }


    public async Task OnGetAsync()
    {
        
    }


    public async Task OnPostRandomAsync()
    {
        int count = await _db.Fish.CountAsync();

        if(count == 0)
        {
            RandomFish = null;
            return;
        }

        int skip = Random.Shared.Next(0,count);

        RandomFish = await _db.Fish
            .AsNoTracking()
            .OrderBy(f => f.Id)
            .Skip(skip)
            .FirstAsync();
    }

}
