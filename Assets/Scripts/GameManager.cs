using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    GameObject block;

    [SerializeField]
    GameObject gamePiece;

    [SerializeField]
    GameObject upgradeSelect;

    [SerializeField]
    List<Sprite> redPieces, bluePieces;

    Board myBoard;

    Dictionary<GamePiece, GameObject> pieceDictionary;

    bool canClick, hasGameFinished, isGamePaused;
    GamePiece clickedPiece;
    List<Move> currentMoves;
    Player currentPlayer;

    private void Awake()
    {
        SpawnBlocks();
        myBoard = new Board();
        pieceDictionary = new Dictionary<GamePiece, GameObject>();
        hasGameFinished = false;
        canClick = true;
        isGamePaused = false;
        currentPlayer = Player.BLUE;
        SpawnPieces();
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tempBlock = Instantiate(block);
                tempBlock.transform.position = new Vector3(i, -j, -1f);
                tempBlock.GetComponent<SpriteRenderer>().color = (i + j) % 2 == 0 ? Color.gray : Color.black;
            }
        }
    }

    void SpawnPieces()
    {
        var playerPos = myBoard.piecePositions;
        foreach (var item in playerPos)
        {
            Player currentPlayer = item.Key.player;
            PieceType currentPiece = item.Key.pieceType;
            Grid pos = item.Value;
            GameObject tempPiece = Instantiate(gamePiece);
            tempPiece.transform.position = new Vector3(pos.x, -pos.y, -2f);
            tempPiece.GetComponent<SpriteRenderer>().sprite = currentPlayer == Player.RED ?
                redPieces[(int)currentPiece] : bluePieces[(int)currentPiece];
            pieceDictionary[item.Key] = tempPiece; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (hasGameFinished || isGamePaused) return;
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Grid clickedGrid = new Grid() { x = (int)(mousePos.x + 0.5), y = (int)-(mousePos.y - 0.5) };

            if(canClick)
            {
                currentMoves = new List<Move>();
                clickedPiece = myBoard.GetPieceAtGrid(clickedGrid);
                if (clickedPiece.pieceNumber == -1 || clickedPiece.player != currentPlayer) return;
                currentMoves = myBoard.GetMoves(clickedPiece);
                canClick = false;
                return;
            }

            foreach (var item in currentMoves)
            {
                if(item.end.x == clickedGrid.x && item.end.y == clickedGrid.y)
                {
                    pieceDictionary[clickedPiece].transform.position = new Vector3(clickedGrid.x, -clickedGrid.y, -2f);

                    if(item.isCapture)
                    {
                        pieceDictionary[item.capturedPiece].SetActive(false);
                        pieceDictionary.Remove(item.capturedPiece);
                        if(item.capturedPiece.pieceType == PieceType.KING)
                        {
                            hasGameFinished = true;
                            return;
                        }
                    }

                    if(item.canCastle)
                    {
                        pieceDictionary[item.rook].transform.position = new Vector3(item.castleGrid.x, -item.castleGrid.y, -2f);
                    }
                    myBoard.UpdateMove(item);

                    if(clickedPiece.player == Player.RED && clickedPiece.pieceType == PieceType.PAWN
                        && item.end.y == 7)
                    {
                        isGamePaused = true;
                        upgradeSelect.SetActive(true);
                    }

                    if (clickedPiece.player == Player.BLUE && clickedPiece.pieceType == PieceType.PAWN
                        && item.end.y == 0)
                    {
                        isGamePaused = true;
                        upgradeSelect.SetActive(true);
                    }

                    currentPlayer = currentPlayer == Player.RED ? Player.BLUE : Player.RED;
                    canClick = true;
                    return;
                }
            }

            currentMoves = new List<Move>();
            clickedPiece = myBoard.GetPieceAtGrid(clickedGrid);
            if (clickedPiece.pieceNumber == -1 || clickedPiece.player != currentPlayer) return;
            currentMoves = myBoard.GetMoves(clickedPiece);
            return;
        }
    }

    public void UpgradeToQueen()
    {
        upgradeSelect.SetActive(false);
        Sprite current = clickedPiece.player == Player.RED ? redPieces[3] : bluePieces[3];
        pieceDictionary[clickedPiece].GetComponent<SpriteRenderer>().sprite = current;
        GamePiece upgradedPiece = myBoard.UpgradePiece(clickedPiece, PieceType.QUEEN);
        GameObject tempObject = pieceDictionary[clickedPiece];
        pieceDictionary.Remove(clickedPiece);
        pieceDictionary[upgradedPiece] = tempObject;
        isGamePaused = false;
    }

    public void UpgradeToKnight()
    {
        upgradeSelect.SetActive(false);
        Sprite current = clickedPiece.player == Player.RED ? redPieces[1] : bluePieces[1];
        pieceDictionary[clickedPiece].GetComponent<SpriteRenderer>().sprite = current;
        GamePiece upgradedPiece = myBoard.UpgradePiece(clickedPiece, PieceType.KNIGHT);
        GameObject tempObject = pieceDictionary[clickedPiece];
        pieceDictionary.Remove(clickedPiece);
        pieceDictionary[upgradedPiece] = tempObject;
        isGamePaused = false;
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
