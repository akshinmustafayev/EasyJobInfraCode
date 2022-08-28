using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class CreateFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FolderName { get; set; }

        public void InvokeAction()
        {
            try
            {
                if (!Directory.Exists(FolderName))
                {
                    Directory.CreateDirectory(FolderName);
                    ExecutionUtils.ExecutionOptionVerbose("Created folder \"" + FolderName + "\"");
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Folder \"" + FolderName + "\" already exists");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
