using Microsoft.EntityFrameworkCore;
using NBADATA.Data;
using NBADATA.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// Servicios
// ---------------------------------------------------------------------
builder.Services.AddDbContext<NBADbContext>(opt =>
    opt.UseInMemoryDatabase("nba"));   // DB en memoria para arrancar rápido
builder.Services.AddRazorPages();

var app = builder.Build();

// ---------------------------------------------------------------------
// Seed de datos de ejemplo (3 jugadores)
// ---------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NBADbContext>();
    if (!db.Players.Any())
    {
        db.Players.AddRange(
            new Player
            {
                FullName = "LeBron James",
                Team = "LAL",
                Position = "F",
                HeightCm = 206,
                WeightKg = 113,
                BirthDate = new DateTime(1984, 12, 30),
                Pts = 25.7,
                Reb = 7.3,
                Ast = 7.3,
                Stl = 1.1,
                Blk = 0.6,
                Tov = 3.2,
                FgPct = 0.518,
                TpPct = 0.365,
                FtPct = 0.756
            },
            new Player
            {
                FullName = "Stephen Curry",
                Team = "GSW",
                Position = "G",
                HeightCm = 188,
                WeightKg = 84,
                BirthDate = new DateTime(1988, 3, 14),
                Pts = 27.0,
                Reb = 4.5,
                Ast = 5.8,
                Stl = 0.9,
                Blk = 0.4,
                Tov = 3.1,
                FgPct = 0.468,
                TpPct = 0.419,
                FtPct = 0.915
            },
            new Player
            {
                FullName = "Nikola Jokić",
                Team = "DEN",
                Position = "C",
                HeightCm = 211,
                WeightKg = 129,
                BirthDate = new DateTime(1995, 2, 19),
                Pts = 26.4,
                Reb = 12.3,
                Ast = 8.9,
                Stl = 1.3,
                Blk = 0.7,
                Tov = 3.0,
                FgPct = 0.586,
                TpPct = 0.358,
                FtPct = 0.817
            }
        );
        db.SaveChanges();
    }
}

// ---------------------------------------------------------------------
// Pipeline HTTP
// ---------------------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
