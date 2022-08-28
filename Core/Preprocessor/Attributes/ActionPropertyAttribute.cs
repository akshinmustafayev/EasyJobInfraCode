using System;

namespace EasyJobInfraCode.Core.Preprocessor.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class ActionPropertyAttribute : System.Attribute
    {
        private bool _Mandatory = false;

        public bool Mandatory { get { return _Mandatory; } set { _Mandatory = value; } }

        public ActionPropertyAttribute(bool Mandatory)
        {
            _Mandatory = Mandatory;
        }
    }
}
