using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using SerilogExample.Models;

namespace SerilogExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FruitController : ControllerBase
    {
        private static readonly ConcurrentBag<Fruit> Fruits = new () {new("Orange" , 0) , new ("Banana" , 10)};

        private readonly ILogger<FruitController> _logger;

        public FruitController(ILogger<FruitController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Fruit>) , StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            _logger.LogDebug("Returning fruits");
            return Ok(Fruits.AsEnumerable());
        }

        [HttpGet("{fruitName}")]
        [ProducesResponseType(typeof(Fruit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByFruitName(string fruitName)
        {
            using var scope = _logger.BeginScope($"Getting {fruitName} from database");

            var fruit = Fruits.Where(c => c.Name.Equals(fruitName , StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (fruit is not Fruit)
            {
                _logger.LogError($"{fruitName} does not exist");
                return NotFound();
            }
            else
            {
                return Ok(fruit);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] Fruit fruit)
        {
            using var scope = _logger.BeginScope($"Adding {fruit.Name} to database");

            if (Fruits.Count(c => c.Name.Equals(fruit.Name, StringComparison.InvariantCultureIgnoreCase)) > 0)
            {
                _logger.LogWarning($"{fruit.Name} already exists");
                return BadRequest($"{fruit.Name} already exists");
            }
            else
            {
                Fruits.Add(fruit);
                _logger.LogDebug($"{fruit.Name} has been added to list");
                return CreatedAtAction(nameof(GetByFruitName), new { fruitName = fruit.Name } , fruit);
            }
        }
    }
}
