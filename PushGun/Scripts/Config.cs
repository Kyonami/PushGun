using System;
public static class Config
{
    public static bool isMuted = false;
    public static bool shouldIgnoreProgramsOnList = false;

    public static string[] ignoreList = new string[]
    {
        "카카오톡", "KakaoTalk",
        "Youtube"
    };
}
