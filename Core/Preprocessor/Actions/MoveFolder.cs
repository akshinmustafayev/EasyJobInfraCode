using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class MoveFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderSource { get; set; }
        public string FolderDestination { get; set; }

        public void InvokeAction()
        {
            try
            {
                Directory.Move(FolderSource, FolderDestination);
                ExecutionUtils.ExecutionOptionVerbose("Moved folder from \"" + FolderSource + "\" to \"" + FolderDestination + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
