 using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Library.Global_Classes
{

    public class clsLoginSettings
    {
        static string keyPath = ConfigurationManager.AppSettings["LibraryDataPath"];
        static string valueName = "StoredCredentals";

        private static RegistryView GetRegistryView()
        {
            return Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
        }

        public static bool ReadLoginDataFromRegistry(ref string username, ref string password)
        {
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, GetRegistryView()))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(keyPath))
                    {
                        if (key != null)
                        {
                            string value = key.GetValue(valueName) as string;
                            if (value != null)
                            {
                                clsLogger.LogMessage($"The value of {valueName} is: {value}", System.Diagnostics.EventLogEntryType.Information);

                                string[] arr1 = value.Split('%');
                                if (arr1.Length == 2)
                                {
                                    username = arr1[0];
                                    password = arr1[1];
                                    return true;
                                }
                            }
                            else
                            {
                                clsLogger.LogMessage($"Value {valueName} not found in the Registry.", System.Diagnostics.EventLogEntryType.Warning);
                            }
                        }
                        else
                        {
                            clsLogger.LogMessage($"Registry key '{keyPath}' not found", System.Diagnostics.EventLogEntryType.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogMessage($"An error occurred: {ex.Message}", System.Diagnostics.EventLogEntryType.Error);
            }

            return false;
        }

        public static bool UpdateLoginDataFromRegistry(string username, string password)
        {
            string valueData = $"{username}%{password}";

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, GetRegistryView()))
                {
                    using (RegistryKey key = baseKey.CreateSubKey(keyPath)) // استخدم CreateSubKey بدلاً من OpenSubKey
                    {
                        if (key != null)
                        {
                            key.SetValue(valueName, valueData, RegistryValueKind.String);
                            clsLogger.LogMessage($"Successfully updated registry value: {valueName}", System.Diagnostics.EventLogEntryType.Information);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogMessage(ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return false;
        }

        public static bool DeleteLoginDataFromRegistry()
        {
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, GetRegistryView()))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(keyPath, true))
                    {
                        if (key != null)
                        {
                            // Check if the value exists before trying to delete it
                            if (key.GetValue(valueName) != null)
                            {
                                key.DeleteValue(valueName, false);
                                clsLogger.LogMessage($"Successfully deleted registry value: {valueName}", System.Diagnostics.EventLogEntryType.Information);
                                return true;
                            }
                            else
                            {
                                clsLogger.LogMessage($"Registry value '{valueName}' not found", System.Diagnostics.EventLogEntryType.Warning);
                                return true; // Value doesn't exist, so consider it "deleted"
                            }
                        }
                        else
                        {
                            clsLogger.LogMessage($"Registry key '{keyPath}' not found", System.Diagnostics.EventLogEntryType.Warning);
                            return true; // Key doesn't exist, so consider it "deleted"
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                clsLogger.LogMessage("UnauthorizedAccessException: Run the program with administrative privileges.", System.Diagnostics.EventLogEntryType.Warning);
            }
            catch (Exception ex)
            {
                clsLogger.LogMessage($"An error occurred: {ex.Message}", System.Diagnostics.EventLogEntryType.Error);
            }
            return false;
        }
    }
}
