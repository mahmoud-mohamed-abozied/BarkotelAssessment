using BarkotelAssessment.Entities;
using BarkotelAssessment.Models;
using BarkotelAssessment.Repositories;
using Microsoft.AspNetCore.Http;
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
    public class AuthorsController : Controller
    {
        private readonly IBaseRepository<Author> _baseRepository;
        private List<string> _allowedExtenstions = new List<string> { ".jpg", ".jpeg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public AuthorsController(IBaseRepository<Author> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _baseRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_baseRepository.GetAll());
        }


        [HttpPost]
        public IActionResult AddAuthor([FromForm] AuthorDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            Author AuthorToSave = new Author();
            if (author.Image != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(author.Image.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (author.Image.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for image is 1MB!");
                
                var file = author.Image;
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    AuthorToSave.Image = ms.ToArray();
                }

            }
            
            AuthorToSave.Name = author.Name;
            AuthorToSave.DOB = DateTime.ParseExact(author.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            AuthorToSave.Bio = author.Bio;

            var createdAuthor = _baseRepository.Add(AuthorToSave);
            return Ok(createdAuthor);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] JsonPatchDocument<Author> patchdoc)
        {
            if (patchdoc == null)
            {
                return BadRequest("You send trash");
            }
            var author = _baseRepository.GetById(id);
            if (author == null)
            {
                return BadRequest("Author not exist");
            }

            patchdoc.ApplyTo(author, ModelState);
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            _baseRepository.Update(author);
            return NoContent();

        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromForm] AuthorDto authorDto)
        {
            if (authorDto == null)
                return NotFound($"No author was found with ID {id}");

            var author = _baseRepository.GetById(id);
            if (author == null)
            {
                return BadRequest("Author not exist");
            }

            if (authorDto.Image != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(authorDto.Image.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (authorDto.Image.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                var file = authorDto.Image;
                if (file.Length > 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    author.Image = ms.ToArray();
                }
            }

            author.Name = authorDto.Name;
            author.DOB = DateTime.ParseExact(authorDto.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            author.Bio = authorDto.Bio;

            _baseRepository.Update(author);

            return Ok(author);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _baseRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            _baseRepository.Delete(author);
            return NoContent();
        }       
    }
}
