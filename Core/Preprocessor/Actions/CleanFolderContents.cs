using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class CleanFolderContents : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderName { get; set; }

        public void InvokeAction()
        {
            try
            {
                if (Directory.Exists(FolderName))
                {
                    ExecutionUtils.ExecutionOptionVerbose("Cleaning up folder \"" + FolderName + "\"\n");
                    DirectoryInfo directoryInfo = new DirectoryInfo(FolderName);

                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                        ExecutionUtils.ExecutionOptionVerbose("\tDeleting: " + file.Name);
                    }
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        dir.Delete(true);
                        ExecutionUtils.ExecutionOptionVerbose("\tDeleting: " + dir.Name);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
