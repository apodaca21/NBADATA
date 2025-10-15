using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NBADATA.Data;
using NBADATA.Models;

namespace NBADATA.Pages.Players;

public class IndexModel : PageModel
{
    private readonly NBADbContext _db;
    public IndexModel(NBADbContext db) => _db = db;

    public List<Player> Players { get; set; } = new();
    [BindProperty(SupportsGet = true)] public string? q { get; set; }

    public async Task OnGetAsync()
    {
        var query = _db.Players.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.FullName.Contains(q) || p.Team.Contains(q));
        Players = await query.OrderBy(p => p.FullName).ToListAsync();
    }

    public IActionResult OnPostCompare([FromForm] int[] selected, [FromForm] string mode)
    {
        if (selected.Length != 2)
            return RedirectToPage(new { q, error = "Selecciona exactamente 2 jugadores" });

        return RedirectToPage("/Compare", new { ids = string.Join(",", selected), mode });
    }
}
