using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class Food : Cell
{

    public static bool IsPresent { get; private set; } = false;

    public Food(int y, int x) : base(y, x) { }

    public static void GenNewFood(Cell[,] field)
    {
        int _y = -1, _x = -1;
        bool isSuccessGeneration = false;
        while (!isSuccessGeneration)
        {
            _y = new Random().Next(field.GetLength(0));
            _x = new Random().Next(field.GetLength(1));
            if (field[_y, _x] is Empty)
                isSuccessGeneration = true;
        }
        field[_y, _x] = new Food(_y, _x);
        IsPresent = true;
    }

    public override Cell? CollisionResolution(Cell cell)
    {
        if (cell is SnakeBody)
        {
            Snake.AddDeltaTail(new SnakeBody(this.Y, this.X));
            IsPresent = false;
            return cell;
        }
        return this;
    }

    public override string ToString()
    {
        return "@";
    }
}