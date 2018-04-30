
#r "./../packages/FParsec.1.0.3/lib/net40-client/FParsecCS.dll"
#r "./../packages/FParsec.1.0.3/lib/net40-client/FParsec.dll"


open FParsec

let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

test pfloat "1.25"

