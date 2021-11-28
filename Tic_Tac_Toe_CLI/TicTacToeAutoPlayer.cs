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
                if (!gameTreeBuilt)
                {
                    BuildChildren(currentState, PlayerType.Player);
                }

                Minimax(currentState, 0, 0, Turn.min);

                nextTurn = 
            }
        }

        public void BuildChildren(GameState state, PlayerType playingPlayer)
        {
            state.FindPossibleMoves();
            foreach (Point move in state.possibleMoves)
            {
                GameState child;

                PlayerType newTurn;
                if (playingPlayer == PlayerType.Computer)
                {
                    newTurn = PlayerType.Player;
                }
                else
                {
                    newTurn = PlayerType.Computer;
                }

                child = state.CreateChildState(move, newTurn);

                if (!child.GameOver)
                {
                    BuildChildren(child, newTurn);
                }
            }

            gameTreeBuilt = true;
        }


        public GameState Minimax(GameState state, int alpha, int beta, Turn minMaxSelector)
        {
            if (state.GameOver)
            {
                return state;
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
