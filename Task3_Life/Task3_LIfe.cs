using System;

namespace GameOfLife
{

    public class LifeSimulation
    {
        private int _height;
        private int _width;
        private bool[,] cells;
        private int[,] cellsCondition;
        private string[,] genes;
        private int[] cntCellsX;
        private int cntTotalCellsX;
        private int cntTotalExtras;

        /// <summary>
        /// Создаем новую игру
        /// </summary>
        /// <param name="Heigth">Высота поля.</param>
        /// <param name="Width">Ширина поля.</param>

        public LifeSimulation(int Height, int Width)
        {
            _height = Height;
            _width = Width;
            cells = new bool[Height, Width];
            cellsCondition = new int[Height, Width];
            genes = new string[Height, Width];
            cntCellsX = new int[_height];
            GenerateField();
        }

        /// <summary>
        /// Перейти к следующему поколению и вывести результат на консоль.
        /// </summary>
        public void DrawAndGrow(bool draw)
        {
            if (draw)
            {
                DrawGame();
            }
            Grow();
        }

        /// <summary>
        /// Двигаем состояние на одно вперед, по установленным правилам
        /// 1) клетка в окружении более 3 клеток умирает;
        /// 2) клетка в окружении менее 2 клеток умирает;
        /// 3) после смерти клетки ее место оказывается зараженным на 1 поколение (новая клетка не может на нем появиться);
        /// 4) через 1 поколения место умершей клетки становится ядром питательной среды для появления новых клеток (6-9 ячеек);
        /// 5) клетка появляется в окружении 2 или 3 живых клеток, но только в питательной среде;
        /// 6) клетки могут иметь ген "экстраверта" (подсмотрено на habr'е);
        /// 7) если появившаяся клетка имеет ген "экстраверта", то в следующем поколении, она может не умереть, 
        ///    а переместиться на 1 клетку в любом направлении, чтобы оказаться в окружении 2 или 3 клеток, но
        ///    только при условии, что место не "заражено" после смерти предыдущей клетки.
        /// </summary>

