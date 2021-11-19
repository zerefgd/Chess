using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { RED, BLUE };

public struct Grid { public int x, y; };

public enum PieceType { ROOK,KNIGHT,BISHOP,QUEEN,KING,PAWN };

public struct GamePiece
{
    public Player player;
    public PieceType pieceType;
    public int pieceNumber;
}

public struct Move
{
    public Grid start, end;
    public bool isCapture;
    public GamePiece capturedPiece;

    public bool canEnpassant;

    public bool canCastle;
    public GamePiece rook;
    public Grid castleGrid;
}

public static class Constants
{
    
}
