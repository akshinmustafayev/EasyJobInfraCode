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
        public string ActionDescription { get; set; }
        public string FileSource { get; set; }
        public string FileDestination { get; set; }
        public string Overwrite { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                File.Move(FileSource, FileDestination, bool.Parse(Overwrite));
                ExecutionUtils.ExecutionOptionVerbose("Moved file from \"" + FileSource + "\" to \"" + FileDestination + "\" while overwrite equals \"" + Overwrite + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
