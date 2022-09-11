using System;
using System.Collections.Generic;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class SetVariableValue : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Variable { get; set; } = "";
        public string Value { get; set; } = "";
        public string ExactVariableCheckFromVariable { get; set; } = "false";
        public string ExactVariableCheckFromValue { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Main Action
                Value = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Value, bool.Parse(ExactVariableCheckFromValue)).GetTextValue();
                EasyJobInfraCode.VariableProcessorInstance.SetValuesToVariables(Variable, Value, bool.Parse(ExactVariableCheckFromVariable));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
