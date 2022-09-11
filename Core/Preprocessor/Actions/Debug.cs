using System;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class Debug : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Destination { get; set; } = "console";
        public string File { get; set; } = "";
        public string Append { get; set; } = "true";
        public string Message { get; set; }
        public string Encoding { get; set; } = "ASCII";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                Destination = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Destination, bool.Parse(ExactVariableCheck)).GetTextValue();
                File = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(File, bool.Parse(ExactVariableCheck)).GetTextValue();
                Append = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Append, bool.Parse(ExactVariableCheck)).GetTextValue();
                Message = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Message, bool.Parse(ExactVariableCheck)).GetTextValue();
                Encoding = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Encoding, bool.Parse(ExactVariableCheck)).GetTextValue();

                // Main Action
                string destination = Destination.ToLower();

                switch (destination)
                {
                    case "console":
                        {
                            Console.WriteLine(Message);
                        }
                        break;
                    case "file":
                        {
                            if(File == "")
                            {
                                Console.WriteLine("File name or path must be specified!");
                                return;
                            }

                            System.Text.Encoding encoding = System.Text.Encoding.ASCII;

                            if (Encoding == "UTF8") { encoding = System.Text.Encoding.UTF8; }
                            else if (Encoding == "UTF32") { encoding = System.Text.Encoding.UTF32; }
                            else if (Encoding == "BigEndianUnicode") { encoding = System.Text.Encoding.BigEndianUnicode; }
                            else if (Encoding == "Latin1") { encoding = System.Text.Encoding.Latin1; }
                            else if (Encoding == "ASCII") { encoding = System.Text.Encoding.ASCII; }

                            if (bool.Parse(Append))
                            {
                                System.IO.File.AppendAllText(File, Message, encoding);
                            }
                            else
                            {
                                System.IO.File.WriteAllText(File, Message, encoding);
                            }
                        }
                        break;
                }

                // Verbose
                ExecutionUtils.ExecutionOptionVerbose("Debug message \"" + Message + "\" to \"" + Destination+ "\" destination with \"" + Encoding + "\" encoding while append is \"" + Append + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
