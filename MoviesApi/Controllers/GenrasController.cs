using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GenrasController : ControllerBase
    {
        private readonly ApplicationDbContext _contex;

        public GenrasController(ApplicationDbContext contex)
        {
            _contex = contex;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genras = await _contex.Genras.OrderBy(g => g.Name).ToListAsync();
            return Ok(genras);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateGenrasDto dto)
        {
            var genra = new Genra { Name = dto.Name };
            await _contex.Genras.AddAsync(genra);
            _contex.SaveChanges();
            return Ok(genra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CreateGenrasDto dto)
        {
            var genra = await _contex.Genras.FirstOrDefaultAsync(g => g.Id == id);

            if (genra == null)
                return BadRequest($"No Genra was found With id = {id}");

            genra.Name = dto.Name;

            _contex.SaveChanges();

            return Ok(genra);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        { 
            var genra = _contex.Genras.FirstOrDefault(g => g.Id == id);

            if (genra == null)
                return BadRequest($"No Genra was found With id = {id}");

            _contex.Genras.Remove(genra);
            _contex.SaveChanges();

            return Ok(genra);
        }

    }
}
