open Tetris
open Canvas

/// TESTS OF TETROMINO CLASSES

let square = Tetris.square(0,0)
let straight = Tetris.straight(10,4)
let t = Tetris.t(10,10)
let l = Tetris.l(true, (4,5))
let z = Tetris.z(false, (0,3))

printfn "----Test 1 ~ Instantiating all Tetrominos ----\n"
let test1_1 = printfn "Test 1.1 ~ Square:\nImage:\n %A \nColor: %A \nOffset: %A\n " (square.image) (square.col) (square.offset)
let test1_2 = printfn "Test 1.2 ~ Straight:\nImage:\n %A \nColor: %A \nOffset: %A\n" (straight.image) (straight.col) (straight.offset)
let test1_3 = printfn "Test 1.3 ~ t:\nImage:\n %A \n Color: %A \nOffset: %A \n" (t.image) (t.col) (t.offset)
let test1_4 = printfn "Test 1.4 ~ l:\nImage:\n %A \n Color: %A \nOffset: %A \n" (l.image) (l.col) (l.offset)
let test1_5 = printfn "Test 1.5 ~ z:\nImage:\n %A \nColor: %A \nOffset: %A \n" (z.image) (z.col) (z.offset)

printfn "----Test 2 ~ Changing values of tetraminos ----\n"
square.offset <- (100,100)
printfn "Test 2.1 ~ Square updated offset: %A" (square.offset)

square.offset <- (-100,10000000)
printfn "Test 2.2 ~ Square updated offset (negative values): %A" (square.offset)

square.image <- array2D [[true;true;true];[false;true;false];[false;true;false]]
printfn "Test 2.3 ~ Square updated image: %A" (square.image)

printfn "----Test 3 ~ Tests of tetromino Class methods ----\n"
let tetromino =  Tetris.t((10,10)) :> Tetris.tetromino
printfn "Test 3.1 ~ Calling ToString() result: %s" (tetromino.ToString())

let tetromino1 = tetromino
printfn "Test 3.2 ~ When using = lexeme, tetromino and tetromino1, are pointing to the same object on the heap: %A" (tetromino = tetromino1)

let copy = tetromino.clone()
printfn "Test 3.3 ~ When using .clone(), tetromino and tetromino1, doesn't point to the same object on the heap, but are two independent instances: %A" (tetromino = copy)


printfn "Test 3.4 ~ Calling rotateRight() on tetramino. Image before rotating: %A" (tetromino.image)
tetromino.rotateRight()
printfn "Image after rotating once: %A" (tetromino.image)
for _ in [1..4] do tetromino.rotateRight()
printfn "Image after rotating 4 times (full circle): %A" (tetromino.image)

///TEST OF BOARD CLASS
let board:Tetris.board = (new Tetris.board(10, 20))
printfn "Test 4.1 ~ Creating a board class. Expected result is a 2D array with None as each element. result:\n%A" (board)
board.newPiece()
printfn "Test 4.2 ~ calling newPiece() on the board object. Results in a piece being put on the board:\n%A\n" (board)
board.take().Value |> board.clearTetrominoFromBoard
printfn "Test 4.3 ~ calling take() on the board object. Results in taking the piece off the board:\n%A\n" (board)

runAppTest ()


