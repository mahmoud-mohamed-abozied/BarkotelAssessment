using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Entities
{
    public class AuthorBook
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
