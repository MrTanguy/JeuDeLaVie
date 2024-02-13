using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuDeLaVie
{
    internal class Game
    {
        int x;
        int y;
        int[,] games;

        public Game(int x, int y)
        {
            this.x = x;
            this.y = y;
            games = new int[x, y];

            Random random = new Random(); // Initialisez le générateur de nombres aléatoires une seule fois

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (random.Next(100) < 30) // Utilisez le même générateur de nombres aléatoires
                    {
                        games[i, j] = 1;
                    }
                    else
                    {
                        games[i, j] = 0;
                    }
                }
            }
        }

        public int[,] NewState()
        {

            int[,] nextGame = new int[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int alive = 0;

                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (!(k == 0 && l == 0) &&
                                i + k >= 0 && i + k < x &&
                                j + l >= 0 && j + l < y)
                            {
                                if (games[i + k, j + l] == 1)
                                {
                                    alive++;
                                }
                            }
                        }
                    }
                    if (alive >= 3)
                    {
                        nextGame[i, j] = 1;
                    }
                }
            }
            games = nextGame;
            return nextGame;
        }
    }
}
