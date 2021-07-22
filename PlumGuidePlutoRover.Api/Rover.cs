using System;
using System.Collections.Generic;
using System.Linq;

namespace PlumGuidePlutoRover.Api
{
    public class Rover : IRover
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Heading Heading { get; private set; } = Heading.N;
        public string Location => $"{X},{Y},{Heading}";
        
        private readonly Heading _minHeading = Enum.GetValues(typeof(Heading)).Cast<Heading>().Min();
        private readonly Heading _maxHeading = Enum.GetValues(typeof(Heading)).Cast<Heading>().Max();

        private readonly IReadOnlyDictionary<Heading, Axis> _headingToAxis = new Dictionary<Heading, Axis>
        {
            { Heading.N, Axis.Y },
            { Heading.S, Axis.Y },
            { Heading.E, Axis.X },
            { Heading.W, Axis.X },
        };

        private Axis _axis = Axis.Y;

        private Grid _grid { get; set; }

        public Rover(Grid grid, int startX, int startY, Heading startHeading)
        {
            if (startX < grid.MinX || startX > grid.MaxX || startY < grid.MinY || startY > grid.MaxY)
            {
                throw new ArgumentOutOfRangeException();
            }

            X = startX;
            Y = startY;

            _grid = grid;

            Heading = startHeading;
        }

        public virtual bool Travel(TravelDirection move)
        {
            var velocity = 1;
            if (Heading == Heading.S || Heading == Heading.W)
            {
                velocity = -1;
            }

            var isMoved = false;
            if (Heading == Heading.N || Heading == Heading.S)
            {
                var newY = MoveRover(Y, velocity, move);
                if (!_grid.HasObstacle(X, newY))
                {
                    Y = newY;
                    isMoved = true;
                }
            }
            else if (Heading == Heading.E || Heading == Heading.W)
            {
                var newX = MoveRover(X, velocity, move);
                if (!_grid.HasObstacle(newX, Y))
                {
                    X = newX;
                    isMoved = true;
                }
            }

            return isMoved;
        }

        public virtual void Turn(TurnDirection direction)
        {
            switch (direction)
            {
                case TurnDirection.R:
                    Heading += 1;
                    break;
                case TurnDirection.L:
                    Heading -= 1;
                    break;
                default:
                    throw new ArgumentException($"Invalid turn direction {direction}");
            }

            if (Heading < _minHeading)
            {
                Heading = _maxHeading;
            }
            else if (Heading > _maxHeading)
            {
                Heading = _minHeading;
            }

            _axis = _headingToAxis[Heading];
        }

        private int MoveRover(int currentLocation, int velocity, TravelDirection move)
        {
            int newLocation;
            switch (move)
            {
                case TravelDirection.F:
                    newLocation = currentLocation + velocity;
                    break;
                case TravelDirection.B:
                    newLocation = currentLocation - velocity;
                    break;
                default:
                    throw new ArgumentException($"Invalid move direction {move}");
            }

            if (_axis == Axis.X)
            {
                return WrapLocation(newLocation, _grid.MinX, _grid.MaxX);
            }

            return WrapLocation(newLocation, _grid.MinY, _grid.MaxY);
        }

        private static int WrapLocation(int location, int minLocation, int maxLocation)
        {
            if (location < minLocation)
            {
                return maxLocation;
            }

            if (location > maxLocation)
            {
                return minLocation;
            }

            return location;
        }
    }
}