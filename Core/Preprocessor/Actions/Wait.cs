using System;
using System.Threading;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class Wait : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Milliseconds { get; set; }
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                Milliseconds = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Milliseconds, bool.Parse(ExactVariableCheck));

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Waiting for \"" + Milliseconds + "\" milliseconds\n");

                // Main Action
                int milliseconds;
                Int32.TryParse(Milliseconds, out milliseconds);
                Thread.Sleep(milliseconds);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
