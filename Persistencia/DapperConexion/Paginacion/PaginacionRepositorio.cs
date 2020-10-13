using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
  public class PaginacionRepositorio : IPaginacion
  {
    private readonly IFactoryConnection factoryConnection;

    public PaginacionRepositorio(IFactoryConnection factoryConnection)
    {
      this.factoryConnection = factoryConnection;
    }

    public async Task<PaginacionModel> devolverPaginacion(string storedProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
    {
      PaginacionModel paginacionModel = new PaginacionModel();
      List<IDictionary<string, object>> listaReporte = null;
      int totalRecords = 0;
      int totalPaginas = 0;
      try
      {
        var connection = this.factoryConnection.GetConnection();
        DynamicParameters parameters = new DynamicParameters();

        foreach (var param in parametrosFiltro)
        {
          parameters.Add("@" + param.Key, param.Value);
        }

        parameters.Add("@NumeroPagina", numeroPagina);
        parameters.Add("@CantidadElementos", cantidadElementos);
        parameters.Add("@Ordenamiento", ordenamientoColumna);

        parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
        parameters.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

        var result = await connection.QueryAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure
        );

        listaReporte = result.Select(record => (IDictionary<string, object>)record).ToList();

        paginacionModel.ListaRecords = listaReporte;
        paginacionModel.NumeroPaginas = parameters.Get<int>("@TotalPaginas");
        paginacionModel.TotalRecords = parameters.Get<int>("@TotalRecords");
      }
      catch (Exception e)
      {

        throw new Exception("No se pudo ejecutar el storedProcedure: ", e);
      }
      finally
      {
        this.factoryConnection.CloseConnection();
      }

      return paginacionModel;
    }
  }
}