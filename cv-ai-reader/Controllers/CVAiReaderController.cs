using Microsoft.AspNetCore.Mvc;
using Models.Models.DTO;
using Services.Services;
using Services.strategies.PDFStrategies;

namespace cv_ai_reader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CVAiReaderController : ControllerBase
    {
        private readonly IOllamaService _ollamaService;
        private readonly IPdfStrategyContext _pdfStrategyContext;
        public CVAiReaderController(IOllamaService ollamaService, IPdfStrategyContext pdfStrategyContext)
        {
            _ollamaService = ollamaService;
            _pdfStrategyContext = pdfStrategyContext;
        }

        [HttpPost("pdf-reader")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PdfReader([FromForm] CVComparingRequest pDFRequest)
        {
            string pdfRequirmentsOutput = "";
            if (pDFRequest == null)
                return BadRequest("Debe subir un archivo PDF válido.");

            if (pDFRequest.JobRequirementsPdf != null)
            {
                pdfRequirmentsOutput = await _pdfStrategyContext.ExtractTextAsync(pDFRequest.JobRequirementsPdf.OpenReadStream());
            }
            if (pDFRequest.JobRequirementsText != null)
            {
                pdfRequirmentsOutput = pDFRequest.JobRequirementsText;
            }

            using var stream = pDFRequest.Pdf.OpenReadStream();
            var extractedText = await _pdfStrategyContext.ExtractTextAsync(stream);

            var prompt = "Analiza la siguiente información y compara los requisitos del trabajo con el currículum del candidato:\n\n" +
               $"### Requisitos del Trabajo:\n{pdfRequirmentsOutput}\n\n" +
               $"### Currículum del Candidato:\n{extractedText}\n\n" +
               "### Objetivo:\n" +
               "Evalúa qué tan bien el candidato se ajusta a los requisitos del puesto y asigna una calificación del 1 al 10 según los siguientes criterios:\n\n" +
               "### Criterios de Evaluación:\n" +
               "- **De 1 a 8:** La calificación dependerá del grado de cumplimiento de los requisitos.\n" +
               "  - Si cumple con menos de la mitad de los requisitos: **1 a 4**.\n" +
               "  - Si cumple con la mitad de los requisitos: **5 a 6**.\n" +
               "  - Si cumple con casi todos los requisitos pero le falta algo de experiencia o conocimientos específicos: **7 a 8**.\n" +
               "- **De 9 a 10:** Solo si el candidato cumple con **todos los requisitos** y tiene **una experiencia sólida en el puesto**.\n\n" +
               "### Reglas Especiales:\n" +
               "- Para puestos **senior o semi-senior**, si el candidato **no cumple con al menos la mitad de los requisitos, la calificación será automáticamente 0**.\n" +
               "- Para puestos **junior**, la evaluación será más flexible, ya que se espera que el candidato esté en proceso de aprendizaje.\n\n" +
               "### Instrucciones Estrictas:\n" +
               "- **La respuesta debe estar exclusivamente en español**. No debe incluir palabras en otro idioma.\n" +
               "- **La salida debe ser estrictamente en formato JSON**, con la siguiente estructura:\n\n" +
               "```json\n" +
               "{\n" +
               "  \"resumen\": \"(Resumen detallado de la comparación en español)\",\n" +
               "  \"nota\": (Número del 1 al 10 según los criterios de evaluación),\n" +
              "  \"Estado del Candidato\": (Si el puesto es **junior** y la nota está entre **6 y 10**, el candidato será considerado **APTO**. Para puestos **senior o semi-senior**, solo serán considerados **APTO** si la nota está entre **9 y 10**. En cualquier otro caso, el candidato será considerado **NO APTO**.),\\n " +
               "  \"feedback\": \"(Comentarios y sugerencias sobre la adecuación del candidato en español)\"\n" +
               "}\n" +
               "```\n\n" +
               "- **No agregues explicaciones adicionales**, solo responde en el formato JSON indicado.\n" +
               "- Si la información del currículum es insuficiente para hacer una evaluación completa, menciona esto en el `feedback`, pero **respeta siempre el formato JSON**.\n\n" +
               "### Requisitos de Precisión:\n" +
               "- Si el candidato tiene **habilidades muy relevantes pero no las ha detallado bien**, menciónalo en el `feedback`.\n" +
               "- Si el candidato tiene **habilidades irrelevantes para el puesto**, no las incluyas en la evaluación.\n" +
               "- Si la información del currículum es **ambigua o incompleta**, deja claro en el `feedback` que se necesita más información.\n\n" +
               "⚠️ **Es obligatorio que la respuesta cumpla con estas instrucciones al 100%. Cualquier contenido fuera de este formato será considerado incorrecto.**";

            var response = await _ollamaService.GetOllamaResponseAsync(prompt, "llama3.2:latest");

            return Ok(new { response });
        }

    }
}
