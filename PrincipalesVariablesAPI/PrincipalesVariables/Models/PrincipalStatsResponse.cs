namespace PrincipalesVariables.Models;

public class PrincipalStatsResponse
{
    public int status { get; set; }
    public Stat[] results { get; set; }
}

public class Stat
{
    public int idVariable { get; set; }
    public int cdSerie { get; set; }
    public string descripcion { get; set; }
    public string fecha { get; set; }
    public decimal  valor { get; set; }
}