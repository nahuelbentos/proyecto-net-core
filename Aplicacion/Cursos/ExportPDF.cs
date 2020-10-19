using System.IO;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;


namespace Aplicacion.Cursos
{
  public class ExportPDF
  {
    public class Consulta : IRequest<Stream> { }

    public class Manejador : IRequestHandler<Consulta, Stream>
    {
      private readonly CursosOnlineContext context;

      public Manejador(CursosOnlineContext context)
      {
        this.context = context;
      }

      public async Task<Stream> Handle(Consulta request, CancellationToken cancellationToken)
      {
        var cursos = await this.context.Curso.ToListAsync();

        Font fuenteTitulo = new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Blue);
        Font fuenteHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black);
        Font fuenteData = new Font(Font.HELVETICA, 6f, Font.NORMAL, BaseColor.Black);

        MemoryStream workStream = new MemoryStream();
        Rectangle rectangle = new Rectangle(PageSize.A4);

        Document document = new Document(rectangle, 0, 0, 50, 100);

        PdfWriter pdfWriter = PdfWriter.GetInstance(document, workStream);

        pdfWriter.CloseStream = false;

        document.Open();
        document.AddTitle("Lista de cursos en la Universidad");
        PdfPTable table = new PdfPTable(1);
        table.WidthPercentage = 90;
        PdfPCell cell = new PdfPCell(new Phrase("Lista de cursos de SQL Server", fuenteTitulo));
        cell.Border = Rectangle.NO_BORDER;

        table.AddCell(cell);

        document.Add(table);


        PdfPTable tableCursos = new PdfPTable(2);
        float[] widths = new float[] { 40, 60 };
        tableCursos.SetWidthPercentage(widths, rectangle);

        PdfPCell cellHeaderTitulo = new PdfPCell(new Phrase("Curso", fuenteHeader));
        tableCursos.AddCell(cellHeaderTitulo);
        PdfPCell cellHeaderDescripcion = new PdfPCell(new Phrase("Descripcion", fuenteHeader));
        tableCursos.AddCell(cellHeaderDescripcion);
        tableCursos.WidthPercentage = 90;

        foreach (var curso in cursos)
        {
          PdfPCell cellDataTitulo = new PdfPCell(new Phrase(curso.Titulo, fuenteData));
          tableCursos.AddCell(cellDataTitulo);
          PdfPCell cellDataDescripcion = new PdfPCell(new Phrase(curso.Descripcion, fuenteData));
          tableCursos.AddCell(cellDataDescripcion);
        }

        document.Add(tableCursos);



        document.Close();

        byte[] byteData = workStream.ToArray();

        workStream.Write(byteData, 0, byteData.Length);
        workStream.Position = 0;

        return workStream;

      }
    }
  }
}