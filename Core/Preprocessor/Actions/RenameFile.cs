using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class RenameFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string Overwrite { get; set; } = "false";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FileName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileName, bool.Parse(ExactVariableCheck));
                NewFileName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(NewFileName, bool.Parse(ExactVariableCheck));
                Overwrite = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Overwrite, bool.Parse(ExactVariableCheck));

                // Main Action
                File.Move(FileName, NewFileName, bool.Parse(Overwrite));

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Renamed file from \"" + FileName + "\" to \"" + NewFileName + "\" while overwrite equals \"" + Overwrite + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
