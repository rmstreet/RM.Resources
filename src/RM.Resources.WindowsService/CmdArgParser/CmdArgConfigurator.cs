
namespace RM.Resources.WindowsService.CmdArgParser
{
    using Utilities;
    using System;
    using System.Collections.Generic;
    using static System.Console;
    public class CmdArgConfigurator
    {
        CmdArgConfiguration config;

        public CmdArgConfigurator(CmdArgConfiguration config) => this.config = config;

        public void AddParameter(CmdArgParam param) => config.parameters.Add(param);

        public void AddParameters(List<CmdArgParam> parameters) => config.parameters.AddRange(parameters);

        public void AddParameters(params CmdArgParam[] parameters) => config.parameters.AddRange(parameters);

        public void UseDefaultHelp() => config.parameters.Add(new CmdArgParam("Shows application help", "help", (val) => DisplayHelp()));

        public void UseAppDescription(string description) => config.AppDescription = description;

        public void ShowHelpOnExtraArguments() => config.ShowHelpOnExtraArguments = true;

        public void CustomHelp(Action<HelpData> helpAction) => config.CustomHelp = helpAction;

        public void DisplayHelp()
        {
            var helpData = new HelpData(config);
            if (config.CustomHelp != null)
            {
                try
                {
                    config.CustomHelp(helpData);
                }
                catch (Exception e)
                {
                    WriteLine("Custom help provided in this implementation thrown an exception.");
                    WriteLine("Exception details:");
                    WriteLine(e.ToString());
                    WriteLine("Showing default help instead:");
                    Help.Show(helpData);
                }
            }
            else
            {
                Help.Show(helpData);
            }
        }
    }
}
