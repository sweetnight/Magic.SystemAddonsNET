using System.Diagnostics;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace Magic.SystemAddonsNET
{
    public static class Process
    {
        public static void KillChromes()
        {
            var procs = System.Diagnostics.Process.GetProcesses();

            foreach (var proc in procs)
            {
                if (proc.ProcessName == "chrome")
                {
                    try
                    {
                        proc.Kill();

                    }
                    catch
                    {

                    }
                }
            }
        } // end of method

        public static void KillChromium()
        {
            var procs = System.Diagnostics.Process.GetProcesses();

            foreach (var proc in procs)
            {
                if (proc.ProcessName == "chrome")
                {
                    try
                    {
                        // Check if MainModule is not null before accessing FileName
                        if (proc.MainModule != null)
                        {
                            string? processPath = proc.MainModule?.FileName;

                            // Periksa apakah proses berada dalam direktori yang biasa digunakan oleh Chromium
                            if (processPath != null && processPath.Contains("chrome-win32"))
                            {
                                proc.Kill();
                            }
                        }
                    }
                    catch
                    {
                        // Handle exceptions if necessary
                    }
                }
            }
        }


        public static void KillChromedrivers()
        {
            var procs = System.Diagnostics.Process.GetProcesses();

            foreach (var proc in procs)
            {
                if (proc.ProcessName == "chromedriver")
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch
                    {

                    }
                }
            }
        } // end of method

        public static void KillChromeForTesting(string chromeBinary)
        {

            var procs = System.Diagnostics.Process.GetProcessesByName("chrome");

            foreach (var proc in procs)
            {
                try
                {
                    if (proc.MainModule!.FileName!.Equals(chromeBinary, StringComparison.OrdinalIgnoreCase))
                    {
                        proc.Kill();
                    }
                }
                catch (Exception ex)
                {
                    // Tangani pengecualian jika diperlukan, misalnya, log kesalahan atau menampilkan pesan
                    Console.WriteLine($"Tidak dapat menghentikan proses: {ex.Message}");
                }
            }

        } // end of method

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint SetThreadExecutionState(uint esFlags);

        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;

        public static void PreventSleep()
        {
            // Mengaktifkan sistem dan display agar tetap menyala
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
        }

        public static void AllowSleep()
        {
            // Mengembalikan state ke normal
            SetThreadExecutionState(ES_CONTINUOUS);
        }

    } // end of class
} // end of namespace
