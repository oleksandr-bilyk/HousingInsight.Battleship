module Domain

open System

let boardWidth = 10;
let boardHeight = 10;
type Coordinate = { x: int; y:int }
type ShipClass = { length : int; name : string }
type Orientation = | Horizontal | Vertical
let battleshipClass = { length = 5; name = "Battleship" }
let destroyerClass = { length = 4; name = "Destroyer" };

let seqByRundomizer length random = 
    let usedIndexes = Array.create length false
    let nextIndexFromRange freeCellsCount =
        let randomFreeCellIndex = random freeCellsCount
        let rec findIndex currentIndex currentFreeIndex =
            let nextFindIndex = currentIndex + 1 |> findIndex
            if usedIndexes.[currentIndex] then 
                currentFreeIndex |> nextFindIndex
            else if currentFreeIndex < randomFreeCellIndex then
                currentFreeIndex + 1 |> nextFindIndex
            else 
                usedIndexes.[currentIndex] <- true
                currentIndex
        findIndex 0 0
    seq { length .. -1 .. 1 } |> Seq.map nextIndexFromRange

let allCoordinatesByRundomizer width height random = 
    let totalIter = width * height
    seqByRundomizer totalIter random |> Seq.map 
        (fun freeCellIndex -> { x = freeCellIndex % width; y = freeCellIndex / width})
let random =
    let random = new Random()
    let next max = random.Next(max)
    next
let allCoordinatesRandomBySize width height =
    let random = random
    allCoordinatesByRundomizer width height random

let defaultShipSet =
    [ 
        battleshipClass, 1
        destroyerClass, 2 
    ]

type CoordinateOrientation = (Coordinate*Orientation)

let seqAppendRandom =
    let random = new Random()
    let f horizontal vertical =
        if random.Next(2) = 0 then Seq.append horizontal vertical 
        else Seq.append vertical horizontal
    f

let shipVolumeCoordinates headCoordinate orientation length =
    let next = 
        match orientation with 
        | Horizontal -> (fun c -> { x = c.x + 1 ; y = c.y }) 
        | Vertical -> (fun c -> { x = c.x ; y = c.y + 1 })
    Seq.unfold 
        (fun (l, h) -> if l < 1 then None else Some (h, ((l - 1), (next h))))
        (length, headCoordinate)
let allCoordinatesByLength length width height = 
    let horizontalCoordinates = 
        allCoordinatesRandomBySize (width - length + 1) height
        |> Seq.map (fun c -> (c, Horizontal))
    let verticalCoordinates = 
        allCoordinatesRandomBySize width (height - length + 1)
        |> Seq.map (fun c -> (c, Vertical))
    seqAppendRandom horizontalCoordinates verticalCoordinates
let allCoordinatesByLengthDefaultBoard length = allCoordinatesByLength length boardWidth boardHeight

type CoordinateOrientationShipClass = { headCoordinate: Coordinate; orientation: Orientation; shipClass:ShipClass }

let coordinateOrientationShipClassVolumeSet e = 
    shipVolumeCoordinates e.headCoordinate e.orientation e.shipClass.length
    |> Set.ofSeq

let setsHaveIntersection a b = Set.intersect a b |> Set.count |> (>) <| 0

let tryAppendShipClass length (existing : CoordinateOrientationShipClass list) = 
    let existingAsSets = existing |> List.map coordinateOrientationShipClassVolumeSet 
    length |> allCoordinatesByLengthDefaultBoard |> Seq.tryPick 
        (fun (coordinate, orientation) -> 
            let candidateVolume = shipVolumeCoordinates coordinate orientation length |> Set.ofSeq
            let existingIntersec existing = setsHaveIntersection existing candidateVolume
            if List.exists existingIntersec existingAsSets then None
            else Some(coordinate, orientation)
        )
        
let randomShipBoardBySet shipSet  = 
    let inputSetAsCollection = List.collect (fun (s, c) -> [ for _ in 1 .. c -> s]) shipSet
    let rec foldShips shipList resultList =
        match shipList with
        | [] -> Some(resultList)
        | (h::t) -> 
            match tryAppendShipClass h.length resultList with
            | None -> None
            | Some(coordinate, orientation) -> 
                let newShip = { headCoordinate = coordinate; orientation = orientation; shipClass = h}
                foldShips t (newShip::resultList)
    foldShips inputSetAsCollection []

type ShipActor = { shipClass:ShipClass; volume: Set<Coordinate> }
let shipActorFromDeclaration ship =
    let volume = shipVolumeCoordinates ship.headCoordinate ship.orientation ship.shipClass.length |> Set.ofSeq
    { shipClass = ship.shipClass; volume = volume }
let randomShipBoardWithDefaultSet () = 
    match randomShipBoardBySet defaultShipSet with
    | Some(allocation) -> allocation |> List.map shipActorFromDeclaration
    | None -> raise (InvalidOperationException("Not enough space for default ships."))

type ShotResult = 
    | GameContinue of ships : ShipActor list * hit : bool 
    | GameWin

let shotToGameBoard (shipsInput : ShipActor list) (hitCoordinate : Coordinate) = 
    let rec iter shipsInput anyLiveShipDetected hitDetected shipsOutput =
        match shipsInput with
        | [] -> 
            if anyLiveShipDetected then GameContinue(shipsOutput, hitDetected)
            else GameWin
        | h::t -> 
            let shipUpdated, hitHappenedInCurrentIter = 
                if hitDetected then h, false
                else 
                    let updatedHeatCells = Set.remove hitCoordinate h.volume
                    if updatedHeatCells.Count = h.volume.Count then h, false
                    else { h with volume = updatedHeatCells }, true
            let hitDetectedOutput = if hitDetected then true else hitHappenedInCurrentIter
            let anyLiveShipDetectedOutput = if anyLiveShipDetected then true else shipUpdated.volume.Count > 0
            iter t anyLiveShipDetectedOutput hitDetectedOutput (shipUpdated::shipsOutput)
    iter shipsInput false false []
