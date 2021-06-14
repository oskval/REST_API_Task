using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AnimalsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<AnimalDto>> CreateAnimal(AnimalDto animalDto)
        {;
            var newAnimal = _mapper.Map<Animal>(animalDto);
            await _context.Animals.AddAsync(newAnimal);
            await _context.SaveChangesAsync();
            return Ok(animalDto);
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
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAnimal(int id, AnimalDto animalDto)
        {
            var animal = await _context.Animals.FindAsync(id);
            _mapper.Map(animalDto, animal);
            _context.Animals.Update(animal);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Does not work

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateAnimal(int id, JsonPatchDocument<AnimalDto> patchDoc)
        {
            var animalFromContext = await _context.Animals.FindAsync(id);
            var animalToPatch = _mapper.Map<AnimalDto>(animalFromContext);
            patchDoc.ApplyTo(animalToPatch, ModelState);

            _mapper.Map(animalToPatch, animalFromContext);
            _context.Animals.Update(animalFromContext);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AddInitialData/{count}")]
        public async Task<ActionResult<IEnumerable<Animal>>> AddInitialData(int count)
        {
            
            return Ok();
        }

    }
}