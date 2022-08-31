using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class CopyFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderSource { get; set; }
        public string FolderDestination { get; set; }
        public string Overwrite { get; set; } = "false";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FolderSource = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderSource, bool.Parse(ExactVariableCheck));
                FolderDestination = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderDestination, bool.Parse(ExactVariableCheck));
                Overwrite = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Overwrite, bool.Parse(ExactVariableCheck));

                // Main Action
                CopyDirectory(FolderSource, FolderDestination, bool.Parse(Overwrite));

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Copied folder from \"" + FolderSource + "\" to \"" + FolderDestination + "\" while overwrite equals \"" + Overwrite + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void CopyDirectory(string sourceDir, string destinationDir, bool overwrite)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            {
                Console.WriteLine($"Source directory not found \"{dir.FullName}\"");
                return;
            }

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
                ExecutionUtils.ExecutionOptionVerbose("Destionation folder \"" + destinationDir + "\" does not exist. Created new folder.");
            }

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, overwrite);
                ExecutionUtils.ExecutionOptionVerbose("Copying file from \"" + file.FullName + "\" to \"" + targetFilePath + "\" while overwrite equals \"" + overwrite + "\"");
            }

            // Copy subdirectories
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, overwrite);
                ExecutionUtils.ExecutionOptionVerbose("Copying directory from \"" + subDir.FullName + "\" to \"" + newDestinationDir + "\" while overwrite equals \"" + overwrite + "\"");
            }
        }
    }
}
