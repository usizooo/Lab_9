using System;

public static class Snake
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public static Direction HeadDirection { get; private set; } = Direction.Up;

    public static List<SnakeBody> Body { get; private set; } = new List<SnakeBody>();

    public static SnakeBody Tail { get; private set; }

    public static Queue<SnakeBody> DeltaTails { get; private set; } = new Queue<SnakeBody>();
    
    public static void AddSnakeBody(SnakeBody snakeBody) => Body.Add(snakeBody);
    public static void AddDeltaTail(SnakeBody deltaTail) => DeltaTails.Enqueue(deltaTail);

    public static void GenSnake(Cell[,] field)
    {
        int _y = field.GetLength(0) / 2;
        int _x = field.GetLength(1) / 2;
        AddSnakeBody(new SnakeBody(_y, _x, true));
        field[_y, _x] = Body[0];
    }

    // Обработка шага змеи
    public static void Step()
    {
        // Сначала добавим новые клетки с телом змеи, если они есть
        SetNewTail();
        // Продублируем хвост змеи
        Tail = new SnakeBody(Body[Body.Count - 1].Y, Body[Body.Count - 1].X);
        // Идём с конца до головы змеи (не включая её)
        for (int i = Body.Count - 1; i > 0; i--)
        {
            Direction bodyDirection;
            // Направление движения очередной клетки тела змеи высчитаем через разницу координат
            // данной клетки и той, что перед неё
            Tuple<int, int> shift = new Tuple<int, int>(Body[i - 1].Y - Body[i].Y, Body[i - 1].X - Body[i].X);
            switch (shift)
            {
                case (1, 0):
                    bodyDirection = Direction.Down;
                    break;
                case (0, 1):
                    bodyDirection = Direction.Right;
                    break;
                case (-1, 0):
                    bodyDirection = Direction.Up;
                    break;
                case (0, -1):
                    bodyDirection = Direction.Left;
                    break;
                default:
                    throw new ArgumentException("Incorrect shift");
            }
            // Сдвинем данную клетку с телом змеи в высчитанном направлении
            Body[i].Step(bodyDirection);
        }
        // Т.к. перед головой нет клетки, то её направляем отдельно
        Body[0].Step(HeadDirection);
    }

    // Метод для добавления новых клеток змеи
    private static void SetNewTail()
    {
        // Если нечего добавлять,
        if (DeltaTails.Count == 0) 
            // то выйдем
            return;
        // Пройдемся по всем клеткам тела змеи
        foreach (var body in Body)
            // Если хоть у одной в данный момент такие же координаты, что и у клетки
            // с некогда съеденной едой (которая должна превратиться в тело змеи),
            if (DeltaTails.First().Y == body.Y && DeltaTails.First().X == body.X)
                // то пока проигнорируем добавление новых клеток
                return;
        // Если дошли до сюда, значит пора добавить новые клетки к телу змеи
        AddSnakeBody(DeltaTails.Dequeue());
    }

    // Метод для установления направления головы змеи.
    // Запрещаем ходить в строго противоположном направлении данному
    public static void SetDirection(ConsoleKeyInfo input)
    {
        switch (input.Key)
        {
            case ConsoleKey.W:
                if (HeadDirection != Direction.Down)
                    HeadDirection = Direction.Up;
                break;
            case ConsoleKey.A:
                if (HeadDirection != Direction.Right)
                    HeadDirection = Direction.Left;
                break;
            case ConsoleKey.S:
                if (HeadDirection != Direction.Up)
                    HeadDirection = Direction.Down;
                break;
            case ConsoleKey.D:
                if (HeadDirection != Direction.Left)
                    HeadDirection = Direction.Right;
                break;
            default:
                break;
        }
    }
}