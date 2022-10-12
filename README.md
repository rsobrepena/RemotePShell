# RemotePShell

C# console application that executes a remote powershell command to a target windows server.

Please note: this application is based on the most excellent Powershell.cs gist by mdhorda, link below for original script. 
https://gist.github.com/mdhorda/70c012af0c793938ff1f

About this application: this console app makes use of the System.Management.Automation.Runspace functionality so you can execute powershell commands on a remote server 
to which you have a AD account and password in.

To get this namespace for System.Management.Automation install this Nuget package (hyperlink below).

https://www.nuget.org/packages/Microsoft.PowerShell.5.ReferenceAssemblies/1.1.0?_src=template

More than likely you will see an error when you run this the first time because you need to add the target server to the trustedhost list.
To get the WSMAN trustedhost list. Open powershell in admin mode and type the line below:

Get-Item WSMan:localhost\client\trustedHosts

Assuming your server name is DEVSERVER then you can type this line to add this 

Set-Item WSMan:localhost\client\trustedhosts -value DEVSERVER -Force

Good luck, if you encounter issues please email me at rsobrepena@gmail.com
