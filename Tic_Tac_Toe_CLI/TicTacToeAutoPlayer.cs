using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe_CLI
{
    public enum PlayerType
    {
        Player,
        Computer,
        NA,
        Neither
    }
    class TicTacToeAutoPlayer
    {
        //-1 = free field, 0 - player, 1 - computer
        public static int[,] board = new int[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
        public Point nextTurn;
        public GameState currentState;
        public bool gameOver;
        public PlayerType winner;
        public bool gameTreeBuilt;

        public TicTacToeAutoPlayer(bool computerStartsFirst)
        {
            currentState = new GameState(PlayerType.Player, board, Point.Empty);
        }

        public void AskForNextTurn()
        {
            Console.WriteLine("Your turn!");
            nextTurn.X = int.Parse(Console.ReadLine());
            nextTurn.Y = int.Parse(Console.ReadLine());
        }


        public void Play()
        {
            while (!currentState.GameOver)
            {
                AskForNextTurn();
                currentState.PlaceMove(nextTurn, PlayerType.Player);
                currentState.EvaluateState();

                BuildChildren(currentState, PlayerType.Player, 0);
                PrintBoard();
                Minimax(currentState, int.MinValue, int.MaxValue, Turn.max, 0);

                var items = from child in currentState.Children
                            orderby child.value ascending
                            select child;


                GameState nextBest = items.Last();
                nextTurn = nextBest.DifferenceFromParent;
                currentState.PlaceMove(nextTurn, PlayerType.Computer);
                PrintBoard();
                currentState.EvaluateState();
                int a = 14;
            }

            Console.WriteLine("=================");
            Console.WriteLine("=== Game Over ===");
            Console.WriteLine("=================");
            Console.WriteLine($"The Winner is {currentState.WinningPlayer}");
        }

        public void BuildChildren(GameState state, PlayerType playingPlayer, int recDepth)
        {
            state.BuildChildren(playingPlayer, 0);
            gameTreeBuilt = true;
        }


        public int Minimax(GameState state, int alpha, int beta, Turn minMaxSelector, int recLevel)
        {
            if (state.GameOver)
            {
                return state.value;
            }

            switch (minMaxSelector)
            {
                case Turn.min:
                    int minEval = int.MaxValue;
                    foreach (GameState child in state.Children)
                    {

                        int result = Minimax(child, alpha, beta, Turn.max, recLevel++);

                        if (result < minEval)
                        {
                            minEval = result;
                        }

                        int newBeta = result;
                        if (beta > result)
                        {
                            newBeta = beta;
                        }

                        if (newBeta <= alpha)
                        {
                            break;
                        }
                    }

                    state.value = minEval;
                    //Console.WriteLine($"At rec level {recLevel}, State value: {state.value}, Min eval: {minEval} ");
                    return minEval;

                case Turn.max:
                    int maxEval = int.MinValue;
                    foreach (GameState child in state.Children)
                    {

                        int result = Minimax(child, alpha, beta, Turn.min, recLevel++);

                        if (result > maxEval)
                        {
                            maxEval = result;
                        }

                        int newAlpha = result;
                        if (alpha > result)
                        {
                            newAlpha = alpha;
                        }

                        if (beta <= newAlpha)
                        {
                            break;
                        }
                    }

                    state.value = maxEval;
                    //Console.WriteLine($"At rec level {recLevel}, State value: {state.value}, Min eval: {maxEval} ");
                    return maxEval;
            }

            return 55;
        }


        public void PrintBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentState.Board[i, j] == -1)
                    {
                        Console.Write("~ ");
                    }
                    else
                    {
                        Console.Write($"{currentState.Board[i, j]} ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

    }
}
