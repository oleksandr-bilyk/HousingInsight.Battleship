using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HousingInsight.Battleship
{
    public sealed class Ship
    {
        public Ship(
            ShipClass shipClass, 
            ShipOrientation orientation, 
            ShipCoordinate headCoordinate, 
            List<ShipCoordinate> cells
        )
        {
            ShipClass = shipClass ?? throw new ArgumentNullException(nameof(shipClass));
            Orientation = orientation;
            HeadCoordinate = headCoordinate ?? throw new ArgumentNullException(nameof(headCoordinate));

            if (!ShipAllocationLogic.ValidateShipBarier(headCoordinate, shipClass.Length, orientation))
                throw new ArgumentException("Ship out of the board.");

            Cells = cells.Select(item => new ShipCell(item)).ToList();
        }

        public ShipClass ShipClass { get; }
        public ShipOrientation Orientation { get; }
        public ShipCoordinate HeadCoordinate { get; }
        public List<ShipCell> Cells { get; }
        public bool IsDestroyed => Cells.All(item => item.Hit);

        public static Ship ConstructShipWithCoordinate(
            ShipClass shipClass,
            ShipOrientation orientation,
            ShipCoordinate headCoordinate
        )
        {
            var result = TryConstructShipWithCoordinate(shipClass, orientation, headCoordinate);
            if (result.Item1) return result.Item2;
            else throw new ArgumentException("Not good ship class/orientation/headCoordinate.");
        }

        public static Tuple<bool, Ship> TryConstructShipWithCoordinate(
            ShipClass shipClass,
            ShipOrientation orientation,
            ShipCoordinate headCoordinate
        )
        {
            var cellsResult = ShipAllocationLogic.TryGetCellsOnBoard(headCoordinate, shipClass.Length, orientation);
            if (cellsResult.Item1)
            {
                return new Tuple<bool, Ship>(true, new Ship(shipClass, orientation, headCoordinate, cellsResult.Item2));
            }
            else
            {
                return new Tuple<bool, Ship>(false, null);
            }
        }

        public bool TryHit(ShipCoordinate coordinate)
        {
            ShipCell hitCell = Cells.FirstOrDefault(item => item.Coordinate == coordinate);
            if (hitCell != null)
            {
                hitCell.Hit = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
