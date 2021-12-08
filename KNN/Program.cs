using System;

namespace KNN
{
    class Program
    {
        public static int[] size = new int[2];
        public static string axisXnamne { get; set; }
        public static string axisYname { get; set; }
        public static int neighbours { get; set; }
        public static int[,] cell { get; set; }
        public static int[] nearestBox = new int[2];
        static void Main(string[] args)
        {
            while (true)
            {
                char choice = ' ';
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
                (size, axisXnamne, axisYname, choice) = Setup();

                if (choice == '1')
                {
                    cell = new int[size[0], size[1]];
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
                else
                {
                    BigData ML = new BigData();
                    ML.Run();
                }


            }

        }

        static void setCell()
        {
            for (int x = 0; x < size[0]; x++)
            {
                for (int y = 0; y < size[1]; y++)
                {
                    cell[x, y] = 0;
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
                System.Console.WriteLine(i + 1);
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
        static (int[], string, string, char) Setup()
        {
            Console.WriteLine("Welcome to k-nearest neighbors algorithm");
            Console.WriteLine("Train algorithm before use");
            Console.WriteLine();
            System.Console.WriteLine("Press 1 to train algorithm manually (visualized 2D graph)");
            System.Console.WriteLine("Else press any key to train algorithm with a predefined dataset");
            char choice = Console.ReadKey().KeyChar;
            Console.Clear();
            if (choice == '1')
            {
                Console.WriteLine("Name axis X:");
                string axisXnamne = Console.ReadLine();
                Console.WriteLine("Name axis Y:");
                string axisYname = Console.ReadLine();
                Console.Clear();
                System.Console.WriteLine("Select graph size (x,y):");
                string[] sizeS = new string[2];
                sizeS = Console.ReadLine().Split(',');
                return (size = Array.ConvertAll(sizeS, int.Parse), axisXnamne, axisYname, choice);
            }
            else
            {
                return (size = new int[2] { 10, 10 }, "X", "Y", choice);
            }

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

        }
        static void PlaceNeighbour(int[][] data, int count, string[] input)
        {
            Console.SetCursorPosition(data[0][count], data[1][count]);
            try
            {
                if (input[2] == "v")
                {
                    cell[data[0][count], data[1][count]]++;
                    Console.Write("v");
                }
                else
                {
                    cell[data[0][count], data[1][count]]--;
                    Console.Write("n");
                }
            }
            catch (System.Exception)
            {
                Console.Write("n");
            }

        }

        static void StartTesting()
        {
            int[] testpoint = new int[2];

            int valid = 0;

            Console.SetCursorPosition(size[0] + 4, 0);
            Console.Write("Training complete!");
            Console.SetCursorPosition(size[0] + 4, 1);
            Console.Write("Enter point to test(x,y):");
            testpoint = Array.ConvertAll(Console.ReadLine().Split(','), int.Parse);

            int L = testpoint[1] + nearestBox[1];
            if (L > size[1])
            {
                L = size[1];
            }
            int R = testpoint[0] - nearestBox[0];
            if (R < 0)
            {
                R = 0;
            }

            for (int y = testpoint[1] - nearestBox[1]; y < testpoint[1] + nearestBox[1]; y++)
            {
                for (int x = testpoint[0] - nearestBox[0]; x < testpoint[0] + nearestBox[0]; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("O");
                    System.Threading.Thread.Sleep(130);
                    valid = valid + cell[x, y];
                }
            }
            Console.SetCursorPosition(size[0], 1);
            Console.Clear();
            if (valid > 0)
            {
                Console.WriteLine("Valid width a strength of {0}", valid);
            }
            else if (valid < 0)
            {
                Console.WriteLine("Invalid with a strength of {0}", valid);
            }
            else
            {
                Console.WriteLine("No or opposite neighbours found");
            }
            Console.ReadKey();
        }

    }
    class BigData
    {
        public string[] memory { get; set; }
        public string[] dataSet { get; set; }
        public long[,,,,] KNN { get; set; }
        public string[][] dataStrings { get; set; }
        public int graphSize = 20;
        public long valid = 0;
        public int testSize { get; set; }
        public int dimension = 0;
        public string pathToMemory = @"\\fs1.ad.ssis.nu\21ulwe\Desktop\Vscode repos\KNN\memory.txt";
        public BigData()
        {

        }

        public void Run()
        {
            string select;
            Console.WriteLine("Welcome to advanced automation KNN (0-5D)");
            Console.WriteLine("Paste file path to train data or type [memory] to access stored paths:");
            select = Console.ReadLine();
            if (select == "memory")
            {
                memory = System.IO.File.ReadAllLines(pathToMemory);
                for (int i = 0; i < memory.Length; i++)
                {
                    Console.WriteLine("Press {0} to access {1} ", i, memory[i]);
                }
                dataSet = System.IO.File.ReadAllLines(memory[int.Parse(Console.ReadKey().KeyChar.ToString())]);
                Console.Clear();
            }
            else
            {
                while (true)
                {
                    try
                    {
                        dataSet = System.IO.File.ReadAllLines(select);
                        break;
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("File not found");
                        select = Console.ReadLine();
                    }
                }

                Console.Clear();
                Console.WriteLine("Press y to save path to memory else any key to continue");
                if (Console.ReadKey().KeyChar == 'y')
                {
                    Console.Clear();
                    System.IO.File.WriteAllText(pathToMemory, select);
                    Console.WriteLine("Path saved");
                }
                else { Console.Clear(); }
            }
            Console.WriteLine("Press any key to start training:");
            Console.ReadKey();

            Train();
        }

        public void Train()
        {
            KNN = new long[graphSize, graphSize, graphSize, graphSize, graphSize];
            dataStrings = new string[dataSet.Length][];

            for (int i = 0; i < dataSet.Length; i++)
            {
                dataStrings[i] = dataSet[i].Split(',');
            }
            dimension = dataStrings[0].Length - 1;
            System.Console.WriteLine("Dimension: {0}", dimension);
            for (int i = 0; i < dataSet.Length; i++)
            {
                switch (dimension)
                {
                    case 1:
                        if (dataStrings[i][1] == "v")
                        {
                            KNN[0, 0, 0, 0, int.Parse(dataStrings[i][0])]++;
                        }
                        else
                        {
                            KNN[0, 0, 0, 0, int.Parse(dataStrings[i][0])]--;
                        }
                        break;
                    case 2:
                        if (dataStrings[i][2] == "v")
                        {
                            KNN[0, 0, 0, int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]++;
                        }
                        else
                        {
                            KNN[0, 0, 0, int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]--;
                        }
                        break;
                    case 3:
                        if (dataStrings[i][3] == "v")
                        {
                            KNN[0, 0, int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]++;
                        }
                        else
                        {
                            KNN[0, 0, int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]--;
                        }
                        break;
                    case 4:
                        if (dataStrings[i][4] == "v")
                        {
                            KNN[0, int.Parse(dataStrings[i][3]), int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]++;
                        }
                        else
                        {
                            KNN[0, int.Parse(dataStrings[i][3]), int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]--;
                        }
                        break;
                    case 5:
                        if (dataStrings[i][5] == "v")
                        {
                            KNN[int.Parse(dataStrings[i][4]), int.Parse(dataStrings[i][3]), int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]++;
                        }
                        else
                        {
                            KNN[int.Parse(dataStrings[i][4]), int.Parse(dataStrings[i][3]), int.Parse(dataStrings[i][2]), int.Parse(dataStrings[i][1]), int.Parse(dataStrings[i][0])]--;
                        }
                        break;
                    default:
                        Console.WriteLine("Dimension not supported");
                        break;
                }
            }



            Console.Clear();
            Console.WriteLine("Training complete");
            Console.WriteLine("Press any key to test:");

            Console.Clear();
            Console.WriteLine("Enter a value for each dimension:");
            int[] test = new int[] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < dimension; i++)
            {
                Console.WriteLine("Enter value for dimension {0}", i + 1);
                test[i] = int.Parse(Console.ReadLine());
            }
            System.Console.WriteLine("Choose test size:");
            testSize = int.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Calculating...");
            System.Threading.Thread.Sleep(1500);
            for (int x = test[0] - testSize; x < test[0] + testSize; x++)
            {
                for (int y = test[1] - testSize; y < test[1] + testSize; y++)
                {
                    int Z = 0;
                    if (dimension > 2) { Z = test[2] - testSize; } else { Z = 0; }
                    for (int z = Z; z < test[2] + testSize; z++)
                    {
                        int U = 0;
                        if (dimension > 3) { U = test[3] - testSize; } else { U = 0; }
                        for (int u = U; u < test[3] + testSize; u++)
                        {
                            int V = 0;
                            if (dimension > 4) { V = test[4] - testSize; } else V = 0;
                            for (int v = test[4] - testSize; v < test[4] + testSize; v++)
                            {
                                switch (dimension)
                                {
                                    case 1:
                                        valid = valid + KNN[0, 0, 0, 0, x];
                                        KNN[0, 0, 0, 0, x] = 0;
                                        break;
                                    case 2:
                                        valid = valid + KNN[0, 0, 0, y, x];
                                        KNN[0, 0, 0, y, x] = 0;
                                        break;
                                    case 3:
                                        valid = valid + KNN[0, 0, z, y, x];
                                        KNN[0, 0, z, y, x] = 0;
                                        break;
                                    case 4:
                                        valid = valid + KNN[0, u, z, y, x];
                                        KNN[0, u, z, y, x] = 0;
                                        break;
                                    case 5:
                                        valid = valid + KNN[v, u, z, y, x];
                                        KNN[v, u, z, y, x] = 0;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Test complete!");
            if (valid > 10)
            {
                Console.WriteLine("Highly valid width a score of {0}", valid);
            }
            else if (valid > 5)
            {
                Console.WriteLine("Valid width a score of {0}", valid);
            }
            else if (valid > 0)
            {
                Console.WriteLine("Lowly valid width a score of {0}", valid);
            }
            else
            {
                Console.WriteLine("Invalid width a score of {0}", valid);
            }
            Console.ReadKey();
        }
    }
}
