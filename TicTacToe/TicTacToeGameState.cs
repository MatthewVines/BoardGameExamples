using System;
using System.Collections.Generic;
using System.Linq;
using BoardGameExamples.Core;
using BoardGameExamples.Core.Utility;

namespace TicTacToe
{
    public class TicTacToeGameState : IGameState
    {
        private const string EmptyBoard = "---------";

        public const char XSpace = 'x';
        public const char OSpace = 'o';
        public const char EmptySpace = '-';

        public string SerializedBoardState { get; set; }

        public TicTacToeGameState() : this(EmptyBoard){}
    
        public TicTacToeGameState(string serializedBoardState)
        {
            SerializedBoardState = serializedBoardState;
        }
        
        public override string ToString()
        {
            return SerializedBoardState;
        }

        public IEnumerable<IEnumerable<char>> GetPositionLines()
        {
            int[,] lines =
            {
                {0, 1, 2}, // horizontal top
                {3, 4, 5}, // horizontal middle
                {6, 7, 8}, // horizontal bottom
                {0, 3, 6}, // vertical left
                {1, 4, 7}, // vertical middle
                {2, 5, 8}, // vertical right
                {0, 4, 8}, // diagonal left to right
                {2, 4, 6}  // diagonal right to left
            };

            IList<IEnumerable<char>> result = new List<IEnumerable<char>>();

            for (int i = lines.GetLowerBound(0); i <= lines.GetUpperBound(0); i++)
            {
                result.Add(new char[3]
                {
                    SerializedBoardState[lines[i, 0]],
                    SerializedBoardState[lines[i, 1]], 
                    SerializedBoardState[lines[i, 2]]
                });
            }

            return result;
        }

        public GameStateResult DetermineCurrentStateResult()
        {
            IEnumerable<IEnumerable<char>> positionLines = GetPositionLines();

            foreach (IEnumerable<char> line in positionLines)
            {
                int xCount = 0;
                int oCount = 0;

                foreach (char play in line)
                {
                    if (play == XSpace)
                    {
                        xCount++;
                    }
                    else if (play == OSpace)
                    {
                        oCount++;
                    }

                    if (xCount == 3)
                    {
                        return GameStateResult.XWinner;
                    }

                    if (oCount == 3)
                    {
                        return GameStateResult.OWinner;
                    }
                }
            }
            // There is no winner, so if the board is full it's draw, otherwise, we must be in progress.
            return SerializedBoardState.Contains(EmptySpace) ? GameStateResult.InProgress : GameStateResult.Draw;            
        }

        public bool IsTerminal()
        {
            GameStateResult result = DetermineCurrentStateResult();

            return (result == GameStateResult.Draw 
                || result == GameStateResult.XWinner 
                || result == GameStateResult.OWinner);
        }

        public IEnumerable<IGameState> GetChildStates(bool isMaxPlayerTurn)
        {
            List<TicTacToeGameState> children = new List<TicTacToeGameState>();

            IList<int> indexes = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            indexes.Shuffle(new Random());

            foreach (int index in indexes)
            {
                char[] newBoardState = SerializedBoardState.ToCharArray();
                if (newBoardState[index] == EmptySpace)
                {
                    newBoardState[index] = isMaxPlayerTurn ? XSpace : OSpace;
                    string newBoardString = new string(newBoardState);

                    IList<string> transformations = GetTransformations(newBoardString);

                    if (!children.Any(x => transformations.Contains(x.SerializedBoardState)))
                    {
                        children.Add(new TicTacToeGameState(newBoardString));
                    }
                }
            }
            return children;
        }

        private IList<string> GetTransformations(string boardState)
        {
            IList<string> transformations = new List<string> {boardState};

            char[] original = boardState.ToCharArray();
            char[] rotate90 = new char[9]
            {
                original[6],
                original[3],
                original[0],
                original[7],
                original[4],
                original[1],
                original[8],
                original[5],
                original[2]
            };
            transformations.Add(new string(rotate90));

            transformations.Add(new string(original.Reverse().ToArray()));
            transformations.Add(new string(rotate90.Reverse().ToArray()));

            return transformations;
        }

        public bool IsSpaceAvailable(int spaceIndex)
        {
            return SerializedBoardState[spaceIndex] == EmptySpace;
        }
    }
}