using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NBADATA.Data;
using NBADATA.Models;

namespace NBADATA.Pages;

public class CompareModel : PageModel
{
    private readonly NBADbContext _db;
    public CompareModel(NBADbContext db) => _db = db;

    public Player A { get; set; } = null!;
    public Player B { get; set; } = null!;
    [BindProperty(SupportsGet = true)] public string mode { get; set; } = "stats";

    public async Task<IActionResult> OnGetAsync(string ids, string? mode = "stats")
    {
        this.mode = mode ?? "stats";
        var parts = ids?.Split(',')?.Select(int.Parse).ToArray();
        if (parts is null || parts.Length != 2) return RedirectToPage("/Players/Index");

        var players = await _db.Players.Where(p => parts.Contains(p.Id)).ToListAsync();
        if (players.Count != 2) return RedirectToPage("/Players/Index");
        A = players[0]; B = players[1];
        return Page();
    }
}
