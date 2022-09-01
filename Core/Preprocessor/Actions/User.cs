using System;
using System.Collections.Generic;
using System.DirectoryServices;
using EasyJobInfraCode.Core.Argprocessor;
using EasyJobInfraCode.Core.Types;

namespace EasyJobInfraCode.Core.Preprocessor.Actions
{
    public class User : IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; } = "";
        public string Name { get; set; }
        public string Action { get; set; }
        public string Password { get; set; } = "";
        public string Description { get; set; } = "";
        public List<object> Groups { get; set; } = new List<object> { };
        public string ExactVariableCheck { get; set; } = "false";

        public void InvokeAction()
        {
            try
            {
                // Variables actions
                Name = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Name, bool.Parse(ExactVariableCheck));
                Action = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Action, bool.Parse(ExactVariableCheck));
                Password = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Password, bool.Parse(ExactVariableCheck));
                Description = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(Description, bool.Parse(ExactVariableCheck));

                // Main Action
                string action = Action.ToLower();

                DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + Constants.ENV_MACHINENAME + ",computer");

                switch (action)
                {
                    case "create":
                        {
                            DirectoryEntry newUserEntry = directoryEntry.Children.Add(Name, "user");
                            if(Password != "") { newUserEntry.Invoke("SetPassword", new object[] { Password }); }
                            if(Description != "") { newUserEntry.Invoke("Put", new object[] { "Description", Description }); }
                            newUserEntry.CommitChanges();

                            ExecutionUtils.ExecutionOptionVerbose("User account \"" + Name + "\" has been created");

                            if (Groups.Count > 0)
                            {
                                foreach (object Group in Groups)
                                {
                                    string group = Group.ToString();
                                    group = EasyJobInfraCode.VariableProcessorInstance.SetValuesFromVariables(group, bool.Parse(ExactVariableCheck));

                                    DirectoryEntry grp = directoryEntry.Children.Find(group, "group");
                                    if (grp != null) 
                                    { 
                                        ExecutionUtils.ExecutionOptionVerbose("\tAdding user to group: \"" + group + "\"");
                                        grp.Invoke("Add", new object[] { newUserEntry.Path.ToString() });
                                    }
                                }
                            }
                        }
                        break;
                    case "delete":
                        {
                            DirectoryEntries users = directoryEntry.Children;
                            DirectoryEntry user = users.Find(Name);
                            users.Remove(user);
                            ExecutionUtils.ExecutionOptionVerbose("User account \"" + Name + "\" has been removed");
                        }
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
