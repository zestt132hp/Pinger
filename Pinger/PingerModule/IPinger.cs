using System;

namespace Pinger.PingerModule
{
    public interface IPinger:IPingerProperty
    {
        void StartWork();
        void StopWork();
        void SetInterval(String interval);
    }
  }
