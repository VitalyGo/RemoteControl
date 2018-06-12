using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;

namespace RemoteControl.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        // GET api/file/consul
        [HttpDelete("{id}")]
        public string Get(string id)
        {
            if (id.ToLower().Equals("consul"))
            {
                ConsulFile consulFile = new ConsulFile();
                consulFile.server = true;
                consulFile.bootstrap_expect = 0;
                consulFile.datacenter = "";
                consulFile.log_level = "INFO";
                consulFile.retry_join = new List<string>();
                consulFile.bind_addr = "";

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(ConsulFile));

                try
                {
                    using (var stream = System.IO.File.Create(@"C:\ProgramData\Consul\consul.d\server\basic.json"))
                    {
                        js.WriteObject(stream, consulFile);
                        return "success";
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else if (id.ToLower().Equals("cti-server"))
            {
                string filePath = @"C:\Program Files (x86)\Amcom\bin\Spok CTI Server\Spok_CTI_Server_Service.exe";

                try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(filePath);

                    configuration.AppSettings.Settings["ConnectionFlag"].Value = "0";
                    configuration.AppSettings.Settings["Integration"].Value = "";
                    configuration.AppSettings.Settings["LogName"].Value = "";
                    configuration.AppSettings.Settings["LogPath"].Value = "";
                    configuration.AppSettings.Settings["PhoneServerIP"].Value = "0.0.0.0";
                    configuration.AppSettings.Settings["PhoneServerPort"].Value = "0";
                    configuration.AppSettings.Settings["TimeOut"].Value = "0";
                    configuration.Save();

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else if (id.ToLower().Equals("query"))
            {
                string filePath = @"C:\Program Files (x86)\Amcom\bin\Spok CTI Server\SpokSystemQueryService.exe.config";

                try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(filePath);

                    configuration.AppSettings.Settings["ServerIP"].Value = "0.0.0.0";
                    configuration.AppSettings.Settings["ServerPort"].Value = "0";
                    configuration.AppSettings.Settings["ConnectionType"].Value = "";
                    configuration.AppSettings.Settings["LogPath"].Value = "";
                    configuration.AppSettings.Settings["LogName"].Value = "";
                    configuration.AppSettings.Settings["MonitorACDQueueList"].Value = "";
                    configuration.AppSettings.Settings["ACDQueuePollInterval"].Value = "0";
                    configuration.Save();

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

                return "Invalid operator: " + id;
        }

        // POST api/file
        [HttpPost]
        public JsonResult Post([FromBody]FileInfo fileInfo)
        {
            string fileText;

            if (fileInfo.name.Equals("consul", StringComparison.CurrentCultureIgnoreCase))
            {
                fileText = System.IO.File.ReadAllText(@"C:\ProgramData\Consul\consul.d\server\basic.json");

                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(ConsulFile));

                using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(fileText)))
                {
                    ConsulFile file = (ConsulFile)dcjs.ReadObject(stream);
                    return Json(file);
                }

            }
            else if (fileInfo.name.Equals("rabbit", StringComparison.CurrentCultureIgnoreCase))
            {
                fileText = System.IO.File.ReadAllText(@"C:\Users\Administrator\AppData\Roaming\RabbitMQ\rabbitmq.config");
            }
            else if (fileInfo.name.Equals("cti-server", StringComparison.CurrentCultureIgnoreCase))
            {
                CtiServerFile ctiServerFile = new CtiServerFile();

                string filePath = @"C:\Program Files (x86)\Amcom\bin\Spok CTI Server\Spok_CTI_Server_Service.exe";

                try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(filePath);

                    if(configuration.AppSettings.Settings["ConnectionFlag"].Value.Equals("1"))
                    {
                        ctiServerFile.connectionFlag = true;
                    }
                    else
                    {
                        ctiServerFile.connectionFlag = false;
                    }
                    
                    ctiServerFile.integration = configuration.AppSettings.Settings["Integration"].Value;
                    ctiServerFile.logName = configuration.AppSettings.Settings["LogName"].Value;
                    ctiServerFile.logPath = configuration.AppSettings.Settings["LogPath"].Value;
                    ctiServerFile.phoneServerIP = configuration.AppSettings.Settings["PhoneServerIP"].Value;
                    ctiServerFile.phoneServerPort = Int32.Parse(configuration.AppSettings.Settings["PhoneServerPort"].Value);
                    ctiServerFile.timeOut = Int32.Parse(configuration.AppSettings.Settings["TimeOut"].Value);

                    return Json(ctiServerFile);
                }
                catch(Exception ex)
                {
                    return Json(ex.Message);
                }

            }
            else if (fileInfo.name.Equals("query", StringComparison.CurrentCultureIgnoreCase))
            {
                CtiQueryFile ctiQueryFile = new CtiQueryFile();

                string filePath = @"C:\Program Files (x86)\Amcom\bin\Spok CTI Server\SpokSystemQueryService.exe";

                try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(filePath);

                    ctiQueryFile.ServerIP = configuration.AppSettings.Settings["ServerIP"].Value;
                    ctiQueryFile.ServerPort = Int32.Parse(configuration.AppSettings.Settings["ServerPort"].Value);
                    ctiQueryFile.ConnectionType = configuration.AppSettings.Settings["ConnectionType"].Value;
                    ctiQueryFile.LogPath = configuration.AppSettings.Settings["LogPath"].Value;
                    ctiQueryFile.LogName = configuration.AppSettings.Settings["LogName"].Value;
                    ctiQueryFile.MonitorACDQueueList = configuration.AppSettings.Settings["MonitorACDQueueList"].Value;
                    ctiQueryFile.ACDQueuePollInterval = Int32.Parse(configuration.AppSettings.Settings["ACDQueuePollInterval"].Value);

                    return Json(ctiQueryFile);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }

            return Json("Invalid file type requested");
            
        }
    }

    public class FileInfo
    {
        public string name;
        public string message;
    }

    [DataContract]
    public class ConsulFile
    {
        [DataMember]
        public bool server;

        [DataMember]
        public int bootstrap_expect;

        [DataMember]
        public string datacenter;

        [DataMember]
        public string log_level;

        [DataMember]
        public List<string> retry_join;

        [DataMember]
        public string bind_addr;

    }

    [DataContract]
    public class CtiServerFile
    {
        [DataMember]
        public string integration;

        [DataMember]
        public Boolean connectionFlag;

        [DataMember]
        public string phoneServerIP;

        [DataMember]
        public int phoneServerPort;

        [DataMember]
        public int timeOut;

        [DataMember]
        public string logPath;

        [DataMember]
        public string logName;

    }

    [DataContract]
    public class CtiQueryFile
    {
        [DataMember]
        public string ServerIP;

        [DataMember]
        public int ServerPort;

        [DataMember]
        public string ConnectionType;

        [DataMember]
        public string LogPath;

        [DataMember]
        public string LogName;

        [DataMember]
        public string MonitorACDQueueList;

        [DataMember]
        public int ACDQueuePollInterval;
    }
}
