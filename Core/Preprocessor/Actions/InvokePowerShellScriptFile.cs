using System;
using System.Collections.Generic;
using CliWrap;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class InvokePowerShellScriptFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string FileName { get; set; }
        public List<object> FileArguments { get; set; } = new List<object> { "" };
        public string PowerShellArguments { get; set; } = "";
        public string WorkingDirectory { get; set; } = Constants.ENV_SYSTEM_DIRECTORY;
        public string PowerShellExecutable { get; set; } = Constants.ENV_SYSTEM_DIRECTORY + "\\WindowsPowerShell\\v1.0\\powershell.exe";

        public async void InvokeAction()
        {
            try
            {
                string fileArgumentsData = GetFileArgumentsData();

                ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData + "\n");

                await Cli.Wrap(PowerShellExecutable)
                    .WithArguments(PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData)
                    .WithWorkingDirectory(WorkingDirectory)
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public string GetFileArgumentsData()
        {
            string argumentsData = "";

            if(FileArguments.Count > 0)
            {
                for (int i = 0; i < FileArguments.Count; i += 1)
                {
                    if (i == FileArguments.Count - 1)
                        argumentsData = argumentsData + "\"" + FileArguments[i].ToString() + "\"";
                    else
                        argumentsData = argumentsData + "\"" + FileArguments[i].ToString() + "\" ";
                }
            }

            return argumentsData;
        }
    }
}
