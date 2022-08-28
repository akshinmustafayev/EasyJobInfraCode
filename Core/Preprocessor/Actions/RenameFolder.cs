using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class RenameFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderName { get; set; }
        public string NewFolderName { get; set; }

        public void InvokeAction()
        {
            try
            {
                Directory.Move(FolderName, NewFolderName);
                ExecutionUtils.ExecutionOptionVerbose("Renamed folder from \"" + FolderName + "\" to \"" + NewFolderName + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
