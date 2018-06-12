using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ServiceProcess;
using System.ComponentModel;

namespace RemoteControl.Controllers
{
    [Route("api/[controller]")]
    public class ServicesController : Controller
    {
        // POST api/services
        [HttpPost]
        public ServiceInfo Post([FromBody]ServiceInfo service)
        {
            ServiceController serviceController;
            ServiceInfo serviceInfo = new ServiceInfo();

            try
            {
                serviceController = new ServiceController(service.name);
            }
            catch (ArgumentException)
            {
                serviceInfo.message = "Invalid service name: " + service.name;
                return serviceInfo;
            }

            using (serviceController)
            {
                ServiceControllerStatus status;

                try
                {
                    serviceController.Refresh(); // calling sc.Refresh() is unnecessary on the first use of `Status` but if you keep the ServiceController in-memory then be sure to call this if you're using it periodically.
                    status = serviceController.Status;

                    serviceInfo.name = service.name;
                    serviceInfo.state = status.ToString();
                    serviceInfo.message = string.Empty;
                    return serviceInfo;

                }
                catch (Exception ex)
                {
                    // A Win32Exception will be raised if the service-name does not exist or the running process has insufficient permissions to query service status.
                    // See Win32 QueryServiceStatus()'s documentation.
                    // Win32Exception was not working
                    serviceInfo.message = "Error Getting Status: " + ex.Message;
                    return serviceInfo;
                }
            }
        }
    }

    public class ServiceInfo
    {
        public string name;
        public string state;
        public string message;
    }
}
