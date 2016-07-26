using System;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;


namespace CB.Application.WindowStartup
{
    public class Startup
    {
        #region Methods
        /// <summary>
        ///     Get command-line arguments set to the startup registry key of the application.
        /// </summary>
        /// <param name="appName">The name of the application that is used to set the startup registry key</param>
        /// <returns>An array of string containing command-line arguments if any; otherwise null</returns>
        public static string[] GetStartupArguments(string appName)
        {
            using (var rk = GetStartupKey())
            {
                if (ContainsAppName(rk, appName))
                {
                    var args = rk.GetValue(appName).ToString().Split(new[] { '\"' },
                        StringSplitOptions.None).Where(s => !string.IsNullOrWhiteSpace(s));
                    return args.ToArray();
                }
                return null;
            }
        }

        /// <summary>
        ///     Check whether the application name has been set in startup registry key.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <returns>True if  the application name has been set in startup registry key; otherwise false</returns>
        public static bool IsStartupSet(string appName)
        {
            using (var rk = GetStartupKey())
            {
                return ContainsAppName(rk, appName);
            }
        }

        public static bool IsStartupSet()
            => IsStartupSet(GetCurrentAppName());

        /// <summary>
        ///     Delete the information of an application from the startup registry key.
        /// </summary>
        /// <param name="appName">The name the the application.</param>
        /// <returns>True if succeed; otherwise false.</returns>
        public static bool ResetStartup(string appName)
            => DeleteStartupKey(appName);

        public static bool ResetStartup()
            => ResetStartup(GetCurrentAppName());

        public static bool SetStartup(string appName, params string[] args)
            => SetStartup(appName, GetCurrentAppPath(), args);

        public static bool SetStartup(params string[] args)
            => SetStartup(GetCurrentAppName(), GetCurrentAppPath(), args);

        /// <summary>
        ///     Write to the startup registry key the information of an application.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="appPath">The path to the executable file of the application.</param>
        /// <param name="args">The command-line arguments that will be passed when the application is started up.</param>
        /// <returns>True if succeed; otherwise false.</returns>
        public static bool SetStartup(string appName, string appPath, params string[] args)
        {
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(appPath))
            {
                throw new ArgumentException(string.Format("{0} or {0} must not be null or empty.",
                    nameof(appName)), nameof(appPath));
            }

            var arguments = args.Where(s => !string.IsNullOrEmpty(s))
                                .Select(PutInQuotes).ToArray();

            var quotedAppPath = PutInQuotes(appPath);
            var value = arguments.Any() ? quotedAppPath + " " + string.Join(" ", arguments) : quotedAppPath;
            return SetStartupKeyValue(appName, value);
        }
        #endregion


        #region Implementation
        /// <summary>
        ///     Checks whether the application name has existed in startup registry key.
        /// </summary>
        /// <param name="rk">The startup registry key.</param>
        /// <param name="appName">The name of the application.</param>
        /// <returns>True if the application name has existed; otherwise false</returns>
        private static bool ContainsAppName(RegistryKey rk, string appName)
        {
            return rk != null && rk.GetValueNames().Contains(appName);
        }

        private static bool DeleteStartupKey(string appName)
        {
            using (var rk = GetStartupKey())
            {
                if (rk != null)
                {
                    rk.DeleteValue(appName, false);
                    return true;
                }
            }
            return false;
        }

        private static string GetCurrentAppName()
            => Assembly.GetEntryAssembly().GetName().Name;

        private static string GetCurrentAppPath()
            => Assembly.GetEntryAssembly().Location;

        private static RegistryKey GetStartupKey()
            => Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        private static string PutInQuotes(string s)
            => $@"""{s}""";

        private static bool SetStartupKeyValue(string appName, string value)
        {
            using (var rk = GetStartupKey())
            {
                if (rk != null)
                {
                    rk.SetValue(appName, value, RegistryValueKind.String);
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}