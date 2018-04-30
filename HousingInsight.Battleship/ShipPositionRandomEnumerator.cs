using System;
using System.Collections.Generic;
using System.Linq;

namespace HousingInsight.Battleship
{
    public static class ShipPositionRandomEnumerator
    {
        /// <summary>
        /// Returns random cells without repeat.
        /// </summary>
        public static IEnumerable<ShipCoordinate> GetAllCoordinatesRandomEnumerable() =>
            GetAllCoordinatesRandomEnumerableCore(ShipCoordinate.BoardWidth, ShipCoordinate.BoardHeight)
                .Select(item => new ShipCoordinate(item.Item1, item.Item2));

        public static IEnumerable<Tuple<int, int>> GetAllCoordinatesRandomEnumerableCore(int matrixWidth, int matrixHeight)
        {
            int totalIter = matrixWidth * matrixHeight;

            bool[] cell = new bool[totalIter];
            var randomizer = new Random();
            for (int freeCellsCount = totalIter; freeCellsCount > 0; freeCellsCount--)
            {
                int rendomFreeCellIndex = randomizer.Next(freeCellsCount - 1);
                int currentFreeIndex = 0;
                for (int i = 0; i < cell.Length; i++)
                {
                    if (cell[i] || currentFreeIndex < rendomFreeCellIndex)
                    {
                        currentFreeIndex++;
                    }
                    else
                    {
                        cell[i] = true;
                        yield return GetCoordinateByIndex(i, matrixWidth);
                        break;
                    }
                }
            }
        }

        private static Tuple<int, int> GetCoordinateByIndex(int index, int matrixWidth) => new Tuple<int, int>(
            index % matrixWidth,
            index / matrixWidth
        );
    }
}
