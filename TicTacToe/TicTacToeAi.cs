using System;
using System.Collections.Generic;
using BoardGameExamples.Core;

namespace TicTacToe
{
    public class TicTacToeAi : Ai<TicTacToeGameState>
    {
        public Difficulty Difficulty { get; set; }

        public TicTacToeAi() : this(Difficulty.Unbeatable) { }

        public TicTacToeAi(Difficulty difficulty)
        {
            Difficulty = difficulty;
        }

        public TicTacToeGameState FindNextMove(TicTacToeGameState currentGameState, bool isMaxPlayer)
        {
            return MinimaxWithAlphaBetaPruning(currentGameState, (int)Difficulty, isMaxPlayer);
        }

        protected override int ComputeScore(TicTacToeGameState gameState)
        {
            int score = 0;

            IEnumerable<IEnumerable<char>> positionLines = gameState.GetPositionLines();

            foreach (IEnumerable<char> positionLine in positionLines)
            {
                score += ScoreLineForX(positionLine);
            }

            return score;
        }

        //  Heuristics used:
        //      If there are 0 O's on the line, the score is 1^xCount
        //      If there are 0 X's on the line, the score is -1^oCount
        //      Which means that if the line is blank the score is 1 (because 1^0 = 1)
        //      If the line is full and not a winning line, or has both X and O, it is worth 0 points.  ie (XXO or XO-)
        private int ScoreLineForX(IEnumerable<char> positionStates)
        {
            int xCount = 0;
            int oCount = 0;

            foreach (char t in positionStates)
            {
                switch (t)
                {
                    case TicTacToeGameState.XSpace:
                        xCount++;
                        break;
                    case TicTacToeGameState.OSpace:
                        oCount++;
                        break;
                    default:
                        break; // Do Nothing, intentionally left blank
                }
            }

            if (oCount == 0)
            {
                return (int)Math.Pow(10, xCount);
            }

            if (xCount == 0)
            {
                return -(int)Math.Pow(10, oCount);
            }

            return 0;
        }
    }
}