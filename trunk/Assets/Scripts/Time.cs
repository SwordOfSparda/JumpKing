
static class Time
{
    // return the current timecode - 1970/1/1 00:00:00.
    static public System.UInt64 CurrentUsec()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10;
    }
}

public class Clock
{
    System.UInt64 d_start = 0;

    /** Resets clock */
    public void Reset()
    {
        this.d_start = (System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks;
    }

    /** Returns milliseconds since initialisation or last reset */
    public System.UInt64 Milliseconds()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - this.d_start) / 10000;
    }
    /** Returns microseconds since initialisation or last reset */
    public System.UInt64 Microseconds()
    {
        // 1970/1/1 00:00:00 = 621355968000000000 Ticks
        return ((System.UInt64)System.DateTime.Now.ToUniversalTime().Ticks - this.d_start) / 10;
    }
}
