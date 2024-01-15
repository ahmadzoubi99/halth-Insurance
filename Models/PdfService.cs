using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public class PdfService
{
    public void CreatePdfWithMessage(string outputPath, string message)
    {
        using (var document = new Document())
        {
            PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
            document.Open();

            var paragraph = new Paragraph(message);
            document.Add(paragraph);

            document.Close();
        }
    }
}
