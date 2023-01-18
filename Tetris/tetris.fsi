module Tetris
open Canvas

type Color = Yellow | Cyan | Blue | Orange | Red | Green | Purple
type position = int * int
//type Angle = Left | Right | Below

///METHODS FOR TETROMINO CLASS
///<summary> The constructor with its initial shape , its final color, and its inital offset </summary>
/// <param name=a> A representation of the shape of the tetramino as a boolan Array 2D</param>
/// <param name=c> The color of the tetromino </param>
/// <param name=0> The initial position of the tetromino upper left corner </param>
/// <returns> A tetromino object</returns>
new: a: bool[,] * c: Color * o: position -> tetromino

///<summary> Prints a string describing the shape of the tetromino </summary>
/// <returns> A string</returns>
val ToString: unit -> string

///<summary> Makes a copy of the tetromino </summary>
/// <returns> A tetromino object</returns>
val clone: unit -> tetromino

///<summary> Rotates the tetromino 90 degrees counter-clockwise </summary>
/// <returns> Updates the position of the tetramino but returns nothing</returns>
val rotateRight: unit -> unit

///PROPERTIES FOR TETRAMINO CLASS
/// <summary>
/// The color of the tetromino.
/// </summary>
val col: Color
/// <summary>
/// The height of the tetromino, in number of fields.
/// </summary>
val height: int
/// <summary>
/// A two-dimensional array representing the shape of the tetromino,
/// with each element being a boolean indicating whether that specific field
/// is to be occupied by the tetromino.
/// </summary>
val image: bool[,]
/// <summary>
/// The position of the top-left corner of the tetromino on the board.
/// </summary>
val offset: position
/// <summary>
/// The width of the tetromino, in number of fields.
/// </summary>
val width: int

///METHODS FOR BOARD CLASS
/// <summary>
/// The constructor of a board of w x h fields and which creates the first active piece at the top.
/// </summary>
/// <param name="w">The width of the board, in number of fields.</param>
/// <param name="h">The height of the board, in number of fields.</param>
new: w: int * h: int -> board

/// <summary>
/// Makes a string representation of this board.
/// </summary>
/// <returns>A string representation of the board, like T, Z etc..</returns>
val ToString: unit -> string

/// <summary>
/// Makes a new piece and puts it on the board if possible.
/// </summary>
/// <returns>A tetromino object representing the new piece, or None if the piece could not be placed on the board.</returns>
val newPiece: unit -> tetromino option

/// <summary>
/// Puts a piece on the board if possible.
/// </summary>
/// <param name="t">The tetromino to be placed on the board.</param>
/// <returns>Returns true if the piece was successfully placed on the board, false otherwise.</returns>
val put: t: tetromino -> bool

/// <summary>
/// Takes the active piece from the board.
/// </summary>
/// <returns>A tetromino object representing the active piece on the board, or None if no piece is active.</returns>
val take: unit -> tetromino option

///PROPERTIES FOR BOARD CLASS
/// <summary>
/// A two-dimensional array representing the state of the board, with each element being a Some color or None.
/// </summary>
val board: Color option[,]
/// <summary>
/// The height of the board
/// </summary>
val height: int
/// <summary>
/// The width of the board
/// </summary>
val width: int