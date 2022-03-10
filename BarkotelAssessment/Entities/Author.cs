using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Entities
{
    public class Author
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }
        public byte[] Image { get; set; }
        public string Bio { get; set; }

        public ICollection<Book> Books { set; get; } = new List<Book>();
        public List<AuthorBook> AuthorBook { set; get; } = new List<AuthorBook>();


    }
}
