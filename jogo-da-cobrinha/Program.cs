using System.Security.Cryptography;

internal class Program
{
    private static Thread trd;
    public static string[,] grade = {   {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#"}
    };

    private static List<Coordinate> coordinates = new List<Coordinate> ();
    private static ConsoleKey pathSnakeAuto = ConsoleKey.UpArrow;
    private static int points = 0;
    private static int bestPoints = 0;

    private static void Main(string[] args)
    {
        StartGame();
        trd = new Thread(ThreadTask);
        trd.Start();

        while (true)
        {
            DrawGrid();
            pathSnakeAuto = Console.ReadKey().Key;
            Thread.Sleep(1);
        }
    }

    private static void ThreadTask()
    {
        while (true)
        {
            MoveSnake();
            Thread.Sleep(800);
        }        
    }

    private static void MoveSnake()
    {
        switch (pathSnakeAuto)
        {
            case ConsoleKey.LeftArrow: //Left
                MoveHead(1, 0, coordinates[0].Collum, 0);
                break;
            case ConsoleKey.DownArrow: //Down
                MoveHead(0, -1, coordinates[0].Row, 7);
                break;
            case ConsoleKey.RightArrow: //Right
                MoveHead(-1, 0, coordinates[0].Collum, 7);
                break;
            case ConsoleKey.UpArrow: //Up
                MoveHead(0, 1, coordinates[0].Row, 0);
                break;
            case ConsoleKey.Escape:
                Environment.Exit(0);
                return;
        }
    }

    public static void MoveHead(int factorC, int factorL, int collumOrLine, int limit)
    {
        if(collumOrLine != limit)
        {
            if (grade[coordinates[0].Row - (1 * factorL), coordinates[0].Collum - (1 * factorC)] == "#")
            {
                grade[coordinates[0].Row - (1 * factorL), coordinates[0].Collum - (1 * factorC)] = "O";
                
                if (coordinates.Count > 0)
                {
                    for (int i = coordinates.Count-1; i>0; i--)
                    {
                        coordinates[i].Collum = coordinates[i - 1].Collum;
                        coordinates[i].Row = coordinates[i - 1].Row;
                    }
                    coordinates[0].Collum -= factorC;
                    coordinates[0].Row -= factorL;
                    DrawSnake();
                }
                else
                {
                    grade[coordinates[0].Row, coordinates[0].Collum] = "#";
                    coordinates[0].Collum -= factorC;
                    coordinates[0].Row -= factorL;
                }
                

            }
            else if(grade[coordinates[0].Row - (1 * factorL), coordinates[0].Collum - (1 * factorC)] == "X")
            {
                Coordinate coordinate = new Coordinate();
                coordinate.Row = coordinates[0].Row - (1 * factorL);
                coordinate.Collum = coordinates[0].Collum - (1 * factorC);
                coordinates.Insert(0,coordinate);
                points++;

                DrawSnake();

                RandomFruit();
            }
            else
            {
                if(bestPoints<points)
                {
                    bestPoints = points;
                }
                StartGame();
            }
        }        
    }

    public static void DrawSnake()
    {
        ClearSneak();

        foreach (var coordinate in coordinates)
        {
            grade[coordinate.Row, coordinate.Collum] = "O";
        }

        DrawGrid();
    }

    public static void ClearSneak()
    {
        Console.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(grade[i, j] != "X")
                {
                    grade[i, j] = "#";
                }                
            }
        }
    }

    public static void ClearGrid()
    {
        Console.Clear();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                grade[i, j] = "#";
            }
        }
    }

    public static void StartGame()
    {
        ClearGrid();
        coordinates.Clear();
        Coordinate coordinate = new Coordinate();
        coordinate.Collum = new Random().Next(0, 7);
        coordinate.Row = new Random().Next(0, 7);
        coordinates.Add(coordinate);
        grade[coordinate.Row, coordinate.Collum] = "O";
        points = 0;
        DrawSnake();
        RandomFruit();
    }

    public static void RandomFruit()
    {
        bool isFruit = true;
        while (isFruit)
        {
            int collum = new Random().Next(0, 7);
            int line = new Random().Next(0, 7);
            if (grade[line, collum] != "O")
            {
                grade[line, collum] = "X";
                isFruit = false;
            }
        }        
    }

    public static void DrawGrid()
    {
        Console.Clear();
        Console.WriteLine(" =====SNAKE=====\n");
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Console.Write(" "+grade[i, j]);
            }
            Console.Write("\n");
        }

        Console.WriteLine($"\n Points: {points}");
        Console.WriteLine($"\n   Best: {bestPoints}");
    }
}