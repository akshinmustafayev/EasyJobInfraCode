﻿using System;
using System.IO;
using YamlDotNet;
using YamlDotNet.Serialization;
using CommandLine;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Preprocessor;
using EasyJobInfraCode.Core.Preprocessor.Actions;
using System.Collections.Generic;
using EasyJobInfraCode.Core.Varprocessor;

namespace EasyJobInfraCode
{
    public class EasyJobInfraCode
    {
        public static VariableProcessor VariableProcessorInstance;
        public static bool VerboseEnabled = false;

        static void Main(string[] args)
        {
            // Init processor instances
            VariableProcessorInstance = new VariableProcessor();

            // Begin to parse
            Parser.Default.ParseArguments<ExecutionOptions>(args).WithParsed(ProcessYaml).WithNotParsed(ProcessYamlErrors);
        }

        private static void ProcessYaml(ExecutionOptions executionOptions)
        {
            if (executionOptions.File != null && !File.Exists(executionOptions.File))
            {
                Console.WriteLine("Specified file does not exist!");
                return;
            }

            if (executionOptions.Cleanup == true)
            {
                ExecutionUtils.ExecutionOptionCleanup();
                return;
            }

            string yamlFileContents = File.ReadAllText(executionOptions.File);
            var yamlDeserializer = new DeserializerBuilder().Build();
            YamlWorkflow yamlWorkflow = yamlDeserializer.Deserialize<YamlWorkflow>(yamlFileContents);
            VariableProcessorInstance.InitVariable(yamlWorkflow.Variables);

            if (executionOptions.Verbose == true)
            {
                Console.WriteLine("########### Detailed Verbose enabled! ##########");
                Console.WriteLine($"# File: {executionOptions.File}");
                Console.WriteLine("\tName: " + yamlWorkflow.Name + "\n" +
                    "\tDescription: " + yamlWorkflow.Description + "\n" +
                    "\tAuthor: " + yamlWorkflow.Author + "\n" +
                    "\tUrl: " + yamlWorkflow.Url + "\n" +
                    "\tVersion: " + yamlWorkflow.Version + "\n" +
                    "\tCopyrigth: " + yamlWorkflow.Copyrigth + "\n\n");
                Console.WriteLine("################### Steps! ####################\n");
                VerboseEnabled = true;
            }

            foreach (var Step in yamlWorkflow.Steps)
            {
                if (Step.ContainsKey("ActionType"))
                {
                    object actionType = "";
                    Step.TryGetValue("ActionType", out actionType);
                    object actionName = "";
                    Step.TryGetValue("ActionName", out actionName);

                    if (actionName.ToString().Contains(' '))
                    {
                        Console.WriteLine("Step with name '" + Step["ActionName"] + "' has space character in it. It must not have space characters!");
                        continue;
                    }

                    switch (actionType.ToString())
                    {
                        case "CopyFile":
                            {
                                CopyFile copyFile = Step.ToObject<CopyFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"CopyFile step data from yaml: " +
                                    $"\n\tActionType: {copyFile.ActionType}" +
                                    $"\n\tActionName: {copyFile.ActionName}" +
                                    $"\n\tActionDescription: {copyFile.ActionDescription}" +
                                    $"\n\tFileSource: {copyFile.FileSource}" +
                                    $"\n\tFileDestination: {copyFile.FileDestination}" +
                                    $"\n\tOverwrite: {copyFile.Overwrite}");

                                copyFile.InvokeAction();
                            }
                            break;
                        case "MoveFile":
                            {
                                MoveFile moveFile = Step.ToObject<MoveFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"MoveFile step data from yaml: " +
                                    $"\n\tActionType: {moveFile.ActionType}" +
                                    $"\n\tActionName: {moveFile.ActionName}" +
                                    $"\n\tActionDescription: {moveFile.ActionDescription}" +
                                    $"\n\tFileSource: {moveFile.FileSource}" +
                                    $"\n\tFileDestination: {moveFile.FileDestination}" +
                                    $"\n\tOverwrite: {moveFile.Overwrite}");

                                moveFile.InvokeAction();
                            }
                            break;
                        case "RenameFile":
                            {
                                RenameFile renameFile = Step.ToObject<RenameFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"RenameFile step data from yaml: " +
                                    $"\n\tActionType: {renameFile.ActionType}" +
                                    $"\n\tActionName: {renameFile.ActionName}" +
                                    $"\n\tActionDescription: {renameFile.ActionDescription}" +
                                    $"\n\tFileName: {renameFile.FileName}" +
                                    $"\n\tNewFileName: {renameFile.NewFileName}" +
                                    $"\n\tOverwrite: {renameFile.Overwrite}");

                                renameFile.InvokeAction();
                            }
                            break;
                        case "CreateFile":
                            {
                                CreateFile createFile = Step.ToObject<CreateFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"CreateFile step data from yaml: " +
                                    $"\n\tActionType: {createFile.ActionType}" +
                                    $"\n\tActionName: {createFile.ActionName}" +
                                    $"\n\tActionDescription: {createFile.ActionDescription}" +
                                    $"\n\tFileName: {createFile.FileName}" +
                                    $"\n\tFileContents: {createFile.FileContents}" +
                                    $"\n\tFileEncoding: {createFile.FileEncoding}");

                                createFile.InvokeAction();
                            }
                            break;
                        case "DeleteFile":
                            {
                                DeleteFile deleteFile = Step.ToObject<DeleteFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"DeleteFile step data from yaml: " +
                                    $"\n\tActionType: {deleteFile.ActionType}" +
                                    $"\n\tActionName: {deleteFile.ActionName}" +
                                    $"\n\tActionDescription: {deleteFile.ActionDescription}" +
                                    $"\n\tFileName: {deleteFile.FileName}");

