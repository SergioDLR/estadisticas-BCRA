using PrincipalesVariables.DTOs;
using PrincipalesVariables.Models;
using System.Data.SqlClient;
using Dapper;
    
namespace PrincipalesVariables.Services;

public class StatsBcraServices
{
  

    private readonly IConfiguration _config;
    public StatsBcraServices(IConfiguration config)
    {
        _config = config;
    }
    public async Task<ResponseDTO<Value[]>> GetMainStats()
    {
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        var res = await conection.QueryAsync<Value>("select value.dateValue, value.idVariable, value.value, variable.cdSerie, variable.description from Value value inner join Variable variable on variable.idVariable = value.idVariable");
       
        foreach (var stat in res)
        {
            stat.variation = 0;
            var values = await conection.QueryAsync<Value>(@"select top 2 value.dateValue, value.idVariable, value.value, variable.cdSerie, variable.description from Value value inner join Variable variable on variable.idVariable = value.idVariable where value.idVariable = @Id order by value.dateValue desc",new{Id=stat.idVariable});
            var enumerable = values as Value[] ?? values.ToArray();
            if (enumerable.Count() == 2 && enumerable[0].value != 0)
            {
                stat.variation = (((enumerable[1].value - enumerable[0].value) / enumerable[0].value) * 100);
            }
        }
        var responseDto = new ResponseDTO<Value[]> { data = res.ToArray()};
        return responseDto;
    }

    public async Task<ResponseDTO<Value>> GetMainStatsById(int id)
    {
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        var res = await conection.QueryAsync<Value>(@"select value.dateValue, value.idVariable, value.value, variable.cdSerie, variable.description from Value value inner join Variable variable on variable.idVariable = value.idVariable where value.idVariable = @Id",new{Id=id});
        
        var responseDto = new ResponseDTO<Value> { data = res.FirstOrDefault()};
        return responseDto;
    }

   


    public async Task<ResponseDTO<Variable[]>> GetVariableCombo()
    {
       
        var conection = new SqlConnection(_config.GetConnectionString("DbStringConnection"));
        
        var res = await conection.QueryAsync<Variable>("select description,idVariable,cdSerie from Variable");
        
        var responseDto = new ResponseDTO<Variable[]> { data = res.ToArray()};
        return responseDto;
    }
}