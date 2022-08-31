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
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FolderName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderName, bool.Parse(ExactVariableCheck));

                // Main Action
                if (!Directory.Exists(FolderName))
                {
                    Directory.CreateDirectory(FolderName);

                    // Verbose
                    ExecutionUtils.ExecutionOptionVerbose("Created folder \"" + FolderName + "\"");
                }
                else
                {
                    // Verbose
                    ExecutionUtils.ExecutionOptionVerbose("Folder \"" + FolderName + "\" already exists");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
