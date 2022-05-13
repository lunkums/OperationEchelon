using System;

public class Program
{
	public static int[] possibleNumbers = { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4 };
	public static int[] edgeSigns = { -1, -1, -1, 1 };

	public static void Main()
	{
		Random rand = new Random();
		int[,] matrix;
		int rows, i, columns, j;
		int sign;
		string strMatrix = "";

		rows = rand.Next(2, 5);
		columns = rand.Next(2, 5);
		matrix = new int[rows, columns];
		for (i = 0; i < rows; i++)
		{
			for (j = 0; j < columns; j++)
			{
				matrix[i, j] = possibleNumbers[rand.Next(0, possibleNumbers.Length)];
				sign = edgeSigns[rand.Next(0, edgeSigns.Length)];
				if (i != 0 && i != rows - 1)
					sign *= -1;
				matrix[i, j] *= sign;
				strMatrix += matrix[i, j];
				if (j < columns - 1)
					strMatrix += " ";
			}
			strMatrix += "\n";
		}
		Console.Write(strMatrix);
	}
}