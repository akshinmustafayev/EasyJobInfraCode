using System;
using System.Threading;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class Wait : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string Milliseconds { get; set; }

        public void InvokeAction()
        {
            try
            {
                ExecutionUtils.ExecutionOptionVerbose("Waiting for \"" + Milliseconds + "\" milliseconds\n");
                Thread.Sleep(Convert.ToInt32(Milliseconds));
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
