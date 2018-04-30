using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HousingInsight.Battleship.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The Game of Battleship.");
            Console.WriteLine("Enter coordinate." + CoordinateParser.GetHelp());
            Console.WriteLine("Enter 'exit' word to exit.");
            var gameBoard = GameBoard.NewRandomGame();
            while (InputIteration(gameBoard))
            {
            }
        }

        private static bool InputIteration(GameBoard gameBoard)
        {
            string command = System.Console.ReadLine();
            if (command.ToUpper() == "EXIT") return false;
            ShipCoordinate coordinate;
            if (CoordinateParser.TryGetCoordinate(command, out coordinate))
            {
                ShotResult shotResult = gameBoard.Shot(coordinate);
                switch (shotResult)
                {
                    case ShotResult.Miss:
                        Console.WriteLine("Missed shot.");
                        return true;
                    case ShotResult.Hit:
                        Console.WriteLine("Hit shot.");
                        return true;
                    case ShotResult.GameWin:
                        Console.WriteLine("All battheships destroyed.");
                        return false;
                    default: throw new NotSupportedException();
                }
            }
            else return true;
        }

        
    }
}
