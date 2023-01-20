Console.Write("Введите количество строк: ");
int cntRows = int.Parse(Console.ReadLine());
Console.Write("Введите количество столбцов: ");
int cntCols = int.Parse(Console.ReadLine());

Random rand = new Random();

int[,] matrix1 = new int[cntRows, cntCols];
int[,] matrix2 = new int[cntRows, cntCols];
int[,] matrixSum = new int[cntRows, cntCols];

Console.WriteLine("\nМатрица 1:\n");

for (int i = 0; i < matrix1.GetLength(0); i++)
{
    for (int j = 0; j < matrix1.GetLength(1); j++)
    {
        matrix1[i, j] = rand.Next(-10, 10);
        Console.Write($"{matrix1[i, j]}\t\t");
    }
    Console.WriteLine();
}

Console.WriteLine("\nМатрица 2:\n");

for (int i = 0; i < matrix2.GetLength(0); i++)
{
    for (int j = 0; j < matrix2.GetLength(1); j++)
    {
        matrix2[i, j] = rand.Next(-10, 10);
        Console.Write($"{matrix2[i, j]}\t\t");
    }
    Console.WriteLine();
}

// Т.к. сложение возможно только в случае одинаковой размерности матрицы для вложенного
// цикла используем cntRows, cntCols. Однако может использоваться GetLength в отношении
// любой из матриц.

Console.WriteLine("\nСумма матриц 1 и 2:\n");

for (int i = 0; i < cntRows; i++)
{
    for (int j = 0; j < cntCols; j++)
    {
        matrixSum[i, j] = matrix1[i, j] + matrix2[i, j];
        Console.Write($"{matrixSum[i, j]}\t\t");
    }
    Console.WriteLine();
}
