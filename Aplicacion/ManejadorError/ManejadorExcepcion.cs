using System.Net;
using System;
namespace Aplicacion.ManejadorError
{
  public class ManejadorExcepcion : Exception
  {
    public HttpStatusCode code { get; }
    public object errores { get; }

    public ManejadorExcepcion(HttpStatusCode code, object errores = null)
    {
      this.code = code;
      this.errores = errores;
    }
  }
}