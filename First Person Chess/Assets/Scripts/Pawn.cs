using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private bool firstMove = true;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    override public void Setup()
    {
        (teamMultiplier, pieceNumber) = ChessPieces.Setup(chessPiece);
        firstMove = true;
        isAlive = true;

        posCombination[0] = pieceNumber; // Letter position

        if (teamMultiplier == 1) // Number position
        {
            posCombination[1] = 1;
        }
        else
        {
            posCombination[1] = 6;
        }

        chessPiece.transform.position = new Vector3(ChessPieces.letterPos[posCombination[0]], 0, ChessPieces.numberPos[posCombination[1]]);

        listNumber = ChessPieces.SaveBoardState(chessPiece, this);
    }

    override public bool CheckMoveByRules()
    {
        // Take out with 1 step forward and 1 step aside
        if (newPosCombination[1] == posCombination[1] + teamMultiplier && (newPosCombination[0] == posCombination[0] + 1 || newPosCombination[0] == posCombination[0] - 1))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber, canTakeOut: true, isPawn: true))
            {
                posCombination = (int[])newPosCombination.Clone();
            }
        }
        // 1 or 2 steps forward
        else if ((newPosCombination[1] == posCombination[1] + teamMultiplier * 2 && firstMove) || (newPosCombination[1] == posCombination[1] + teamMultiplier))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber, canTakeOut: false, isPawn: true))
            {
                posCombination = (int[])newPosCombination.Clone();
                firstMove = false;
            }
        }

        if (posCombination[0] == newPosCombination[0] && posCombination[1] == newPosCombination[1]) // If they are the same the move is "legal"
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}