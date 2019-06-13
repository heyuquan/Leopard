using Leopard.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.Config
{
    public class SetOneDI
    {
        public string Name { get; set; } = "This Is DefaultValue";
        public string Value { get; set; }
    }

    public class MySettingDI : IConfigDependency
    {
        public SetOne SetOne { get; set; }
        public string OtherValue { get; set; }
    }
}
