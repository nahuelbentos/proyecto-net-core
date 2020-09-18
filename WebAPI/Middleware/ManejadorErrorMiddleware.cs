using System.Runtime.CompilerServices;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
  public class ManejadorErrorMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILogger<ManejadorErrorMiddleware> logger;

    public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
    {
      this.next = next;
      this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await next(context);

      }
      catch (Exception exception)
      {
        await ManejadorExcepcionAsincrona(context, exception, this.logger);
        // throw;
      }
    }

    private async Task ManejadorExcepcionAsincrona(HttpContext context, Exception exception, ILogger<ManejadorErrorMiddleware> logger)
    {
      object errores = null;

      switch (exception)
      {
        case ManejadorExcepcion manejador:
          logger.LogError(exception, "Manejador Error");
          errores = manejador.errores;
          context.Response.StatusCode = (int)manejador.code;
          break;
        case Exception e:
          logger.LogError(exception, "Error de Servidor");
          errores = string.IsNullOrWhiteSpace(e.Message) ? "Error " : e.Message;
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          break;
      }
      context.Response.ContentType = "application/json";
      if (errores != null)
      {
        var resultados = JsonConvert.SerializeObject(new { errores });
        await context.Response.WriteAsync(resultados);
      }





    }


  }
}