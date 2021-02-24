
using System;
using System.Runtime.InteropServices;

[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Vector2
{
    public float x,y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2(double x, double y)
    {
        this.x = (float)x;
        this.y = (float)y;
    }

    public bool IsZero()
    {
        if (this.x == 0 && this.y == 0) return true;
        else return false;
    }
    public bool IsGarbage()
    {
        if (this.x == float.MaxValue && this.y == float.MaxValue) return true;
        if (this.x == 0 && this.y == 0) return true;
        else return false;
    }

    public static float DistanceSquared(Vector2 pos1, Vector2 pos2)
    {
        return ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y));
    }

    public static float Distance(Vector2 pos1, Vector2 pos2)
    {
        return (float) Math.Sqrt(DistanceSquared(pos1, pos2));
    }


    public static Vector2[] pointsInBetween(Vector2 p1, Vector2 p2, int quantity)
    {
        var points = new Vector2[quantity];
        float ydiff = p2.y - p1.y, xdiff = p2.x - p1.x;
        double slope = (double)(p2.y - p1.y) / (p2.x - p1.x);
        double x, y;

        --quantity;

        for (double i = 0; i < quantity; i++)
        {
            y = slope == 0 ? 0 : ydiff * (i / quantity);
            x = slope == 0 ? xdiff * (i / quantity) : y / slope;
            points[(int)i] = new Vector2((x) + p1.x, (y) + p1.y);
        }

        points[quantity] = p2;
        return points;
    }

    public static Vector2 Zero
    {
        get
        {
            return new Vector2(0, 0);
        }
    }

    public static Vector2 operator +(Vector2 f1, Vector2 f2)
    {
        Vector2 result = new Vector2(f1.x + f2.x, f1.y + f2.y);
        return result;
    }

    public static Vector2 operator /(Vector2 f1, int num)
    {
        Vector2 result = new Vector2(f1.x / num, f1.y / num);
        return result;
    }

    public static Vector2 operator /(Vector2 f1, float num)
    {
        Vector2 result = new Vector2(f1.x / num, f1.y / num);
        return result;
    }

    public static bool operator ==(Vector2 f1, Vector2 f2)
    {
        return f1.x == f2.x && f1.y == f2.y;
    }

    public static bool operator !=(Vector2 f1, Vector2 f2)
    {
        return f1.x != f2.x || f1.y != f2.y;
    }

    public bool isVisible(Vector2 p2, float lightRadius)
    {
        double lightDistance = 2.009;
        double percentage = lightRadius / 5;
        return (Distance(this, p2) < (lightDistance * percentage));
    }

}