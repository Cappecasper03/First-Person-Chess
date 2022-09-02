using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject chessPiece;

    public int[] posCombination = new int[2]; // letterPos, numberPos
    protected int[] newPosCombination = new int[2]; // letterPos, numberPos
    public float teamMultiplier; // 1f = Light, -1f = Dark
    protected int pieceNumber = 0;

    public bool teamIsMoving = false;
    public bool hasNewPosition = false;
    public bool isAlive = true;
    public bool isOutSideBoard = false;
    public int listNumber = 0;

    virtual public void Setup() { }

    public void SetPosition()
    {
        chessPiece.transform.position = new Vector3(ChessPieces.letterPos[posCombination[0]], 0, ChessPieces.numberPos[posCombination[1]]);
    }

    public void CheckForNewPosition()
    {
        (hasNewPosition, newPosCombination) = ChessPieces.HasNewPosition(posCombination, chessPiece.transform.position);

        if (newPosCombination == null || (posCombination[0] == newPosCombination[0] && posCombination[1] == newPosCombination[1]))
        {
            isOutSideBoard = true;
        }
        else
        {
            isOutSideBoard = false;
        }
    }

    virtual public bool CheckMoveByRules()
    {
        return false;
    }
}
