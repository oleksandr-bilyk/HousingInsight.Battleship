using System;
using System.Diagnostics;

namespace HousingInsight.Battleship
{
    public sealed class ShipCoordinate
    {
        public static int BoardWidth => 10;
        public static int BoardHeight => 10;

        /// <summary>
        /// Position inside of the board.
        /// </summary>
        public ShipCoordinate(int x, int y)
        {
            if (x < 0 || x >= BoardWidth)
            {
                string errorMessage = $"Horizontal position should be in range {0}-{ShipCoordinate.BoardWidth}";
                Debug.Fail(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            if (y < 0 || y >= BoardHeight)
            {
                string errorMessage = $"Vertical position should be in range {0}-{ShipCoordinate.BoardWidth}";
                Debug.Fail(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            X = x;
            Y = y;
        }

        public static ShipCoordinate Zero => new ShipCoordinate(0, 0);

        public static bool TryConstruct(int x, int y, out ShipCoordinate position)
        {
            position = null;
            if (x < 0 || x >= BoardWidth)
            {
                return false;
            }
            position = null;
            if (y < 0 || y >= BoardHeight)
            {
                return false;
            }

            position = new ShipCoordinate(x, y);
            return true;
        }

        public int X { get; }
        public int Y { get; }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as ShipCoordinate);
        public bool Equals(ShipCoordinate other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(ShipCoordinate x, ShipCoordinate y)
        {
            if (object.ReferenceEquals(x, null)) return y.Equals(x);
            else return x.Equals(y);
        }

        public static bool operator !=(ShipCoordinate x, ShipCoordinate y) => !(x == y);
    }
}
