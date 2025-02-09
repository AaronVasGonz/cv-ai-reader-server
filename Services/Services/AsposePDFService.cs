using Aspose.Pdf;
using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services;

public interface IAsposePDFService
{
    Task<string> ExtractTextAsync(Stream pdfStream);
}

public class AsposePDFService : IAsposePDFService
{

    public AsposePDFService()
    {

    }

    public async Task<string> ExtractTextAsync(Stream pdfStream)
    {
        try
        {
            if (pdfStream == null || pdfStream.Length == 0)
                throw new ArgumentException("El stream del PDF no puede estar vacío.", nameof(pdfStream));

            using Document pdfDocument = new Document(pdfStream);
            TextAbsorber textAbsorber = new TextAbsorber();

            pdfDocument.Pages.Accept(textAbsorber);

            return textAbsorber.Text;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

}
