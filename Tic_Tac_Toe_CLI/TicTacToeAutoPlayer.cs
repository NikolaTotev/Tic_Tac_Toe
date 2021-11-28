using System;
using System.Collections.Generic;
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
            Turn nextMiniMaxSelection = Turn.min;
            while (!currentState.GameOver)
            {
                AskForNextTurn();
                currentState.PlaceMove(nextTurn, PlayerType.Player);
                currentState.EvaluateState();

                BuildChildren(currentState, PlayerType.Player, 0);
                PrintBoard();
                Minimax(currentState, 0, 0, Turn.max, 0);

                //switch (nextMiniMaxSelection)
                //{
                //    case Turn.min:
                //        nextMiniMaxSelection = Turn.max;
                //        break;
                //    case Turn.max:
                //        nextMiniMaxSelection = Turn.min;
                //        break;
                //}

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
            state.Children.Clear();
            state.FindPossibleMoves();
            PlayerType newTurn;
            if (playingPlayer == PlayerType.Computer)
            {
                newTurn = PlayerType.Player;
            }
            else
            {
                newTurn = PlayerType.Computer;
            }

            foreach (Point move in state.possibleMoves)
            {
                GameState child;

                child = state.CreateChildState(move, newTurn);

                if (!child.GameOver)
                {
                    BuildChildren(child, newTurn, recDepth+1);
                }
            }

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

                        int result = Minimax(child, 0, 0, Turn.max, recLevel++);

                        if (result < minEval)
                        {
                            minEval = result;
                        }
                    }

                    state.value = minEval;
                    //Console.WriteLine($"At rec level {recLevel}, State value: {state.value}, Min eval: {minEval} ");
                    return minEval;

                case Turn.max:
                    int maxEval = int.MinValue;
                    foreach (GameState child in state.Children)
                    {

                        int result = Minimax(child, 0, 0, Turn.min, recLevel++);

                        if (result > maxEval)
                        {
                            maxEval = result;
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
