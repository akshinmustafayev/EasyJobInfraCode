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
    public class RunExecutable : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Executable { get; set; }
        public List<object> Arguments { get; set; } = new List<object> { };
        public List<object> Credentials { get; set; } = new List<object> { };
        public string WorkingDirectory { get; set; } = Constants.ENV_SYSTEM_DIRECTORY;
        public string OutBuffer { get; set; } = "";
        public string ErrBuffer { get; set; } = "";
        public string ExactVariableCheck { get; set; } = "false";
        public string CheckArgumentsForVariables { get; set; } = "false";

        public async void InvokeAction()
        {
            try
            {
                // Variables actions
                Executable = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Executable, bool.Parse(ExactVariableCheck)).GetTextValue();
                WorkingDirectory = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(WorkingDirectory, bool.Parse(ExactVariableCheck)).GetTextValue();
                
                // Main Action
                string fileArgumentsData = "";

                if (Arguments.Count > 0)
                {
                    fileArgumentsData = ListUtil.ConvertListToString(Arguments);

                    if (bool.Parse(CheckArgumentsForVariables))
                    {
                        fileArgumentsData = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(fileArgumentsData, bool.Parse(ExactVariableCheck)).GetTextValue();
                    }
                }

                var stdOutBuffer = new StringBuilder();
                var stdErrBuffer = new StringBuilder();

                if (Credentials.Count == 3)
                {
                    ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + Executable + "\" " + fileArgumentsData + " with Credentials: " + ListUtil.ConvertListToString(Credentials, "'", ", ") + "\n");

                    var result = Cli.Wrap(Executable)
                        .WithArguments(fileArgumentsData)
                        .WithWorkingDirectory(WorkingDirectory)
                        .WithValidation(CommandResultValidation.None)
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .WithCredentials(new Credentials(Credentials[0].ToString(), Credentials[1].ToString(), Credentials[2].ToString()));

                    Task.WaitAll(result.ExecuteAsync());
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + Executable + "\" " + fileArgumentsData + " with Credentials: " + ListUtil.ConvertListToString(Credentials, "'", ", ") + "\n");

                    var result = Cli.Wrap(Executable)
                        .WithArguments(fileArgumentsData)
                        .WithWorkingDirectory(WorkingDirectory)
                        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                        .WithValidation(CommandResultValidation.None);

                    Task.WaitAll(result.ExecuteAsync());
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
