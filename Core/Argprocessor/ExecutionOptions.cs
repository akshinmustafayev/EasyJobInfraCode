using System;
using CommandLine;

namespace EasyJobInfraCode.Core.Argprocessor
{
    public class ExecutionOptions
    {
        // Set1
        [Option('f', "file", Required = true, HelpText = "Set file to process.", SetName = "Set1")]
        public string File { get; set; }


        // Set2
        [Option('c', "cleanup", Required = true, HelpText = "Cleanup temp folder.", SetName = "Set2")]
        public bool Cleanup { get; set; }


        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}
