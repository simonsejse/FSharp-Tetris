module Tetris
open Canvas

type Color =
    | Yellow
    | Cyan
    | Blue
    | Orange
    | Red
    | Green
    | Purple

type position = int * int

type Angle = Left | Right | Below

let GRID_WIDTH = 10
let GRID_HEIGHT = 20

let WINDOW_WIDTH: int = 300
let WINDOW_HEIGHT: int = 600
let rand = System.Random()

exception BoardIsNotAnEmptySpace

let colors =
    Map
        [ (Yellow, yellow)
          (Cyan, fromRgb (224, 255, 255))
          (Blue, blue)
          (Orange, fromRgb (255, 140, 0))
          (Red, red)
          (Green, green)
          (Purple, fromRgb (148, 0, 211)) ]

type tetromino(a: bool[,], c: Color, o: position) =
    let mutable _image = a
    let mutable _width  = Array2D.length1 _image
    let mutable _height  = Array2D.length2 _image
    let mutable _offset = o
    
    member this.image
        with get () = _image
        and set(img) = _image <- img
    member this.offset 
        with get() = _offset
        and set(newPos) = _offset <- newPos
    member this.height
        with get() = _height
        and set(h) = _height <- h
    member this.width
        with get() = _width
        and set(w) = _width <- w
    member _.col = c
    member this.rotateRight ()=
        let rows = this.width
        let cols = this.height
        let im: bool [,]= this.image
        let mutable rotated= Array2D.create cols rows false
        for i in 0 .. rows - 1 do
            for j in 0 .. cols - 1 do
                rotated.[j, rows - 1 - i] <- im.[i, j]
        this.image <- rotated
        this.width <- Array2D.length1 rotated
        this.height <- Array2D.length2 rotated
    member this.shiftPositionBy (pos:position)  = this.offset <- (fst pos + fst this.offset, snd pos + snd this.offset)
    member _.clone() = new tetromino (a, c, o)
    override this.ToString()=
        match c with
            | Yellow -> "Square"
            | Blue -> "Straight"
            | Purple-> "T"
            | Orange -> "L"
            | Green -> "Z"


type square(o: int*int) = 
    inherit tetromino(array2D [ [ true; true]; [ true; true] ], Yellow, o)

type straight(o: int*int) = 
    inherit tetromino(array2D [[true]; [true]; [true]; [true]], Blue, o) 

type t(o: int*int) = 
    inherit tetromino(array2D [ [ true; true; true ]; [ false; true; false ] ], Purple, o)


type l(flipped:bool, o: int*int) = 
    inherit tetromino((if flipped then (array2D [[ true; false; ]; [ true; false; ]; [ true; true ]])
                        else array2D [[ false; true; ]; [ false; true; ]; [ true; true ]]), Orange, o)

type z(flipped: bool, o: int*int) = 
    inherit tetromino((if flipped then array2D [ [ false; true; true ]; [ true; true; false ] ]
                        else array2D [ [ true; true; false ]; [ false; true; true ] ]), Green, o)

