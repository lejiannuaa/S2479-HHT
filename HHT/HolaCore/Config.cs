using System;

using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace HolaCore
{
    public class Config
    {
        private const string suffixDLL = ".dll";
        private const string suffixUPDATE = ".update";
        public const int APPDll = 0;
        public const int APPIn = 1;
        public const int APPOut = 2;
        public const int APPExcel = 3;

        public const int APPInventory = 4;
        public const int APPBusiness = 5;

        //本地日志文件名称
        public static string LogFile = "LogTime";

        //本地保存门店号
       // public static string stoNO;

        public static string LogFileSuffix = DateTime.Now.Date.ToShortDateString() + ".txt";

        //服务端&本地Config文件名
        public static string XML = "config.xml";

        //本地临时Config文件名
        public static string XMLUpdate = XML + suffixUPDATE;

        //本地临时Shell文件名
        public static string APPUpdate = Assembly.GetExecutingAssembly().GetName().Name + suffixDLL + suffixUPDATE;

        //服务器同步日期
        private static string strDate = "";
        public static string Date
        {
            get
            {
                if (string.IsNullOrEmpty(strDate))
                {
                    if (Server != null && !string.IsNullOrEmpty(Server.date))
                    {
                        strDate = Server.date;
                    }
                }

                return strDate;
            }

            set
            {
                if (value != null)
                {
                    strDate = value;

                    if (Server != null)
                    {
                        Server.date = strDate;
                    }
                }
            }
        }

        public static int Year
        {
            get
            {
                if (Date.Length == 8)
                {
                    return int.Parse(Date.Substring(0, 4));
                }

                return 2013;
            }
        }

        public static int Month
        {
            get
            {
                if (Date.Length == 8)
                {
                    return int.Parse(Date.Substring(4, 2));
                }

                return 1;
            }
        }

        public static int Day
        {
            get
            {
                if (Date.Length == 8)
                {
                    return int.Parse(Date.Substring(6, 2));
                }

                return 1;
            }
        }

        //用户名
        private static string strUser = "";
        public static string User
        {
            get
            {
                if (string.IsNullOrEmpty(strUser))
                {
                    if (Server != null && !string.IsNullOrEmpty(Server.user))
                    {
                        strUser = Server.user;
                    }
                }

                return strUser;
            }

            set
            {
                if (value != null)
                {
                    strUser = value;

                    if (Server != null)
                    {
                        Server.user = strUser;
                    }
                }
            }
        }


        //ftp用户名//
        private static string ftpUser = "";
        public static string FtpUser
        {
            get
            {
                if (string.IsNullOrEmpty(ftpUser))
                {
                    if (ftpUser != null && !string.IsNullOrEmpty(Ftp.user))
                    {
                        ftpUser = Ftp.user;
                    }
                }

                return ftpUser;
            }

            set
            {
                if (value != null)
                {
                    ftpUser = value;

                    if (Ftp != null)
                    {
                        Ftp.user = ftpUser;
                    }
                }
            }
        }

        //ftp密码//
        private static string ftpPassword = "";
        public static string FtpPassword
        {
            get
            {
                if (string.IsNullOrEmpty(ftpPassword))
                {
                    if (ftpPassword != null && !string.IsNullOrEmpty(Ftp.password))
                    {
                        ftpPassword = Ftp.password;
                    }
                }

                return ftpPassword;
            }

            set
            {
                if (value != null)
                {
                    ftpPassword = value;

                    if (Ftp != null)
                    {
                        Ftp.password = ftpPassword;
                    }
                }
            }
        }

        //ftp服务器ip//
        private static string ftpHost = "";
        public static string FtpHost
        {
            get
            {
                if (string.IsNullOrEmpty(ftpHost))
                {
                    if (ftpHost != null && !string.IsNullOrEmpty(Ftp.host))
                    {
                        ftpHost = Ftp.host;
                    }
                }

                return ftpHost;
            }

            set
            {
                if (value != null)
                {
                    ftpHost = value;

                    if (Ftp != null)
                    {
                        Ftp.host = ftpHost;
                    }
                }
            }
        }



        //服务端默认IP
        private static string strIPServer = "";
        public static string IPServer
        {
            get
            {
                if (string.IsNullOrEmpty(strIPServer))
                {
                    if (Server != null && !string.IsNullOrEmpty(Server.ipserver))
                    {
                        strIPServer = Server.ipserver;
                    }
                }

                return strIPServer;
            }

            set
            {
                if (value != null)
                {
                    strIPServer = value;

                    if (Server != null)
                    {
                        Server.ipserver = strIPServer;
                    }
                }
            }
        }

        private static string timeOut = "";
        public static string TimeOut
        {
            get
            {
                if (string.IsNullOrEmpty(timeOut))
                {
                    if (Server != null && !string.IsNullOrEmpty(Server.timeout))
                    {
                        timeOut = Server.timeout;
                    }
                }

                return timeOut;
            }
        }

        //向本地写日志
        public static void writeFile(string ss, string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(fileName, FileMode.Append);
                string str = ss + ";" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + (DateTime.Now.Ticks / 1000000000000000.0).ToString();
                byte[] log = Encoding.UTF8.GetBytes(str + "\n" + "********************************************" + "\n");
                fs.Write(log, 0, log.Length);

                fs.Close();

                fs = null;
            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }

        //下载服务器IP

        private static string strIPHttp = "";
        public static string IPHttp
        {
            get
            {

                if (string.IsNullOrEmpty(strIPHttp))
                {
                    if (Server != null && !string.IsNullOrEmpty(Server.iphttp))
                    {
                        strIPHttp = Server.iphttp;
                    }
                }

                return strIPHttp;
            }

            set
            {
                strIPHttp = value;
            }
        }

        #region GetAdapters
        [DllImport("Iphlpapi.dll")]
        public static extern uint GetAdaptersAddresses(uint Family, uint flags, IntPtr Reserved,
            IntPtr PAdaptersAddresses, ref uint pOutBufLen);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class IP_Adapter_Addresses
        {
            public uint Length;
            public uint IfIndex;
            public IntPtr Next;

            public IntPtr AdapterName;
            public IntPtr FirstUnicastAddress;
            public IntPtr FirstAnycastAddress;
            public IntPtr FirstMulticastAddress;
            public IntPtr FirstDnsServerAddress;

            public IntPtr DnsSuffix;
            public IntPtr Description;

            public IntPtr FriendlyName;

            [MarshalAs(UnmanagedType.ByValArray,
                 SizeConst = 8)]
            public Byte[] PhysicalAddress;

            public uint PhysicalAddressLength;
            public uint flags;
            public uint Mtu;
            public uint IfType;

            public uint OperStatus;

            public uint Ipv6IfIndex;
            public uint ZoneIndices;

            public IntPtr FirstPrefix;
        }

        private static void GetAdapters()
        {
            IntPtr PAdaptersAddresses = new IntPtr();

            uint pOutLen = 100;
            PAdaptersAddresses = Marshal.AllocHGlobal(100);

            uint ret =
                GetAdaptersAddresses(0, 0, (IntPtr)0, PAdaptersAddresses, ref pOutLen);

            if (ret == 111)
            {
                Marshal.FreeHGlobal(PAdaptersAddresses);
                PAdaptersAddresses = Marshal.AllocHGlobal((int)pOutLen);
                ret = GetAdaptersAddresses(0, 0, (IntPtr)0, PAdaptersAddresses, ref pOutLen);
            }

            IP_Adapter_Addresses adds = new IP_Adapter_Addresses();

            IntPtr pTemp = PAdaptersAddresses;

            // 默认获取第一个MAC
            //while (pTemp != (IntPtr)0)
            {
                Marshal.PtrToStructure(pTemp, adds);

                strMAC = string.Empty;
                for (int i = 0; i < adds.PhysicalAddressLength; i++)
                {
                    strMAC += string.Format("{0:X2}", adds.PhysicalAddress[i]);
                }

                pTemp = adds.Next;
            }
        }
        #endregion

        //无线网卡地址
        private static string strMAC = "";
        public static string MAC
        {
            get
            {
                if (string.IsNullOrEmpty(strMAC))
                {
                    GetAdapters();
                }

                return strMAC;
            }
        }

        //当前可执行文件路径
        private static string strDirLocal = "";
        public static string DirLocal
        {
            get
            {
                if (string.IsNullOrEmpty(strDirLocal))
                {
                    strDirLocal = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\";
                }

                return strDirLocal;
            }
        }

        //无线网卡IP地址
        private static string strIPLocal = "";
        public static string IPLocal
        {
            get
            {
                if (string.IsNullOrEmpty(strIPLocal))
                {
                    IPAddress[] list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                    // 默认获取第一个IP
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (list[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            strIPLocal = list[i].ToString();
                            break;
                        }
                    }
                }

                return strIPLocal;
            }
        }

        //重复登录状态值
        public static string loginTwice = "";
        public static string LoginTwice
        {
            get
            {
                if (string.IsNullOrEmpty(loginTwice))
                {
                    reset(DirLocal + XML);
                }
                return loginTwice;
            }
        }

        //本地配置文件Server信息
        private static XMLServer xmlServer = null;

        public static XMLServer Server
        {
            get
            {
                if (xmlServer == null)
                {
                    reset(DirLocal + XML);
                }

                return xmlServer;
            }
        }

        //本地配置文件ftp信息
        private static XMLftp xmlftp = null;

        public static XMLftp Ftp
        {
            get
            {
                if (xmlftp == null)
                {
                    reset(DirLocal + XML);
                }

                return xmlftp;
            }
        }



        //本地配置文件Api信息
        private static ArrayList xmlApi = null;
        public static ArrayList Api
        {
            get
            {
                if (xmlApi == null)
                {
                    reset(DirLocal + XML);
                }
                return xmlApi;
            }
        }


        //读取本地配置文件信息
        public static void reset(string xml)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeReader reader = null;
            try
            {
                doc.Load(xml);
                reader = new XmlNodeReader(doc);

                getServer(reader);
                //读取本地ftp信息
                //getftp(reader);
                getApi(reader);
                


                if (string.IsNullOrEmpty(xmlServer.ipserver))
                {
                    IPServer = xmlServer.ipserver;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        //保存当前配置到本地配置文件
        public static void save()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(DirLocal + XML);
                XmlNodeList NodeList = doc.SelectNodes("/config/server/*");
                XmlNode NodeParent = doc.SelectSingleNode("/config/server");
                XmlNode Node = NodeList[0];
                while (Node != null)
                {
                    string Xpath = "/config/server/" + Node.Name;
                    XmlNode NodeTmp = doc.SelectSingleNode(Xpath);
                    switch (Node.Name)
                    {
                        case "ver":
                            Node.InnerText = xmlServer.ver;
                            break;
                        case "user":
                            Node.InnerText = User;
                            break;
                        case "ipserver":
                            Node.InnerText = IPServer;
                            break;
                        default:
                            break;
                    }
                    Node = Node.NextSibling;
                }
                if (doc.SelectSingleNode("/config/server/date") == null)
                {
                    XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "date", null);
                    newNode.InnerText = Server.date;
                    XmlNode BeforeNode = doc.SelectSingleNode("/config/server/user");
                    NodeParent.InsertAfter(newNode, BeforeNode);
                }
                else
                {
                    doc.SelectSingleNode("/config/server/date").InnerText = Server.date;
                }
                if (doc.SelectSingleNode("/config/server/login") == null)
                {
                    XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "login", null);
                    newNode.InnerText = loginTwice;
                    XmlNode BeforeNode = doc.SelectSingleNode("/config/server/user");
                    NodeParent.InsertAfter(newNode, BeforeNode);
                }
                else
                {
                    doc.SelectSingleNode("/config/server/login").InnerText = loginTwice;
                }

                doc.Save(DirLocal + XML);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //配置文件Server结构
        public class XMLServer
        {
            public string ver;
            public string date;
            public string user;
            public string ipserver;
            public string iphttp;
            public ArrayList ip;
            public string port;
            public string dir;
            public ArrayList app;
            public string timeout;
        }

        //配置文件Api结构
        public class XMLApi
        {
            public string sn;
            public string op;
            public string file;
        }

        //配置文件ftp结构
        public class XMLftp
        {
            public string host;
            public string user;
            public string password;
        }


        //获取指定配置文件版本信息
        public static string getVer(string xml)
        {
            string ver = "";

            XmlDocument doc = new XmlDocument();
            XmlNodeReader reader = null;
            try
            {
                doc.Load(xml);
                reader = new XmlNodeReader(doc);

                string name = null;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            name = reader.Name;
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name.Equals("server"))
                            {
                                throw new Exception("End!");
                            }
                            else if (reader.Name.Equals(name))
                            {
                                name = null;
                            }
                            else
                            {
                                throw new Exception("Error!");
                            }
                            break;

                        case XmlNodeType.Text:
                            if (name.Equals("ver"))
                            {
                                ver = reader.Value;
                                throw new Exception("Found!");
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return ver;
        }

        //获取本地配置文件Server信息
        private static void getServer(XmlNodeReader reader)
        {
            xmlServer = new XMLServer();
            xmlftp = new XMLftp();

            string name = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        name = reader.Name;
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name.Equals("server"))
                        {
                            return;
                        }
                        else if (reader.Name.Equals(name))
                        {
                            name = null;
                        }
                        else
                        {
                            return;
                        }
                        break;

                    case XmlNodeType.Text:
                        switch (name)
                        {
                            case "ver":
                                xmlServer.ver = reader.Value;
                                break;

                            case "date":
                                xmlServer.date = reader.Value;
                                break;

                            case "user":
                                xmlServer.user = reader.Value;
                                break;

                            case "login":
                                loginTwice = reader.Value;
                                break;

                            case "ipserver":
                                xmlServer.ipserver = reader.Value;
                                break;

                            case "iphttp":
                                xmlServer.iphttp = reader.Value;
                                break;

                            case "port":
                                xmlServer.port = reader.Value;
                                break;

                            case "dir":
                                xmlServer.dir = reader.Value;
                                break;

                            case "timeout":
                                xmlServer.timeout = reader.Value;
                                break;

                            case "ftphost":
                                xmlftp.host = reader.Value;
                                break;

                            case "ftpuser":
                                xmlftp.user = reader.Value;
                                break;

                            case "ftppassword":
                                xmlftp.password = reader.Value;
                                break;

                            default:
                                if (name.IndexOf("ip", 0) == 0)
                                {
                                    if (xmlServer.ip == null)
                                        xmlServer.ip = new ArrayList();
                                    xmlServer.ip.Add(reader.Value);

                                    string[] ipSegLocal = IPLocal.Split('.');
                                    string isl = name.Substring(2, name.Length - 2);
                                    string iss = ipSegLocal.GetValue(2).ToString();

                                    if (isl.Equals(iss))
                                    {
                                        IPServer = reader.Value;
                                        xmlServer.ipserver = reader.Value;
                                    }
                                }
                                else if (name.IndexOf("app", 0) == 0)
                                {
                                    if (xmlServer.app == null)
                                        xmlServer.app = new ArrayList();
                                    xmlServer.app.Add(reader.Value);
                                }
                                break;
                        }
                        break;
                }

            }

        }

        //获取本地配置文件ftp信息
        //private static void getftp(XmlNodeReader reader)
        //{
        //    xmlftp = new XMLftp();

        //    string name = null;
        //    while (reader.Read())
        //    {
        //        switch (reader.NodeType)
        //        {
        //            case XmlNodeType.Element:
        //                name = reader.Name;
        //                break;

        //            case XmlNodeType.EndElement:
                        
        //                if (reader.Name.Equals("ftp"))
        //                {
        //                    return;
        //                }
        //                else if (reader.Name.Equals(name))
        //                {
        //                    name = null;
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //                break;

        //            case XmlNodeType.Text:
        //                switch (name)
        //                {
        //                    case "host":
        //                        xmlftp.host = reader.Value;
        //                        break;

        //                    case "user":
        //                        xmlftp.user = reader.Value;
        //                        break;

        //                    case "password":
        //                        xmlftp.password = reader.Value;
        //                        break;

        //                    default:
        //                        break;

        //                }
        //                break;
        //        }
                
        //    }

        //}

        //获取本地配置文件Api信息
        private static void getApi(XmlNodeReader reader)
        {
            xmlApi = new ArrayList();

            string name = null;
            XMLApi api = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        name = reader.Name;
                        if (name.Equals("api"))
                        {
                            api = new XMLApi();
                        }
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name.Equals("api"))
                        {
                            if (api != null)
                            {
                                xmlApi.Add(api);
                            }
                        }
                        else if (reader.Name.Equals(name))
                        {
                            name = null;
                        }
                        else
                        {
                            return;
                        }
                        break;

                    case XmlNodeType.Text:
                        switch (name)
                        {
                            case "sn":
                                if (api != null)
                                    api.sn = reader.Value;
                                break;

                            case "op":
                                if (api != null)
                                    api.op = reader.Value;
                                break;

                            case "file":
                                if (api != null)
                                    api.file = reader.Value;
                                break;

                            default:
                                break;
                        }
                        break;
                }
            }
        }

        public static string getApiFile(string sn, string op)
        {
            try
            {
                if (Api != null)
                {
                    for (int i = 0; i < Api.Count; i++)
                    {
                        XMLApi api = (XMLApi)Api[i];

                        if (sn.Equals(api.sn) && op.Equals(api.op))
                        {
                            return api.file;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public static string addJSONConfig(string json, string sn, string op)
        {
            try
            {
                int nIndex = json.IndexOf("{", 1) + 1;
                StringBuilder strBuilder = new StringBuilder(json.Substring(0, nIndex));
                strBuilder.Append("\"config\":{\"type\":\"");
                strBuilder.Append(sn.Substring(0, 1));
                strBuilder.Append("\",\"direction\":\"Client->Server\",\"id\":\"");
                strBuilder.Append(sn + op);
                strBuilder.Append("\"},");
                strBuilder.Append(json.Substring(nIndex, json.Length - nIndex));
                return strBuilder.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

    } // class  Config

} // namespace HolaCore