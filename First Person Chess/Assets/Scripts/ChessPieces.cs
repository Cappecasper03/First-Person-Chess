using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieces : MonoBehaviour
{
    public static float[] letterPos = new float[] { -8.98f, -6.39f, -3.81f, -1.3f, 1.23f, 3.75f, 6.3f, 8.85f }; // A, B, C, D, E, F, G, H
    private static float letterSpace = 2.547143f;
    public static float[] numberPos = new float[] { -8.55f, -6.06f, -3.59f, -1.16f, 1.27f, 3.75f, 6.22f, 8.66f }; // 1, 2, 3, 4, 5, 6, 7, 8
    private static float numberSpace = 2.458571f;

    private static List<Piece> pieces = new List<Piece>();
    private static List<Piece> kings = new List<Piece>();

    private static int newPositions = 0;
    private static string lastWinner = "";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        for (int i = 0; i < pieces.Count; i++) // Checking if piece can/has moved
        {
            if (pieces[i].isAlive)
            {
                CheckPosition(pieces[i]);
            }
        }

        for (int i = 0; i < pieces.Count; i++) // Checking if move is "legal"
        {
            if (pieces[i].isAlive)
            {
                CheckMove(pieces[i]);
            }
        }

        newPositions = 0;

        for (int i = 0; i < kings.Count; i++) // Checking if someone won
        {
            if (!kings[i].isAlive && kings[i].tag == "Dark")
            {
                lastWinner = "Light";
            }
            else if (!kings[i].isAlive && kings[i].tag == "Light")
            {
                lastWinner = "Dark";
            }
        }
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(10f, 50f, 200f, 25f), $"Last winner: {lastWinner}");
    }

    private void Restart()
    {
        Piece[] temp = new Piece[pieces.Count];
        pieces.CopyTo(temp);

        pieces.Clear();
        kings.Clear();
        newPositions = 0;
        Player.currentTeam = "Light";

        foreach (Piece piece in temp)
        {
            piece.chessPiece.SetActive(true);
            piece.Setup();
        }
    }

    public static bool CheckTakeOut(int[] newPosCombination, float teamMultiplier, int listNumber, int movedAxis = -1, int notMovedAxis = -1, int letterAxis = 0, int numberAxis = 0, bool canTakeOut = default, int[] posCombination = null, bool isPawn = false)
    {
        Piece canTake = default;

        // Pawn
        if (isPawn)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                // Checking if piece is alive & is not itself & has the same position
                if (pieces[i].isAlive && pieces[i].listNumber != listNumber && newPosCombination[0] == pieces[i].posCombination[0] && newPosCombination[1] == pieces[i].posCombination[1])
                {
                    // Checking if it can take out and if it is a piece from the other team
                    if (canTakeOut && teamMultiplier != pieces[i].teamMultiplier)
                    {
                        canTake = pieces[i];
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (canTake != default) // Checking if it found a piece to take out
            {
                canTake.isAlive = false;
                canTake.chessPiece.SetActive(false);
                return true;
            }

            if (canTakeOut) // Checking if it can take out a piece but hasn't
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // Knight & king
        else if (movedAxis == -1 && notMovedAxis == -1 && letterAxis == 0 && numberAxis == 0 && posCombination == null)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                // Checking if piece is alive & is not itself & has the same position
                if (pieces.Count > i && pieces[i].isAlive && pieces[i].listNumber != listNumber && newPosCombination[0] == pieces[i].posCombination[0] && newPosCombination[1] == pieces[i].posCombination[1])
                {
                    // Checking if it is a piece from the other team
                    if (teamMultiplier != pieces[i].teamMultiplier)
                    {
                        pieces[i].isAlive = false;
                        pieces[i].chessPiece.SetActive(false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        // Rook & Queen (vertically or horizontally)
        else if (movedAxis != -1 && notMovedAxis != -1 && letterAxis == 0 && numberAxis == 0 && posCombination != null)
        {
            int start = 0;
            int end = 0;

            if (newPosCombination[movedAxis] < posCombination[movedAxis])
            {
                start = newPosCombination[movedAxis];
                end = posCombination[movedAxis];
            }
            else if (newPosCombination[movedAxis] > posCombination[movedAxis])
            {
                start = posCombination[movedAxis];
                end = newPosCombination[movedAxis];
            }


            for (int i = start; i <= end; i++)
            {
                for (int j = 0; j < pieces.Count; j++)
                {
                    // Checking if piece is alive & is not itself & has the same position & if it is a piece from the other team
                    if (pieces.Count > j && pieces[j].isAlive && pieces[j].listNumber != listNumber && newPosCombination[0] == pieces[j].posCombination[0] && newPosCombination[1] == pieces[j].posCombination[1] && teamMultiplier != pieces[j].teamMultiplier)
                    {
                        canTake = pieces[j];
                    }
                    // Checking if piece is alive & is not itself & has been "jumped" over
                    else if (pieces.Count > j && pieces[j].isAlive && pieces[j].listNumber != listNumber && pieces[j].posCombination[movedAxis] == i && pieces[j].posCombination[notMovedAxis] == newPosCombination[notMovedAxis])
                    {
                        return false;
                    }
                }
            }
        }
        // Bishop & Queen (diagonally)
        else if (movedAxis == -1 && notMovedAxis == -1 && letterAxis != 0 && numberAxis != 0 && posCombination != null)
        {
            int lengt = 0;

            if (newPosCombination[0] < posCombination[0])
            {
                lengt = posCombination[0] - newPosCombination[0];
            }
            else
            {
                lengt = newPosCombination[0] - posCombination[0];
            }

            for (int i = 0; i <= lengt; i++)
            {
                for (int j = 0; j < pieces.Count; j++)
                {
                    // Checking if piece is alive & is not itself & has the same position & if it is a piece from the other team
                    if (pieces.Count > j && pieces[j].isAlive && pieces[j].listNumber != listNumber && newPosCombination[0] == pieces[j].posCombination[0] && newPosCombination[1] == pieces[j].posCombination[1] && teamMultiplier != pieces[j].teamMultiplier)
                    {
                        canTake = pieces[j];
                    }
                    // Checking if piece is alive & is not itself & has been "jumped" over
                    else if (pieces.Count > j && pieces[j].isAlive && pieces[j].listNumber != listNumber && pieces[j].posCombination[0] == posCombination[0] + i * letterAxis && pieces[j].posCombination[1] == posCombination[1] + i * numberAxis)
                    {
                        return false;
                    }
                }
            }
        }

        if (canTake != default)
        {
            canTake.isAlive = false;
            canTake.chessPiece.SetActive(false);
        }

        return true;
    }

    private void CheckPosition(Piece piece)
    {
        if (Player.isMakingAMove)
        {
            if ((Player.currentTeam == "Light" && piece.teamMultiplier == 1f) || (Player.currentTeam == "Dark" && piece.teamMultiplier == -1f))
            {
                piece.teamIsMoving = true;
            }
        }
        else if (!Player.isMakingAMove && piece.teamIsMoving)
        {
            piece.CheckForNewPosition();
            piece.teamIsMoving = false;
        }
    }

    private void CheckMove(Piece piece)
    {
        if (!Player.isMakingAMove && newPositions == 1 && piece.hasNewPosition)
        {
            if (!piece.isOutSideBoard && piece.CheckMoveByRules())
            {
                if (Player.currentTeam == "Light")
                {
                    Player.currentTeam = "Dark";
                }
                else
                {
                    Player.currentTeam = "Light";
                }
            }

            piece.hasNewPosition = false;
            piece.SetPosition();
        }
        else if (!Player.isMakingAMove && newPositions > 1 && piece.hasNewPosition)
        {
            piece.SetPosition();
        }
    }

    public static int SaveBoardState(GameObject chessPiece, Piece piece)
    {
        if (chessPiece.name.ToLower() == "king")
        {
            kings.Add(piece);
        }

        pieces.Add(piece);
        return pieces.Count - 1;
    }

    public static (bool, int[]) HasNewPosition(int[] posCombination, Vector3 position)
    {
        // Gets the corners of the square the piece started on
        Vector3 topLeft = new Vector3(letterPos[posCombination[0]] - (letterSpace / 2), 0, numberPos[posCombination[1]] - (numberSpace / 2));
        Vector3 bottomRight = new Vector3(letterPos[posCombination[0]] + (letterSpace / 2), 0, numberPos[posCombination[1]] + (numberSpace / 2));

        if (position.x > topLeft.x && position.x < bottomRight.x && position.z > topLeft.z && position.z < bottomRight.z)
        {
            return (false, null); // Piece hasn't moved
        }
        else
        {
            bool foundNewPosCombination = false;
            int[] newPosCombination = (int[])posCombination.Clone();

            for (int i = 0; i < letterPos.Length; i++)
            {
                for (int j = 0; j < numberPos.Length; j++)
                {
                    // Gets the corners of the square every square on the board (one by one)
                    topLeft = new Vector3(letterPos[i] - (letterSpace / 2), 0, numberPos[j] - (numberSpace / 2));
                    bottomRight = new Vector3(letterPos[i] + (letterSpace / 2), 0, numberPos[j] + (numberSpace / 2));

                    if (position.x > topLeft.x && position.x < bottomRight.x && position.z > topLeft.z && position.z < bottomRight.z)
                    { // Checking for the new position
                        newPosCombination[0] = i;
                        newPosCombination[1] = j;
                        foundNewPosCombination = true;
                        break;
                    }
                }

                if (foundNewPosCombination)
                {
                    break;
                }
            }

            newPositions++;
            return (true, newPosCombination);
        }
    }

    public static (float, int) Setup(GameObject chessPiece)
    {
        float teamMultiplier;
        int pieceNr = 0;

        if (chessPiece.tag == "Light")
        {
            teamMultiplier = 1f;
        }
        else
        {
            teamMultiplier = -1f;
        }

        string[] name = chessPiece.name.ToLower().Split();

        if (name.Length > 1)
        {
            string temp = name[1].Trim('(', ')');
            pieceNr = int.Parse(temp);
        }

        return (teamMultiplier, pieceNr);
    }
}