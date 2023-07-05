using System;
using Foundation;

public static class Utilizer
{
    /// <returns>"[Hour:Minute]"</returns>
    public static string GetCurrentTime()
    {
        DateTime now = DateTime.Now;
        return $"[{now.Hour}:{now.Minute}]";
    }

    public static NSData ParseImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);

        return NSData.FromArray(imageBytes);
    }

    /// <summary>
    /// Config.ignoreList에 있는 프로그램인지 판별합니다.
    /// </summary>
    public static bool IsProgramShouldBeIgnored(string programName)
    {
        if (Config.shouldIgnoreProgramsOnList)
            return false;

        string[] ignoreList = Config.ignoreList;

        for(int i = 0; i < ignoreList.Length; i++)
            if (ignoreList[i].Equals(programName))
                return true;

        return false;
    }
}