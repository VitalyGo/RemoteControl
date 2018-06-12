using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RemoteControl.Common
{
    public class Common
    {

        private Object ReadRegistry(string subKey, string keyName)
        {


            RegistryKey rk = Registry.LocalMachine;
            RegistryKey sk1 = rk.OpenSubKey(subKey);

            
            if (sk1 == null)
            {
                return "";
            }
            else
            {
                try
                {
                    var regValue = sk1.GetValue(keyName.ToUpper());
                    if (regValue.GetType() == typeof(String))
                        return (String)regValue;
                    else
                        return ((Int32)regValue).ToString();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
        private bool WriteRegistry(string subKey, string keyName, object value, RegistryValueKind kind)
        {
            try
            {
                var rk = Registry.LocalMachine;
                var skms = Registry.LocalMachine.OpenSubKey(subKey, true);


                if (skms == null)
                {
                    skms = rk.CreateSubKey(subKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }

                // Save the value
                skms.SetValue(keyName.ToUpper(), value, kind);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
