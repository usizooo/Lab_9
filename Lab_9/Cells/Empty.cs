using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class Empty : Cell
{
    public Empty(int y, int x) : base(y, x) { }

    public override Cell? CollisionResolution(Cell cell)
    {
        if (cell is SnakeBody)
            return cell;
        return this;
    }

    public override string ToString()
    {
        return " ";
    }
}
