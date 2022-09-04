using System;
using System.Collections.Generic;

namespace EasyJobInfraCode.Core.Preprocessor
{
    public class YamlWorkflow
    {
        public string Name { get; set; }
        public string Description { get; set; } = "Yaml workflow";
        public string Author { get; set; } = "";
        public string Url { get; set; } = "";
        public string Version { get; set; } = "1.0";
        public string Copyright { get; set; } = "";
        public List<object> Variables { get; set; } = new List<object> { };
        public List<Dictionary<string, object>> Steps { get; set; }
    }
}
