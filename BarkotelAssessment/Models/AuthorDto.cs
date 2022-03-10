using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Models
{
    public class AuthorDto
    {
        public string Name { get; set; }
        public string DOB { get; set; }
        public IFormFile Image { get; set; }
        public string Bio { get; set; }
    }
}
