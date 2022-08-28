using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class DeleteFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderName { get; set; }

        public void InvokeAction()
        {
            try
            {
                Directory.Delete(FolderName, true);
                ExecutionUtils.ExecutionOptionVerbose("Deleted folder \"" + FolderName + "\"");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
