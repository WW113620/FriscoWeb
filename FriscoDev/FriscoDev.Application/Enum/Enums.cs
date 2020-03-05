namespace FriscoDev.Application.Enum
{
    public class Enums
    {
    }
    public enum TimerModeEnum
    {
        Off = 0,
        Period = 1,
        Daily = 2,
        SelectedDays = 3,
        AlwaysOn = 4,
    };

    public enum TimerFunctionTypeEnum
    {
        SpeedDisplay = 0,
        SpeedLimit = 1,
        TrafficStatistics = 2,
        Message1 = 3,
        Message2 = 4,
        Message3 = 5
    };


    public enum TimerCalendarControlEnum
    {
        Off = 0,
        On = 1,
    };

    public enum DirectionEnum
    {
        Off = 0,
        On = 1,
    };
}
