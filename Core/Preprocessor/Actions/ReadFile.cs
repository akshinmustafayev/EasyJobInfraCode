using System;
using System.Collections.Generic;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class ReadFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FileName { get; set; }
        public string Out { get; set; } = "";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                FileName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileName, bool.Parse(ExactVariableCheck));
                
                // Main Action
                if (!File.Exists(FileName))
                {
                    Console.WriteLine("File \"" + FileName + "\" does not exist.");
                    return;
                }

                string data = File.ReadAllText(FileName);

                ExecutionUtils.ExecutionOptionVerbose("File \"" + FileName + "\" was read to memory.");
                EasyJobInfraCode.VariableProcessorInstance.SetValuesToVariables(Out, data.ToString(), bool.Parse(ExactVariableCheck));
                Out = data;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
