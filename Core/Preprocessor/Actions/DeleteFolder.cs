using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class DeleteFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FolderName { get; set; }
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FolderName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderName, bool.Parse(ExactVariableCheck));

                // Main Action
                Directory.Delete(FolderName, true);

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Deleted folder \"" + FolderName + "\"");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
