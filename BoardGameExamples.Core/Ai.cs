using System;
using System.Collections.Generic;

namespace BoardGameExamples.Core
{
    public abstract class Ai<T> where T : class, IGameState
    {
        protected const int AlphaStart = int.MinValue + 1;
        protected const int BetaStart = int.MaxValue - 1;

        public T Minimax(T state, int depth, bool isMaxPlayer)
        {
            T nextMove;
            Minimax(state, depth, isMaxPlayer, out nextMove);
            return nextMove;
        }

        protected int Minimax(T state, int depth, bool isMaxPlayer, out T childWithMax)
        {
            childWithMax = null;
            if (depth == 0 || state.IsTerminal())
            {
                return ComputeScore(state);
            }

            List<T> childStates = state.GetChildStates(isMaxPlayer) as List<T>;

            if (childStates == null)
            {
                throw new Exception("Game state that was not Terminal failed to find child states.");
            }

            int bestScore = isMaxPlayer ? int.MinValue + 1 : int.MaxValue - 1;
            foreach (T childState in childStates)
            {
                T temp;
                int score = Minimax(childState, depth - 1, !isMaxPlayer, out temp);

                if (isMaxPlayer)
                {
                    if (score > bestScore)
                    {
                        childWithMax = childState;
                        bestScore = score;
                    }
                }
                else
                {
                    if (score < bestScore)
                    {
                        childWithMax = childState;
                        bestScore = score;
                    }
                }
            }
            return bestScore;
        }

        public T MinimaxWithAlphaBetaPruning(T state, int depth, bool isMaxPlayer)
        {
            T nextMove;
            MinimaxWithAlphaBetaPruning(state, depth, AlphaStart, BetaStart, isMaxPlayer, out nextMove);
            return nextMove;
        }

        protected int MinimaxWithAlphaBetaPruning(T state, int depth, int alpha, int beta, bool isMaxPlayer, out T childWithMax)
        {
            childWithMax = null;
            if (depth == 0 || state.IsTerminal())
            {
                return ComputeScore(state);
            }

            List<T> childStates = state.GetChildStates(isMaxPlayer) as List<T>;

            if (childStates == null)
            {
                throw new Exception("Game state that was not Terminal failed to find child states.");
            }

            foreach (T childState in childStates)
            {
                T temp;
                int childScore = MinimaxWithAlphaBetaPruning(childState, depth - 1, alpha, beta, !isMaxPlayer, out temp);

                if (isMaxPlayer)
                {
                    if (childScore > alpha)
                    {
                        childWithMax = childState;
                        alpha = childScore;
                    }
                }
                else
                {
                    if (childScore < beta)
                    {
                        childWithMax = childState;
                        beta = childScore;
                    }
                }

                if (alpha >= beta) // no need to search any farther, this is the pruning step.
                {
                    break;
                }
            }

            return isMaxPlayer ? alpha : beta;
        }

        protected abstract int ComputeScore(T gameState);
    }
}