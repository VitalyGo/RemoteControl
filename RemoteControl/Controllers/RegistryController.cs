using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RemoteControl.Controllers
{
    [Route("api/[controller]")]
    public class RegistryController : Controller
    {

        //// GET api/registry
        [HttpGet]
        public LMC ValidateLmc(string key)
        {
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\NECAM\OAILIB"))
                {
                    if(registryKey != null)
                    {
                        Object lmcAddress = registryKey.GetValue("LMC IP ADDRESS");
                        Object lmcPort = registryKey.GetValue("LMC PORT");

                        return new LMC(lmcAddress.ToString(), lmcPort.ToString(), "");
                    }

                    return new LMC("", "", "Something went wrong"); 
                }
            }
            catch(Exception ex)
            {
                return new LMC("", "", "Invalid Registry Key: " + ex.Message);
            }

            
        }
    }

    public class LMC
    {
        public string address;
        public string port;
        public string message;

        public LMC(string address, string port, string message)
        {
            this.address = address;
            this.port = port;
            this.message = message;
        }
    }
}
