namespace PlumGuidePlutoRover.Api
{
    public class Grid
    {
        public bool[,] _grid { get; private set; }

        public int MinX => 0;

        public int MinY => 0;

        public int MaxX { get; private set; }

        public int MaxY { get; private set; }

        public Grid()
        {
            _grid = new bool[100, 100];

            MaxX = 99;
            MaxY = 99;
        }

        public Grid(int x, int y)
        {
            _grid = new bool[x, y];

            MaxX = x - 1;
            MaxY = y - 1;
        }

        public bool SetObstacle(int x, int y, bool hasObstacle)
        {
            if (x > _grid.GetLength(0) || x < 0 || y > _grid.GetLength(1) || y < 0)
            {
                return false;
            }

            _grid[x, y] = hasObstacle;

            return true;
        }

        public bool HasObstacle(int x, int y)
        {
            return _grid[x, y];
        }
    }
}