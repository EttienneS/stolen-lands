using System;
using World;

public static class ScreenBuffer
{
    // code adapted from this: http://cgp.wikidot.com/consle-screen-buffer
    public static int ScreenWidth = 80;
    public static int ScreenHeight = 25;

    //initiate important variables
    public static char[,] screenBufferArray = new char[ScreenWidth, ScreenHeight]; //main buffer array
    public static string screenBuffer; //buffer as string (used when drawing)
    public static Char[] arr; //temporary array for drawing string

    //this method takes a string, and a pair of coordinates and writes it to the buffer
    public static void Draw(char c, Coordinate coordinate)
    {
        if (coordinate.X < 0 || coordinate.Y < 0 || coordinate.X >= ScreenWidth || coordinate.Y >= ScreenHeight)
            return;

        //split text into array
        screenBufferArray[coordinate.X, coordinate.Y] = c;

    }

    public static void DrawScreen()
    {
        screenBuffer = "";

        //iterate through buffer, adding each value to screenBuffer
        for (int iy = 0; iy < ScreenHeight - 1; iy++)
        {
            for (int ix = 0; ix < ScreenWidth; ix++)
            {
                screenBuffer += screenBufferArray[ix, iy];
            }
            screenBuffer += "\n";
        }
        //set cursor position to top left and draw the string
        Console.SetCursorPosition(0, 0);
        Console.Write(screenBuffer);
        screenBufferArray = new char[ScreenWidth, ScreenHeight];
        //note that the screen is NOT cleared at any point as this will simply overwrite the existing values on screen. Clearing will cause flickering again.

    }

    public static void ResetCursor()
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(ScreenWidth + 5, ScreenHeight+1);
    }

    internal static void Draw(object token, Coordinate coordinate)
    {
        throw new NotImplementedException();
    }
}