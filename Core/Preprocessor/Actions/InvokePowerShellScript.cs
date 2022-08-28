using System;
using System.Collections.Generic;
using System.IO;
using CliWrap;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class InvokePowerShellScript : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string Script { get; set; }
        public List<object> ScriptArguments { get; set; } = new List<object> { };
        public string PowerShellArguments { get; set; } = "";
        public string WorkingDirectory { get; set; } = Constants.ENV_SYSTEM_DIRECTORY;
        public string PowerShellExecutable { get; set; } = Constants.ENV_SYSTEM_DIRECTORY + "\\WindowsPowerShell\\v1.0\\powershell.exe";

        public async void InvokeAction()
        {
            try
            {
                string scriptArgumentsData = "";
                string encoddedCommand = Base64Encode(Script);

                if (ScriptArguments.Count > 0)
                {
                    scriptArgumentsData = GetScriptArgumentsData();

                    if (!Directory.Exists(Constants.APP_TEMP_PATH))
                        Directory.CreateDirectory(Constants.APP_TEMP_PATH);

                    string tempFile = Constants.APP_TEMP_PATH + Guid.NewGuid() + ".ps1";
                    File.WriteAllText(tempFile, Script);

                    ExecutionUtils.ExecutionOptionVerbose("Created: \"" + tempFile + "\" in \"" + Constants.APP_TEMP_PATH + "\" directory");
                    ExecutionUtils.ExecutionOptionVerbose("Starting: \"" + PowerShellExecutable + "\" " + PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData + "\n");

                    await Cli.Wrap(PowerShellExecutable)
                             .WithArguments(PowerShellArguments + " -File \"" + tempFile + "\" " + scriptArgumentsData)
                             .WithWorkingDirectory(WorkingDirectory)
                             .WithValidation(CommandResultValidation.None)
                             .ExecuteAsync();
                }
                else
                {
                    ExecutionUtils.ExecutionOptionVerbose("Executing script directly from yaml, since arguments were not specified\n");

                    await Cli.Wrap(PowerShellExecutable)
                             .WithArguments(PowerShellArguments + " -Encodedcommand " + encoddedCommand)
                             .WithWorkingDirectory(WorkingDirectory)
                             .WithValidation(CommandResultValidation.None)
                             .ExecuteAsync();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public string GetScriptArgumentsData()
        {
            string argumentsData = "";

            if (ScriptArguments.Count > 0)
            {
                for (int i = 0; i < ScriptArguments.Count; i += 1)
                {
                    if (i == ScriptArguments.Count - 1)
                        argumentsData = argumentsData + "\"" + ScriptArguments[i].ToString() + "\"";
                    else
                        argumentsData = argumentsData + "\"" + ScriptArguments[i].ToString() + "\" ";
                }
            }

            return argumentsData;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
