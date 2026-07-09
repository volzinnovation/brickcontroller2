using System;
using System.IO;
using Microsoft.Maui.Storage;

namespace BrickController2.Helpers
{
    public static class PathHelper
    {
        public static string AddAppDataPathToFilename(string filename)
        {
            var appDataPath = Environment.GetEnvironmentVariable("BRICKCONTROLLER_APP_DATA_DIR");
            if (string.IsNullOrWhiteSpace(appDataPath))
            {
                try
                {
                    appDataPath = FileSystem.AppDataDirectory;
                }
                catch
                {
                    appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                }
            }

            Directory.CreateDirectory(appDataPath);
            return Path.Combine(appDataPath, filename);
        }
    }
}
