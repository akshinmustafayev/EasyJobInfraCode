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
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FolderSource = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderSource, bool.Parse(ExactVariableCheck));
                FolderDestination = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FolderDestination, bool.Parse(ExactVariableCheck));
                
                // Main Action
                Directory.Move(FolderSource, FolderDestination);

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Moved folder from \"" + FolderSource + "\" to \"" + FolderDestination + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
