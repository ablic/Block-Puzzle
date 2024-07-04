using System.Drawing;
using UnityEngine;

namespace RaspberryGames.BlockPuzzle
{
    public static class FigureMatrices
    {
        public static int[][,] matrices =
        {
            new [,] // POINT
            { 
                { 1 }
            },

            new [,] // LINE 2
            {
                { 1, 1 },
                { 0, 0 },
            },

            new [,] // LINE 3
            {
                { 0, 0, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 },
            },

            new [,] // LINE 4
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
            },

            new [,] // LINE 5
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
            },

            new [,] // SQUARE 2
            {
                { 1, 1 },
                { 1, 1 },
            },

            new [,] // SQUARE 3
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 1, 1 },
            },

            new [,] // SQUARE 4
            {
                { 1, 1, 1, 1 },
                { 1, 1, 1, 1 },
                { 1, 1, 1, 1 },
                { 1, 1, 1, 1 },
            },

            new [,] // CORNER 3
            {
                { 1, 0 },
                { 1, 1 }
            },

            new [,] // CORNER 4
            {
                { 0, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 1 },
            },

            new [,] // CORNER 5
            {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 1 },
            },

            new [,] // T
            {
                { 0, 0, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 },
            },

            new [,] // U
            {
                { 1, 0, 1 },
                { 1, 1, 1 },
                { 0, 0, 0 },
            },

            new [,] // CROSS
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 },
            },

            new [,] // SNAKE
            {
                { 0, 0, 0 },
                { 0, 1, 1 },
                { 1, 1, 0 },
            },
        };

        public static int[,] GetRandom()
        {
            int[,] initial = matrices.GetRandomElement();
            int size = initial.GetLength(0);

            initial = Rotate(initial, size);
            initial = Reflect(initial, size);

            return initial;
        }

        private static int[,] Rotate(int[,] initial, int size)
        {
            int[,] rotated = new int[size, size];
            int rotations = Random.Range(0, 4);

            if (rotations == 0)
                return initial;

            for (int i = 0; i < rotations; i++)
            {
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        rotated[x, y] = initial[y, size - 1 - x];
                    }
                }
            }

            return rotated;
        }

        private static int[,] Reflect(int[,] initial, int size)
        {
            int[,] reflected = new int[size, size];

            if (Random.value > 0.5f)
                return initial;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    reflected[x, y] = initial[size - 1 - x, y];
                }
            }

            return reflected;
        }
    }
}