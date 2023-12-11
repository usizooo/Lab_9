using System;

public class Program
{
    public static void PrintGameOver()
    {
        Console.SetCursorPosition(30, 0);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("GAME OVER!!!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(0, 0);
    }

    public static void IncorrectInput()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Неверный ввод, попробуйте снова");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void Main()
    {
        Game.Run();
    }
}