type board(w: int, h: int) =
    let mutable _board: Color option[,] = Array2D.create w h None
    member val Score:int = 0 with get, set
    member val activePiece: option<tetromino> = None with get, set
    member this.newPiece() : tetromino option =
        printfn "Score is %d" this.Score
        let tetromino =
            let offsetPosition = (rand.Next(8), 0) //x ranges ~ 0 to ~ 9, y = 0
            let shape = rand.Next(0, 4)
            let isFlipped = rand.Next(2) = 0
            match shape with
            | 0 -> (square (offsetPosition)) :> tetromino
            | 1 -> (straight (offsetPosition)) :> tetromino
            | 2 -> (t (offsetPosition)) :> tetromino
            | 3 -> (l (isFlipped, offsetPosition)) :> tetromino
            | _ -> (z (isFlipped, offsetPosition)) :> tetromino
        
        match this.put tetromino with
        | true ->
            this.Score <- this.Score + 1
            Some tetromino
        | false -> None
    member this.put(t: tetromino) =
        let (offsetX, offsetY) = t.offset
        
        (t, t.col |> Some) |> this.drawTetrominoOnBoard
        this.activePiece <- Some t
        
        true
    
    member this.drawTetrominoOnBoard (tetromino:tetromino) = (tetromino, tetromino.col |> Some) |> this.drawTetrominoOnBoard
   
    member this.hasAnyLegalMoves (t:tetromino) =
       (t |> this.isPositionClear Left && t |> this.isPositionClear Below) 
       || (t |> this.isPositionClear Right && t |> this.isPositionClear Below) 
       || (t |> this.isPositionClear Below)
       
    member this.removeRows () = 
        let mutable index:int option = None 
        for column = 0 to this.height - 1 do
            let mutable allIsSome = true
            for row = 0 to this.width - 1 do
                if this.board[row, column].IsNone then
                    allIsSome <- false
            if allIsSome then
                for i = 0 to this.width - 1 do
                    this.board[i, column] <- None
                for i = column downto 1 do 
                    for j = 0 to this.width - 1 do
                        this.board[j, i] <- this.board[j, i - 1]
    member _.drawTetrominoOnBoard (tetromino:tetromino,s: option<Color>) =
        let (x, y) = tetromino.offset
        tetromino.image 
        |> Array2D.iteri  
            (fun i j p ->
                let iy = y + i
                let jx = x + j
                match p with
                | true -> _board.[jx, iy] <- s
                | _ -> ())

    member this.isPositionClear (angle: Angle) (tetromino:tetromino) = 
        let direction = 
            match angle with
            | Below -> (0, 1)
            | Right -> (1, 1)
            | Left -> (-1, 1)
        
        let image = tetromino.image
        let (x, y) = (fst direction + fst tetromino.offset, snd direction + snd tetromino.offset)
        
        try 
            image 
            |> Array2D.iteri  
                (fun i j p ->
                let iy = y + i
                let jx = x + j
                match _board.[jx, iy] with
                |Some c when p -> 
                    (raise (BoardIsNotAnEmptySpace))
                |_ -> ())
            true
        with _ -> 
            false
 
    member this.clearTetrominoFromBoard (tetromino:tetromino) = (tetromino, None) |> this.drawTetrominoOnBoard
    member this.take() : tetromino option = this.activePiece

    member val board: Color option[,] = _board with get, set
    member this.width: int = Array2D.length1 this.board
    member this.height: int = Array2D.length2 this.board
    override this.ToString() = 
        let rowCount = this.height
        let colCount = this.width
        let sb = System.Text.StringBuilder()
        for row in 0 .. rowCount - 1 do
            for col in 0 .. colCount - 1 do
                if this.board[col, row].IsSome then
                    "[X]" |> sb.Append
                else
                    "[ ]" |> sb.Append
            sb.AppendLine()
        sb.ToString()





let board: board = board (GRID_WIDTH, GRID_HEIGHT)

type state = board

let react (s: state) (k: Canvas.key) : state option =
    let key = getKey k
    match key with
    |RightArrow | LeftArrow | DownArrow -> 
        match s.take() with
        |None -> Some s
        | Some activePiece -> 

            activePiece |> s.clearTetrominoFromBoard

            let angle = 
                match key with
                |RightArrow -> Right
                |LeftArrow -> Left
                |DownArrow -> Below

            let isPositionClear = activePiece |> s.isPositionClear angle
            match isPositionClear with
            | true -> 
                let shift = 
                    match angle with
                        | Right -> (1, 1)
                        | Left -> (-1, 1)
                        | Below -> (0, 1)

                activePiece.shiftPositionBy shift
            
               
                let noLegalMoves = not (activePiece |> s.hasAnyLegalMoves)
               
                activePiece |> s.drawTetrominoOnBoard
                
                if (noLegalMoves) then
                    board.removeRows()
                    s.activePiece <- s.newPiece()
                    
                    //call here

            | false -> 
                activePiece |> s.drawTetrominoOnBoard
            Some s
    | Space ->
        let activePiece = s.take()
        match activePiece with
        |Some t -> 
    
            t |> s.clearTetrominoFromBoard
            t.rotateRight()
            t |> s.drawTetrominoOnBoard
        |None -> raise (System.Exception("Spillet må være slut!"))
        Some s
    | _ -> 
        Some s
    
let (^.+) (x: int, y: int) (i: int) = (x + i, y + i)
let draw (w: int) (h: int) (s: state) =
    let canvas: canvas = create w h
    s.board
    |> Array2D.iteri (fun row column v ->
        match v with
        | Some (c: Color) -> setFillBox canvas colors.[c] (row * 30, column*30) ((row * 30, column * 30) ^.+ 30)
        | None -> ())


    canvas

let runAppTest () =
    board.newPiece ()
    do runApp "Tetris" WINDOW_WIDTH WINDOW_HEIGHT draw react board
    ()

//runAppTest ()
