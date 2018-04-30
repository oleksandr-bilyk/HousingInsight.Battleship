module CoordinateParser
open System
open Domain
open FParsec

let minXChar = 'A';
let maxXChar = Convert.ToChar(Convert.ToInt32(minXChar) + boardWidth - 1)
let minY = 1
let maxY = boardHeight;
type Result = | Success of Coordinate | Error of string

// TODO: Refactor to FParsec parser combinator or RegEx type provider
let tryParseCoordinate (command:string) =
    if command.Length < 2 then
        Error("Too short line.");
    else
        let xChar = command.[0]
        let x = Convert.ToInt32(xChar) - System.Convert.ToInt32(minXChar)
        if x < 0 || x >= boardWidth then
            Error(sprintf "First char should be from '%c' to '%c'." minXChar maxXChar)
        else
            let yString = command.Substring(1);
            let (b, y) = Int32.TryParse(yString)
            if not b then
                Error("Cannot parse Y coordinate.");
            else 
                if y < minY || y > maxY then
                    Error(sprintf "Y coordinate must be between %i and %i." minY maxY);
                else 
                    Success({x = x; y = y - 1})
let getCoordinate command =
    match tryParseCoordinate command with
    | Success(v) -> v
    | Error(m) -> raise (InvalidOperationException(m)) 

let coordinateParsingFormatHelp () = 
    let sampleCoordinateString = "D7";
    let sampleCoordinate = getCoordinate sampleCoordinateString;
    [
        sprintf "X coordinate is between '%c' to '%c'." minXChar maxXChar
        sprintf " Y coordinate is between {%i} and {%i}." minY maxY
        sprintf 
            "Fox example %s coordinate means X=%i|Y=%i" 
            sampleCoordinateString sampleCoordinate.x (sampleCoordinate.y + 1)
    ]
    |> String.concat ""
