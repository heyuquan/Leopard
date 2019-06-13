using System;

namespace Leopard.Configuration
{
    // 方案来源：https://github.com/jonechenug/ZHS.Configuration.Sample/tree/master/src/ZHS.Configuration.Core

    /// <summary>
    /// 配置获取
    /// </summary>
    public interface IConfigurationGeter
    {
        /// <summary>
        /// 根据key获取配置
        /// </summary>
        TConfig Get<TConfig>(string key);

        /// <summary>
        /// 根据 Type.Name 获取配置
        /// </summary>
        TConfig Get<TConfig>();

        String this[string key] { get;}
    }
}