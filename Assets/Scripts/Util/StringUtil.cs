using System.Text;

public static class StringUtil
{
    private static StringBuilder sb = new StringBuilder();

    public static string SecondsToMinuteSeconds(int seconds)
    {
        sb.Clear()
            .Append((seconds / 60).ToString("D2"))
            .Append(":")
            .Append((seconds % 60).ToString("D2"));
        return sb.ToString();
    }
}