using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class Wall : Cell
{
    public Wall(int y, int x) : base(y, x) { }

    public override Cell? CollisionResolution(Cell cell)
    {
        if (cell is SnakeBody)
        {
            Program.PrintGameOver();
            return null;
        }
        return this;
    }

    public override string ToString()
    {
        return "#";
    }
}