using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace RemotePShell
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press a key to start");
            Console.ReadKey();
            var script = @"get-childitem C:\InstallFiles\";
            var computer = Globals.ServerAddress;
            var username = Globals.ServerUsername;
            var password = Globals.ServerPassword;
            string errors;
            IEnumerable<PSObject> output;
            bool success = RunPowerShellScriptRemote(script, computer, username, password, out output, out errors);
            if (success) {
                Console.WriteLine("Success!");
                foreach (var psObj in output) {
                    Console.WriteLine(psObj.ToString());
                }
            }
            else
            {
                Console.WriteLine("Error: " + errors);
            }
            Console.ReadKey();
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        //Run this function if you want to execute a local PowerShell script.
        private bool RunPowerShellScriptLocal(string script, out IEnumerable<PSObject> output, out string errors)
        {
            return RunPowerShellScriptInternal(script, out output, out errors, null);
        }

        private static bool RunPowerShellScriptRemote(string script, string computer, string username, string password, out IEnumerable<PSObject> output, out string errors)
        {
            output = Enumerable.Empty<PSObject>();
            var credentials = new PSCredential(username, ConvertToSecureString(password));
            var connectionInfo = new WSManConnectionInfo(false, computer, 5985, "/wsman", "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", credentials);
            var runspace = RunspaceFactory.CreateRunspace(connectionInfo);

            try
            {
                Console.WriteLine("Executing Script");
                runspace.Open();
            }
            catch (Exception e)
            {
                errors = e.Message;
                Console.WriteLine(errors);
                return false;
            }

            return RunPowerShellScriptInternal(script, out output, out errors, runspace);
        }

        private static bool RunPowerShellScriptInternal(string script, out IEnumerable<PSObject> output, out string errors, Runspace runspace)
        {
            output = Enumerable.Empty<PSObject>();

            using (var ps = PowerShell.Create())
            {
                ps.Runspace = runspace;
                ps.AddScript(script);

                try
                {
                    output = ps.Invoke();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occurred in PowerShell script: " + e);
                    errors = e.Message;
                    return false;
                }

                if (ps.Streams.Error.Count > 0)
                {
                    errors = String.Join(Environment.NewLine, ps.Streams.Error.Select(e => e.ToString()));
                    return false;
                }

                errors = String.Empty;
                return true;
            }
        }
    }
}