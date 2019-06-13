using System;

namespace Leopard.Configuration
{
    public class ConfigurationGeterLocator
    {
        private readonly IConfigurationGeter _currentServiceProvider;
        private static IConfigurationGeter _serviceProvider;

        public ConfigurationGeterLocator(IConfigurationGeter currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static ConfigurationGeterLocator Current => new ConfigurationGeterLocator(_serviceProvider);

        public static void SetLocatorProvider(IConfigurationGeter serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TConfig Get<TConfig>(String key)
        {
            return _currentServiceProvider.Get<TConfig>(key);
        }

        public TConfig Get<TConfig>()
        {
            return _currentServiceProvider.Get<TConfig>(typeof(TConfig).Name);
        }

        String this[string key] => _currentServiceProvider[key];
    }
}