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
        public string ActionDescription { get; set; }
        public string FileName { get; set; }
        public List<object> Out { get; set; } = new List<object> { };

        public void InvokeAction()
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    Console.WriteLine("File \"" + FileName + "\" does not exist.");
                    return;
                }

                string data = File.ReadAllText(FileName);

                if (Out.Count > 0)
                {
                    foreach (object outVar in Out)
                    {
                        EasyJobInfraCode.VariableProcessorInstance.SetVariableValue(outVar.ToString(), data);
                    }
                }

                //ExecutionUtils.ExecutionOptionVerbose("Copied file from \"" + FileSource + "\" to \"" + FileDestination + "\" while overwrite equals \"" + Overwrite + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
