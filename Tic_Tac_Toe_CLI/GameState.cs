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
        public Players m_FirstPlayer;
        public Players winner = Players.NA;
        public Players currentTurn;
        public int[,] Board = new int[,] { { -1, -1, -1 }, { -1, -1, -1 }, { -1, -1, -1 } };
        public List<Point> possibleMoves = new List<Point>();
        public List<GameState> Children = new List<GameState>();
        public bool gameOver;
        public string CharacteristicString;

        public int nextBestMove;
        public string nextBestMoveCharacteristicString;
        public GameState nextBestState;


        public GameState(Players firstPlayer, int[,] board, Players turn)
        {
            m_FirstPlayer = firstPlayer;
            currentTurn = turn;
            Board = board;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    builder.Append(board[i, j].ToString());
                }
            }

            CharacteristicString = builder.ToString();

            EvaluateState();
        }

        public void EvaluateState()
        {
            Tuple<Players, bool> winCheck = CheckForWinners();
            winner = winCheck.Item1;
            if (winCheck.Item2)
            {
                gameOver = true;
            }
            value = EvalFunction();
        }

        public int EvalFunction()
        {
            if (winner == Players.Neither)
            {
                return 0;
            }

            if (winner == Players.NA)
            {
                return 2;
            }
            return (winner == m_FirstPlayer) ? 1 : -1;

        }

        public Tuple<Players, bool> CheckForWinners()
        {
            bool hasEqualRow = false;

            //Check Rows
            for (int i = 0; i < 3; i++)
            {
                if (Board[i, 0] == Board[i, 1] && Board[i, 1] == Board[i, 2])
                {
                    if (Board[i, 0] != -1)
                    {
                        return new Tuple<Players, bool>(GetPlayerType(Board[i, 0]), true);
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
                        return new Tuple<Players, bool>(GetPlayerType(Board[i, 0]), true);
                    }
                }
            }

            //Check Diagonals
            if (Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2])
            {
                if (Board[0, 0] != -1)
                {
                    return new Tuple<Players, bool>(GetPlayerType(Board[0, 0]), true);
                }
            }

            if (Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0])
            {
                if (Board[2, 0] != -1)
                {
                    return new Tuple<Players, bool>(GetPlayerType(Board[2, 0]), true);
                }
            }

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
                return new Tuple<Players, bool>(Players.Neither, true);
            }

            return new Tuple<Players, bool>(Players.Player, false);
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

            if (possibleMoves.Count == 0)
            {
                gameOver = true;
            }
        }

        public GameState CreateChildState(Point move, Players turn)
        {
            int[,] newBoard = new int[3, 3];
            Array.Copy(Board, newBoard, Board.Length);
            switch (turn)
            {
                case Players.Player:
                    newBoard[move.X, move.Y] = 0;
                    break;
                case Players.Computer:
                    newBoard[move.X, move.Y] = 1;
                    break;
            }

            GameState newChild = new GameState(m_FirstPlayer, newBoard, turn);
            Children.Add(newChild);
            return newChild;
        }

        public void PlaceMove(Point move, Players turn)
        {
            switch (turn)
            {
                case Players.Player:
                    Board[move.X, move.Y] = 0;
                    break;
                case Players.Computer:
                    Board[move.X, move.Y] = 1;
                    break;
            }
        }
    }
}
