using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.strategies.PDFStrategies;

public class AsposePDFSt : IPDFStrategy
{
    private readonly IAsposePDFService _asposePDFService;
    public AsposePDFSt(IAsposePDFService asposePDFService)
    {
        _asposePDFService = asposePDFService;
    }
    public async Task<string> ExtractTextAsync(Stream stream)
    {
        return await _asposePDFService.ExtractTextAsync(stream);
    }
}
