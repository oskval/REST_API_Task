using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        List<AnimalDto> InitialAnimals = new List<AnimalDto>();
        HttpClientHandler _clientHandler = new HttpClientHandler();
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
            InitialAnimals = new List<AnimalDto>();
             using (var httpClient = new HttpClient(_clientHandler))
            {
                using(var response=await httpClient.GetAsync("https://cat-fact.herokuapp.com/facts/random?animal_type=cat&amount=" + count))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    InitialAnimals = JsonConvert.DeserializeObject<List<AnimalDto>>(apiResponse);
                }
            }
            var newAnimal = new Animal();

            foreach(var animalDto in InitialAnimals)
            {
                newAnimal = _mapper.Map<Animal>(animalDto);
                await _context.Animals.AddAsync(newAnimal);

            }
            await _context.SaveChangesAsync();

            return Ok(InitialAnimals);
        }

    }
}