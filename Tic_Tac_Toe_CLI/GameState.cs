using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe_CLI
{
    enum Turn { min, max }
    class GameState
    {
        public int alpha;
        public int beta;
        public int value;
        public int[,] Board = new int[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
        public List<Point> possibleMoves = new List<Point>();
        public List<GameState> Children = new List<GameState>();
  


        public GameState(Players firstPlayer, int[,] board, Players turn)
        {
           
        }

        public void EvaluateState()
        {
           
        }

        public int EvalFunction()
        {
          

        }

        public Tuple<Players, bool> CheckForWinners()
        {
           
        }


        public Players GetPlayerType(int symbol)
        {
            Players playerType = Players.Player;

            switch (symbol)
            {
                case 0:
                    playerType = m_FirstPlayer;
                    break;
                case 1:
                    if (m_FirstPlayer == Players.Computer)
                    {
                        playerType = Players.Player;
                    }
                    else
                    {
                        playerType = Players.Computer;
                    }
                    break;
            }

            return playerType;
        }

        public void FindPossibleMoves()
        {
           
        }

        public GameState CreateChildState(Point move, Players turn)
        {
            int[,] newBoard = new int[3, 3];
            Array.Copy(Board, newBoard, Board.Length);
            
        }

        public void PlaceMove(Point move, Players turn)
        {
           
        }
    }
}
