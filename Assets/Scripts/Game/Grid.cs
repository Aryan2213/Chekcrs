using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject redPiecePrefabs, WhitePiecePrefabs;
    public Vector3 boardOffset = new Vector3(-4.0f, 0.0f, -4.0f);
    public Vector3 pieceOffset = new Vector3(.5f, 0, .5f);
    public Piece[,] pieces = new Piece[8, 8];

    //Part 2
    private Vector2Int mouseOver;
    private Piece selectedPiece;

    Piece GetPiece(Vector2Int cell)
    {
        return pieces[cell.x, cell.y];
    }

    bool IsOutOfBounds(Vector2Int cell)
    {
        return cell.x < 0 || cell.x >= 8 ||
               cell.y < 0 || cell.y >= 8;
    }
    bool TryMove(Piece selected, Vector2Int desiredCell)
    {
        Vector2Int startCell = selected.cell;
        if (!ValidMove(selected, desiredCell))
        {
            MovePiece(selected, startCell);
            return false;
        }
        MovePiece(selected, desiredCell);
        return true;
    }


    Piece SelectPiece(Vector2Int cell)
    {
        if (IsOutOfBounds(cell))
        {
            return null;
        }


        Piece piece = GetPiece(cell);


        if (piece)
        { return piece; }
        return null;
    }


    void MouseOver()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver = new Vector2Int(-1, -1);
        }
    }

    void DragPiece(Piece selected)
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit))
        {
            selected.transform.position = hit.point + Vector3.up;
        }
    }









    Vector3 GetWorldPosition(Vector2Int cell)
    {
        return new Vector3(cell.x, 0, cell.y) + boardOffset + pieceOffset;
    }

    void MovePiece(Piece piece, Vector2Int newCell)
    {
        Vector2Int oldCell = piece.cell;

        pieces[oldCell.x, oldCell.y] = null;
        pieces[newCell.x, newCell.y] = piece;

        piece.oldCell = oldCell;
        piece.cell = newCell;

        piece.transform.localPosition = GetWorldPosition(newCell);
    }
    void GeneratePiece(GameObject prefabs, Vector2Int desiredCell)
    {
        GameObject clone = Instantiate(prefabs, transform);
        Piece piece = clone.GetComponent<Piece>();
        piece.oldCell = desiredCell;
        piece.cell = desiredCell;
        MovePiece(piece, desiredCell);
    }
    void GenerateBoard()
    {
        Vector2Int desiredCell = Vector2Int.zero;

        for (int y = 0; y < 3; y++)
        {
            bool oddRow = y % 2 == 0;
            for (int x = 0; x < 8; x += 2)
            {
                desiredCell.x = oddRow ? x : x + 1;
                desiredCell.y = y;
                GeneratePiece(WhitePiecePrefabs, desiredCell);
            }
        }
        for (int y = 5; y < 8; y++)
        {
            bool oddRow = y % 2 == 0;
            for (int x = 0; x < 8; x += 2)
            {
                desiredCell.x = oddRow ? x : x + 1;
                desiredCell.y = y;
                GeneratePiece(redPiecePrefabs, desiredCell);
            }
        }
    }
    //---------------------------------------------------------------------------------------
    bool ValidMove(Piece selected, Vector2Int desiredCell)
    {
        Vector2Int direction = selected.cell - desiredCell;

        #region Rule #01
        if (IsOutOfBounds(desiredCell))
        {

            Debug.Log("<color=red>Invalid - youcannout move out side of the map</color>");
            return false;
        }
        #endregion
        #region Rule #02
        if (selected.cell == desiredCell)
        {
            Debug.Log("<color=red>Invalid - putting pieces back don,t count as a valid move</color>");
            return false;
        }
        #endregion
        #region Rule #03
        if (GetPiece(desiredCell))
        {
            Debug.Log("<color=red>Invalid - You can't go on top of other pieces</color>");
            return false;
        }
        #endregion
        #region Rule #04
        #endregion
        #region Rule #05
        
        #endregion
        #region Rule #06
        #endregion
        #region Rule #07
        #endregion
        Debug.Log("<Color=green>Success - valid move detected!</color>");
        return true;
    }

    //---------------------------------------------------------------------------------------
    void CheckForcedMove(Piece piece)
    {
        Vector2Int cell = piece.cell;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = 0; y <= 1; y += 2)
            {
                Vector2Int offset = new Vector2Int(x, y);
                Vector2Int desiredCell = cell + offset;
                #region Check #1
                if (!piece.isKing)
                {
                    if (piece.isWhite)
                    {
                        if (desiredCell.y < cell.y)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    if (desiredCell.y > cell.y)
                    {
                        continue;
                    }
                }
                #endregion
                #region Check #2
                if (IsOutOfBounds(desiredCell))
                {
                    continue;
                }
                #endregion

                Piece detectedPiece = GetPiece(desiredCell);

                #region Check #3
                if (detectedPiece == null)
                {
                    continue;
                }
                #endregion
                #region Check #4
                if (detectedPiece.isWhite == piece.isWhite)
                {
                    continue;
                }
                #endregion
                Vector2Int jumpCell = cell + (offset * 2);

                #region Check #5
                if(IsOutOfBounds(jumpCell))
                {
                    continue;
                }
                #endregion
                #region Check #6
                detectedPiece = GetPiece(jumpCell);

                if(detectedPiece)
                {
                    continue;
                }
                #endregion

                #region Store Forced Move
                #endregion

            }
        }

    }




























    //------------------------------------------------------------
    private void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        MouseOver();
        if (Input.GetMouseButtonDown(0))
        {
            selectedPiece = SelectPiece(mouseOver);
        }
        if (selectedPiece)
        {
            DragPiece(selectedPiece);
            if (Input.GetMouseButtonUp(0))
            {

                TryMove(selectedPiece, mouseOver);
                selectedPiece = null;
            }
        }


    }

















}
