using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe_CLI
{
    public enum Players
    {
        Player,
        Computer,
        NA,
        Neither
    }
    class TicTacToeAutoPlayer
    {
        public bool winnerFound;

        //-1 = free field, 0 - player, 1 - computer
        public static int[,] board = new int[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
        public Point nextTurn;
        public bool waitingForPlayerToChoose;
        public int totalTurns = 0;
        public Players firstPlayer;
        public Players winner = Players.NA;
        public GameState currentState = new GameState(Players.Player, board: board, Players.Player);
        public bool childrenBuilt;

        public TicTacToeAutoPlayer(bool computerStartsFirst)
        {
            firstPlayer = Players.Player;
            if (computerStartsFirst)
            {
                firstPlayer = Players.Computer;
            }
        }

        public void AskForNextTurn()
        {
            waitingForPlayerToChoose = true;
            Console.WriteLine("Your turn!");
            nextTurn.X = int.Parse(Console.ReadLine());
            nextTurn.Y = int.Parse(Console.ReadLine());
        }


        public void Play()
        {
            while (!currentState.gameOver)
            {
                AskForNextTurn();
                currentState.PlaceMove(nextTurn, Players.Player);
                if (!childrenBuilt)
                {
                    BuildChildren(currentState, Players.Player, totalTurns);
                }
                totalTurns++;
                PrintBoard();
                Minimax(currentState, 0, 0, 0, Turn.max);
                currentState = currentState.nextBestState;
                totalTurns++;
                PrintBoard();
            }
        }

        public void BuildChildren(GameState state, Players turn, int numberOfTurns)
        {
            state.FindPossibleMoves();
            foreach (Point move in state.possibleMoves)
            {
                GameState child;
                Players newTurn;
                if (turn == Players.Computer)
                {
                    newTurn = Players.Player;
                }
                else
                {
                    newTurn = Players.Computer;
                }

                child = state.CreateChildState(move, newTurn);

                if (numberOfTurns < 9)
                {
                    BuildChildren(child, newTurn, numberOfTurns++);
                }
            }

            childrenBuilt = true;
        }


        public GameState Minimax(GameState state, int numberOfMoves, int alpha, int beta, Turn turn)
        {
            state.EvaluateState();

            if (state.Children.Count == 0 || state.gameOver)
            {
                return state;
            }
            GameState result;
            switch (turn)
            {
                case Turn.min:
                    int minEval = int.MaxValue;
                    GameState minState = null;

                    //Console.WriteLine($"Rec depth {numberOfMoves}, Char str {state.CharacteristicString}, Child Num {state.Children.Count}");
                    foreach (GameState child in state.Children)
                    {
                        result = Minimax(child, numberOfMoves + 1, alpha, beta, Turn.max);
                        if (result.value < minEval)
                        {
                            minEval = result.value;
                            minState = result;
                        }
                    }

                    state.nextBestMoveCharacteristicString = minState.CharacteristicString;
                    state.nextBestState = minState;
                    state.nextBestMove = minState.nextBestMove;
                    return minState;

                case Turn.max:
                    int maxEval = int.MinValue;
                    GameState maxState = null;

                    //Console.WriteLine($"Rec depth {numberOfMoves}, Char str {state.CharacteristicString}, Child Num {state.Children.Count}");
                    foreach (GameState child in state.Children)
                    {
                        result = Minimax(child, numberOfMoves + 1, alpha, beta, Turn.min);
                        //Console.WriteLine($"Rec depth: {numberOfMoves} Res Val {result.value}");
                        if (result.value > maxEval)
                        {
                            maxEval = result.value;
                            maxState = result;
                        }

                        int a = 2;
                    }
                    state.nextBestMoveCharacteristicString = maxState.CharacteristicString;
                    state.nextBestState = maxState;
                    state.nextBestMove = maxState.nextBestMove;
                    return maxState;

                default:
                    throw new ArgumentOutOfRangeException(nameof(turn), turn, null);
            }
        }



        public void PrintBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"{board[i, j]} ");
                }
                Console.WriteLine();
            }
        }

    }
}
