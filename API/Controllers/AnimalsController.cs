using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly DataContext _context;
        public AnimalsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals()
        {
            var animals = await _context.Animals.ToListAsync();
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            return Ok(animal);
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> AddAnimal(Animal animal)
        {
            var newAnimal = await _context.Animals.AddAsync(animal);
            await _context.SaveChangesAsync();
            return animal;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAllAnimals()
        {
            _context.Animals.RemoveRange(_context.Animals);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}