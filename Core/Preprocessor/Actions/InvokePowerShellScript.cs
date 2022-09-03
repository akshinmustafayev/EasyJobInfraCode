using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Preprocessor.Utils;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class InvokePowerShellScript : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Script { get; set; }
        public List<object> ScriptArguments { get; set; } = new List<object> { };
        public string PowerShellArguments { get; set; } = "";
        public List<object> Credentials { get; set; } = new List<object> { };
        public string WorkingDirectory { get; set; } = Constants.ENV_SYSTEM_DIRECTORY;
        public string PowerShellExecutable { get; set; } = Constants.ENV_SYSTEM_DIRECTORY + "\\WindowsPowerShell\\v1.0\\powershell.exe";
        public string OutBuffer { get; set; } = "";
        public string ErrBuffer { get; set; } = "";
        public string ExactVariableCheck { get; set; } = "false";
        public string CheckScriptArgumentsForVariables { get; set; } = "false";

        public async void InvokeAction()
        {
            try
            {
                // Variables actions
                PowerShellArguments = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(PowerShellArguments, bool.Parse(ExactVariableCheck));
                WorkingDirectory = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(WorkingDirectory, bool.Parse(ExactVariableCheck));
                PowerShellExecutable = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(PowerShellExecutable, bool.Parse(ExactVariableCheck));

                // Main Action
                string scriptArgumentsData = "";
                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();

                if (ScriptArguments.Count > 0)
                {
                    scriptArgumentsData = ListUtil.ConvertListToString(ScriptArguments);

                    if (bool.Parse(CheckScriptArgumentsForVariables))
                    {
                        scriptArgumentsData = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(scriptArgumentsData, bool.Parse(ExactVariableCheck));
                    }

                    if (!Directory.Exists(Constants.APP_TEMP_PATH))
                        Directory.CreateDirectory(Constants.APP_TEMP_PATH);

                    string tempFile = Constants.APP_TEMP_PATH + Guid.NewGuid() + ".ps1";
                    File.WriteAllText(tempFile, Script);

                    ExecutionUtils.ExecutionOptionVerbose("Created: \"" + tempFile + "\" in \"" + Constants.APP_TEMP_PATH + "\" directory");
                    
                    if (Credentials.Count == 3)
                    {
                        ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData + " with Credentials: " + ListUtil.ConvertListToString(Credentials, "'", ", ") + "\n");

                        var result = Cli.Wrap(PowerShellExecutable)
                             .WithArguments(PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData)
                             .WithWorkingDirectory(WorkingDirectory)
                             .WithValidation(CommandResultValidation.None)
                             .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                             .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                             .WithCredentials(new Credentials(Credentials[0].ToString(), Credentials[1].ToString(), Credentials[2].ToString()));

                        Task.WaitAll(result.ExecuteAsync());
                    }
                    else
                    {
                        ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData + "\n");

                        var result = Cli.Wrap(PowerShellExecutable)
                             .WithArguments(PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData)
                             .WithWorkingDirectory(WorkingDirectory)
                             .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                             .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                             .WithValidation(CommandResultValidation.None);

                        Task.WaitAll(result.ExecuteAsync());
                    }
                }
                else
                {
                    if(Credentials.Count == 3)
                    {
                        ExecutionUtils.ExecutionOptionVerbose("Executing script directly from yaml since arguments were not specified, with Credentials: " + ListUtil.ConvertListToString(Credentials, "'", ", ") + "\n");

                        var result = Cli.Wrap(PowerShellExecutable)
                                 .WithArguments(PowerShellArguments + " -Command \"" + Script + "\"")
                                 .WithWorkingDirectory(WorkingDirectory)
                                 .WithValidation(CommandResultValidation.None)
                                 .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                                 .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                                 .WithCredentials(new Credentials(Credentials[0].ToString(), Credentials[1].ToString(), Credentials[2].ToString()));

                        Task.WaitAll(result.ExecuteAsync());
                    }
                    else
                    {
                        ExecutionUtils.ExecutionOptionVerbose("Executing script directly from yaml, since arguments were not specified\n");

                        var result = Cli.Wrap(PowerShellExecutable)
                                 .WithArguments(PowerShellArguments + " -Command \"" + Script + "\"")
                                 .WithWorkingDirectory(WorkingDirectory)
                                 .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                                 .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                                 .WithValidation(CommandResultValidation.None);

                        Task.WaitAll(result.ExecuteAsync());
                    }
                }

                EasyJobInfraCode.VariableProcessorInstance.SetValuesToVariables(OutBuffer, stdOutBuffer.ToString(), bool.Parse(ExactVariableCheck));
                EasyJobInfraCode.VariableProcessorInstance.SetValuesToVariables(ErrBuffer, stdErrBuffer.ToString(), bool.Parse(ExactVariableCheck));

                OutBuffer = stdOutBuffer.ToString();
                ErrBuffer = stdErrBuffer.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
