using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board 
{
    public Dictionary<GamePiece, Grid> piecePositions;
    public List<Move> pieceMoves;
    GamePiece enpassantLeft, enpassantRight;
    Move enpassantMoveLeft, enpassantMoveRight, prevMove;

    readonly int[,] startPos = new int[8, 8]
    {
        {1,2,3,4,5,3,2,1},
        {6,6,6,6,6,6,6,6},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0},
        {-6,-6,-6,-6,-6,-6,-6,-1},
        {-1,-2,-3,-4,-5,-3,-2,-1},
    };
    /*{
       { 1,0,0,0,5,0,0,1},
       { 0,-6,2,0,0,0,0,0},
       { 0,0,0,0,0,0,0,0},
       { 0,0,0,0,0,0,0,0},
       { 0,0,0,0,0,0,0,0},
       { 0,0,0,0,0,0,0,0},
       { 0,0,6,0,0,0,0,0},
       { -1,0,0,0,-5,0,0,-1}

   };*/

    readonly List<Grid> queenDirections = new List<Grid>()
    {
        new Grid(){ x = 0, y = 1 },new Grid(){ x = 0, y = -1 },new Grid(){ x = 1, y = 0 },new Grid(){ x = -1, y = 0 },
        new Grid(){ x = 1, y = 1 },new Grid(){ x = 1, y = -1 },new Grid(){ x = -1, y = 1 },new Grid(){ x = -1, y = -1 }
    };

    readonly List<Grid> rookDirections = new List<Grid>()
    {
        new Grid(){ x = 0, y = 1 },new Grid(){ x = 0, y = -1 },new Grid(){ x = 1, y = 0 },new Grid(){ x = -1, y = 0 }
    };

    readonly List<Grid> bishopDirections = new List<Grid>()
    {
        new Grid(){ x = 1, y = 1 },new Grid(){ x = 1, y = -1 },new Grid(){ x = -1, y = 1 },new Grid(){ x = -1, y = -1 }
    };

    readonly List<Grid> knightDirections = new List<Grid>()
    {
        new Grid(){ x = 2, y = 1 },new Grid(){ x = 2, y = -1 },new Grid(){ x = 1, y = 2 },new Grid(){ x = -1, y = 2 },
        new Grid(){ x = -2, y = 1 },new Grid(){ x = -2, y = -1 },new Grid(){ x = -1, y = -2 },new Grid(){ x = 1, y = -2 }
    };

    public Board()
    {
        piecePositions = new Dictionary<GamePiece, Grid>();
        prevMove = new Move();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (startPos[i, j] != 0)
                {
                    Player player = startPos[i, j] > 0 ? Player.RED : Player.BLUE;
                    PieceType piece = (PieceType)(Mathf.Abs(startPos[i, j])) - 1;
                    int pieceNumber = 1;
                    GamePiece currentPiece = new GamePiece()
                    {
                        player = player,
                        pieceType = piece,
                        pieceNumber = pieceNumber
                    };
                    while(piecePositions.ContainsKey(currentPiece))
                    {
                        currentPiece.pieceNumber += 1;
                    }
                    piecePositions[currentPiece] = new Grid() { x = j,y = i };
                }
            }
        }
    }

    public List<Move> GetMoves(GamePiece currentPiece)
    {
        pieceMoves = new List<Move>();
        PieceType piece = currentPiece.pieceType;
        Grid currentPos = piecePositions[currentPiece];

        switch(piece)
        {
            //QUEEN
            case PieceType.QUEEN:
                List<Grid> currentDirections = queenDirections;
                foreach (Grid tempDirection in currentDirections)
                {
                    Grid toCheck = new Grid() { x = currentPos.x + tempDirection.x, y = currentPos.y + tempDirection.y };
                    while(IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                        toCheck = new Grid() { x = toCheck.x + tempDirection.x, y = toCheck.y + tempDirection.y };
                    }
                    if(IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).player !=  currentPiece.player)
                    {
                        pieceMoves.Add(new Move()
                        {
                            start = currentPos,
                            end = toCheck,
                            isCapture = true,
                            capturedPiece = GetPieceAtGrid(toCheck)
                        });
                    }
                }
                break;

            //ROOK
            case PieceType.ROOK:
                currentDirections = rookDirections;
                foreach (Grid tempDirection in currentDirections)
                {
                    Grid toCheck = new Grid() { x = currentPos.x + tempDirection.x, y = currentPos.y + tempDirection.y };
                    while (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                        toCheck = new Grid() { x = toCheck.x + tempDirection.x, y = toCheck.y + tempDirection.y };
                    }
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move()
                        {
                            start = currentPos,
                            end = toCheck,
                            isCapture = true,
                            capturedPiece = GetPieceAtGrid(toCheck)
                        });
                    }
                }
                break;

            //BISHOP
            case PieceType.BISHOP:
                currentDirections = bishopDirections;
                foreach (Grid tempDirection in currentDirections)
                {
                    Grid toCheck = new Grid() { x = currentPos.x + tempDirection.x, y = currentPos.y + tempDirection.y };
                    while (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                        toCheck = new Grid() { x = toCheck.x + tempDirection.x, y = toCheck.y + tempDirection.y };
                    }
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move()
                        {
                            start = currentPos,
                            end = toCheck,
                            isCapture = true,
                            capturedPiece = GetPieceAtGrid(toCheck)
                        });
                    }
                }
                break;

            //KNIGHT
            case PieceType.KNIGHT:
                currentDirections = knightDirections;
                foreach (Grid tempDirection in currentDirections)
                {
                    Grid toCheck = new Grid() { x = currentPos.x + tempDirection.x, y = currentPos.y + tempDirection.y };
                    if(IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                    }
                    else if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1 
                        && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move()
                        {
                            start = currentPos,
                            end = toCheck,
                            isCapture = true,
                            capturedPiece = GetPieceAtGrid(toCheck)
                        });
                    }
                }
                break;

            //KING
            case PieceType.KING:
                currentDirections = queenDirections;
                foreach (Grid tempDirection in currentDirections)
                {
                    Grid toCheck = new Grid() { x = currentPos.x + tempDirection.x, y = currentPos.y + tempDirection.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                    }
                    else if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1
                        && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move()
                        {
                            start = currentPos,
                            end = toCheck,
                            isCapture = true,
                            capturedPiece = GetPieceAtGrid(toCheck)
                        });
                    }
                }

                //CASTLE
                if(currentPiece.player == Player.RED && currentPos.x == 4 && currentPos.y == 0
                    && GetPieceAtGrid(new Grid() { x = 7, y = 0 }).player ==  Player.RED
                    && GetPieceAtGrid(new Grid() { x = 7, y = 0 }).pieceType == PieceType.ROOK
                    && GetPieceAtGrid(new Grid() { x = 5, y = 0 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 6, y = 0 }).pieceNumber == -1)
                {
                    pieceMoves.Add(new Move()
                    {
                        start = currentPos,
                        end = new Grid() { x = 6, y = 0 },
                        isCapture = false,
                        canCastle = true,
                        rook = GetPieceAtGrid(new Grid() { x = 7 , y = 0 }),
                        castleGrid = new Grid() { x = 5, y = 0 }
                    });
                }

                if (currentPiece.player == Player.RED && currentPos.x == 4 && currentPos.y == 0
                    && GetPieceAtGrid(new Grid() { x = 0, y = 0 }).player == Player.RED
                    && GetPieceAtGrid(new Grid() { x = 0, y = 0 }).pieceType == PieceType.ROOK
                    && GetPieceAtGrid(new Grid() { x = 1, y = 0 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 2, y = 0 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 3, y = 0 }).pieceNumber == -1)
                {
                    pieceMoves.Add(new Move()
                    {
                        start = currentPos,
                        end = new Grid() { x = 2, y = 0 },
                        isCapture = false,
                        canCastle = true,
                        rook = GetPieceAtGrid(new Grid() { x = 0, y = 0 }),
                        castleGrid = new Grid() { x = 3, y = 0 }
                    });
                }

                if (currentPiece.player == Player.BLUE && currentPos.x == 4 && currentPos.y == 7
                    && GetPieceAtGrid(new Grid() { x = 7, y = 7 }).player == Player.BLUE
                    && GetPieceAtGrid(new Grid() { x = 7, y = 7 }).pieceType == PieceType.ROOK
                    && GetPieceAtGrid(new Grid() { x = 5, y = 7 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 6, y = 7 }).pieceNumber == -1)
                {
                    pieceMoves.Add(new Move()
                    {
                        start = currentPos,
                        end = new Grid() { x = 6, y = 7 },
                        isCapture = false,
                        canCastle = true,
                        rook = GetPieceAtGrid(new Grid() { x = 7, y = 7 }),
                        castleGrid = new Grid() { x = 5, y = 7 }
                    });
                }

                if (currentPiece.player == Player.BLUE && currentPos.x == 4 && currentPos.y == 7
                    && GetPieceAtGrid(new Grid() { x = 0, y = 7 }).player == Player.BLUE
                    && GetPieceAtGrid(new Grid() { x = 0, y = 7 }).pieceType == PieceType.ROOK
                    && GetPieceAtGrid(new Grid() { x = 1, y = 7 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 2, y = 7 }).pieceNumber == -1
                    && GetPieceAtGrid(new Grid() { x = 3, y = 7 }).pieceNumber == -1)
                {
                    pieceMoves.Add(new Move()
                    {
                        start = currentPos,
                        end = new Grid() { x = 2, y = 7 },
                        isCapture = false,
                        canCastle = true,
                        rook = GetPieceAtGrid(new Grid() { x = 0, y = 7 }),
                        castleGrid = new Grid() { x = 3, y = 7 }
                    });
                }
                break;

            //PAWN
            case PieceType.PAWN:
                if(currentPiece.player == Player.RED)
                {
                    Grid tempDirection = new Grid() { x = 0, y = 1 };
                    Grid toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if(IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                    }

                    bool canDouble = IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1;

                    //DOUBLE
                    tempDirection = new Grid() { x = 0, y = 2 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1 && canDouble)
                    {
                        bool canEnpassant = false;

                        Grid left = new Grid() { x = toCheck.x - 1, y = toCheck.y };
                        if (IsValidGrid(left) && GetPieceAtGrid(left).pieceNumber != -1
                            && GetPieceAtGrid(left).player == Player.BLUE
                            && GetPieceAtGrid(left).pieceType == PieceType.PAWN)
                        {
                            canEnpassant = true;
                            enpassantLeft = GetPieceAtGrid(left);
                            enpassantMoveLeft = new Move()
                            {
                                start = left,
                                end = new Grid() { x = currentPos.x, y = currentPos.y + 1 },
                                isCapture = true,
                                capturedPiece = currentPiece
                            };
                        }

                        Grid right = new Grid() { x = toCheck.x + 1, y = toCheck.y };
                        if (IsValidGrid(right) && GetPieceAtGrid(right).pieceNumber != -1
                            && GetPieceAtGrid(right).player == Player.BLUE
                            && GetPieceAtGrid(right).pieceType == PieceType.PAWN)
                        {
                            canEnpassant = true;
                            enpassantRight = GetPieceAtGrid(right);
                            enpassantMoveRight = new Move()
                            {
                                start = right,
                                end = new Grid() { x = currentPos.x, y = currentPos.y + 1 },
                                isCapture = true,
                                capturedPiece = currentPiece
                            };
                        }

                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false, canEnpassant = canEnpassant});
                    }

                    //CAPTURE
                    tempDirection = new Grid() { x = 1, y = 1 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1
                        && GetPieceAtGrid(toCheck).player !=  currentPiece.player)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = true, capturedPiece = GetPieceAtGrid(toCheck)});
                    }

                    //CAPTURE
                    tempDirection = new Grid() { x = -1, y = 1 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1
                        && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = true, capturedPiece = GetPieceAtGrid(toCheck) });
                    }

                    //EnpassantCapture
                    if (currentPiece.player == enpassantLeft.player && currentPiece.pieceNumber == enpassantLeft.pieceNumber
                        && currentPiece.pieceType == enpassantLeft.pieceType && prevMove.canEnpassant)
                    {
                        pieceMoves.Add(enpassantMoveLeft);
                    }
                    if (currentPiece.player == enpassantRight.player && currentPiece.pieceNumber == enpassantRight.pieceNumber
                        && currentPiece.pieceType == enpassantRight.pieceType && prevMove.canEnpassant)
                    {
                        pieceMoves.Add(enpassantMoveRight);
                    }

                }
                else
                {
                    Grid tempDirection = new Grid() { x = 0, y = -1 };
                    Grid toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false });
                    }

                    bool canDouble = IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1;

                    //DOUBLE
                    tempDirection = new Grid() { x = 0, y = -2 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber == -1 && canDouble)
                    {
                        
                        bool canEnpassant = false;

                        Grid left = new Grid() { x = toCheck.x - 1, y = toCheck.y };
                        if (IsValidGrid(left) && GetPieceAtGrid(left).pieceNumber != -1
                            && GetPieceAtGrid(left).player == Player.RED
                            && GetPieceAtGrid(left).pieceType == PieceType.PAWN)
                        {
                            canEnpassant = true;
                            enpassantLeft = GetPieceAtGrid(left);
                            enpassantMoveLeft = new Move()
                            {
                                start = left,
                                end = new Grid() { x = currentPos.x, y = currentPos.y + 1 },
                                isCapture = true,
                                capturedPiece = currentPiece
                            };
                        }

                        Grid right = new Grid() { x = toCheck.x + 1, y = toCheck.y };
                        if (IsValidGrid(right) && GetPieceAtGrid(right).pieceNumber != -1
                            && GetPieceAtGrid(right).player == Player.RED
                            && GetPieceAtGrid(right).pieceType == PieceType.PAWN)
                        {
                            canEnpassant = true;
                            enpassantRight = GetPieceAtGrid(right);
                            enpassantMoveRight = new Move()
                            {
                                start = right,
                                end = new Grid() { x = currentPos.x, y = currentPos.y + 1 },
                                isCapture = true,
                                capturedPiece = currentPiece
                            };
                        }
                        
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = false, canEnpassant = canEnpassant});
                    }

                    //CAPTURE
                    tempDirection = new Grid() { x = 1, y = -1 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1
                        && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = true, capturedPiece = GetPieceAtGrid(toCheck) });
                    }

                    //CAPTURE
                    tempDirection = new Grid() { x = -1, y = -1 };
                    toCheck = new Grid() { x = tempDirection.x + currentPos.x, y = tempDirection.y + currentPos.y };
                    if (IsValidGrid(toCheck) && GetPieceAtGrid(toCheck).pieceNumber != -1
                        && GetPieceAtGrid(toCheck).player != currentPiece.player)
                    {
                        pieceMoves.Add(new Move() { start = currentPos, end = toCheck, isCapture = true, capturedPiece = GetPieceAtGrid(toCheck) });
                    }

                    //EnpassantCapture
                    if(currentPiece.player == enpassantLeft.player && currentPiece.pieceNumber == enpassantLeft.pieceNumber
                        && currentPiece.pieceType == enpassantLeft.pieceType && prevMove.canEnpassant)
                    {
                        pieceMoves.Add(enpassantMoveLeft);
                    }
                    if (currentPiece.player == enpassantRight.player && currentPiece.pieceNumber == enpassantRight.pieceNumber
                        && currentPiece.pieceType == enpassantRight.pieceType && prevMove.canEnpassant)
                    {
                        pieceMoves.Add(enpassantMoveRight);
                    }
                }
                break;

            default:
                break;
        }
        return pieceMoves;
    }

    public void UpdateMove(Move currentMove)
    {
        prevMove = currentMove;
        GamePiece selected = GetPieceAtGrid(currentMove.start);
        piecePositions[selected] = currentMove.end;
        if(currentMove.isCapture)
        {
            piecePositions.Remove(currentMove.capturedPiece);
        }

        if(selected.pieceType == PieceType.KING && currentMove.canCastle)
        {
            piecePositions[currentMove.rook] = currentMove.castleGrid;
        }
    }

    public GamePiece UpgradePiece(GamePiece currentPiece, PieceType upgraded)
    {
        Grid currentPos = piecePositions[currentPiece];
        piecePositions.Remove(currentPiece);
        currentPiece.pieceType = upgraded;
        while(piecePositions.ContainsKey(currentPiece))
        {
            currentPiece.pieceNumber += 1;
        }
        piecePositions[currentPiece] = currentPos;
        return currentPiece;
    }

    public bool IsValidGrid(Grid temp)
    {
        return temp.x >= 0 && temp.x < 8 && temp.y >= 0 && temp.y < 8;
    }

    public GamePiece GetPieceAtGrid(Grid temp)
    {
        foreach (var item in piecePositions)
        {
            if(item.Value.x == temp.x && item.Value.y == temp.y)
            {
                return item.Key;
            }
        }
        return new GamePiece() { pieceNumber = -1 };
    }
}
