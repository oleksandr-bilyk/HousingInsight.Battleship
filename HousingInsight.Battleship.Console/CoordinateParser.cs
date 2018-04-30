using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingInsight.Battleship.ConsoleApp
{
    public static class CoordinateParser
    {
        public static char MinXChar => 'A';
        public static char MaxXChar => Convert.ToChar(Convert.ToInt32(MinXChar) + ShipCoordinate.BoardWidth - 1);
        public static int MinY => 1;
        public static int MaxY => ShipCoordinate.BoardHeight;
        public static string GetHelp()
        {
            var sampleCoordinateString = "D7";
            var sampleCoordinate = GetCoordinate(sampleCoordinateString);
            return $"X coordinate is between '{MinXChar}' to '{MaxXChar}'."
                + $" Y coordinate is between {MinY} and {MaxY}."
                + $"Fox example {sampleCoordinateString} coordinate means X={sampleCoordinate.X}|Y={sampleCoordinate.Y + 1}";
        }

        private static ShipCoordinate GetCoordinate(string command)
        {
            ShipCoordinate result;
            if (TryGetCoordinate(command, out result)) return result;
            else throw new ArgumentException();
        }

        public static bool TryGetCoordinate(string command, out ShipCoordinate coordinate)
        {
            coordinate = null;
            if (command.Length < 2)
            {
                System.Console.Error.WriteLine("Too short line.");
                return false;
            }
            char xChar = command[0];
            
            
            int x = Convert.ToInt32(xChar) - System.Convert.ToInt32(MinXChar);
            if (x < 0 || x >= ShipCoordinate.BoardWidth)
            {
                System.Console.Error.WriteLine($"First char should be from '{MinXChar}' to '{MaxXChar}'.");
                return false;
            }

            string yString = command.Substring(1);
            if (!int.TryParse(yString, out int y))
            {
                System.Console.Error.WriteLine("Cannot parse Y coordinate.");
                return false;
            }
            if (y < MinY || y > MaxY)
            {
                System.Console.Error.WriteLine($"Y coordinate must be between {MinY} and {MaxY}.");
                return false;
            }

            coordinate = new ShipCoordinate(x, y - 1);
            return true;
        }
    }
}
