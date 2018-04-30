using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HousingInsight.Battleship
{
    public static class ShipAllocationLogic
    {
        public static bool TestNoOverlap(Ship newShip, IEnumerable<Ship> existingShips) => 
            existingShips.SelectMany(
                ship => ship.Cells.Select(cell => cell.Coordinate)
            ).Intersect(
                newShip.Cells.Select(cell => cell.Coordinate)
            ).Count() == 0;

        /// <summary>
        /// Returns ship cells if it doesn't go out of the board.
        /// </summary>
        public static Tuple<bool, List<ShipCoordinate>> TryGetCellsOnBoard(ShipCoordinate headPosition, int length, ShipOrientation orientation)
        {
            var positionUpdate = GetShipDirectionPositionUpdate(orientation);
            ShipCoordinate currentPosition = headPosition;
            var resultList = new List<ShipCoordinate> { currentPosition };
            for (int tailIndex = 1; tailIndex < length; tailIndex++)
            {
                var update = positionUpdate(currentPosition);
                if (update.Item1)
                {
                    currentPosition = update.Item2;
                    resultList.Add(update.Item2);
                }
                else
                {
                    return new Tuple<bool, List<ShipCoordinate>>(false, new List<ShipCoordinate>());
                }
            }
            return new Tuple<bool, List<ShipCoordinate>>(true, resultList);
        }

        public static Func<ShipCoordinate, Tuple<bool, ShipCoordinate>> GetShipDirectionPositionUpdate(ShipOrientation orientation)
        {
            switch (orientation)
            {
                case ShipOrientation.Horizontal: return TryIncrementHorizontalPosition;
                case ShipOrientation.Vertical: return TryIncrementVerticalPosition;
                default:
                    Debug.Fail("Not supported.");
                    throw new NotSupportedException();
            }
        }

        public static bool ValidateShipBarier(ShipCoordinate position, int length, ShipOrientation orientation)
        {
            var getShipDirectionPositionUpdate = GetShipDirectionPositionUpdate(orientation);
            ShipCoordinate currentPosition = position;
            for (int i = 1; i < length; i++)
            {
                var update = getShipDirectionPositionUpdate(currentPosition);
                if (update.Item1)
                {
                    currentPosition = update.Item2;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static Tuple<bool, ShipCoordinate> TryIncrementHorizontalPosition(ShipCoordinate position)
        {
            ShipCoordinate newPosition;
            if (ShipCoordinate.TryConstruct(position.X + 1, position.Y, out newPosition))
            {
                return new Tuple<bool, ShipCoordinate>(true, newPosition);
            }
            else
            {
                return new Tuple<bool, ShipCoordinate>(false, ShipCoordinate.Zero);
            }
        }

        public static Tuple<bool, ShipCoordinate> TryIncrementVerticalPosition(ShipCoordinate position)
        {
            ShipCoordinate newPosition;
            if (ShipCoordinate.TryConstruct(position.X, position.Y + 1, out newPosition))
            {
                return new Tuple<bool, ShipCoordinate>(true, newPosition);
            }
            else
            {
                return new Tuple<bool, ShipCoordinate>(false, ShipCoordinate.Zero);
            }
        }
    }
}
