using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
  public class InstructorRepositorio : IInstructor
  {
    private readonly IFactoryConnection factoryConnection;

    public InstructorRepositorio(IFactoryConnection factoryConnection)
    {
      this.factoryConnection = factoryConnection;
    }

    public async Task<int> Actualiza(Guid instructorId, string nombre, string apellidos, string grado)
    {
      var storedProcedure = "usp_instructor_editar";

      try
      {
        var connection = this.factoryConnection.GetConnection();
        var resultado = await connection.ExecuteAsync(storedProcedure, new
        {
          InstructorId = instructorId,
          Nombre = nombre,
          Apellidos = apellidos,
          Grado = grado
        }, commandType: CommandType.StoredProcedure);

        this.factoryConnection.CloseConnection();
        return resultado;

      }
      catch (Exception e)
      {

        throw new Exception("No se pudo editar el instructor: ", e);
      }
    }

    public async Task<int> Eliminar(Guid id)
    {
      var storedProcedure = "usp_instructor_eliminar";

      try
      {
        var connection = this.factoryConnection.GetConnection();
        var resultado = await connection.ExecuteAsync(storedProcedure, new
        {
          InstructorId = id
        }, commandType: CommandType.StoredProcedure);

        this.factoryConnection.CloseConnection();
        return resultado;

      }
      catch (Exception e)
      {

        throw new Exception("No se pudo eliminar el instructor: ", e);
      }
    }

    public async Task<int> Nuevo(string nombre, string apellidos, string grado)
    {
      var storedProcedure = "usp_instructor_nuevo";

      try
      {
        var connection = this.factoryConnection.GetConnection();

        var resultado = await connection.ExecuteAsync(storedProcedure, new
        {
          InstructorId = Guid.NewGuid(),
          Nombre = nombre,

          Apellidos = apellidos,
          Grado = grado
        }, commandType: CommandType.StoredProcedure);

        this.factoryConnection.CloseConnection();
        return resultado;
      }
      catch (Exception e)
      {

        throw new Exception("No se pudo realizar la acción, ", e);
      }



    }

    public async Task<IEnumerable<InstructorModel>> ObtenerLista()
    {
      IEnumerable<InstructorModel> instructorList = null;
      var storedProcedure = "usp_Obtener_Instructores";

      try
      {
        var connection = this.factoryConnection.GetConnection();
        instructorList = await connection.QueryAsync<InstructorModel>(storedProcedure, null, commandType: CommandType.StoredProcedure);
      }
      catch (Exception e)
      {

        throw new Exception("error en la consulta", e);
      }
      finally
      {
        factoryConnection.CloseConnection();
      }

      return instructorList;
    }

    public async Task<InstructorModel> ObtenerPorId(Guid id)
    {
      InstructorModel instructor = null;
      var storedProcedure = "usp_obtener_instructores_porId";

      try
      {
        var connection = this.factoryConnection.GetConnection();
        instructor = await connection.QueryFirstAsync<InstructorModel>(
            storedProcedure,
            new { InstructorId = id },
            commandType: CommandType.StoredProcedure);

        factoryConnection.CloseConnection();

        return instructor;
      }
      catch (Exception e)
      {

        throw new Exception("Error en la consulta", e);
      }

    }
  }
}