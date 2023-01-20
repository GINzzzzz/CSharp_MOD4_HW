Console.Write("Введите количество строк: ");
int cntRows = int.Parse(Console.ReadLine());
Console.Write("Введите количество столбцов: ");
int cntCols = int.Parse(Console.ReadLine());

Random rand = new Random();

int[,] matrix = new int[cntRows, cntCols];

for (int i = 0; i < matrix.GetLength(0); i++)
{
    for (int j = 0; j < matrix.GetLength(1); j++)
    {
        matrix[i, j] = rand.Next(-10, 10);
        Console.Write($"{matrix[i, j]} ");
    }
    Console.WriteLine("\t");
}