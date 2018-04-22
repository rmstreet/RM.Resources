
namespace RM.Resources.WindowsService.CmdArgParser.Utilities
{
    using System.Collections.Generic;

    public class HelpData
    {
        public HelpData(CmdArgConfiguration config)
        {
            _parameters = new List<CmdArgParam>();
            _parameters.AddRange(config.parameters);
            AppDescription = config.AppDescription;
        }

        private List<CmdArgParam> _parameters;

        public IReadOnlyCollection<CmdArgParam> GetParameters()
        {
            return _parameters;
        }

        public string AppDescription { get; set; }
    }
}
