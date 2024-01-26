using System.Security.Cryptography;

internal class Program
{
    private static Thread trd;
    public static string[,] grade = {   {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"},
                                        {"#","#","#","#","#","#","#","#","#","#","#","#","#","#","#","#"}
    };

    private static List<Coordinate> coordinates = new List<Coordinate> ();
    private static string pathSnakeAuto = "8";

    private static void Main(string[] args)
    {
        StartGame();
        trd = new Thread(ThreadTask);
        trd.Start();

        while (true)
        {
            DrawGrid();
            var ch = Console.ReadKey();
            switch (ch.Key)
            {
                case ConsoleKey.LeftArrow:
                    pathSnakeAuto = "4";
                    break;
                case ConsoleKey.RightArrow:
                    pathSnakeAuto = "6";
                    break;
                case ConsoleKey.UpArrow:
                    pathSnakeAuto = "8";
                    break;
                case ConsoleKey.DownArrow:
                    pathSnakeAuto = "5";
                    break;
            }
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
            case "4": //Left
                MoveHead(1, 0, coordinates[0].Collum, 0, 4);
                break;
            case "5": //Down
                MoveHead(0, -1, coordinates[0].Row, 7, 5);
                break;
            case "6": //Right
                MoveHead(-1, 0, coordinates[0].Collum, 15, 6);
                break;
            case "8": //Up
                MoveHead(0, 1, coordinates[0].Row, 0, 8);
                break;
            case "0":
                return;
        }
    }

    public static void MoveHead(int factorC, int factorL, int collumOrLine, int limit, int path)
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

                DrawSnake();

                RandomFruit();
            }
            else
            {
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
            for (int j = 0; j < 16; j++)
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
            for (int j = 0; j < 16; j++)
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
        coordinate.Collum = new Random().Next(0, 15);
        coordinate.Row = new Random().Next(0, 7);
        coordinates.Add(coordinate);
        grade[coordinate.Row, coordinate.Collum] = "O";
        DrawSnake();
        RandomFruit();
    }

    public static void RandomFruit()
    {
        bool isFruit = true;
        while (isFruit)
        {
            int collum = new Random().Next(0, 15);
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
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                Console.Write(grade[i, j]);
            }
            Console.Write("\n");
        }
    }
}