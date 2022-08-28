using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJobInfraCode.Core.Varprocessor
{
    public class VariableProcessor
    {
        public static Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string> { };

        public VariableProcessor()
        {
            
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
                Variables.Add(variableName, "");
            }
        }

        public void InitVariable(List<object> variables)
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
    }
}
