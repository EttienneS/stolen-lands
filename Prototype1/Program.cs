using System;
using System.Threading;
using World;

namespace Prototype1
{
    class Program
    {
        public static int WorldWidth = 200;
        public static int WorldHeight = 200;
        public static WorldService World = new WorldService(WorldWidth, WorldHeight);

        public static int ViewX;
        public static int ViewY;

        public static bool Debug;

        static void Main(string[] args)
        {
            Console.Clear();

            World.GenerateWorld(70, 110);

            var worldThread = new Thread(Update);
            var uiThread = new Thread(Draw);

            var inputThread = new Thread(Input);

            worldThread.Start();
            uiThread.Start();
            inputThread.Start();

            while (true)
            {
                Thread.Sleep(5);
            }
        }

        static void Input()
        {
            ScreenBuffer.ResetCursor();

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (ViewX + ScreenBuffer.ScreenWidth < WorldWidth)
                        {
                            ViewX++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:

                        if (ViewX > 0)
                        {
                            ViewX--;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (ViewY > 0)
                        {
                            ViewY--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (ViewY + ScreenBuffer.ScreenHeight < WorldHeight)
                        {
                            ViewY++;
                        }
                        break;
                    case ConsoleKey.D:
                        Debug = !Debug;
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        ViewX = 0;
                        ViewY = 0;
                        break;
                }

                ScreenBuffer.ResetCursor();
            }
        }

        static void Update()
        {
            //World.GenerateWorld(5, 15);
            World.Update();

            Thread.Sleep(10);
        }

        static void Draw()
        {
            var blank = ' ';

            while (true)
            {
                var viewBox = new Box(new Coordinate(ViewX, ViewY), new Coordinate(ViewX + ScreenBuffer.ScreenWidth, ViewY + ScreenBuffer.ScreenHeight));

                for (int x = 0; x < ScreenBuffer.ScreenWidth; x++)
                {
                    for (int y = 0; y < ScreenBuffer.ScreenHeight; y++)
                    {
                        ScreenBuffer.Draw(blank, new Coordinate(x, y));
                    }
                }

                foreach (var location in World.Locations)
                {
                    if (viewBox.Contains(location.Coordinate))
                    {
                        ScreenBuffer.Draw(location.Token, location.Coordinate.Adjust(-ViewX, -ViewY));
                    }

                }

                if (Debug)
                {
                    DrawString($"ViewX: {ViewX}", new Coordinate(0, 0));
                    DrawString($"ViewY: {ViewY}", new Coordinate(0, 1));
                }

                ScreenBuffer.DrawScreen();
                Thread.Sleep(100);

                //if (blank == '.')
                //{
                //    blank = ',';
                //}
                //else
                //{
                //    blank = '.';
                //}

            }
        }

        private static void DrawString(string text, Coordinate coord)
        {
            var shifter = 0;
            foreach (var c in text)
            {
                ScreenBuffer.Draw(c, coord.Adjust(shifter, 0));
                shifter++;
            }
        }
    }


}
