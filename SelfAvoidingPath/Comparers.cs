using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SelfAvoidingPath
{
    static class Comparers
    {
        public static PathComparer PathComparer = new PathComparer();
    }

    internal class PathComparer : IEqualityComparer<Path>
    {
        public bool Equals(Path a, Path b)
        {
            return a.walkDirections == b.walkDirections;
        }

        public int GetHashCode(Path obj)
        {
            throw new NotImplementedException();
        }
    }
}