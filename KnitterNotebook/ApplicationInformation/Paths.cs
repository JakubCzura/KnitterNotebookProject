﻿using System.IO;

namespace KnitterNotebook.ApplicationInformation
{
    public class Paths
    {
        public static string UsersDirectories => Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, "UsersDirectories");

        public static string UserDirectory(string nickname) => Path.Combine(UsersDirectories, nickname);

        public static string ImageToSavePath(string nickname, string sourceImageName) => Path.Combine(UserDirectory(nickname), sourceImageName);
    }
}