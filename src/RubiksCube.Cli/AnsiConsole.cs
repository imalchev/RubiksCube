using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RubiksCube.Cli
{
    /// <summary>
    /// <see cref="CubeFaceRenderer" /> writes true-color ANSI escape sequences directly, since the
    /// console colors cube faces need (e.g. a real orange) don't exist in the 16-color
    /// <see cref="ConsoleColor" /> palette. Modern terminals (Windows Terminal, most Linux/macOS
    /// terminals) already interpret these; the legacy Windows console host (plain cmd.exe/PowerShell
    /// windows, not running inside Windows Terminal) needs virtual terminal processing turned on
    /// explicitly, or the escape codes print as literal garbage instead of colors.
    /// </summary>
    internal static class AnsiConsole
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public static void EnableTrueColor()
        {
            if (!OperatingSystem.IsWindows())
            {
                return; // Linux/macOS terminals already interpret these escape codes.
            }

            TryEnable();
        }

        [SupportedOSPlatform("windows")]
        private static void TryEnable()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);

            if (handle == IntPtr.Zero || !GetConsoleMode(handle, out var mode))
            {
                return;
            }

            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }
    }
}
