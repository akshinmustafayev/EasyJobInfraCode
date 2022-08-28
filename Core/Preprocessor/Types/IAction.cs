using System;
using System.Collections.Generic;
using System.Text;

namespace EasyJobInfraCode.Core.Types
{
    interface IAction
    {
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public void InvokeAction();

        //public void CheckActionResult();
        //public IAction OnResultTrue();
        //public IAction OnResultFalse();
    }
}
