using Microsoft.AspNetCore.Mvc;
using PlumGuidePlutoRover.Api.Controllers;
using Xunit;

namespace PlumGuidePlutoRover.Api.Tests
{
    public class MoveControllerTests
    {
        private readonly Grid _grid;
        private readonly Rover _rover;

        public MoveControllerTests()
        {
            _grid = new Grid(10, 10);
            _rover = new Rover(_grid, 0, 0, Heading.N);
        }

        [Theory]
        [InlineData("F", "0,1,N")]
        [InlineData("FFRFF", "2,2,E")]
        public void Travel_Returns_Correct_Location(string command, string expectedLocation)
        {
            var controller = new MoveController(_rover);

            var response = controller.Travel(command);
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedLocation, result.Value);
        }

        [Fact]
        public void Travel_Returns_Correct_Location_WithObstacle()
        {
            _grid.SetObstacle(5, 5, true);

            var controller = new MoveController(_rover);

            var response = controller.Travel("FFFFFRFFFFF");
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal("4,5,E", result.Value);
        }

        [Theory]
        [InlineData("FFFFFFF", "5,2,N")]
        [InlineData("BBBBBBB", "5,8,N")]
        [InlineData("LFFFFFFF", "8,5,W")]
        [InlineData("LBBBBBBB", "2,5,W")]
        public void Travel_Returns_Correct_Location_WhenWrapped(string commands, string expectedLocation)
        {
            var rover = new Rover(_grid, 5, 5, Heading.N);

            var controller = new MoveController(rover);

            var response = controller.Travel(commands);
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedLocation, result.Value);
        }

        [Fact]
        public void Travel_Returns_BadRequest_If_InvalidCommand()
        {
            var controller = new MoveController(_rover);

            var response = controller.Travel("A");
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}