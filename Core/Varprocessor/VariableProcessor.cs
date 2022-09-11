using System;
using System.Collections.Generic;
using System.Linq;
using EasyJobInfraCode.Core.Argprocessor;

namespace EasyJobInfraCode.Core.Varprocessor
{
    public class VariableProcessor
    {
        private string _tempText = "";

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
                ExecutionUtils.ExecutionOptionVerbose($"Variable {variableName} already exists. Please choose another name for a new variable.");
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

        public void InitVariables(string variables)
        {
            if (variables.Length == 0)
                return;

            string[] variablesArray = variables.Split(",");

            foreach (string variable in variablesArray)
            {
                string tempVariable = variable.TrimStart(' ').TrimEnd(' ');

                if (tempVariable.Contains(' '))
                {
                    ExecutionUtils.ExecutionOptionVerbose("Variable contains space character. Variable must be specified without whice spaces. Example: $varTest1");
                    continue;
                }

                InitVariable(tempVariable);
            }
        }

        public void SetVariableValue(string variableName, string variableValue)
        {
            if (variableName.StartsWith("$"))
            {
                if (Variables.ContainsKey(variableName))
                {
                    Variables[variableName] = variableValue;
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Specified variable was not found");
                }
            }
            else
            {
                ExecutionUtils.ExecutionOptionVerbose("Specified value is not a variable. Variable must start with $ character. Example: $someVar1");
            }
        }

        public string GetTextValue()
        {
            return _tempText;
        }

        public VariableProcessor SetValuesFromVariables(string text, bool exactVariableCheck)
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

            _tempText = text;

            return this;
        }

        public VariableProcessor SetValuesToVariables(string text, string value, bool exactVariableCheck)
        {
            foreach (KeyValuePair<string, string> variable in Variables)
            {
                if (exactVariableCheck)
                {
                    if (text.Split().Contains(variable.Key))
                    {
                        if (Variables.ContainsKey(variable.Key))
                        {
                            Variables[variable.Key] = value;
                        }
                    }
                }
                else
                {
                    if (text.Contains(variable.Key))
                    {
                        if (Variables.ContainsKey(variable.Key))
                        {
                            Variables[variable.Key] = value;
                        }
                    }
                }
            }

            _tempText = text;

            return this;
        }
    }
}
