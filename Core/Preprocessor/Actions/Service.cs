using System;
using System.ServiceProcess;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class Service : IAction
    {
        public string ActionType { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Name { get; set; }
        public string Action { get; set; }
        public string Command { get; set; } = "";
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                Name = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Name, bool.Parse(ExactVariableCheck));
                Action = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Action, bool.Parse(ExactVariableCheck));
                Command = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Command, bool.Parse(ExactVariableCheck));

                // Main Action
                ServiceController service = new ServiceController(Name);
               
                Action = Action.ToLower();

                switch (Action.ToLower())
                {
                    case "stop":
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Stopping service: \"" + service.DisplayName + "\"\n");
                            service.Stop();
                        }
                        break;
                    case "start":
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Starting service: \"" + service.DisplayName + "\"\n");
                            service.Start();
                        }
                        break;
                    case "pause":
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Pausing service: \"" + service.DisplayName + "\"\n");
                            service.Pause();
                        }
                        break;
                    case "continue":
                        {
                            ExecutionUtils.ExecutionOptionVerbose("Continue service: \"" + service.DisplayName + "\"\n");
                            service.Continue();
                        }
                        break;
                    case "executecommand":
                        {
                            if(Command != "")
                            {
                                int executecommand;
                                Int32.TryParse(Command, out executecommand);

                                if(executecommand >= 128 && executecommand <=256)
                                { 
                                    ExecutionUtils.ExecutionOptionVerbose("Execute commad \"" + executecommand.ToString() + "\" has been sent to service \"" + service.DisplayName + "\"\n");
                                    service.ExecuteCommand(executecommand);
                                }
                                else
                                {
                                    ExecutionUtils.ExecutionOptionVerbose("Execute commad \"" + executecommand.ToString() + "\" was not sent to service \"" + service.DisplayName + "\". The value must be between 128 and 256, inclusive.\n");
                                }
                            }
                        }
                        break;
                }

                service.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
