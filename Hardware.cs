using System.Management;
using System.Runtime.InteropServices;

namespace Magic.SystemAddonsNET
{
    public class Hardware
    {

        public static string? GetProcessorID()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                string? ProcessorID = null;

                foreach (ManagementObject managObj in managCollec)
                {
                    ProcessorID = managObj.Properties["processorID"].Value?.ToString();
                    break;
                }

                return ProcessorID;
            }
            else
            {
                // Handle the non-Windows case or return null or a default value
                return null;
            }
        } // end of method

        public static string? GetMotherboardSerialNumber()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementClass managClass = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                string? MotherboardSerialNumber = null;

                foreach (ManagementObject managObj in managCollec)
                {
                    MotherboardSerialNumber = managObj.Properties["SerialNumber"].Value?.ToString();
                    break;
                }

                return MotherboardSerialNumber;
            }
            else
            {
                // Handle the non-Windows case or return null
                return null;
            }
        } // end of method

        public static long GetAvailableDiskSpace(string driveName)
        {

            DriveInfo drive = new DriveInfo(driveName);

            // parameter dalam MB
            return drive.AvailableFreeSpace / (1024 * 1024);

        } // end of method

        public static bool CheckDiskFreeSpace(string driveName, long requiredSpace)
        {

            long availableSpace = GetAvailableDiskSpace(driveName);

            return (availableSpace > requiredSpace) ? true : false;

        } // end of method

        public static long GetDirectorySize(string folderPath)
        {

            DirectoryInfo directory = new DirectoryInfo(folderPath);

            long totalSize = 0;

            // Menghitung ukuran file di folder ini
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            // Menghitung ukuran file di subfolder
            DirectoryInfo[] subDirectories = directory.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                totalSize += GetDirectorySize(subDirectory.FullName);
            }
            
            // hasil dalam MB
            return totalSize / (1024 * 1024);

        } // end of method

    } // end of namespace
} // end of namespace