                                deleteFile.InvokeAction();
                            }
                            break;
                        case "CreateFolder":
                            {
                                CreateFolder createFolder = Step.ToObject<CreateFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"CreateFolder step data from yaml: " +
                                    $"\n\tActionType: {createFolder.ActionType}" +
                                    $"\n\tActionName: {createFolder.ActionName}" +
                                    $"\n\tActionDescription: {createFolder.ActionDescription}" +
                                    $"\n\tFolderName: {createFolder.FolderName}");

                                createFolder.InvokeAction();
                            }
                            break;
                        case "DeleteFolder":
                            {
                                DeleteFolder deleteFolder = Step.ToObject<DeleteFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"DeleteFolder step data from yaml: " +
                                    $"\n\tActionType: {deleteFolder.ActionType}" +
                                    $"\n\tActionName: {deleteFolder.ActionName}" +
                                    $"\n\tActionDescription: {deleteFolder.ActionDescription}" +
                                    $"\n\tFolderName: {deleteFolder.FolderName}");

                                deleteFolder.InvokeAction();
                            }
                            break;
                        case "CopyFolder":
                            {
                                CopyFolder copyFolder = Step.ToObject<CopyFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"CopyFolder step data from yaml: " +
                                    $"\n\tActionType: {copyFolder.ActionType}" +
                                    $"\n\tActionName: {copyFolder.ActionName}" +
                                    $"\n\tActionDescription: {copyFolder.ActionDescription}" +
                                    $"\n\tFolderSource: {copyFolder.FolderSource}" +
                                    $"\n\tFolderDestination: {copyFolder.FolderDestination}" +
                                    $"\n\tOverwrite: {copyFolder.Overwrite}");

                                copyFolder.InvokeAction();
                            }
                            break;
                        case "MoveFolder":
                            {
                                MoveFolder moveFolder = Step.ToObject<MoveFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"MoveFolder step data from yaml: " +
                                    $"\n\tActionType: {moveFolder.ActionType}" +
                                    $"\n\tActionName: {moveFolder.ActionName}" +
                                    $"\n\tActionDescription: {moveFolder.ActionDescription}" +
                                    $"\n\tFolderSource: {moveFolder.FolderSource}" +
                                    $"\n\tFolderDestination: {moveFolder.FolderDestination}");

                                moveFolder.InvokeAction();
                            }
                            break;
                        case "RenameFolder":
                            {
                                RenameFolder renameFolder = Step.ToObject<RenameFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"RenameFolder step data from yaml: " +
                                    $"\n\tActionType: {renameFolder.ActionType}" +
                                    $"\n\tActionName: {renameFolder.ActionName}" +
                                    $"\n\tActionDescription: {renameFolder.ActionDescription}" +
                                    $"\n\tFolderName: {renameFolder.FolderName}" +
                                    $"\n\tNewFolderName: {renameFolder.NewFolderName}");

                                renameFolder.InvokeAction();
                            }
                            break;
                        case "Wait":
                            {
                                Wait wait = Step.ToObject<Wait>();

                                ExecutionUtils.ExecutionOptionVerbose($"Wait step data from yaml: " +
                                    $"\n\tActionType: {wait.ActionType}" +
                                    $"\n\tActionName: {wait.ActionName}" +
                                    $"\n\tActionDescription: {wait.ActionDescription}" +
                                    $"\n\tMilliseconds: {wait.Milliseconds}");

                                wait.InvokeAction();
                            }
                            break;
                        case "InvokePowerShellScriptFile":
                            {
                                InvokePowerShellScriptFile invokePowerShellScriptFile = Step.ToObject<InvokePowerShellScriptFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"InvokePowerShellScriptFile step data from yaml: " +
                                    $"\n\tActionType: {invokePowerShellScriptFile.ActionType}" +
                                    $"\n\tActionName: {invokePowerShellScriptFile.ActionName}" +
                                    $"\n\tActionDescription: {invokePowerShellScriptFile.ActionDescription}" +
                                    $"\n\tFileName: {invokePowerShellScriptFile.FileName}" +
                                    $"\n\tFileArguments: {invokePowerShellScriptFile.GetFileArgumentsData()}" +
                                    $"\n\tPowerShellArguments: {invokePowerShellScriptFile.PowerShellArguments}" +
                                    $"\n\tWorkingDirectory: {invokePowerShellScriptFile.WorkingDirectory}" +
                                    $"\n\tPowerShellExecutable: {invokePowerShellScriptFile.PowerShellExecutable}");

                                invokePowerShellScriptFile.InvokeAction();
                            }
                            break;
                        case "InvokePowerShellScript":
                            {
                                InvokePowerShellScript invokePowerShellScript = Step.ToObject<InvokePowerShellScript>();

                                ExecutionUtils.ExecutionOptionVerbose($"InvokePowerShellScript step data from yaml: " +
                                    $"\n\tActionType: {invokePowerShellScript.ActionType}" +
                                    $"\n\tActionName: {invokePowerShellScript.ActionName}" +
                                    $"\n\tActionDescription: {invokePowerShellScript.ActionDescription}" +
                                    $"\n\tScript: {invokePowerShellScript.Script}" +
                                    $"\n\tScriptArguments: {invokePowerShellScript.GetScriptArgumentsData()}" +
                                    $"\n\tPowerShellArguments: {invokePowerShellScript.PowerShellArguments}" +
                                    $"\n\tWorkingDirectory: {invokePowerShellScript.WorkingDirectory}" +
                                    $"\n\tPowerShellExecutable: {invokePowerShellScript.PowerShellExecutable}");

                                invokePowerShellScript.InvokeAction();
                            }
                            break;
                        case "CleanFolderContents":
                            {
                                CleanFolderContents cleanFolderContents = Step.ToObject<CleanFolderContents>();

                                ExecutionUtils.ExecutionOptionVerbose($"CleanFolderContents step data from yaml: " +
                                    $"\n\tActionType: {cleanFolderContents.ActionType}" +
                                    $"\n\tActionName: {cleanFolderContents.ActionName}" +
                                    $"\n\tActionDescription: {cleanFolderContents.ActionDescription}" +
                                    $"\n\tFolderName: {cleanFolderContents.FolderName}");

                                cleanFolderContents.InvokeAction();
                            }
                            break;
                    }
                }
            }
        }

        private static void ProcessYamlErrors(IEnumerable<Error> errors)
        {
            /*
            foreach(Error error in errors)
            {
                Console.WriteLine(error.ToString());
            }*/
        }
    }
}
