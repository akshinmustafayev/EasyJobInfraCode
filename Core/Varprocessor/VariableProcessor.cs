using System;
using System.Collections.Generic;
using System.Linq;
using EasyJobInfraCode.Core.Argprocessor;

namespace EasyJobInfraCode.Core.Varprocessor
{
    public class VariableProcessor
    {
        public Dictionary<string, string> Variables { get; set; }

        public VariableProcessor()
        {
            Variables = new Dictionary<string, string> { };
        }

        public string GetVariableValue(string variableName)
        {
            string variableValue = "";

            if (Variables.ContainsKey(variableName))
            {
                variableValue = Variables[variableName];
            }

            return variableValue;
        }

        public void InitVariable(string variableName)
        {
            if (Variables.ContainsKey(variableName))
            {
                Console.WriteLine("Specified variable already exists. Please choose another name.");
                Environment.Exit(0);
                return;
            }
            else
            {
                if (variableName.StartsWith("$"))
                {
                    Variables.Add(variableName, "");
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Specified value is not a variable. Variable must start with $ character. Example: $someVar1");
                }
            }
        }

        public void InitVariables(List<object> variables)
        {
            if (variables.Count == 0)
                return;

            foreach (object variable in variables)
            {
                InitVariable(variable.ToString());
            }
        }

        public void SetVariableValue(string variableName, string variableValue)
        {
            if (Variables.ContainsKey(variableName))
            {
                Variables[variableName] = variableValue;
            }
        }

        public string SetValuesFromVariables(string text, bool exactVariableCheck)
        {
            foreach (KeyValuePair<string, string> variable in Variables)
            {
                if (exactVariableCheck)
                {
                    if (text.Split().Contains(variable.Key))
                    {
                        text = text.Replace(variable.Key, variable.Value);
                    }
                }
                else
                {
                    if (text.Contains(variable.Key))
                    {
                        text = text.Replace(variable.Key, variable.Value);
                    }
                }
            }

            return text;
        }
    }
}
