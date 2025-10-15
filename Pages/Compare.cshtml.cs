using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NBADATA.Models;
using NBADATA.Data;    

namespace NBADATA.Pages
{
    public class CompareModel : PageModel
    {
    private readonly NBADbContext _db;

    public CompareModel(NBADbContext db) => _db = db;

        [BindProperty(SupportsGet = true)]
        public string? Mode { get; set; } = "basic";

        [BindProperty(SupportsGet = true, Name = "ids")]
        public string? IdsRaw { get; set; }

    [BindProperty(SupportsGet = true, Name = "player1")]
    public string? Player1 { get; set; }

    [BindProperty(SupportsGet = true, Name = "player2")]
    public string? Player2 { get; set; }

    public int[] SelectedIds { get; private set; } = Array.Empty<int>();
    public List<Player> Players { get; private set; } = new();
    public Player? Result1 { get; private set; }
    public Player? Result2 { get; private set; }
        public List<PropertyInfo> Props { get; private set; } = new();
        public string DebugMsg { get; private set; } = "";

        public async Task OnGet()
        {
            SelectedIds = ParseIds(IdsRaw);
            DebugMsg = $"IdsRaw='{IdsRaw}' -> {SelectedIds.Length} ids: [{string.Join(",", SelectedIds)}]";

            if (SelectedIds.Length > 0)
            {
                Players = await _db.Set<Player>()
                                   .Where(p => SelectedIds.Contains(p.Id))
                                   .ToListAsync();
                DebugMsg += $" | Players found: {Players.Count}";
            }

            if (!string.IsNullOrWhiteSpace(Player1))
            {
                var term = Player1.Trim();
                Result1 = await _db.Players
                                    .Where(p => EF.Functions.Like(p.FullName, $"%{term}%"))
                                    .FirstOrDefaultAsync();
                if (Result1 != null && !Players.Any(p => p.Id == Result1.Id)) Players.Add(Result1);
                DebugMsg += $" | Player1='{term}' -> {(Result1 != null ? Result1.FullName : "(not found)")}";
            }

            if (!string.IsNullOrWhiteSpace(Player2))
            {
                var term = Player2.Trim();
                Result2 = await _db.Players
                                    .Where(p => EF.Functions.Like(p.FullName, $"%{term}%"))
                                    .FirstOrDefaultAsync();
                if (Result2 != null && !Players.Any(p => p.Id == Result2.Id)) Players.Add(Result2);
                DebugMsg += $" | Player2='{term}' -> {(Result2 != null ? Result2.FullName : "(not found)")}";
            }

            var t = typeof(Player);
            Props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p =>
                        IsSimple(p.PropertyType) &&
                        p.Name != "Id") // excluye PK de la tabla
                     .OrderBy(p => p.Name)
                     .ToList();
        }

        private static int[] ParseIds(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return Array.Empty<int>();

            return raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim())
                      .Select(s => int.TryParse(s, out var n) ? n : (int?)null)
                      .Where(n => n.HasValue)
                      .Select(n => n!.Value)
                      .Distinct()
                      .ToArray();
        }

        private static bool IsSimple(Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;
            return t.IsPrimitive
                || t.IsEnum
                || t == typeof(string)
                || t == typeof(decimal)
                || t == typeof(double)
                || t == typeof(float)
                || t == typeof(DateTime)
                || t == typeof(Guid);
        }
    }
}
