using BarkotelAssessment.Entities;
using BarkotelAssessment.Models;
using BarkotelAssessment.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBaseRepository<Book> _baseRepository;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".jpeg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public BooksController(IBaseRepository<Book> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _baseRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            var returnedBook = new BookDto();
            returnedBook.Title = book.Title;
            returnedBook.Description = book.Description;
            returnedBook.DateOfPublication = book.DateOfPublication.ToString();

            string imageBase64Data = Convert.ToBase64String(book.Cover);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            returnedBook.Cover = imageDataURL;

            return Ok(book);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var books = _baseRepository.GetAll();
            var returnedBooks = new List<BookDto>();
            foreach (var book in books)
            {
                string imageBase64Data = Convert.ToBase64String(book.Cover);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

                returnedBooks.Add(new BookDto()
                {
                    Title = book.Title,
                    Description = book.Description,
                    DateOfPublication = book.DateOfPublication.ToString(),
                    Cover = imageDataURL
                });
                

            }
            return Ok(returnedBooks);
        }

        /*[HttpGet(("search/{word}"))]
        public IActionResult GetByName(string word)
        {
            return Ok(_baseRepository.Find(b => b.Title.Contains(word) || b.Description.Contains(word), new[] { "Author" }));
        }*/
        
        [HttpPost]
        public IActionResult AddBook([FromForm] BookForManipulation book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            if (!_allowedExtenstions.Contains(Path.GetExtension(book.Cover.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (book.Cover.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var file = book.Cover;
            Book BookToSave = new Book();
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                BookToSave.Cover = ms.ToArray();
            }

            BookToSave.Title = book.Title;
            BookToSave.DateOfPublication = DateTime.ParseExact(book.DateOfPublication, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            BookToSave.Description = book.Description;
            //var author = new Author { Name = "name1" };
            /*BookToSave.AuthorBook = new List<AuthorBook>
            {
              new AuthorBook {
                //Author = author,
                //AuthorId = book.Authorid,
                Book = BookToSave
              }
            };*/
            var createdBook = _baseRepository.Add(BookToSave);
            return Ok(createdBook);
        }



        [HttpPatch("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] JsonPatchDocument<Book> patchdoc)
        {
            if (patchdoc == null)
            {
                return BadRequest("You send trash");
            }
            var book = _baseRepository.GetById(id);
            if (book == null)
            { 
                return BadRequest("Book not exist");
            }
            
            patchdoc.ApplyTo(book, ModelState);
            if (!ModelState.IsValid)
            {
               return new UnprocessableEntityObjectResult(ModelState);
            }
            _baseRepository.Update(book);
            return NoContent();

        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromForm] BookForManipulation bookDto)
        {
            if (bookDto == null)
                return NotFound($"No author was found with ID {id}");

            var book = _baseRepository.GetById(id);
            if (book == null)
            {
                return BadRequest("Author not exist");
            }

            if (bookDto.Cover != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(bookDto.Cover.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (bookDto.Cover.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                var file = bookDto.Cover;
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    book.Cover = ms.ToArray();
                }
            }

            book.Title = bookDto.Title;
            book.DateOfPublication = DateTime.ParseExact(bookDto.DateOfPublication, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            book.Description = bookDto.Description;

            _baseRepository.Update(book);

            return Ok(book);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _baseRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            _baseRepository.Delete(book);
            return NoContent();

        }
    }
}
