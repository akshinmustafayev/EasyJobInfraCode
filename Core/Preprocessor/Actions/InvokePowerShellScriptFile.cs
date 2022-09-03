using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Preprocessor.Utils;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class InvokePowerShellScriptFile : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; } = "";
        public string FileName { get; set; }
        public List<object> FileArguments { get; set; } = new List<object> { };
        public string PowerShellArguments { get; set; } = "";
        public List<object> Credentials { get; set; } = new List<object> { };
        public string WorkingDirectory { get; set; } = Constants.ENV_SYSTEM_DIRECTORY;
        public string PowerShellExecutable { get; set; } = Constants.ENV_SYSTEM_DIRECTORY + "\\WindowsPowerShell\\v1.0\\powershell.exe";
        public string OutBuffer { get; set; } = "";
        public string ErrBuffer { get; set; } = "";
        public string ExactVariableCheck { get; set; } = "false";
        public string CheckFileArgumentsForVariables { get; set; } = "false";

        public async void InvokeAction()
        {
            try
            {
                // Variables actions
                FileName = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(FileName, bool.Parse(ExactVariableCheck));
                PowerShellArguments = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(PowerShellArguments, bool.Parse(ExactVariableCheck));
                WorkingDirectory = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(WorkingDirectory, bool.Parse(ExactVariableCheck));
                PowerShellExecutable = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(PowerShellExecutable, bool.Parse(ExactVariableCheck));

                // Main Action
                string fileArgumentsData = "";

                if(FileArguments.Count > 0)
                {
                    fileArgumentsData = ListUtil.ConvertListToString(FileArguments);

                    if (bool.Parse(CheckFileArgumentsForVariables))
                    {
                        fileArgumentsData = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(fileArgumentsData, bool.Parse(ExactVariableCheck));
                    }
                }

                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();

                if (Credentials.Count == 3)
                {
                    ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData + " with Credentials: " + ListUtil.ConvertListToString(Credentials, "'", ", ") + "\n");

                    var result = Cli.Wrap(PowerShellExecutable)
                        .WithArguments(PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData)
                        .WithWorkingDirectory(WorkingDirectory)
                        .WithValidation(CommandResultValidation.None)
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .WithCredentials(new Credentials(Credentials[0].ToString(), Credentials[1].ToString(), Credentials[2].ToString()));

                    Task.WaitAll(result.ExecuteAsync());
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData + "\n");

                    var result = Cli.Wrap(PowerShellExecutable)
                        .WithArguments(PowerShellArguments + " -File \"" + FileName + "\" " + fileArgumentsData)
                        .WithWorkingDirectory(WorkingDirectory)
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .WithValidation(CommandResultValidation.None);

                    Task.WaitAll(result.ExecuteAsync());
                }

                OutBuffer = stdOutBuffer.ToString();
                ErrBuffer = stdErrBuffer.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
