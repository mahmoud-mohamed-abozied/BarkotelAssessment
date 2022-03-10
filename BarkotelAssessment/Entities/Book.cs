using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfPublication { get; set; }

        public byte[] Cover { get; set; }

        public string Description { get; set; }
        public ICollection<Author> Authors { set; get; } = new List<Author>();
        public List<AuthorBook> AuthorBook { set; get; } = new List<AuthorBook>();



    }
}
