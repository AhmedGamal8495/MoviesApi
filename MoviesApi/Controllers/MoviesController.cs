using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies
                .OrderByDescending(x => x.Rate)
                .Include(g => g.Genra)
                .Select(m => new MovieDetalisDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Rate = m.Rate,
                    StoryLine = m.StoryLine,
                    Year = m.Year,
                    Poster = m.Poster,
                    GenraId = m.GenraId,
                    GenraName = m.Genra.Name
                })
                .ToListAsync();
            return Ok(movies);
        }


        [HttpGet("genraId")]
        public async Task<IActionResult> GetByGenraIdAsync(byte genraId)
        {
            var movies = await _context.Movies
                .Where(m=>m.Genra.Id == genraId)
                .OrderByDescending(x => x.Rate)
                .Include(g => g.Genra)
                .Select(m => new MovieDetalisDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Rate = m.Rate,
                    StoryLine = m.StoryLine,
                    Year = m.Year,
                    Poster = m.Poster,
                    GenraId = m.GenraId,
                    GenraName = m.Genra.Name
                })
                .ToListAsync();
            return Ok(movies);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(int Id)
        {
            var movie =  await _context.Movies
                .Include(g => g.Genra)
                .SingleOrDefaultAsync(m=>m.Id==Id);

            if (movie == null)
                return NotFound();

            var dto = new MovieDetalisDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Rate = movie.Rate,
                StoryLine = movie.StoryLine,
                Year = movie.Year,
                Poster = movie.Poster,
                GenraId = movie.GenraId,
                GenraName = movie.Genra.Name
            }; 

            return Ok(dto);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is Required .. !");

            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only Jpg or Png images Allowed !!");
            

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max Length is 1Mb ");

            var isvalidGenra = await _context.Genras.AnyAsync(g => g.Id == dto.GenraId);

            if (!isvalidGenra)
                return BadRequest("Invalid Genra ID ..!");



            using var datastrem = new MemoryStream();

            await dto.Poster.CopyToAsync(datastrem);

            var movie = new Movie
            {
                GenraId = dto.GenraId,
                Title = dto.Title,
                Year = dto.Year,
                Poster = datastrem.ToArray(),
                StoryLine = dto.StoryLine,
                Rate = dto.Rate,
            };

            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return BadRequest("Movie Not Found ..! ");

            var isvalidGenra = await _context.Genras.AnyAsync(g => g.Id == dto.GenraId);

            if (!isvalidGenra)
                return BadRequest("Invalid Genra ID ..!");

            if (dto.Poster != null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only Jpg or Png images Allowed !!");


                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max Length is 1Mb ");

                using var datastrem = new MemoryStream();

                await dto.Poster.CopyToAsync(datastrem);

                movie.Poster = datastrem.ToArray();
            }

           


            movie.GenraId = dto.GenraId;
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.StoryLine = dto.StoryLine;
            movie.Rate = dto.Rate;

            _context.SaveChanges();

            return Ok(movie);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie =  await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound("Movie Not Found ..! ");

            _context.Remove(movie);
            _context.SaveChanges();


            return Ok(movie);
            
        }
    }
}
