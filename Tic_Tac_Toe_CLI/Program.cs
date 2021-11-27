using System;

namespace Tic_Tac_Toe_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToeAutoPlayer autoPlayer = new TicTacToeAutoPlayer(false);
            autoPlayer.Play();
        }
    }
}
