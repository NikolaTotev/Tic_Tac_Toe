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
        public int[,] board = new int[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
        public Point nextTurn;
        public bool waitingForPlayerToChoose;
        public int totalTurns = 0;
        public Players firstPlayer;
        public Players winner = Players.NA;
        public GameState currentState;

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

        public void CalculateNextTurn()
        {

        }

        public void Play()
        {
            while (!currentState.gameOver)
            {
                AskForNextTurn();
                currentState.PlaceMove(nextTurn, Players.Player);
                totalTurns++;
                BuildChildren(currentState, Players.Computer, totalTurns);
                currentState = Minimax(currentState, totalTurns, 0, 0, Turn.max);
                totalTurns++;
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
        }


        public GameState Minimax(GameState state, int numberOfMoves, int alpha, int beta, Turn turn)
        {
            GameState nextState = null;
            if (numberOfMoves == 9)
            {
                state.EvaluateState();
                return state;
            }
            switch (turn)
            {
                case Turn.min:
                    int minEval = int.MinValue;
                    GameState minState = null;
                    foreach (GameState child in state.Children)
                    {
                        GameState result = Minimax(child, numberOfMoves + 1, alpha, beta, Turn.max);
                        if (result.value < minEval)
                        {
                            minEval = result.value;
                            minState = result;
                        }
                    }
                    return minState;
                    
                case Turn.max:
                    int maxEval = int.MinValue;
                    GameState maxState = null;
                    foreach (GameState child in state.Children)
                    {
                        GameState result = Minimax(child, numberOfMoves + 1, alpha, beta, Turn.min);
                        if (result.value > maxEval)
                        {
                            maxEval = result.value;
                            maxState = result;
                        }
                    }
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
                    Console.Write($"{board[i,j]} ");
                }
                Console.WriteLine();
            }
        }

    }
}
