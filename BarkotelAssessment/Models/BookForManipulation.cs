using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Models
{
    public class BookForManipulation
    {
        public string Title { get; set; }
        public string DateOfPublication { get; set; }
        public IFormFile Cover { get; set; }
        public string Description { get; set; }
    }
}
