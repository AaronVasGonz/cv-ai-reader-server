using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.strategies.PDFStrategies;

public interface IPdfStrategyContext
{
    Task<string> ExtractTextAsync(Stream stream);
    void SetPDFStrategy(IPDFStrategy pDFStrategy);
}

public class PdfStrategyContext : IPdfStrategyContext
{
    private IPDFStrategy _strategy;
    private readonly IAsposePDFService _asposePDFService;

    public PdfStrategyContext(IAsposePDFService asposePDFService, IPDFStrategy pDFStrategy)
    {
        _asposePDFService = asposePDFService;
        _strategy = pDFStrategy ?? new AsposePDFSt(_asposePDFService);
    }

    public void SetPDFStrategy(IPDFStrategy pDFStrategy)
    {
        _strategy = pDFStrategy;
    }

    public async Task<string> ExtractTextAsync(Stream stream)
    {
        return await _strategy.ExtractTextAsync(stream);
    }
}
