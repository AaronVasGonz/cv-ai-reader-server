using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace Models.Models.DTO;

public class CVComparingRequest
{
    public IFormFile Pdf { get; set; }

    public string? JobRequirementsText { get; set; }

    public IFormFile? JobRequirementsPdf { get; set; }
}
