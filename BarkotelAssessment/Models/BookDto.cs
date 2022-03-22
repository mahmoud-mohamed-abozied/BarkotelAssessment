﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public string DateOfPublication { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
    }
}
