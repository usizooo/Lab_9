using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Cell
{
    public int Y { get; protected set; }
    public int X { get; protected set; }
    public Cell(int y, int x)
    {
        Y = y;
        X = x;
    }

    public abstract Cell? CollisionResolution(Cell cell);
}