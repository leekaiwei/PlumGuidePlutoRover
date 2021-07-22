# PlumGuidPlutoRover

API to control Pluto Rover made in C# ASP.NET 5.

## Information

There is a `MoveController` with a single endpoint `Travel()` `http://localhost:11703/travel`, which accepts a string input of movement commands e.g. `FFRFF` in the query parameter called `commands` via a POST request.

There is `Rover` class which holds information needed to make decisions on how to move itself. It implements `IRover`, which has methods defined for movement. It requires a `Grid` object to obtain some of this information.

The `Grid` class holds the grid information that the rover can travel to as well as any grid location that has obstacles within them.

The valid directions possible are separated into `TravelDirection` and `TurnDirection` as one changes the X,Y coordinate while the other only changes the facing direrction.