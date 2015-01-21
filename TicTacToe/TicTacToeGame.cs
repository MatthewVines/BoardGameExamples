using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class TicTacToeGame
    {
        public bool XPlayerIsAi { get; private set; }
        public bool OPlayerIsAi { get; private set; }

        public bool XMakesNextMove { get; private set; }

        protected Stack<TicTacToeGameState> GameStateHistory;
        public TicTacToeGameState CurrentGameState
        {
            get { return GameStateHistory.Peek(); }
            protected set { GameStateHistory.Push(value); }
        }

        private readonly TicTacToeAi _ai;

        public TicTacToeGame() : this(true, true) { }

        public TicTacToeGame(bool xPlayerIsAi, bool oPlayerIsAi) : this(xPlayerIsAi, oPlayerIsAi, Difficulty.Unbeatable) { }

        public TicTacToeGame(bool xPlayerIsAi, bool oPlayerIsAi, Difficulty difficulty)
        {
            GameStateHistory = new Stack<TicTacToeGameState>();
            CurrentGameState = new TicTacToeGameState();
            XPlayerIsAi = xPlayerIsAi;
            OPlayerIsAi = oPlayerIsAi;
            _ai = new TicTacToeAi(difficulty);
            XMakesNextMove = true;
        }

        public TicTacToeGameState MakePlayerMove(int moveIndex)
        {
            if (moveIndex < 0 || moveIndex > 8)
            {
                throw new ArgumentOutOfRangeException("moveIndex", moveIndex, "Must be a value in the range [0-8]");
            }

            char[] newGameState = CurrentGameState.SerializedBoardState.ToCharArray();

            if (XMakesNextMove)
            {
                newGameState[moveIndex] = TicTacToeGameState.XSpace;
            }
            else
            {
                newGameState[moveIndex] = TicTacToeGameState.OSpace;
            }

            CurrentGameState = new TicTacToeGameState(new string(newGameState));

            XMakesNextMove = !XMakesNextMove;
            return CurrentGameState;
        }

        public TicTacToeGameState MakeComputerMove()
        {
            TicTacToeGameState nextGameState = _ai.FindNextMove(CurrentGameState, XMakesNextMove);
            if (nextGameState != null)
            {
                CurrentGameState = nextGameState;
            }

            XMakesNextMove = !XMakesNextMove;
            return CurrentGameState;
        }

        public bool IsGameOver()
        {
            return CurrentGameState.IsTerminal();
        }

        public GameStateResult GetWinner()
        {
            return CurrentGameState.DetermineCurrentStateResult();
        }

        public bool NextMoveIsComputer()
        {
            return (XMakesNextMove && XPlayerIsAi)
                   || (!XMakesNextMove && OPlayerIsAi);
        }
    }
}