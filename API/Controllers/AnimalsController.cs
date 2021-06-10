using System.Collections.Generic;
using System.Linq;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Animal>> GetAnimals()
        {
            var animals = _context.Animals.ToList();
            return animals;
        }

        [HttpGet("{id}")]
        public ActionResult<Animal> GetAnimal(int id)
        {
            var animal = _context.Animals.Find(id);
            return animal;
        }
    }
}