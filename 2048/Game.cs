using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2048
{
    class Game
    {
        private const int size = 4;
        private Tile[,] board;
        private const uint startTileValue = 2;

        private uint score = 0;
        private bool gameOver = false;
        private bool moved = true;

        /// <summary>
        /// Игровое поле и его значения
        /// </summary>
        public Tile[,] Board { get { return board; } }
        /// <summary>
        /// Статус проигрыша
        /// </summary>
        public bool GameOver { get { return gameOver; } }
        /// <summary>
        /// Текущий игровой счет
        /// </summary>
        public uint Score { get { return score; } }

        private Random rnd;

        /// <summary>
        /// Создать новую игру
        /// </summary>
        public Game()
        {
            board = new Tile[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    board[i, j] = new Tile(0);
                }
            }

            rnd = new Random();

            AddRandomTile();
        }

        /// <summary>
        /// Обновить игровое состояние
        /// </summary>
        /// <param name="direction">Направление в котором происходит свайп</param>
        public void Update(Directions direction)
        {
            if (gameOver)
                return;

            moved = false;
            switch(direction)
            {
                case Directions.Up:
                    for (int j = 0; j < size; j++)
                    {
                        int i = 1;
                        while(i < size)
                        {
                            if (board[i, j].value != 0)
                                MoveVertical(i, j, -1);
                            i++;
                        }
                    }
                    break;
                case Directions.Down:
                    for (int j = 0; j < size; j++)
                    {
                        int i = size - 2;
                        while(i >= 0)
                        {
                            if (board[i, j].value != 0)
                                MoveVertical(i, j, 1);
                            i--;
                        }
                    }
                    break;
                case Directions.Left:
                    for (int i = 0; i < size; i++)
                    {
                        int j = 1;
                        while(j < size)
                        {
                            if (board[i, j].value != 0)
                                MoveHorizontal(i, j, -1);
                            j++;
                        }
                    }
                    break;
                case Directions.Right:
                    for (int i = 0; i < size; i++)
                    {
                        int j = size - 2;
                        while(j >= 0)
                        {
                            if (board[i, j].value != 0)
                                MoveHorizontal(i, j, 1);
                            j--;
                        }
                    }
                    break;
            }

            ResetBlockedTiles();

            if (moved)
            {
                AddRandomTile();
            }

            gameOver = !CanMove();
        }

        /// <summary>
        /// Сбросить статус всех тайлов
        /// </summary>
        private void ResetBlockedTiles()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j].blocked = false;
                }
            }
        }

        /// <summary>
        /// Произвести вертикальный свайп
        /// </summary>
        /// <param name="i">Координата i</param>
        /// <param name="j">Координата j</param>
        /// <param name="d">Направление</param>
        private void MoveVertical(int i, int j, int d)
        {
            if (board[i + d, j].value != 0 && board[i + d, j].value == board[i, j].value && !board[i, j].blocked && !board[i + d, j].blocked)
            {
                board[i, j].value = 0;
                board[i + d, j].value *= 2;
                score += board[i + d, j].value;
                board[i + d, j].blocked = true;
                moved = true;
            }
            else if(board[i + d, j].value == 0 && board[i, j].value != 0)
            {
                board[i + d, j].value = board[i, j].value;
                board[i, j].value = 0;
                moved = true;
            }
            if (d > 0)
            {
                if (i + d < size - 1)
                {
                    MoveVertical(i + d, j, 1);
                }
            }
            else
            {
                if (i + d > 0)
                {
                    MoveVertical(i + d, j, -1);
                }
            }
        }

        /// <summary>
        /// Произвести горизонтальный свайп
        /// </summary>
        /// <param name="i">Координата i</param>
        /// <param name="j">Координата j</param>
        /// <param name="d">Направление</param>
        private void MoveHorizontal(int i, int j, int d)
        {
            if (board[i, j + d].value != 0 && board[i, j + d].value == board[i, j].value && !board[i, j].blocked && !board[i, j + d].blocked)
            {
                board[i, j].value = 0;
                board[i, j + d].value *= 2;
                score += board[i, j + d].value;
                board[i, j + d].blocked = true;
                moved = true;
            }
            else if (board[i, j + d].value == 0 && board[i, j].value != 0)
            {
                board[i, j + d].value = board[i, j].value;
                board[i, j].value = 0;
                moved = true;
            }
            if (d > 0)
            {
                if (j + d < size - 1)
                {
                    MoveHorizontal(i, j + d, 1);
                }
            }
            else
            {
                if (j + d > 0)
                {
                    MoveHorizontal(i, j + d, -1);
                }
            }
        }

        /// <summary>
        /// Проверить, возможен ли свайп
        /// </summary>
        /// <returns>Возможность свайпа</returns>
        private bool CanMove()
        {
            if (AvailableTilesCount() != 0)
                return true;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (CheckValue(i + 1, j, board[i, j].value)) return true;
                    if (CheckValue(i - 1, j, board[i, j].value)) return true;
                    if (CheckValue(i, j + 1, board[i, j].value)) return true;
                    if (CheckValue(i, j - 1, board[i, j].value)) return true;
                 }
            }
            return false;
        }

        /// <summary>
        /// Сравнить значения тайлов
        /// </summary>
        /// <param name="i">Координаты тайла</param>
        /// <param name="j">Координата тайла</param>
        /// <param name="value">Значение тайла</param>
        /// <returns></returns>
        private bool CheckValue(int i, int j, uint value)
        {
            if (i < 0 || i > size - 1 || j < 0 || j > size - 1)
                return false;
            return board[i, j].value == value;
        }

        /// <summary>
        /// Добавить рандомный тайл
        /// </summary>
        private void AddRandomTile()
        {
            List<Tuple<int, int>> avTiles = AvailableTiles();
            if (avTiles.Count == 0)
                return;
            Tuple<int, int> randomTile = avTiles[rnd.Next(avTiles.Count)];
            board[randomTile.Item1, randomTile.Item2] = new Tile(startTileValue);
        }

        /// <summary>
        /// Получить список свободных тайлов
        /// </summary>
        /// <returns>Список тайлов</returns>
        private List<Tuple<int, int>> AvailableTiles()
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j].value == 0)
                        result.Add(new Tuple<int, int>(i, j));
                }
            }

            return result;
        }

        /// <summary>
        /// Число пустых тайлов
        /// </summary>
        /// <returns>Число тайлов</returns>
        private int AvailableTilesCount()
        {
            return AvailableTiles().Count;
        }
    }

    class Tile
    {
        public uint value = 0;
        public bool blocked = false;

        public Tile(uint value)
        {
            this.value = value;
        }
    }

    /// <summary>
    /// Указывает направление перемещения тайлов
    /// </summary>
    enum Directions
    {
        Left,
        Up,
        Right,
        Down
    }
}
