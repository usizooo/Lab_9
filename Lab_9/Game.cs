using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

public static class Game
{
    // Перечисление для указания сложности
    public enum Difficulty
    {
        Easy = 3,
        Medium = 2,
        Hard = 1
    }

    private static Cell[,] field = new Cell[0, 0];
    private static int height;
    private static int width;
    private static bool isGame = true;
    private static Difficulty difficulty;
    private static int gameSpeed = 150;
    // Объект, который искусственным образом будет разделяемым ресурсом для потоков
    // ввода и отрисовки игры.
    private static object locker = new object();

    // Основной метод запускающий игру, параметризуем сложностью
    public static void Run()
    {
        // Ожидаем от пользователя ввод уровня сложности
        InitDifficulty();

        // Инициализируем поле в соответствии с указанной сложностью
        InitField();

        // Создаём два потока:
        // на отрисовку 
        Thread rendering = new Thread(Rendering);
        // и на считывание движений змеи
        Thread control = new Thread(PlayerControl);
        control.Start();
        rendering.Start();

        // Пока игра запущена работают эти два потока
        while (isGame);

        // Как игра закончилась сливаем эти потоки с основным
        control.Join();
        rendering.Join();

        Console.SetCursorPosition(0, 20);
    }

    private static void InitDifficulty()
    {
        Console.Clear();
        Console.WriteLine("Выберете сложность: ");
        Console.WriteLine("1. Легко ");
        Console.WriteLine("2. Средне ");
        Console.WriteLine("3. Сложно ");

        switch (Convert.ToInt32(Console.ReadLine()))
        {
            case 1:
                difficulty = Difficulty.Easy;
                break;
            case 2:
                difficulty = Difficulty.Medium;
                break;
            case 3:
                difficulty = Difficulty.Hard;
                break;
            default:
                Program.IncorrectInput();
                Console.ReadKey();
                InitDifficulty();
                break;
        };
        Console.Clear();
    }

    private static void InitField()
    {
        // В зависимости от сложности указываем ширину и высоту поля
        height = 5 * ((int)difficulty + 1);
        width = 5 * ((int)difficulty + 1);
        // Инициализируем поел
        field = new Cell[height, width];
        // Сначала по переиметру ставим стены, а внтури пустые клетки
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
                    field[i, j] = new Wall(i, j);
                else
                    field[i, j] = new Empty(i, j);
            }
        }
        // Генерируем змею
        Snake.GenSnake(field);
        // Генерируем еду
        Food.GenNewFood(field);
    }

    private static void Rendering()
    {
        while (isGame)
        {
            lock(locker)
            {
                PrintField();
                NewGameState();
                // Фактически то, на сколько замирает поток и определяет скорость игры
                Thread.Sleep(gameSpeed);
            }
        }
    }

    private static void PrintField()
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (field[i, j] is SnakeBody)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (field[i, j] is Food)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (field[i, j] is Wall)
                    Console.ForegroundColor = ConsoleColor.Blue;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(field[i, j]);
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static void PlayerControl()
    {
        while (isGame)
        {
            lock(locker)
            {
                ConsoleKeyInfo input = default(ConsoleKeyInfo);
                while (Console.KeyAvailable)
                    input = Console.ReadKey(true);
                Snake.SetDirection(input);
            }
        }
    }

    private static void NewGameState()
    {
        Snake.Step();
        for (int i = Snake.Body.Count - 1; i >= 0; i--)
        {
            Cell cell = field[Snake.Body[i].Y, Snake.Body[i].X];
            Cell? newCell = cell.CollisionResolution(Snake.Body[i]);
            if (newCell is SnakeBody)
            {
                field[Snake.Body[i].Y, Snake.Body[i].X] = newCell;
            }
            else if (newCell is null)
            {
                isGame = false;
                return;
            }
        }
        field[Snake.Tail.Y, Snake.Tail.X] = new Empty(Snake.Tail.Y, Snake.Tail.X);
        if (!Food.IsPresent)
            Food.GenNewFood(field);

    }
}