using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class DeleteFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FileName { get; set; }
        public void InvokeAction()
        {
            try
            {
                File.Delete(FileName);
                ExecutionUtils.ExecutionOptionVerbose("Deleted file \"" + FileName + "\"");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
