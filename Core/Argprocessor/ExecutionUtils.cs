using System;
using System.IO;
using EasyJobInfraCode.Core.Preprocessor;

namespace EasyJobInfraCode.Core.Argprocessor
{
    public static class ExecutionUtils
    {
        public static void ExecutionOptionCleanup()
        {
            try
            {
                if (Directory.Exists(Constants.APP_TEMP_PATH))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Constants.APP_TEMP_PATH);

                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                        ExecutionOptionVerbose("\tDeleting: " + file.Name);
                    }
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        dir.Delete(true);
                        ExecutionOptionVerbose("\tDeleting: " + dir.Name);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static void ExecutionOptionVerbose(string message = "")
        {
            if (EasyJobInfraCode.VerboseEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}
