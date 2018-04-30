using System;
using System.Diagnostics;

namespace HousingInsight.Battleship
{
    public sealed class ShipClass
    {
        public ShipClass(int length, string name)
        {
            if (length < 0 || length > ShipCoordinate.BoardWidth || length > ShipCoordinate.BoardHeight)
            {
                string errorMessage = $"Ship is too long to to be located on the board.";
                Debug.Fail(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            Length = length;
            Name = name;
        }

        public static ShipClass Battleship => new ShipClass(5, "Battleship");
        public static ShipClass Destroyer => new ShipClass(4, "Destroyer");

        public int Length { get; }
        public string Name { get; }
    }
    

}
