﻿
namespace RM.Resources.WindowsService
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;

    public class HostConfiguration<SERVICE> where SERVICE : IMicroService
    {
        public HostConfiguration()
        {
            OnServiceStop = service => { };
            OnServiceError = e =>
            {
                Console.WriteLine(e.ToString());
            };
        }

        public ActionEnum Action { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public SERVICE Service { get; set; }

        public Func<List<string>, SERVICE> ServiceFactory { get; set; }

        public Action<SERVICE, List<string>> OnServiceStart { get; set; }

        public Action<SERVICE> OnServiceStop { get; set; }

        public Action<Exception> OnServiceError { get; set; }

        public List<string> ExtraArguments { get; set; }
    }
}
