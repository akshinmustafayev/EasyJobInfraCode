using System;
using System.IO;
using YamlDotNet;
using YamlDotNet.Serialization;
using CommandLine;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Preprocessor;
using EasyJobInfraCode.Core.Preprocessor.Actions;
using System.Collections.Generic;
using EasyJobInfraCode.Core.Varprocessor;
using EasyJobInfraCode.Core.Preprocessor.Utils;

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
            VariableProcessorInstance.InitVariables(yamlWorkflow.Variables);

            if (executionOptions.Verbose == true)
            {
                Console.WriteLine("########### Detailed Verbose enabled! ##########");
                Console.WriteLine($"# File: {executionOptions.File}");
                Console.WriteLine("\tName: " + yamlWorkflow.Name + "\n" +
                    "\tDescription: " + yamlWorkflow.Description + "\n" +
                    "\tAuthor: " + yamlWorkflow.Author + "\n" +
                    "\tUrl: " + yamlWorkflow.Url + "\n" +
                    "\tVersion: " + yamlWorkflow.Version + "\n" +
                    "\tCopyrigth: " + yamlWorkflow.Copyrigth + "\n" +
                    "\tVariables: " + ListUtil.ConvertListToString(yamlWorkflow.Variables, "\"", ", ") + "\n\n");
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
                        case "CleanFolderContents":
                            {
                                CleanFolderContents cleanFolderContents = Step.ToObject<CleanFolderContents>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nCleanFolderContents step data from yaml: " +
                                    $"\n\tActionType: {cleanFolderContents.ActionType}" +
                                    $"\n\tActionName: {cleanFolderContents.ActionName}" +
                                    $"\n\tActionDescription: {cleanFolderContents.ActionDescription}" +
                                    $"\n\tFolderName: {cleanFolderContents.FolderName}");

                                cleanFolderContents.InvokeAction();
                            }
                            break;
                        case "CopyFile":
                            {
                                CopyFile copyFile = Step.ToObject<CopyFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nCopyFile step data from yaml: " +
                                    $"\n\tActionType: {copyFile.ActionType}" +
                                    $"\n\tActionName: {copyFile.ActionName}" +
                                    $"\n\tActionDescription: {copyFile.ActionDescription}" +
                                    $"\n\tFileSource: {copyFile.FileSource}" +
                                    $"\n\tFileDestination: {copyFile.FileDestination}" +
                                    $"\n\tOverwrite: {copyFile.Overwrite}");

                                copyFile.InvokeAction();
                            }
                            break;
                        case "CopyFolder":
                            {
                                CopyFolder copyFolder = Step.ToObject<CopyFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nCopyFolder step data from yaml: " +
                                    $"\n\tActionType: {copyFolder.ActionType}" +
                                    $"\n\tActionName: {copyFolder.ActionName}" +
                                    $"\n\tActionDescription: {copyFolder.ActionDescription}" +
                                    $"\n\tFolderSource: {copyFolder.FolderSource}" +
                                    $"\n\tFolderDestination: {copyFolder.FolderDestination}" +
                                    $"\n\tOverwrite: {copyFolder.Overwrite}");

                                copyFolder.InvokeAction();
                            }
                            break;
                        case "CreateFile":
                            {
                                CreateFile createFile = Step.ToObject<CreateFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nCreateFile step data from yaml: " +
                                    $"\n\tActionType: {createFile.ActionType}" +
                                    $"\n\tActionName: {createFile.ActionName}" +
                                    $"\n\tActionDescription: {createFile.ActionDescription}" +
                                    $"\n\tName: {createFile.Name}" +
                                    $"\n\tContent: {createFile.Content}" +
                                    $"\n\tEncoding: {createFile.Encoding}" +
                                    $"\n\tAppend: {createFile.Append}" +
                                    $"\n\tExactVariableCheck: {createFile.ExactVariableCheck}");

                                createFile.InvokeAction();
                            }
                            break;
                        case "CreateFolder":
                            {
                                CreateFolder createFolder = Step.ToObject<CreateFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nCreateFolder step data from yaml: " +
                                    $"\n\tActionType: {createFolder.ActionType}" +
                                    $"\n\tActionName: {createFolder.ActionName}" +
                                    $"\n\tActionDescription: {createFolder.ActionDescription}" +
                                    $"\n\tFolderName: {createFolder.FolderName}");

                                createFolder.InvokeAction();
                            }
                            break;
                        case "Debug":
                            {
                                Debug debug = Step.ToObject<Debug>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nDebug step data from yaml: " +
                                    $"\n\tActionType: {debug.ActionType}" +
                                    $"\n\tActionName: {debug.ActionName}" +
                                    $"\n\tActionDescription: {debug.ActionDescription}" +
                                    $"\n\tDestination: {debug.Destination}" +
                                    $"\n\tFile: {debug.File}" +
                                    $"\n\tAppend: {debug.Append}" +
                                    $"\n\tMessage: {debug.Message}" +
                                    $"\n\tEncoding: {debug.Encoding}");

                                debug.InvokeAction();
                            }
                            break;
                        case "DeleteFile":
                            {
                                DeleteFile deleteFile = Step.ToObject<DeleteFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nDeleteFile step data from yaml: " +
                                    $"\n\tActionType: {deleteFile.ActionType}" +
                                    $"\n\tActionName: {deleteFile.ActionName}" +
                                    $"\n\tActionDescription: {deleteFile.ActionDescription}" +
                                    $"\n\tFileName: {deleteFile.FileName}");

                                deleteFile.InvokeAction();
                            }
                            break;
                        case "DeleteFolder":
                            {
                                DeleteFolder deleteFolder = Step.ToObject<DeleteFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nDeleteFolder step data from yaml: " +
                                    $"\n\tActionType: {deleteFolder.ActionType}" +
                                    $"\n\tActionName: {deleteFolder.ActionName}" +
                                    $"\n\tActionDescription: {deleteFolder.ActionDescription}" +
                                    $"\n\tFolderName: {deleteFolder.FolderName}");

                                deleteFolder.InvokeAction();
                            }
                            break;
                        case "InvokePowerShellScript":
                            {
                                InvokePowerShellScript invokePowerShellScript = Step.ToObject<InvokePowerShellScript>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nInvokePowerShellScript step data from yaml: " +
                                    $"\n\tActionType: {invokePowerShellScript.ActionType}" +
                                    $"\n\tActionName: {invokePowerShellScript.ActionName}" +
                                    $"\n\tActionDescription: {invokePowerShellScript.ActionDescription}" +
                                    $"\n\tScript: {invokePowerShellScript.Script}" +
                                    $"\n\tScriptArguments: {ListUtil.ConvertListToString(invokePowerShellScript.ScriptArguments)}" +
                                    $"\n\tPowerShellArguments: {invokePowerShellScript.PowerShellArguments}" +
                                    $"\n\tWorkingDirectory: {invokePowerShellScript.WorkingDirectory}" +
                                    $"\n\tPowerShellExecutable: {invokePowerShellScript.PowerShellExecutable}");

                                invokePowerShellScript.InvokeAction();
                            }
                            break;
                        case "InvokePowerShellScriptFile":
                            {
                                InvokePowerShellScriptFile invokePowerShellScriptFile = Step.ToObject<InvokePowerShellScriptFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nInvokePowerShellScriptFile step data from yaml: " +
                                    $"\n\tActionType: {invokePowerShellScriptFile.ActionType}" +
                                    $"\n\tActionName: {invokePowerShellScriptFile.ActionName}" +
                                    $"\n\tActionDescription: {invokePowerShellScriptFile.ActionDescription}" +
                                    $"\n\tFileName: {invokePowerShellScriptFile.FileName}" +
                                    $"\n\tFileArguments: {ListUtil.ConvertListToString(invokePowerShellScriptFile.FileArguments)}" +
                                    $"\n\tPowerShellArguments: {invokePowerShellScriptFile.PowerShellArguments}" +
                                    $"\n\tWorkingDirectory: {invokePowerShellScriptFile.WorkingDirectory}" +
                                    $"\n\tPowerShellExecutable: {invokePowerShellScriptFile.PowerShellExecutable}");

                                invokePowerShellScriptFile.InvokeAction();
                            }
                            break;
                        case "MoveFile":
                            {
                                MoveFile moveFile = Step.ToObject<MoveFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nMoveFile step data from yaml: " +
                                    $"\n\tActionType: {moveFile.ActionType}" +
                                    $"\n\tActionName: {moveFile.ActionName}" +
                                    $"\n\tActionDescription: {moveFile.ActionDescription}" +
                                    $"\n\tFileSource: {moveFile.FileSource}" +
                                    $"\n\tFileDestination: {moveFile.FileDestination}" +
                                    $"\n\tOverwrite: {moveFile.Overwrite}");

                                moveFile.InvokeAction();
                            }
                            break;
                        case "MoveFolder":
                            {
                                MoveFolder moveFolder = Step.ToObject<MoveFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nMoveFolder step data from yaml: " +
                                    $"\n\tActionType: {moveFolder.ActionType}" +
                                    $"\n\tActionName: {moveFolder.ActionName}" +
                                    $"\n\tActionDescription: {moveFolder.ActionDescription}" +
                                    $"\n\tFolderSource: {moveFolder.FolderSource}" +
                                    $"\n\tFolderDestination: {moveFolder.FolderDestination}");

                                moveFolder.InvokeAction();
                            }
                            break;
                        case "ReadFile":
                            {
                                ReadFile readFile = Step.ToObject<ReadFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nReadFile step data from yaml: " +
                                    $"\n\tActionType: {readFile.ActionType}" +
                                    $"\n\tActionName: {readFile.ActionName}" +
                                    $"\n\tActionDescription: {readFile.ActionDescription}" +
                                    $"\n\tFileName: {readFile.FileName}" +
                                    $"\n\tSetToVariables: {readFile.Out}");

                                readFile.InvokeAction();
                            }
                            break;
                        case "RenameFile":
                            {
                                RenameFile renameFile = Step.ToObject<RenameFile>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nRenameFile step data from yaml: " +
                                    $"\n\tActionType: {renameFile.ActionType}" +
                                    $"\n\tActionName: {renameFile.ActionName}" +
                                    $"\n\tActionDescription: {renameFile.ActionDescription}" +
                                    $"\n\tFileName: {renameFile.FileName}" +
                                    $"\n\tNewFileName: {renameFile.NewFileName}" +
                                    $"\n\tOverwrite: {renameFile.Overwrite}");

                                renameFile.InvokeAction();
                            }
                            break;
                        case "RenameFolder":
                            {
                                RenameFolder renameFolder = Step.ToObject<RenameFolder>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nRenameFolder step data from yaml: " +
                                    $"\n\tActionType: {renameFolder.ActionType}" +
                                    $"\n\tActionName: {renameFolder.ActionName}" +
                                    $"\n\tActionDescription: {renameFolder.ActionDescription}" +
                                    $"\n\tFolderName: {renameFolder.FolderName}" +
                                    $"\n\tNewFolderName: {renameFolder.NewFolderName}");

                                renameFolder.InvokeAction();
                            }
                            break;
                        case "Service":
                            {
                                Service service = Step.ToObject<Service>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nService step data from yaml: " +
                                    $"\n\tActionType: {service.ActionType}" +
                                    $"\n\tActionName: {service.ActionName}" +
                                    $"\n\tActionDescription: {service.ActionDescription}" +
                                    $"\n\tName: {service.Name}" +
                                    $"\n\tAction: {service.Action}" +
                                    $"\n\tCommand: {service.Command}");

                                service.InvokeAction();
                            }
                            break;
                        case "SetVariableValue":
                            {
                                SetVariableValue setVariableValue = Step.ToObject<SetVariableValue>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nSetVariableValue step data from yaml: " +
                                    $"\n\tActionType: {setVariableValue.ActionType}" +
                                    $"\n\tActionName: {setVariableValue.ActionName}" +
                                    $"\n\tActionDescription: {setVariableValue.ActionDescription}" +
                                    $"\n\tVariables: {setVariableValue.Variable}" +
                                    $"\n\tValue: {setVariableValue.Value}");

                                setVariableValue.InvokeAction();
                            }
                            break;
                        case "User":
                            {
                                User user = Step.ToObject<User>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nUser step data from yaml: " +
                                    $"\n\tActionType: {user.ActionType}" +
                                    $"\n\tActionName: {user.ActionName}" +
                                    $"\n\tActionDescription: {user.ActionDescription}" +
                                    $"\n\tUserName: {user.Name}" +
                                    $"\n\tAction: {user.Action}" +
                                    $"\n\tPassword: {user.Password}" +
                                    $"\n\tDescription: {user.Description}" +
                                    $"\n\tGroups: {ListUtil.ConvertListToString(user.Groups)}");

                                user.InvokeAction();
                            }
                            break;
                        case "Wait":
                            {
                                Wait wait = Step.ToObject<Wait>();

                                ExecutionUtils.ExecutionOptionVerbose($"\nWait step data from yaml: " +
                                    $"\n\tActionType: {wait.ActionType}" +
                                    $"\n\tActionName: {wait.ActionName}" +
                                    $"\n\tActionDescription: {wait.ActionDescription}" +
                                    $"\n\tMilliseconds: {wait.Milliseconds}");

                                wait.InvokeAction();
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
