using System.Diagnostics;

public static class UnityWinAPI
{
    public static bool hasFatalError { get; private set; } = false;

    public static void OpenExplorer(string directory)
    {
        Process process = new Process();
        process.StartInfo = new ProcessStartInfo("explorer.exe", directory);
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.Verb = "runas";
        process.Start();
    }

    // WARNING: 프로그램에 치명적인 오류가 발생했을 때만 이 함수를 실행하세요.
    public static void Exit()
    {
        hasFatalError = true;
    }
}