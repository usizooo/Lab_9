using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public sealed class SnakeBody : Cell
{
    public bool IsHead = false;
    public SnakeBody(int y, int x, bool isHead = false) : base(y, x) 
    {
        IsHead = isHead;
    }


    public void Step(Snake.Direction direction)
    {
        switch (direction)
        {
            case Snake.Direction.Left:
                X--;
                break;
            case Snake.Direction.Right:
                X++;
                break;
            case Snake.Direction.Up:
                Y--;
                break;
            case Snake.Direction.Down:
                Y++;
                break;
        }
    }

    public override Cell? CollisionResolution(Cell cell)
    {
        if (cell is SnakeBody)
        {
            if (((SnakeBody)cell).IsHead)
            {
                Program.PrintGameOver();
                return null;
            }
        }
        return this;
    }

    public override string ToString()
    {
        return "0";
    }
}