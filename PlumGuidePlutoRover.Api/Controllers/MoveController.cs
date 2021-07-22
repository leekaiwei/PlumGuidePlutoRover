using Microsoft.AspNetCore.Mvc;
using System;

namespace PlumGuidePlutoRover.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoveController : ControllerBase
    {
        private readonly IRover _rover;

        public MoveController(IRover rover)
        {
            _rover = rover;
        }

        [HttpPost]
        public IActionResult Travel([FromQuery] string commands)
        {
            foreach (var command in commands)
            {
                if (Enum.TryParse(command.ToString(), out TravelDirection moveDirection))
                {
                    _rover.Travel(moveDirection);
                }
                else if (Enum.TryParse(command.ToString(), out TurnDirection turnDirection))
                {
                    _rover.Turn(turnDirection);
                }
                else
                {
                    return BadRequest($"Invalid command {command}");
                }
            }

            return Ok(_rover.Location);
        }
    }
}