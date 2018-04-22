
namespace RM.Resources.WindowsService.CmdArgParser
{
    using System;
    using System.Collections.Generic;

    public class CmdArgParam
    {
        private Action<string> _value;

        public CmdArgParam(string description, string key)
        {
            Description = description;
            Key = key;
        }

        public CmdArgParam(string description, string key, Action<string> value)
        {
            Description = description;
            Key = key;
            _value = value;
        }

        public string Description { get; private set; }
        public string Key { get; private set; }
               

        public Action<string> GetValue() => _value ?? ((val) => { });

        public CmdArgParam SetValue(Action<string> value)
        {
            _value = value;
            return this;
        }

        public IReadOnlyCollection<string> GetKeys()
        {
            if (string.IsNullOrEmpty(Key))
                return new List<string>();

            var split = Key.Split('|');
            return new List<string>(split);
        }        
    }
}
