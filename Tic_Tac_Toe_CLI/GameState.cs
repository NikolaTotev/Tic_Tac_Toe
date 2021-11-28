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
        public PlayerType WinningPlayer;
        public PlayerType LastPlayer;
        public bool GameOver;
        public Point DifferenceFromParent;

        public GameState(PlayerType lastPlayerToPlay, int[,] board, Point lastMove)
        {
            Array.Copy(board, Board, board.Length);
            LastPlayer = lastPlayerToPlay;
            DifferenceFromParent = new Point(lastMove.X, lastMove.Y);
            EvaluateState();
        }

        public void EvaluateState()
        {
            Tuple<PlayerType, bool> checkResult = GameOverCheck();
            WinningPlayer = checkResult.Item1;
            GameOver = checkResult.Item2;

            if (GameOver)
            {
                switch (WinningPlayer)
                {
                    case PlayerType.Player:
                        value = -1;
                        break;
                    case PlayerType.Computer:
                        value = 1;
                        break;
                    case PlayerType.Neither:
                        value = 0;
                        break;

                }
            }
        }

        public Tuple<PlayerType, bool> GameOverCheck()
        {
            bool hasEqualRow = false;

            //Check Rows
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] == Board[i, 1] && Board[i, 1] == Board[i, 2])
                {
                    if (Board[i, 0] != -1)
                    {
                        return new Tuple<PlayerType, bool>(GetPlayerType(Board[i, 0]), true);
                    }
                }
            }

            //Check Columns
            for (int i = 0; i < 3; i++)
            {
                if (Board[0, i] == Board[1, i] && Board[1, i] == Board[2, i])
                {
                    if (Board[0, i] != -1)
                    {
                        return new Tuple<PlayerType, bool>(GetPlayerType(Board[i, 0]), true);
                    }
                }
            }

            //Check Diagonals
            if (Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2])
            {
                if (Board[0, 0] != -1)
                {
                    return new Tuple<PlayerType, bool>(GetPlayerType(Board[0, 0]), true);
                }
            }

            if (Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0])
            {
                if (Board[2, 0] != -1)
                {
                    return new Tuple<PlayerType, bool>(GetPlayerType(Board[2, 0]), true);
                }
            }

            //Check if there are any available spots on the board.
            bool boardFull = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == -1)
                    {
                        boardFull = false;
                        break;
                    }
                }
            }

            if (boardFull)
            {
                return new Tuple<PlayerType, bool>(PlayerType.Neither, true);
            }

            return new Tuple<PlayerType, bool>(PlayerType.Neither, false);
        }


        public PlayerType GetPlayerType(int symbol)
        {
            switch (symbol)
            {
                case 0:
                    return PlayerType.Player;
                case 1:
                    return PlayerType.Computer;
            }

            return PlayerType.NA;
        }

        public void FindPossibleMoves()
        {
            possibleMoves.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == -1)
                    {
                        possibleMoves.Add(new Point(i, j));
                    }
                }
            }
        }

        public GameState CreateChildState(Point move, PlayerType lastToPlay)
        {
            int[,] newBoard = new int[3, 3];
            Array.Copy(Board, newBoard, Board.Length);

            switch (lastToPlay)
            {
                case PlayerType.Player:
                    newBoard[move.X, move.Y] = 0;
                    break;
                case PlayerType.Computer:
                    newBoard[move.X, move.Y] = 1;
                    break;
            }

            GameState newChild = new GameState(lastToPlay, newBoard, move);
            Children.Add(newChild);
            return newChild;
        }

        public void PlaceMove(Point move, PlayerType player)
        {
            switch (player)
            {
                case PlayerType.Player:
                    Board[move.X, move.Y] = 0;
                    break;
                case PlayerType.Computer:
                    Board[move.X, move.Y] = 1;
                    break;
            }
        }
    }
}
