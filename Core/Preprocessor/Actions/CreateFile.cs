using System;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class CreateFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Name { get; set; }
        public string Content { get; set; }
        public string Encoding { get; set; } = "ASCII";
        public string Append { get; set; } = "true";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                Name = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Name, bool.Parse(ExactVariableCheck));
                Content = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Content, bool.Parse(ExactVariableCheck));
                Encoding = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Encoding, bool.Parse(ExactVariableCheck));

                // Main Action
                System.Text.Encoding encoding = System.Text.Encoding.ASCII;

                if (Encoding == "UTF8") { encoding = System.Text.Encoding.UTF8; }
                else if (Encoding == "UTF32") { encoding = System.Text.Encoding.UTF32; }
                else if (Encoding == "BigEndianUnicode") { encoding = System.Text.Encoding.BigEndianUnicode; }
                else if (Encoding == "Latin1") { encoding = System.Text.Encoding.Latin1; }
                else if (Encoding == "ASCII") { encoding = System.Text.Encoding.ASCII; }

                if (bool.Parse(Append))
                {
                    File.AppendAllText(Name, Content, encoding);
                }
                else
                {
                    File.WriteAllText(Name, Content, encoding);
                }

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Created file with name \"" + Name + "\" using \"" + Encoding + "\" encoding contents of which is \"" + Content + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