        private void Grow()
        {            
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int numOfAliveNeighbors = GetNeighbors(i, j);
                    int culMed = 0;

                    for (int k = i - 1; k < i + 2; k++)
                    {
                        for (int l = j - 1; l < j + 2; l++)
                        {
                            if (!((k < 0 || l < 0) || (k >= _height || l >= _width)))
                            {    
                                culMed += cellsCondition[k, l];
                            }
                        }
                    }


                    if (cells[i, j])
                    {
                        if (numOfAliveNeighbors < 2)
                        {
                            cells[i, j] = false;
                            genes[i, j] = " ";
                            if (!Relocation(i, j))
                            {
                                cellsCondition[i, j] = 2;
                            }
                        }

                        if (numOfAliveNeighbors > 3)
                        {
                            cells[i, j] = false;
                            genes[i, j] = " ";
                            if (!Relocation(i, j))
                            {
                                cellsCondition[i, j] = 2;
                            }
                        }
                    }
                    else
                    {

                        if (numOfAliveNeighbors == 3 && cellsCondition[i, j] < 2)
                        {
                            if(culMed > 0)
                            {
                                cells[i, j] = true;
                                Genetics(i, j);
                            }
                        }
                        else if (cellsCondition[i, j] > 0) cellsCondition[i, j]--;
                    }
                }
            }
        }

        /// <summary>
        /// Смотрим сколько живых соседий вокруг клетки.
        /// </summary>
        /// <param name="x">X-координата клетки.</param>
        /// <param name="y">Y-координата клетки.</param>
        /// <returns>Число живых клеток.</returns>

        private int GetNeighbors(int x, int y)
        {
            int NumOfAliveNeighbors = 0;

            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (!((i < 0 || j < 0) || (i >= _height || j >= _width)))
                    {
                        if (cells[i, j]) NumOfAliveNeighbors++;
                    }
                }
            }
            return NumOfAliveNeighbors;
        }

        /// <summary>
        /// Принятие клетками решения о релокации.
        /// </summary>
        /// <param name="x">Х-координата клетки.</param>
        /// <param name="y">Y-координата клетки</param>
        /// <returns>Решение о релокации (релокация внутри функции), по-умолчанию: false</returns>

        private bool Relocation(int x, int y)
        {
            int numOfFutureNeighbors;
            bool relocationDeside = false;

            if (genes[x, y] == "E")
            {
                for (int k = x - 1; k < x + 2; k++)
                {
                    for (int l = y - 1; l < y + 2; l++)
                    {
                        if (!((k < 0 || l < 0) || (k >= _height || l >= _width)))
                        {
                            if (k != x && l != y && !cells[k, l])
                            {
                                numOfFutureNeighbors = GetNeighbors(k, l);
                                
                                if (numOfFutureNeighbors >= 2)
                                {
                                    cells[k, l] = true;
                                    relocationDeside = true;
                                }
                            }
                        }
                    }
                }
            }
            return relocationDeside;
        }

        /// <summary>
        /// Нарисовать Игру в консоле
        /// </summary>

        private void DrawGame()
        {
            cntTotalCellsX = 0;
            cntTotalExtras = 0;

            for (int i = 0; i < _height; i++)
            {
                cntCellsX[i] = 0;

                for (int j = 0; j < _width * 3 + 2; j++)
                {
                    if (j < _width)
                    {
                        if (cells[i, j])
                        {
                            cntCellsX[i]++;
                            cntTotalCellsX++;
                        }
                        if (genes[i, j] == "E") cntTotalExtras++;

                        Console.Write(cells[i, j] ? "x" : " ");
                    }
                    else if (j == _width) Console.Write($" : {cntCellsX[i]}\t({i})\t");
                    else if ((j > _width) && (j < _width * 2)) Console.Write(cellsCondition[i, j - _width - 1]);
                    else if (j == _width * 2) Console.Write("\t");
                    else if ((j > _width * 2) && (j < _width * 3 + 1)) Console.Write(genes[i, j - _width * 2 - 1]);
                    else if (j == _width * 3 + 1) Console.WriteLine("\r");
                }
            }

            Console.WriteLine($"\nВыживших клеток: {cntTotalCellsX}." +
                    $" Клеток экстравертов: {cntTotalExtras}.");

            //Console.SetCursorPosition(0, Console.WindowTop);
        }

        /// <summary>
        /// Инициализируем случайными значениями
        /// </summary>

        private void GenerateField()
        {
            Random cell_generator = new Random();
            int number;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    number = cell_generator.Next(2);
                    cells[i, j] = ((number == 0) ? false : true);
                    if (cells[i, j]) Genetics(i, j);
                    else genes[i, j] = " ";
                }
            }
        }

        /// <summary>
        /// Инициализация генетического кода клеток
        /// </summary>

        private void Genetics(int x, int y)
        {
            Random gene_generator = new Random();
            int gene;
            gene = gene_generator.Next(2);

            switch (gene)
            {
                case 0:
                    genes[x, y] = "I";
                    break;
                case 1:
                    genes[x, y] = "E";
                    break;
            }
        }

    }

    internal class Task3_LIfe
    {

        // Ограничения игры
        private const int Heigth = 10;
        private const int Width = 30;

        private static void Main(string[] args)
        {
            LifeSimulation sim = new LifeSimulation(Heigth, Width);

            Console.Write("Количество поколений: ");
            uint MaxRuns = uint.Parse(Console.ReadLine());
            Console.Write("Шаг отображения поколения: ");
            int stepDisplay = int.Parse(Console.ReadLine());

            int runs = 0;
            int step = 0;

            while (runs <= MaxRuns)
            {
                if (step == 0)
                {
                    Console.WriteLine($"Поколение {runs + 1}\n");
                    sim.DrawAndGrow(true);
                }
                else if ((step + 1) % stepDisplay == 0)
                {
                    Console.WriteLine($"\nПоколение {runs + 1}\n");
                    sim.DrawAndGrow(true);
                }
                else
                {
                    sim.DrawAndGrow(false);
                }
                step++;
                runs++;
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
