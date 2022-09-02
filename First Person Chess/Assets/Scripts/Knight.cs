using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
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
            posCombination[0] = 1;
        }
        else
        {
            posCombination[0] = 6;
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
        // Moved positive in numbers
        if (newPosCombination[1] == posCombination[1] + 2 && (newPosCombination[0] == posCombination[0] + 1 || newPosCombination[0] == posCombination[0] - 1))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber))
            {
                posCombination = (int[])newPosCombination.Clone();
            }
        }
        // Moved negative in numbers
        else if (newPosCombination[1] == posCombination[1] - 2 && (newPosCombination[0] == posCombination[0] + 1 || newPosCombination[0] == posCombination[0] - 1))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber))
            {
                posCombination = (int[])newPosCombination.Clone();
            }
        }
        // Moved positive in letter
        else if (newPosCombination[0] == posCombination[0] + 2 && (newPosCombination[1] == posCombination[1] + 1 || newPosCombination[1] == posCombination[1] - 1))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber))
            {
                posCombination = (int[])newPosCombination.Clone();
            }
        }
        // Moved negative in letter
        else if (newPosCombination[0] == posCombination[0] - 2 && (newPosCombination[1] == posCombination[1] + 1 || newPosCombination[1] == posCombination[1] - 1))
        {
            if (ChessPieces.CheckTakeOut(newPosCombination, teamMultiplier, listNumber))
            {
                posCombination = (int[])newPosCombination.Clone();
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