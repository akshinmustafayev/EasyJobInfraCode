using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class MoveFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FileSource { get; set; }
        public string FileDestination { get; set; }
        public string Overwrite { get; set; } = "false";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FileSource = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileSource, bool.Parse(ExactVariableCheck));
                FileDestination = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileDestination, bool.Parse(ExactVariableCheck));
                Overwrite = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Overwrite, bool.Parse(ExactVariableCheck));

                // Main Action
                File.Move(FileSource, FileDestination, bool.Parse(Overwrite));

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Moved file from \"" + FileSource + "\" to \"" + FileDestination + "\" while overwrite equals \"" + Overwrite + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
