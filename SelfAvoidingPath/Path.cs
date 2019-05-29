using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SelfAvoidingPath
{
    class Path
    {
        private static char[] Directions = { 'N', 'E', 'S', 'W' };

        public Point currentPosition;
        public string walkDirections;
        public List<Point> visitedPoints;
        public int blockedMoveDirection;
        Random randomGenerator;

        public Path(int n)
        {
            currentPosition = new Point(0, 0);
            visitedPoints = new List<Point>(n + 1) { new Point(0, 0) };
            walkDirections = "";
            randomGenerator = new Random((int)DateTime.Now.Ticks % int.MaxValue);
        }

        public Path(string walkString)
        {
            int n = walkString.Length;
            currentPosition = new Point(0, 0);
            visitedPoints = new List<Point>(n + 1) { new Point(0, 0) };
            walkDirections = "";
            randomGenerator = new Random((int)DateTime.Now.Ticks % int.MaxValue);
            foreach (char c in walkString)
            {
                MakeMove(c);
            }
        }

        public bool QuickCheckIfPathIsSelfAvoiding()
        {
            if(visitedPoints.Count != visitedPoints.Distinct().Count())
            {
                return false;
            }

            var nCount = walkDirections.Count(c => c == 'N');
            var eCount = walkDirections.Count(c => c == 'E');
            var sCount = walkDirections.Count(c => c == 'S');
            var wCount = walkDirections.Count(c => c == 'W');
            if (nCount == eCount && eCount == sCount && sCount == wCount)
            {
                return false;
            }
            if (walkDirections.Contains("NESW") ||
            walkDirections.Contains("NWSE") ||
            walkDirections.Contains("ENWS") ||
            walkDirections.Contains("ESWN") ||
            walkDirections.Contains("SENW") ||
            walkDirections.Contains("SWNE") ||
            walkDirections.Contains("WNES") ||
            walkDirections.Contains("WSEN"))
            {
                return false;
            }
            return true;
        }

        public void MakeMove(char direction)
        {
            //if (visitedPoints.Contains(currentPosition))
            //{
            //    throw new Exception("Punkt był już wcześniej odwiedzony.");
            //}
            //if (visitedPoints.Contains(new Point(currentPosition.x, currentPosition.y)) &&
            //visitedPoints.Contains(new Point(currentPosition.x, currentPosition.y + 1)) &&
            //visitedPoints.Contains(new Point(currentPosition.x + 1, currentPosition.y)) &&
            //visitedPoints.Contains(new Point(currentPosition.x+1, currentPosition.y + 1)))
            //{
            //    throw new Exception("Ślepy zaułek");
            //}
            switch (direction)
            {
                case 'N':// 0
                    currentPosition.Y++;
                    blockedMoveDirection = 2;// 'S';
                    break;
                case 'E':// 1
                    currentPosition.X++;
                    blockedMoveDirection = 3;// 'W';
                    break;
                case 'S':// 2
                    currentPosition.Y--;
                    blockedMoveDirection = 0;// 'N';
                    break;
                case 'W':// 3
                    currentPosition.X--;
                    blockedMoveDirection = 1;// 'E';
                    break;
            }

            walkDirections += direction;
            visitedPoints.Add(new Point(currentPosition.X, currentPosition.Y));
        }

        public void MakeNMoves(int n)
        {
            for (int i = 0; i < n; i++)
            {
                int intDirection;
                do
                {
                    intDirection = randomGenerator.Next(0, 4);
                } while (intDirection == blockedMoveDirection);


                switch (intDirection)
                {
                    case 0:
                        MakeMove('N');
                        break;
                    case 1:
                        MakeMove('E');
                        break;
                    case 2:
                        MakeMove('S');
                        break;
                    case 3:
                        MakeMove('W');
                        break;
                }
            }
        }
    }
}
