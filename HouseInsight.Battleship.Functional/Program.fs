// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Domain
open CoordinateParser
open System

let printGameTitle () =
    printfn "The Game of Battleship."
    printfn "Enter coordinate. %s" (CoordinateParser.coordinateParsingFormatHelp ())
    printfn "Enter 'exit' word to exit."

[<EntryPoint>]
let main argv = 
    printGameTitle ()
    let shipsInitial = randomShipBoardWithDefaultSet ()
    let rec iter shipsInput =
        let command = System.Console.ReadLine();
        if command.ToUpper() = "EXIT" then ()
        else
            match tryParseCoordinate command with
            | Success shotCoordinate -> 
                match shotToGameBoard shipsInput shotCoordinate with
                | GameContinue(ships = shipsUpdated; hit = hitDetected) ->
                    if hitDetected then Console.WriteLine("Hit shot.");
                    else Console.WriteLine("Missed shot.");
                    iter shipsUpdated
                | GameWin -> 
                    Console.WriteLine("All battheships destroyed.");
                    ()
            | Error(m) -> 
                printfn "%s" m
                iter shipsInput
    iter shipsInitial
    printfn "%A" argv
    0 // return an integer exit code
