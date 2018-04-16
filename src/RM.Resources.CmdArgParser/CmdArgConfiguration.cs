
namespace RM.Resources.CmdArgParser
{
    using Utilities;
    using System;
    using System.Collections.Generic;
    public class CmdArgConfiguration
    {
        public List<CmdArgParam> parameters;

        public string AppDescription { get; set; }

        public Action<List<string>> OnUnrecognizedArguments { get; set; }

        public bool ShowHelpOnExtraArguments { get; set; }

        public Action<HelpData> CustomHelp { get; set; }

        public CmdArgConfiguration()
        {
            parameters = new List<CmdArgParam>();
            OnUnrecognizedArguments = (list) => { };
            ShowHelpOnExtraArguments = false;
        }
    }
}
