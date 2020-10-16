using System;

namespace Aplicacion.Documentos
{
  public class ArchivoGenerico
  {
    public Guid? DocumentoId { get; set; }
    public Guid? ObjetoReferencia { get; set; }
    public string Nombre { get; set; }
    public string Extension { get; set; }
    public string Data { get; set; }
  }
}