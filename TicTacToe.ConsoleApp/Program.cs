using System;

namespace TicTacToe.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string playAgain;
            do
            {
                PlayGame();
                Console.WriteLine("Would you like to play again? (y/n*)");
                playAgain = Console.ReadLine();
            } while (playAgain.ToLower() == "y");
        }

        static void PlayGame()
        {
            Console.WriteLine("Computer as X Player? (y/n*)");
            string computerXResult = Console.ReadLine();
            bool computerIsXPlayer = computerXResult.ToLower() == "y";

            Console.WriteLine("Computer as O Player? (y/n*)");
            string computerOResult = Console.ReadLine();
            bool computerIsOPlayer = computerOResult.ToLower() == "y";

            Console.WriteLine("Set AI difficulty ([e]asy/[m]edium/[h]ard/[u]nbeatable*)");
            string difficultyResult = Console.ReadLine();
            
            Difficulty difficulty = Difficulty.Unbeatable;
            if (difficultyResult.ToLower() == "e")
            {
                difficulty = Difficulty.Easy;
            }
            else if (difficultyResult.ToLower() == "m")
            {
                difficulty = Difficulty.Medium;
            }
            else if (difficultyResult.ToLower() == "h")
            {
                difficulty = Difficulty.Hard;
            }

            TicTacToeGame game = new TicTacToeGame(computerIsXPlayer, computerIsOPlayer, difficulty);

            OutputGameState(game.CurrentGameState);

            while (!game.IsGameOver())
            {
                if (game.NextMoveIsComputer())
                {
                    MakeComputerMove(game);
                }
                else
                {
                    MakeUserMove(game);
                }
            }

            switch (game.GetWinner())
            {
                case(GameStateResult.XWinner):
                    Console.WriteLine("The X player wins!");
                    break;
                case(GameStateResult.OWinner):
                    Console.WriteLine("The O player wins!");
                    break;
                default:
                    Console.WriteLine("It's a draw.");
                    break;
            }
        }

        static void MakeUserMove(TicTacToeGame game)
        {
            int moveIndex;
            bool success;

            Console.WriteLine("Your turn. Pick a value [0-8] to place your move.");
            do
            {
                string input = Console.ReadLine();
                bool parsed = int.TryParse(input, out moveIndex);
                bool inRange = moveIndex >= 0 && moveIndex <= 8;
                bool isAvailable = game.CurrentGameState.IsSpaceAvailable(moveIndex);
                
                success = parsed && inRange && isAvailable;

                if (!success)
                {
                    Console.WriteLine("Your Input was invalid.  Pick a value [0-8] to place your move.");    
                }

            } while (!success);
            
            OutputGameState(game.MakePlayerMove(moveIndex));
        }

        static void MakeComputerMove(TicTacToeGame game)
        {
            Console.WriteLine("AI is making a move.");
            OutputGameState(game.MakeComputerMove());
        }

        static void OutputGameState(TicTacToeGameState gameState)
        {
            Console.WriteLine("{0} (0-1-2)\n{1} (3-4-5)\n{2} (6-7-8)",
                gameState.ToString().Substring(0, 3), gameState.ToString().Substring(3, 3),
                gameState.ToString().Substring(6, 3));
        }
    }
}
