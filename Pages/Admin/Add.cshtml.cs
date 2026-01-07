using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishProj.Pages;

public class AddModel : PageModel
{
    private readonly FishContext _db;
    public AddModel(FishContext db) => _db = db;

    [BindProperty]
    public FishModel Fish { get; set; }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
            return Page();
        
        _db.Fish.Add(Fish);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}

