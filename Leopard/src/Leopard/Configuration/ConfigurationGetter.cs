﻿using System;
using Microsoft.Extensions.Configuration;
using Leopard.Configuration;

namespace Leopard.Configuration
{
    public class ConfigurationGetter : IConfigurationGeter
    {
        private readonly IConfiguration _configuration;

        public ConfigurationGetter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TConfig Get<TConfig>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            var section = _configuration.GetSection(key);
            return section.Get<TConfig>();
        }

        public TConfig Get<TConfig>()
        {
            return Get<TConfig>(typeof(TConfig).Name);
        }

        public string this[string key] => _configuration[key];
    }
}