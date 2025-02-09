using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.strategies.PDFStrategies;

public interface IPDFStrategy
{
    Task<string> ExtractTextAsync(Stream stream);
}
