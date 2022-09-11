using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class DeleteFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FileName { get; set; }
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FileName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileName, bool.Parse(ExactVariableCheck)).GetTextValue();

                // Main Action
                File.Delete(FileName);

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Deleted file \"" + FileName + "\"");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
