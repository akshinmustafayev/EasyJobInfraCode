using System;
using System.Text;
using System.IO;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class CreateFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FileName { get; set; }
        public string FileContents { get; set; }
        public string FileEncoding { get; set; } = "ASCII";

        public void InvokeAction()
        {
            try
            {
                byte[] fileContentsBytes = null; 

                switch (FileEncoding)
                {
                    case "UTF8":
                        {
                            fileContentsBytes = Encoding.UTF8.GetBytes(FileContents);
                        }
                        break;
                    case "UTF7":
                        {
                            fileContentsBytes = Encoding.UTF7.GetBytes(FileContents);
                        }
                        break;
                    case "UTF32":
                        {
                            fileContentsBytes = Encoding.UTF32.GetBytes(FileContents);
                        }
                        break;
                    case "BigEndianUnicode":
                        {
                            fileContentsBytes = Encoding.BigEndianUnicode.GetBytes(FileContents);
                        }
                        break;
                    case "Latin1":
                        {
                            fileContentsBytes = Encoding.Latin1.GetBytes(FileContents);
                        }
                        break;
                    case "ASCII":
                        {
                            fileContentsBytes = Encoding.ASCII.GetBytes(FileContents);
                        }
                        break;
                    default:
                        {
                            fileContentsBytes = Encoding.ASCII.GetBytes(FileContents);
                        }
                        break;
                }
                
                File.WriteAllBytes(FileName, fileContentsBytes);
                ExecutionUtils.ExecutionOptionVerbose("Created file with name \"" + FileName + "\" using \"" + FileEncoding + "\" encoding contents of which is \"" + FileContents + "\"\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
