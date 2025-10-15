namespace NBADATA.Models;

public class Player
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Team { get; set; } = "";
    public string Position { get; set; } = "";
    public int HeightCm { get; set; }
    public int WeightKg { get; set; }
    public DateTime BirthDate { get; set; }

    public double Pts { get; set; }
    public double Reb { get; set; }
    public double Ast { get; set; }
    public double Stl { get; set; }
    public double Blk { get; set; }
    public double Tov { get; set; }
    public double FgPct { get; set; }
    public double TpPct { get; set; }
    public double FtPct { get; set; }
}
