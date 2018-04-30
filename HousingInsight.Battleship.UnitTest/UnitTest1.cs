using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace HousingInsight.Battleship.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRandom()
        {
            int width = 3;
            int height = 5;
            var result = ShipPositionRandomEnumerator.GetAllCoordinatesRandomEnumerableCore(width, height).ToList();
            Assert.AreEqual(result.Count, result.Distinct(new TupleEqualityComparer()).Count());

            // All items are present.
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var item = new Tuple<int, int>(x, y);
                    Assert.IsTrue(result.Contains(item, new TupleEqualityComparer()));
                }
            }
        }

        [TestMethod]
        public void TestOverlap()
        {
            Assert.IsFalse(
                ShipAllocationLogic.TestNoOverlap(
                    Ship.ConstructShipWithCoordinate(ShipClass.Destroyer, ShipOrientation.Horizontal, new ShipCoordinate(1, 7)),
                    new Ship[]
                    {
                        Ship.ConstructShipWithCoordinate(ShipClass.Destroyer, ShipOrientation.Horizontal, new ShipCoordinate(1, 7)),
                    }
                )
            );

            Assert.IsFalse(
                ShipAllocationLogic.TestNoOverlap(
                    Ship.ConstructShipWithCoordinate(ShipClass.Destroyer, ShipOrientation.Horizontal, new ShipCoordinate(0, 7)),
                    new Ship[]
                    {
                        Ship.ConstructShipWithCoordinate(ShipClass.Battleship, ShipOrientation.Horizontal, new ShipCoordinate(1, 7)),
                    }
                )
            );

            Assert.IsTrue(
                ShipAllocationLogic.TestNoOverlap(
                    Ship.ConstructShipWithCoordinate(ShipClass.Destroyer, ShipOrientation.Horizontal, new ShipCoordinate(0, 7)),
                    new Ship[]
                    {
                        Ship.ConstructShipWithCoordinate(ShipClass.Battleship, ShipOrientation.Horizontal, new ShipCoordinate(1, 6)),
                    }
                )
            );

            Assert.IsFalse(
                ShipAllocationLogic.TestNoOverlap(
                    Ship.ConstructShipWithCoordinate(ShipClass.Destroyer, ShipOrientation.Horizontal, new ShipCoordinate(0, 1)),
                    new Ship[]
                    {
                        Ship.ConstructShipWithCoordinate(ShipClass.Battleship, ShipOrientation.Vertical, new ShipCoordinate(1, 0)),
                    }
                )
            );
        }

        private sealed class TupleEqualityComparer : IEqualityComparer<Tuple<int, int>>
        {
            public bool Equals(Tuple<int, int> x, Tuple<int, int> y) => x.Item1 == y.Item1 && x.Item2 == y.Item2;

            public int GetHashCode(Tuple<int, int> obj) => obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
        }
    }
}
