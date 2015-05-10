using System;
using System.Runtime.Serialization;

namespace NestSharp
{
    public enum ColorState
    {
        Gray,
        Green,
        Yellow,
        Red
    }

    public enum CoAlarmState
    {
        Ok,
        Warning,
        Emergency
    }

    public enum BatteryHealth
    {
        Ok,
        Replace
    }

    public enum TemperatureScale
    {
        C,
        F
    }

    public enum HvacMode
    {
        Heat,
        Cool,
        HeatCool,
        Off
    }

    public enum TemperatureSettingType
    {
        None,
        High,
        Low
    }

    public enum Away 
    {
        Home,
        Away,
        [EnumMember(Value = "auto-away")]
        AutoAway,
        Unknown
    }
}

