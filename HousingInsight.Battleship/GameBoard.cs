using System;
using System.Collections.Generic;
using System.Linq;

namespace HousingInsight.Battleship
{
    public sealed class GameBoard
    {
        /// <remarks>
        /// I have decided to store only ships and their positions on the field.
        /// Two-dimensional array will ship cells linked to ship entities may be created later 
        /// to improve field testing performance.
        /// </remarks>
        private readonly List<Ship> shipList;
        public GameBoard(List<Ship> shipList)
        {
            this.shipList = shipList;
        }

        public static GameBoard NewRandomGame()
        {
            List<Ship> list;
            if (TryGetRandom(GetDefaultSet(), out list)) return new GameBoard(list);
            else throw new InvalidOperationException("Default set should always be compatible.");
        }

        public ShotResult Shot(ShipCoordinate coordinate)
        {
            if (shipList.Any(ship => ship.TryHit(coordinate)))
            {
                if (shipList.All(ship => ship.IsDestroyed)) return ShotResult.GameWin;
                else return ShotResult.Hit;
            }
            else return ShotResult.Miss;
        }

        public static bool TryGetRandom(Dictionary<ShipClass, int> registry, out List<Ship> list)
        {
            var resultList = new List<Ship>();
            foreach (var classItem in GetDefaultSet())
            {
                for (int i = 0; i < classItem.Value; i++)
                {
                    if (TryGetShipToAdd(classItem.Key, resultList, out Ship newShip))
                    {
                        resultList.Add(newShip);
                    }
                    else
                    {
                        list = null;
                        return false;
                    }
                }
            }
            list = resultList;
            return true;
        }

        public static Dictionary<ShipClass, int> GetDefaultSet() => new Dictionary<ShipClass, int>
        {
            { ShipClass.Battleship, 1 },
            { ShipClass.Destroyer, 2 },
        };

        /// <remarks>
        /// We should add ships without mutual overlay and without out of game board.
        /// Ships position should be maximally random.
        /// </remarks>
        private static bool TryGetShipToAdd(ShipClass shipClass, IEnumerable<Ship> existingShips, out Ship newShip)
        {
            foreach (Tuple<ShipCoordinate, ShipOrientation> item in GetRandomShipCoordinateAndOrientationInBorder(shipClass.Length))
            {
                var newShipCandidate = Ship.ConstructShipWithCoordinate(shipClass, item.Item2, item.Item1);
                if (ShipAllocationLogic.TestNoOverlap(newShipCandidate, existingShips))
                {
                    newShip = newShipCandidate;
                    return true;
                }
            }
            newShip = null;
            return false;
        }

        private static IEnumerable<Tuple<ShipCoordinate, ShipOrientation>> GetRandomShipCoordinateAndOrientationInBorder(int shipLength)
        {
            return GetRandomShipCoordinateAndOrientation().Where(item => ShipAllocationLogic.ValidateShipBarier(item.Item1, shipLength, item.Item2));
        }

        private static IEnumerable<Tuple<ShipCoordinate, ShipOrientation>> GetRandomShipCoordinateAndOrientation()
        {
            return
                from headCoordinate in ShipPositionRandomEnumerator.GetAllCoordinatesRandomEnumerable()
                from orientation in GetAllOrientation()
                select new Tuple<ShipCoordinate, ShipOrientation>(headCoordinate, orientation);
        }

        private static IEnumerable<ShipOrientation> GetAllOrientation() => new ShipOrientation[]
        {
            ShipOrientation.Horizontal,
            ShipOrientation.Vertical,
        };
    }
}
