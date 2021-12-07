using System;

namespace KNN
{
    class Program
    {
        public static int[] size = new int[2];
        public static string axisXnamne { get; set; }
        public static string axisYname { get; set; }
        public static int neighbours { get; set; }
        public static string[,] cell { get; set; }
        public static int[] nearestBox = new int[2];
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Clear();
                (size, axisXnamne, axisYname) = Setup();
                cell = new string[size[0], size[1]];
                setCell();
                Console.Clear();
                Console.WriteLine("Enter the number of neighbours you want to use");
                neighbours = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Press any key to start training");
                Console.ReadKey();
                CreateGraph(size, axisXnamne, axisYname);
                nearestBox[0] = size[0] / 5;
                nearestBox[1] = size[1] / 5;
                StartTraining();
                Console.ReadKey();
            }
            
        }

        static void setCell()
        {
            for (int x = 0; x < size[0]; x++)
            {
                for (int y = 0; y < size[1]; y++)
                {
                    cell[x, y] = " ";
                }
            }
        }
        
        static void CreateGraph(int[] size, string axisXname, string axisYname)
        {
            Console.Clear();
            for (int y = 0; y <= size[1] + 1; y++)
            {
                for (int x = 0; x <= size[0] + 1; x++)
                {
                    if (x == 0 || y == 0 || x == size[0] + 1 || y == size[1] + 1)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("X");
                    }
                }
            }

            for (int i = 0; i < size[1]; i++)
            {
                Console.SetCursorPosition(size[0] + 2, i + 1);
                System.Console.WriteLine(size[1] - i);
            }
            Console.SetCursorPosition(1, size[1] + 2);
            Console.WriteLine("1");
            Console.SetCursorPosition(size[0], size[1] + 2);
            Console.WriteLine(size[0]);
            Console.SetCursorPosition(0, size[1] + 3);
            Console.WriteLine(axisXname);
            Console.SetCursorPosition(size[0] + 5, size[1]);
            Console.WriteLine(axisYname);

        }
        static (int[], string, string) Setup()
        {
            Console.WriteLine("Welcome to the KNN program!");
            Console.WriteLine("Name axis X:");
            string axisXnamne = Console.ReadLine();
            Console.WriteLine("Name axis Y:");
            string axisYname = Console.ReadLine();
            Console.Clear();
            System.Console.WriteLine("Select graph size (x,y):");
            string[] sizeS = new string[2];
            sizeS = Console.ReadLine().Split(',');
            return (size = Array.ConvertAll(sizeS, int.Parse), axisXnamne, axisYname);
        }
        static void StartTraining()
        {
            //data 0 = x, data 1 = y
            int[][] data = new int[][]
            {
                new int[neighbours],
                new int[neighbours]
            };

            for (int i = 0; i < neighbours; i++)
            {
                string[] input = new string[2];
                int y = 0;
                Console.SetCursorPosition(size[0] + 6, y);
                System.Console.WriteLine("Write {0}, {1} for point {2} and if neighbour is valid(v) or not(n). (x,y,v/n):", axisXnamne, axisYname, i);
                y++;
                Console.SetCursorPosition(size[0] + 6, y);
                input = Console.ReadLine().Split(',');
                try
                {
                    data[0][i] = Convert.ToInt32(input[0]);
                    data[1][i] = Convert.ToInt32(input[1]);
                }
                catch (System.Exception)
                {
                    System.Console.Write("Syntax error");
                    System.Threading.Thread.Sleep(1000);
                }

                PlaceNeighbour(data, i, input);
                for (int j = 6; j < 15; j++)
                {
                    Console.SetCursorPosition(size[0] + j, y);
                    Console.Write(" ");
                }
            }

            for (int j = 6; j < 100; j++)
            {
                Console.SetCursorPosition(size[0] + j, 0);
                Console.Write(" ");
                Console.SetCursorPosition(size[0] + j, 1);
                Console.Write(" ");
            }
            while(true)
            {
                char C = StartTesting();
                if(C == 'q')
                {
                    break;
                }
            }
            

        }
        static void PlaceNeighbour(int[][] data, int count, string[] input)
        {
            Console.SetCursorPosition(data[0][count], data[1][count]);
            try
            {
                if (input[2] == "v")
                {
                    cell[data[0][count], data[1][count]] = "v";
                    Console.Write("v");
                }
                else
                {
                    cell[data[0][count], data[1][count]] = "n";
                    Console.Write("n");
                }
            }
            catch (System.Exception)
            {
                Console.Write("n");
            }

        }

        static char StartTesting()
        {
            int[] testpoint = new int[2];

            int valid = 0;

            Console.SetCursorPosition(size[0]+3, 0);
            Console.Write("Training complete!");
            Console.SetCursorPosition(size[0]+3, 1);
            Console.Write("Enter point to test(x,y):");
            testpoint = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);

            int L = testpoint[1] + nearestBox[1];
            if(L > size[1])
            {
                L = size[1];
            }
            int R = testpoint[0] - nearestBox[0];
            if(R < 0)
            {
                R = 0;
            }
            
            for (int y = testpoint[1]-nearestBox[1]; y < L; y++)
            {                    
                for (int x = testpoint[0]-nearestBox[0]; x < R; x++)
                {
                    if (cell[x, y] == "v")
                    {
                        valid++;
                    }
                    else if (cell[x, y] == "n")
                    {
                        valid--;
                    }
                }
            }
            Console.SetCursorPosition(size[0], 1);
            Console.Clear();
            if(valid > 0)
            {
                Console.WriteLine("Valid");
            }
            else
            {
                Console.WriteLine("Not valid");
            }
            System.Console.WriteLine("Press any key to continue or r to restart");
            char C = Console.ReadKey().KeyChar;
            return C;
        }

    }
}
