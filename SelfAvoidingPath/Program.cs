using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SelfAvoidingPath
{
    class Program
    {
        public static int n;
        public static int maxPathsToFound = 10000;
        static void Main(string[] args)
        {
            string consoleInput = "";
            while (true)
            {
                Console.Write("\n\nDługość szukanych ścieżek: ");
                consoleInput = Console.ReadLine();
                if (consoleInput.ToUpper() == "EXIT")
                {
                    break;
                }
                else if (int.TryParse(consoleInput, out n) && n > 0)
                {
                    long allPathsCount = 4 * (long)Math.Pow(3, n - 1);
                    long sapCount = 0;
                    double sapFoundRatio;
                    bool shouldApproximate;

                    if (n < 4)
                    {
                        sapCount = allPathsCount;
                        sapFoundRatio = 1;
                        shouldApproximate = false;
                    }
                    else
                    {
                        List<Path> foundPaths = new List<Path>();
                        shouldApproximate = maxPathsToFound < allPathsCount / 4;
                        long localPathsToFound = shouldApproximate
                            ? maxPathsToFound
                            : allPathsCount / 4;

                        while (foundPaths.Count < localPathsToFound)
                        {
                            bool pathSuccesfullyCreated = false;
                            Path randomPath = new Path(n);
                            while (!pathSuccesfullyCreated)
                            {
                                //randomPath.MakeFakeMove('N');
                                //randomPath.MakeNFakeMoves(n - 1);
                                randomPath.MakeMove('N');
                                randomPath.MakeNMoves(n - 1);
                                if (!foundPaths.Contains(randomPath, Comparers.PathComparer))
                                {
                                    pathSuccesfullyCreated = true;
                                    foundPaths.Add(randomPath);
                                }
                                else
                                {
                                    randomPath.walkDirections = "";
                                    randomPath.visitedPoints = new List<Point>(n + 1) { new Point(0, 0) };
                                    randomPath.currentPosition = new Point(0, 0);
                                }
                            }
                        }

                        //Parallel.ForEach(foundPaths, (randomlyGeneratedPath) =>
                        //{
                        //    if (randomlyGeneratedPath.CheckIfPathIsSelfAvoiding())
                        //    {
                        //        Interlocked.Increment(ref sapCount);
                        //    }
                        //});

                        Parallel.ForEach(foundPaths, (randomlyGeneratedPath) =>
                        {
                            if (randomlyGeneratedPath.QuickCheckIfPathIsSelfAvoiding())
                            {
                                Interlocked.Increment(ref sapCount);
                            }
                        });

                        if (shouldApproximate)
                        {
                            sapFoundRatio = (sapCount / (double)foundPaths.Count);
                            sapCount = (long)(allPathsCount * sapFoundRatio);
                        }
                        else
                        {
                            sapCount *= 4;
                            sapFoundRatio = (sapCount / (double)allPathsCount);
                        }
                    }

                    Console.WriteLine($"Wszystkich ścieżek:\t\t\t{allPathsCount}");
                    Console.WriteLine($"w tym samounikających się{(shouldApproximate ? " około:" : ":\t")}\t{sapCount}");
                    Console.WriteLine($"SAP ratio:\t\t\t\t{sapFoundRatio}");
                }
                else
                {
                    Console.WriteLine("Podana długość jest nieprawidłowa.");
                }
            }
            Console.ReadKey();
        }
    }
}