using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    override public void Setup()
    {
        (teamMultiplier, pieceNumber) = ChessPieces.Setup(chessPiece);
        isAlive = true;

        if (pieceNumber == 0) // Letter position
        {
            posCombination[0] = 0;
        }
        else
        {
            posCombination[0] = 7;
        }

        if (teamMultiplier == 1) // Number position
        {
            posCombination[1] = 0;
        }
        else
        {
            posCombination[1] = 7;
        }

        chessPiece.transform.position = new Vector3(ChessPieces.letterPos[posCombination[0]], 0, ChessPieces.numberPos[posCombination[1]]);

        listNumber = ChessPieces.SaveBoardState(chessPiece, this);
    }

    override public bool CheckMoveByRules()
    {
        for (int i = 1; i < 8; i++)
        {
            // Moved in letter axis
            if (newPosCombination[0] == i && newPosCombination[1] == posCombination[1])
            {
                if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber, movedAxis: 0, notMovedAxis: 1, posCombination: posCombination))
                {
                    posCombination = (int[])newPosCombination.Clone();
                }
                break;
            }
            // Moved in number axis
            else if (newPosCombination[1] == i && newPosCombination[0] == posCombination[0])
            {
                if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber, movedAxis: 1, notMovedAxis: 0, posCombination: posCombination))
                {
                    posCombination = (int[])newPosCombination.Clone();
                }
                break;
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