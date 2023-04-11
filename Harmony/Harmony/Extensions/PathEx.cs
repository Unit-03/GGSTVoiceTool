using System;
using System.IO;

namespace Harmony
{
    public static class PathEx
    {
        public static bool Equivalent(string path1, string path2)
        {
            return string.Equals(Path.GetFullPath(path1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                 Path.GetFullPath(path2).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                 StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
