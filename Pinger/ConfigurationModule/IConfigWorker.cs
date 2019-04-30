using System;
using System.Collections.Generic;

namespace Pinger.ConfigurationModule
{
    public interface IConfigWorker
    {
        Boolean SaveInConfig(params String[] values);
        Dictionary<Int32, PingerModule.IPinger> GetFromConfig();
        Boolean RemoveFromConfig(Int32 index);
        void CreateConfig();
    }
}
