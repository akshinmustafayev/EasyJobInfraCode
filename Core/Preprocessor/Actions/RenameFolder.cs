using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class RenameFolder : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FolderName { get; set; }
        public string NewFolderName { get; set; }
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FolderName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderName, bool.Parse(ExactVariableCheck)).GetTextValue();
                NewFolderName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(NewFolderName, bool.Parse(ExactVariableCheck)).GetTextValue();

                // Main Action
                Directory.Move(FolderName, NewFolderName);

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Renamed folder from \"" + FolderName + "\" to \"" + NewFolderName + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
