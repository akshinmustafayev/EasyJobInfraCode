using System;
using System.Collections.Generic;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class SetVariableValue : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public List<object> Variables { get; set; } = new List<object> { };
        public string Value { get; set; } = "";
        public string ExactVariableCheckFromValue { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                if (Variables.Count > 0)
                {
                    Value = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Value, bool.Parse(ExactVariableCheckFromValue));

                    foreach (object setVar in Variables)
                    {
                        EasyJobInfraCode.VariableProcessorInstance.SetVariableValue(setVar.ToString(), Value);
                        if (setVar.ToString().StartsWith("$"))
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Value \"" + Value + "\" was set to \"" + setVar.ToString() + "\" variable.");
                        }
                        else
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Value \"" + Value + "\" was NOT set to \"" + setVar.ToString() + "\" variable.");
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
