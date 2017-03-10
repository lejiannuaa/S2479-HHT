using System;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
namespace HolaCore
{
    //网络请求回调接口
    public interface ConnCallback
    {
        void progressCallback(int total, int progress);
        void requestCallback(string data, int result, string date);
    }

    public class ConnThread : IDisposable
    {

        public static int TimeOutInterval = 60 * 1000;
        //网络请求发起对象
        private ConnCallback sponsor = null;
       
        //请求线程
        private Thread worker = null;

        //释放标志
        private bool isDisposed = false;

        private const string CallBackGuid = "8C61855381974fd8A8ED3021C906BB57";
        [DllImport("coredll.dll")]
        //计时函数
        public static extern bool QueryPerformanceCounter(ref long count);
        [DllImport("coredll.dll")]
        //频率函数
        public static extern bool QueryPerformanceFrequency(ref long count);
        private long Start = 0;
        private long CStop = 0;
        private long freq = 0;

        #region 返回代码（错误值、内容类型）
        public const int RESULT_EXCEPTION = -2;
        public const int RESULT_UPDATE = -5;
        public const int RESULT_CONNEXCEPTION = -4;
        public const int RESULT_FILE = -1;
        public const int RESULT_TIMEOUT = -3;
        public const int RESULT_OK = 0;
        public const int RESULT_ERROR = 1;
        public const int RESULT_DUPLOGIN = 2;
        public const int RESULT_WARNING = 3;
        #endregion


        #region 下载参数
        //线程运行状态
        public enum State
        {
            Running,
            Stopping,
            Stopped 
        }
        private State eState = State.Stopped;

        //续传
        private bool isContinue = false;

        //远程Uri
        private string downloadFrom = null;

        //本地Path
        private string downloadTo = null;

        //HTTP多线程下载参数
        private int _threadNum;
        private long _fileSize;
        private string _fileName;
        private string _savePath;
        private volatile int _downloadSize;
        private Thread[] _thread;
        private List<string> _tempFiles = new List<string>();
        private Socket s = null;
        private Stream httpFileStream = null;
        private Stream localFileStram = null;
        private HttpWebRequest httprequest = null;
        private HttpWebResponse httpresponse = null;
        #endregion

        #region 发送参数
        string sendMSG = null;
        string response = "";
        string desc = "";
        string result = "";
        string date = "";
        #endregion

        #region 构造、析构函数
        public ConnThread(ConnCallback cc)
        {
            sponsor = cc;
            ServicePointManager.DefaultConnectionLimit = 100;
        }

        public void Dispose()
        {
            Dispose(true);  
            GC.SuppressFinalize(this);  
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    //通过调用托管对象的Dispose()方法释放托管对象
                }

                Abort();
                Stop();
            }

            isDisposed = true;
        }

        ~ConnThread()
        {
            Dispose(false);
        }

        #endregion

        #region 发送接口

        private readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        //重传次数
        private int retryTimes = 0;
        //发送线程
        private Thread SockSendTh = null;
        //接收线程
        private Thread SockReTh = null;
        //发送是否成功
        private bool bSend = false;
        //头发送是否成功
        private bool bSendHead = false;
        //是否超时
        private bool bTimeout = false;
        
        private delegate void SocketCallBack(byte[] buffer, int length);

        private void CallBackConnect(IAsyncResult ar)
        {
            try
            {
                TimeoutObject.Set();
            }
            catch (Exception)
            {
            }
        }

        private void CallBackSend(byte[] buffer, int sizeOfResponse)
        {
            try
            {
                TimeoutObject.Set();
            }
            catch (Exception)
            {
            }
        }

        private void CallBackReceive(byte[] buffer, int sizeOfResponse)
        {
            try
            {
                QueryPerformanceCounter(ref CStop);
                string str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "Recieve ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);
                //MessageBox.Show(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                if (!Encoding.UTF8.GetString(buffer,0,buffer.Length).Substring(0, 32).Equals(CallBackGuid))
                {
                    response = Encoding.UTF8.GetString(buffer, 0, sizeOfResponse);

                    desc = getParameter(response, "desc");
                    result = getParameter(response, "code");
					date = getParameter(response, "date");                    
                }

                TimeoutObject.Set();
            }
            catch (Exception)
            {
            }
        }

        public void Send(string msg)
        {
            try
            {
                Abort();
                eState = State.Running;
                sendMSG = msg;
                worker = new Thread(new ThreadStart(Send));
                worker.Start();
            }
            catch (Exception ex)
            {
                if (sponsor != null)
                {
                    sponsor.requestCallback(ex.ToString(), RESULT_EXCEPTION, date);
                }
            }
        }

        string getParameter(string from, string name)
        {
            try
            {
                string tmp = name + "=";
                if (from.IndexOf(tmp) >= 0)
                {
                    int index = from.IndexOf(tmp) + tmp.Length;
                    tmp = from.Substring(index);
                    int len = tmp.IndexOf(";");
                    if (len > 0)
                    {
                        return tmp.Substring(0, len);
                    }
                    return tmp;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        string AddZero(string OLstring)
        {
            string Ret = OLstring;
            if (Ret.Length < 6)
            {
                StringBuilder addString = new StringBuilder();
                int addStringNo = 6 - Ret.Length;
                addString.Append('0', addStringNo);
                Ret = addString.ToString() + Ret;
            }
            return Ret;
        }

        void Send()
        {
            response = "";
            desc = "服务器未返回任何数据！";
            result = RESULT_ERROR.ToString();
            date = "";
            retryTimes = 1;

            try
            {
                if (string.IsNullOrEmpty(Config.IPServer))
                {
                    desc = "无法匹配服务器!";
                    retryTimes = 0;
                }

                Start = 0;
                CStop = 0;
                QueryPerformanceFrequency(ref freq);

                bSend = false;
                while (!bSend && retryTimes > 0 && eState==State.Running)
                {
                    string sendHead = sendMSG.Substring(0, 32);
                    sendMSG = sendMSG.Substring(32);
                    string FileLength = "XXXX" + AddZero(sendMSG.Length.ToString());
                    sendHead = FileLength + sendHead;
                    string str = "";

                    if (s != null)
                    {
                        s.Close();
                        s = null;
                    }                    
                    s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    
                    Start = 0;
                    CStop = 0;
                    QueryPerformanceCounter(ref Start);

                    if (eState == State.Running)
                    {
                        TimeoutObject.Reset();
                        s.BeginConnect(new IPEndPoint(IPAddress.Parse(Config.IPServer), int.Parse(Config.Server.port)), CallBackConnect, s);
                    }

                    if (!TimeoutObject.WaitOne(TimeOutInterval, false))
                    {
                        QueryPerformanceCounter(ref CStop);
                        str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "ConnTimeout ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                        Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);

                        desc = "连接超时,请重新请求!";
                        bTimeout = true;
                        retryTimes--;
                        continue;
                    }

                    QueryPerformanceCounter(ref CStop);
                    str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "Conn ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                    Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
                    
                    bSendHead = false;
                    bTimeout = false;
                    while (!bSend)
                    {
                        Start = 0;
                        CStop = 0;
                        QueryPerformanceCounter(ref Start);

                        TimeoutObject.Reset();
                        if (!bSendHead)
                            SocketSend(s, sendHead, new SocketCallBack(CallBackSend));
                        else
                            SocketSend(s, sendMSG, new SocketCallBack(CallBackSend));

                        if (!TimeoutObject.WaitOne(TimeOutInterval, false))
                        {
                            QueryPerformanceCounter(ref CStop);
                            str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "SendTimeout ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                            Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);

                            desc = "发送超时,请重新请求!";
                            bTimeout = true;
                            retryTimes--;
                            break;
                        }

                        QueryPerformanceCounter(ref CStop);
                        str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "Send ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                        Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);

                        Start = 0;
                        CStop = 0;
                        QueryPerformanceCounter(ref Start);

                        if (eState == State.Running)
                        {
                            TimeoutObject.Reset();
                            SocketRecieve(s, new SocketCallBack(CallBackReceive));
                        }

                        if (!TimeoutObject.WaitOne(TimeOutInterval, false))
                        {
                            QueryPerformanceCounter(ref CStop);
                            str = "request=" + getParameter(sendMSG, "request") + getParameter(sendMSG, "op") + " ;" + "RecieveTimeout ;" + "RetryTimes=" + (3 - retryTimes).ToString() + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                            Config.writeFile(str, Config.DirLocal + Config.LogFile + Config.LogFileSuffix);

                            desc = "接收超时,请重新请求!";
                            bTimeout = true;
                            retryTimes--;
                            break;
                        }

                        if (!bSendHead)
                            bSendHead = true;
                        else
                            bSend = true;
                    }
                }    
            }
            catch (ThreadAbortException tae)
            {
                retryTimes = 0;
                desc = tae.Message;
                result = RESULT_EXCEPTION.ToString();

                eState = State.Stopping;
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                retryTimes = 0;
                desc = ex.Message;
                result = RESULT_EXCEPTION.ToString();
            }
            finally
            {
                try
                {
                    if (sponsor != null && eState==State.Running)
                    {
                        sponsor.requestCallback(desc, int.Parse(result), date);
                    }                   
                  
                    eState = State.Stopped;
                    if (s != null)
                    {
                        s.Close();
                        s = null;
                    }
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void SocketSend(Socket s, string msg, SocketCallBack callBack)
        {
            SockSendTh = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        byte[] byteOfMsg = Encoding.UTF8.GetBytes(msg);

                        int BsizeOfMsg = byteOfMsg.Length;
                        int sizeOfSended = 0;
                        while (sizeOfSended < BsizeOfMsg && !bTimeout && eState == State.Running)
                        {
                            sizeOfSended += s.Send(byteOfMsg, sizeOfSended, BsizeOfMsg - sizeOfSended, SocketFlags.None);
                        }
                        if (!bTimeout && eState == State.Running)
                        {
                            callBack(null, 0);
                        }
                    }
                    catch (ThreadAbortException tae)
                    {
                        desc = tae.Message;
                        eState = State.Stopping;
                        TimeoutObject.Set();
                    }
                    catch (Exception ex)
                    {
                        desc = ex.Message;
                    }
                }));
            SockSendTh.Start();
        }

        void SocketRecieve(Socket s, SocketCallBack callBack)
        {
            SockReTh = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        byte[] buffer = new byte[2048];

                        int sizeOfResponse = 0;
                        while (sizeOfResponse == 0 && !bTimeout && eState==State.Running)
                        {
                            sizeOfResponse += s.Receive(buffer, sizeOfResponse, s.Available, SocketFlags.None);
                        }
                        if (!bTimeout && eState == State.Running)
                        {
                            callBack(buffer, sizeOfResponse);
                        }
                    }
                    catch (ThreadAbortException tae)
                    {
                        desc = tae.Message;
                        eState = State.Stopping;
                        TimeoutObject.Set();
                    }
                    catch (Exception ex)
                    {
                        desc = ex.Message;
                    }
                }));
            SockReTh.Start();
        }

        #endregion

        #region 下载接口

        public void Download(string from, string to, bool bContinue)
        {
            try
            {
                downloadFrom = from;
                downloadTo = to;
                isContinue = bContinue;

                Abort();
                eState = State.Running;

                _threadNum = 1;
                _thread = new Thread[_threadNum];
                int pos = to.LastIndexOf('\\');
                _savePath = to.Substring(0, pos);
                _fileName = to.Substring(pos + 1, to.Length - pos - 1);

                worker = new Thread(new ThreadStart(DownloadHTTP));
                worker.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DownloadHTTP()
        {
            try
            {
                
                TimeoutObject.Reset();
                Start = 0;
                CStop = 0;
                QueryPerformanceCounter(ref Start);
                
                _thread[0] = new Thread(new ThreadStart(BeginDownload));
                _thread[0].Start();
                QueryPerformanceFrequency(ref freq);
               
                if (TimeoutObject.WaitOne(60000, false))
                {
                    ; 
                }
                else
                {
                    eState = State.Stopping;
                    QueryPerformanceCounter(ref CStop);
                    string str = "DownloadGetFileLength ;" + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                    Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);
                    if (sponsor != null)
                    {
                        sponsor.requestCallback("下载超时,请重试!", RESULT_EXCEPTION, date);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                eState = State.Stopping;
            }
            catch (Exception ex)
            {
                eState = State.Stopping;
                if (sponsor != null)
                {
                    sponsor.requestCallback(ex.Message, RESULT_EXCEPTION, date);
                }
            }
        }

        private void BeginDownload()
        {
            httpFileStream = null;
            localFileStram = null;
            httprequest = null;
            httpresponse = null;
            try
            {
                Start = 0;
                CStop = 0;
                QueryPerformanceCounter(ref Start);

                string tmpFileBlock = string.Format(@"{0}\{1}.dat", _savePath, _fileName);
                _tempFiles.Add(tmpFileBlock);

                if (File.Exists(_tempFiles[0]))
                {
                    File.Delete(_tempFiles[0]);
                }

                httprequest = (HttpWebRequest)WebRequest.Create(downloadFrom);
                httpresponse = (HttpWebResponse)httprequest.GetResponse();
                _fileSize = httpresponse.ContentLength;

                httpFileStream = httpresponse.GetResponseStream();
                localFileStram = new FileStream(_tempFiles[0], FileMode.Append);

                byte[] by = new byte[0xFFFF];
                int getByteSize = httpFileStream.Read(by, 0, (int)by.Length);

                while (eState == State.Running && getByteSize > 0)
                {
                    _downloadSize += getByteSize;

                    if (sponsor != null)
                    {
                        sponsor.progressCallback((int)_fileSize, _downloadSize);
                    }

                    localFileStram.Write(by, 0, getByteSize);
                    getByteSize = httpFileStream.Read(by, 0, (int)by.Length);
                }

                if (localFileStram != null)
                {
                    localFileStram.Close();
                    localFileStram = null;
                }

                if (httpFileStream != null)
                {
                    httpFileStream.Close();
                    httpFileStream = null;
                }
                TimeoutObject.Set();
                QueryPerformanceCounter(ref CStop);
                string str = "DownloadGetFileLength+download ;" + "Time=" + ((CStop - Start) * 1.0 / freq).ToString() + "s";
                Config.writeFile(str, Config.DirLocal +  Config.LogFile + Config.LogFileSuffix);

            }
            catch (ThreadAbortException)
            {
                eState = State.Stopping;
                TimeoutObject.Set();
                if (httpFileStream != null) httpFileStream.Close();
                if (localFileStram != null) localFileStram.Close();
                if (httprequest != null)
                {
                    httprequest.Abort();
                    httprequest = null;
                }
                if (httpresponse != null)
                {
                    httpresponse.Close();
                    httpresponse = null;
                }
            }
            catch (Exception ex)
            {
                eState = State.Stopping;
                TimeoutObject.Set();
                if (httpFileStream != null) httpFileStream.Close();
                if (localFileStram != null) localFileStram.Close();
                if (httprequest != null)
                {
                    httprequest.Abort();
                    httprequest = null;
                }
                if (httpresponse != null)
                {
                    httpresponse.Close();
                    httpresponse = null;
                }
                if (sponsor != null)
                {
                    sponsor.requestCallback(ex.Message, RESULT_EXCEPTION, date);
                }
            }
            finally
            {
                TimeoutObject.Set();
                if (httpFileStream != null) httpFileStream.Close();
                if (localFileStram != null) localFileStram.Close();
                if (httprequest != null)
                {
                    httprequest.Abort();
                    httprequest = null;
                }
                if (httpresponse != null)
                {
                    httpresponse.Close();
                    httpresponse = null;
                }

                DownloadHTTPOK();
            }
        }

        private void DownloadHTTPOK()
        {
            Stream mergeFile = null;
            BinaryWriter AddWriter = null;
            try
            {
                if (eState == State.Running)
                {
                    mergeFile = new FileStream(downloadTo, FileMode.Create);

                    AddWriter = new BinaryWriter(mergeFile);
                    foreach (string file in _tempFiles)
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open))
                        {
                            BinaryReader TempReader = new BinaryReader(fs);
                            AddWriter.Write(TempReader.ReadBytes((int)fs.Length));
                            TempReader.Close();
                        }

                        File.Delete(file);
                    }

                    AddWriter.Close();
                    AddWriter = null;

                    mergeFile.Close();
                    mergeFile = null;

                    if (sponsor != null)
                    {
                        sponsor.requestCallback(downloadTo, RESULT_FILE, date);
                    }
                   
                }
            }
            catch (ThreadAbortException)
            {
                eState=State.Stopping;
                if (mergeFile != null)
                {
                    mergeFile.Close();
                    mergeFile = null;
                }
                if (AddWriter != null)
                {
                    AddWriter.Close();
                    AddWriter = null;
                }
            }
            catch (Exception ex)
            {
                if (sponsor != null)
                {
                    sponsor.requestCallback(ex.Message, RESULT_EXCEPTION, date);
                }
            }
            finally
            {
                eState = State.Stopped;

                if (AddWriter != null) AddWriter.Close();
                if (mergeFile != null) mergeFile.Close();

                GC.Collect();
            }
        }

        #endregion

        #region 取消接口
        public void Abort()
        {
            try
            {
                if (eState == State.Running)
                {
                    eState = State.Stopping;
                }
            }
            catch (Exception)
            {
            }
        }

        public void Stop()
        {
            try
            {
                if (worker != null)
                {
                    worker.Abort();
                }
                if (SockReTh != null)
                {
                    SockReTh.Abort();
                    SockReTh = null;
                }
                if (SockSendTh != null)
                {
                    SockSendTh.Abort();
                    SockSendTh = null;
                }
                if (httpFileStream != null)
                {
                    httpFileStream.Close();
                    httpFileStream = null;
                }
                if (localFileStram != null)
                {
                    localFileStram.Close();
                    localFileStram = null;
                }
                if (httprequest != null)
                {
                    httprequest.Abort();
                    httprequest = null;
                }
                if (httpresponse != null)
                {
                    httpresponse.Close();
                    httpresponse = null;
                }
                if (_thread != null)
                {
                    for (int i = 0; i < _thread.Length; i++)
                    {
                        if (_thread[i] != null)
                        {
                            _thread[i].Abort();
                        }
                    }
                }
                worker = null;
                _thread = null;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }


    #region 局域网共享映射
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ResourceScope
    {
        RESOURCE_CONNECTED = 0x00000001, // Enumerate currently connected resources. The dwUsage member cannot be specified.
        RESOURCE_GLOBALNET = 0x00000002, // Enumerate all resources on the network. The dwUsage member is specified.
        RESOURCE_REMEMBERED = 0x00000003 // Enumerate remembered ( persistent ) connections. The dwUsage member cannot be specified.

    };
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ResourceType : uint
    {
        RESOURCETYPE_ANY = 0x00000000,
        RESOURCETYPE_DISK = 0x00000001,
        RESOURCETYPE_PRINT = 0x00000002,
        RESOURCETYPE_UNKNOWN = 0xFFFFFFFF
    };
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ResourceUsage : uint
    {
        RESOURCEUSAGE_CONNECTABLE = 0x00000001, // The resource is a connectable resource; the name pointed to by the lpRemoteName member can be passed to the WNetAddConnection() function to make a network connection.
        RESOURCEUSAGE_CONTAINER = 0x00000002, // The resource is a container resource; the name pointed to by the lpRemoteName member can be passed to the WNetOpenEnum() function to enumerate the resources in the container.
        RESOURCEUSAGE_ATTACHED = 0x00000010,
        RESOURCEUSAGE_RESERVED = 0x80000000,
        RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
    };
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ResourceDisplayType : uint
    {
        RESOURCEDISPLAYTYPE_GENERIC = 0x00000000, // The method used to display the object does not matter.
        RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001, // The object should be displayed as a domain.
        RESOURCEDISPLAYTYPE_SERVER = 0x00000002, // The object should be displayed as a server.
        RESOURCEDISPLAYTYPE_SHARE = 0x00000003, // The object should be displayed as a share.
        RESOURCEDISPLAYTYPE_FILE = 0x00000004,
        RESOURCEDISPLAYTYPE_GROUP = 0x00000005,
        RESOURCEDISPLAYTYPE_TREE = 0x0000000A,
        RESOURCEDISPLAYTYPE_NDSCONTAINER
    };
    /// <summary>
    ///
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct NETRESOURCEA
    {
        internal ResourceScope scope;
        internal ResourceType type;
        internal ResourceDisplayType displayType;
        internal ResourceUsage usage;
        internal string localName;
        internal string remoteName;
        internal string comment;
        internal string provider;
    }

    /// <summary>
    ///
    /// </summary>
    unsafe internal struct UnmanagedNETRESOURCEA
    {
        internal ResourceScope scope;
        internal ResourceType type;
        internal ResourceDisplayType displayType;
        internal ResourceUsage usage;
        internal char* lpLocalName;
        internal char* lpRemoteName;
        internal char* lpComment;
        internal char* lpProvider;
    }

    /// <summary>
    /// Encapsulates credentials device needs to send to obtain access rights to a network share
    /// </summary>
    public sealed class Credentials
    {



        /// <summary>
        /// Makes a connection to a network resource and can specify a local name for the resource
        /// </summary>
        /// <param name="hwndOwner"></param>
        /// <param name="lpNetResource"></param>
        /// <param name="lpPassword"></param>
        /// <param name="lpUserName"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Coredll.dll", SetLastError = true)]
        internal static extern int WNetAddConnection3(
        IntPtr hwndOwner,
        ref UnmanagedNETRESOURCEA lpNetResource,
        string lpPassword,
        string lpUserName,
        int dwFlags);

        /// <summary>
        /// Breaks the existing network connection
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="dwFlags"></param>
        /// <param name="fForce"></param>
        /// <returns></returns>
        [DllImport("Coredll.dll", SetLastError = true)]
        internal static extern int WNetCancelConnection2(
        string Name,
        int dwFlags,
        bool fForce
        );



        private Credentials()
        {
        }
        static Credentials()
        {
        }
        private static Credentials instance;

        /// <summary>
        /// Provides tthe access to the only instance of Credentials.
        /// </summary>
        /// <returns></returns>
        public static Credentials GetInstance()
        {
            if (instance == null)
            {
                instance = new Credentials();
            }
            return instance;
        }

        /// <summary>
        /// Sends credentials to the server specified i
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void Send(string server, string userName, string password)
        {
            this.password = password;
            this.userName = userName;
            this.server = server;

            flags = 1;
            handle = IntPtr.Zero;
            netResource = new NETRESOURCEA();
            netResource.type = ResourceType.RESOURCETYPE_DISK;
            netResource.localName = "";
            netResource.remoteName = server;
            netResource.provider = null;
            netResource.comment = null;

            // Marshal to unmanaged data representation

            localNameBuffer = MarshalHelper(netResource.localName);
            remoteNameBuffer = MarshalHelper(netResource.remoteName);
            commentBuffer = MarshalHelper(netResource.comment);
            providerBuffer = MarshalHelper(netResource.provider);

            netResourceUnmanaged = new UnmanagedNETRESOURCEA();

            unsafe
            {
                fixed (char*
                pLocalName = localNameBuffer,
                pRemoteName = remoteNameBuffer,
                pComment = commentBuffer,
                pProvider = providerBuffer
                )
                {
                    netResourceUnmanaged.lpLocalName = pLocalName;
                    netResourceUnmanaged.lpRemoteName = pRemoteName;
                    netResourceUnmanaged.lpComment = pComment;
                    netResourceUnmanaged.lpProvider = pProvider;
                    netResourceUnmanaged.type = netResource.type;


                    try
                    {
                        res = WNetAddConnection3(handle, ref netResourceUnmanaged, password, userName, flags);

                        if ((int)WinAPIErrorCodes.ErrorCode.NO_ERROR != res)
                        {
                            throw new Exception(WinAPIErrorCodes.Interpretation(res));
                        }

                    }
                    catch (Exception ex)
                    {
                        //ErrorLogger.GetInstance().Log( "Credentials: " + ex.Message );
                        throw new Exception(ex.Message);
                    }

                }
            }
        }

        public void Disconnect(string server)
        {
            int result = 5;
            try
            {
                result = WNetCancelConnection2(server, 0, true);
                if ((int)WinAPIErrorCodes.ErrorCode.NO_ERROR != result)
                {
                    throw new Exception(WinAPIErrorCodes.Interpretation(result));
                }

            }
            catch (Exception)
            {
                //ErrorLogger.GetInstance().Log("Disconnecting: " + ex.Message);
            }

        }

        // Manually marshal a CLR string
        // into a null-terminated character array
        private char[] MarshalHelper(string s)
        {
            s = s + "\0";
            return s.ToCharArray();
        }

        private int res = 5;
        private string password;
        private string userName;
        private string server;
        private NETRESOURCEA netResource;
        private UnmanagedNETRESOURCEA netResourceUnmanaged;
        private int flags;
        private IntPtr handle;
        private char[] localNameBuffer;
        private char[] remoteNameBuffer;
        private char[] commentBuffer;
        private char[] providerBuffer;
    }

    public class WinAPIErrorCodes
    {
        static WinAPIErrorCodes()
        {

        }


        // The error codes are values that identify different types of errors in the
        // Windows API. Most functions do not report the error code directly; instead,
        // they simply return a flag specifying if some error occured or not. The
        // actual error code can usually be obtained by using the GetLastError
        // function. The following list identifies various error codes defined in
        // the Windows API. Your application may define its own error codes and use
        // them with the error functions. However, make sure that bit 29 ( &H20000000 )
        // is set in any application-defined error code you define. Because no
        // Windows API-defined error code has that bit set, using that bit assures
        // that your error code will not conflict with an existring one.


        public static string Interpretation(int errorCode)
        {
            switch (errorCode)
            {
                #region cases
                case (int)(int)ErrorCode.NO_ERROR:
                    return @"No Error / Success.";

                case (int)ErrorCode.ERROR_INVALID_FUNCTION:
                    return @"Incorrect function.";

                case (int)ErrorCode.ERROR_FILE_NOT_FOUND:
                    return @"The system cannot find the file specified.";

                case (int)ErrorCode.ERROR_PATH_NOT_FOUND:
                    return @"The system cannot find the path specified.";

                case (int)ErrorCode.ERROR_TOO_MANY_OPEN_FILES:
                    return @"The system cannot open the file because too many files are currently open.";

                case (int)ErrorCode.ERROR_ACCESS_DENIED:
                    return @"Access denied.";

                case (int)ErrorCode.ERROR_INVALID_HANDLE:
                    return @"The handle is invalid.";

                case (int)ErrorCode.ERROR_ARENA_TRASHED:
                    return @"The storage control blocks were destroyed.";

                case (int)ErrorCode.ERROR_NOT_ENOUGH_MEMORY:
                    return @"Insufficient memory is available to process the command.";

                case (int)ErrorCode.ERROR_INVALID_BLOCK:
                    return @"The storage control block address is invalid.";

                case (int)ErrorCode.ERROR_BAD_ENVIRONMENT:
                    return @"The environment is incorrect.";

                case (int)ErrorCode.ERROR_BAD_FORMAT:
                    return @"An attempt to load a program with an incorrect format was made.";

                case (int)ErrorCode.ERROR_INVALID_ACCESS:
                    return @"The access code is invalid.";

                case (int)ErrorCode.ERROR_INVALID_DATA:
                    return @"The data are invalid.";

                case (int)ErrorCode.ERROR_OUTOFMEMORY:
                    return @"Insufficient memory is available to complete the operation.";

                case (int)ErrorCode.ERROR_INVALID_DRIVE:
                    return @"The system cannot find the drive specified.";

                case (int)ErrorCode.ERROR_CURRENT_DIRECTORY:
                    return @"The directory cannot be removed.";

                case (int)ErrorCode.ERROR_NOT_SAME_DEVICE:
                    return @"The system cannot move the file to a different disk drive.";

                case (int)ErrorCode.ERROR_NO_MORE_FILES:
                    return @"There are no more files.";

                case (int)ErrorCode.ERROR_WRITE_PROTECT:
                    return @"The disk is write protected.";

                case (int)ErrorCode.ERROR_BAD_UNIT:
                    return @"The system cannot find the device specified.";

                case (int)ErrorCode.ERROR_NOT_READY:
                    return @"The device is not ready.";

                case (int)ErrorCode.ERROR_BAD_COMMAND:
                    return @"The device does not recognize the command.";

                case (int)ErrorCode.ERROR_CRC:
                    return @"Cyclic redundance check ( CRC ) data error.";

                case (int)ErrorCode.ERROR_BAD_LENGTH:
                    return @"The length of the issued command is incorrect.";

                case (int)ErrorCode.ERROR_SEEK:
                    return @"The drive cannot locate a specific area or track on the disk.";

                case (int)ErrorCode.ERROR_NOT_DOS_DISK:
                    return @"The specified disk cannot be accessed.";

                case (int)ErrorCode.ERROR_SECTOR_NOT_FOUND:
                    return @"The drive cannot find the sector requested.";

                case (int)ErrorCode.ERROR_OUT_OF_PAPER:
                    return @"The printer is out of paper.";

                case (int)ErrorCode.ERROR_WRITE_FAULT:
                    return @"The system cannot write to the specified device.";

                case (int)ErrorCode.ERROR_READ_FAULT:
                    return @"The system cannot read from the specified device.";

                case (int)ErrorCode.ERROR_GEN_FAILURE:
                    return @"A device attached to the system is not functioning.";

                case (int)ErrorCode.ERROR_SHARING_VIOLATION:
                    return @"The file cannot be accessed because it is in use by another process.";

                case (int)ErrorCode.ERROR_LOCK_VIOLATION:
                    return @"The file cannot be accessed because another process has locked a portion it.";

                case (int)ErrorCode.ERROR_WRONG_DISK:
                    return @"The wrong disk is in the drive.";

                case (int)ErrorCode.ERROR_SHARING_BUFFER_EXCEEDED:
                    return @"Too many files have been opened for sharing.";

                case (int)ErrorCode.ERROR_HANDLE_EOF:
                    return @"The end of file ( EOF ) has been reached.";

                case (int)ErrorCode.ERROR_HANDLE_DISK_FULL:
                    return @"The disk is full.";

                case (int)ErrorCode.ERROR_NOT_SUPPORTED:
                    return @"The network request is not supported.";

                case (int)ErrorCode.ERROR_REM_NOT_LIST:
                    return @"The remote computer is not available.";

                case (int)ErrorCode.ERROR_DUP_NAME:
                    return @"A duplicate name exists on the network.";

                case (int)ErrorCode.ERROR_BAD_NETPATH:
                    return @"The network path was not found.";

                case (int)ErrorCode.ERROR_NETWORK_BUSY:
                    return @"The network is busy.";

                case (int)ErrorCode.ERROR_DEV_NOT_EXIST:
                    return @"The specified network resource or device is not available.";

                case (int)ErrorCode.ERROR_TOO_MANY_CMDS:
                    return @"The network BIOS command limit has been reached.";

                case (int)ErrorCode.ERROR_ADAP_HDW_ERR:
                    return @"A network adapter hardware error has occured.";

                case (int)ErrorCode.ERROR_BAD_NET_RESP:
                    return @"The specified server cannot perform the requested operation.";

                case (int)ErrorCode.ERROR_UNEXP_NET_ERR:
                    return @"An unexpected network error has occured.";

                case (int)ErrorCode.ERROR_BAD_REM_ADAP:
                    return @"The remote adapter is incompatible.";

                case (int)ErrorCode.ERROR_PRINTQ_FULL:
                    return @"The printer queue is full.";

                case (int)ErrorCode.ERROR_NO_SPOOL_SPACE:
                    return @"No space to store the file waiting to be printed is available on the server.";

                case (int)ErrorCode.ERROR_PRINT_CANCELLED:
                    return @"The file waiting to be printed was deleted.";

                case (int)ErrorCode.ERROR_NETNAME_DELETED:
                    return @"The specified network name is unavailable.";

                case (int)ErrorCode.ERROR_NETWORK_ACCESS_DENIED:
                    return @"Network access is denied.";

                case (int)ErrorCode.ERROR_BAD_DEV_TYPE:
                    return @"The network resource type is incorrect.";

                case (int)ErrorCode.ERROR_BAD_NET_NAME:
                    return @"The network name cannot be found.";

                case (int)ErrorCode.ERROR_TOO_MANY_NAMES:
                    return @"The name limit for the local computer network adapter card was exceeded.";

                case (int)ErrorCode.ERROR_TOO_MANY_SESS:
                    return @"The network BIOS session limit was exceeded.";

                case (int)ErrorCode.ERROR_SHARING_PAUSED:
                    return @"The remote server is paused or is in the process of beig started.";

                case (int)ErrorCode.ERROR_REQ_NOT_ACCEP:
                    return @"The network request was not accepted.";

                case (int)ErrorCode.ERROR_REDIR_PAUSED:
                    return @"The specified printer or disk device is paused.";

                case (int)ErrorCode.ERROR_FILE_EXISTS:
                    return @"The file exists.";

                case (int)ErrorCode.ERROR_CANNOT_MAKE:
                    return @"The directory or file cannot be created.";

                case (int)ErrorCode.ERROR_FAIL_I24:
                    return @"Failure on Interrupt 24 ( INT 24 ).";

                case (int)ErrorCode.ERROR_OUT_OF_STRUCTURES:
                    return @"Storage to process the request is unavailable.";

                case (int)ErrorCode.ERROR_ALREADY_ASSIGNED:
                    return @"The local device name is already in use.";

                case (int)ErrorCode.ERROR_INVALID_PASSWORD:
                    return @"The specified network password is incorrect.";

                case (int)ErrorCode.ERROR_INVALID_PARAMETER:
                    return @"The parameter is incorrect.";

                case (int)ErrorCode.ERROR_NET_WRITE_FAULT:
                    return @"A write fault occured on the network.";

                case (int)ErrorCode.ERROR_NO_PROC_SLOTS:
                    return @"The system cannot start another process at this time.";

                case (int)ErrorCode.ERROR_TOO_MANY_SEMAPHORES:
                    return @"The system cannot create another semaphore.";

                case (int)ErrorCode.ERROR_EXCL_SEM_ALREADY_OWNED:
                    return @"The exclusive semaphore is already owned by another process.";

                case (int)ErrorCode.ERROR_SEM_IS_SET:
                    return @"The semaphore is set and cannot be closed.";

                case (int)ErrorCode.ERROR_TOO_MANY_SEM_REQUESTS:
                    return @"The semaphore cannot be set again.";

                case (int)ErrorCode.ERROR_INVALID_AT_INTERRUPT_TIME:
                    return @"The system cannot request exclusive semaphores at interrupt time.";

                case (int)ErrorCode.ERROR_SEM_OWNER_DIED:
                    return @"The previous ownership of this semaphore has ended.";

                case (int)ErrorCode.ERROR_SEM_USER_LIMIT:
                    return @"The system has reached the semaphore user limit.";

                case (int)ErrorCode.ERROR_DISK_CHANGE:
                    return @"The program stopped because the alternate disk was not inserted.";

                case (int)ErrorCode.ERROR_DRIVE_LOCKED:
                    return @"The disk is in use or is locked by another process.";

                case (int)ErrorCode.ERROR_BROKEN_PIPE:
                    return @"The pipe has been ended.";

                case (int)ErrorCode.ERROR_OPEN_FAILED:
                    return @"The system cannot open the device or file specified.";

                case (int)ErrorCode.ERROR_BUFFER_OVERFLOW:
                    return @"The file name is too long.";

                case (int)ErrorCode.ERROR_DISK_FULL:
                    return @"There is insufficient space on the disk.";

                case (int)ErrorCode.ERROR_NO_MORE_SEARCH_HANDLES:
                    return @"No more file search handles are available.";

                case (int)ErrorCode.ERROR_INVALID_TARGET_HANDLE:
                    return @"The target file handle is incorrect.";

                case (int)ErrorCode.ERROR_INVALID_CATEGORY:
                    return @"The IOCTL call made by the program is incorrect.";

                case (int)ErrorCode.ERROR_INVALID_VERIFY_SWITCH:
                    return @"The verify-on-write parameter is incorrect.";

                case (int)ErrorCode.ERROR_BAD_DRIVER_LEVEL:
                    return @"The system does not support the command requested.";

                case (int)ErrorCode.ERROR_CALL_NOT_IMPLEMENTED:
                    return @"This function is only valid under Windows NT/2000.";

                case (int)ErrorCode.ERROR_SEM_TIMEOUT:
                    return @"The semaphore timeout experiod has expired.";

                case (int)ErrorCode.ERROR_INSUFFICIENT_BUFFER:
                    return @"The data area passed to a system call is too small.";

                case (int)ErrorCode.ERROR_INVALID_NAME:
                    return @"The syntax of the filename, directory name, or volume label is incorrect.";

                case (int)ErrorCode.ERROR_INVALID_LEVEL:
                    return @"The system call level is incorrect.";

                case (int)ErrorCode.ERROR_NO_VOLUME_LABEL:
                    return @"The disk has no volume label.";

                case (int)ErrorCode.ERROR_MOD_NOT_FOUND:
                    return @"The specified module cannot be found.";

                case (int)ErrorCode.ERROR_PROC_NOT_FOUND:
                    return @"The specified procedure cannot be found.";

                case (int)ErrorCode.ERROR_WAIT_NO_CHILDREN:
                    return @"There are no child processes to wait for.";

                case (int)ErrorCode.ERROR_CHILD_NOT_COMPLETE:
                    return @"The program cannot run under Windows NT/2000.";

                case (int)ErrorCode.ERROR_DIRECT_ACCESS_HANDLE:
                    return @"A file handle or open disk partition was attempted to be used for an operation other than raw disk I/O.";

                case (int)ErrorCode.ERROR_NEGATIVE_SEEK:
                    return @"An attempt was made to move a file pointer before the beginning of the file.";

                case (int)ErrorCode.ERROR_SEEK_ON_DEVICE:
                    return @"The file pointer cannot be set on the specified device or file.";

                case (int)ErrorCode.ERROR_IS_JOIN_TARGET:
                    return @"A JOIN or SUBST command cannot be used for a drive that contains previously joined drives.";

                case (int)ErrorCode.ERROR_IS_JOINED:
                    return @"An attempt was made to use a JOIN or SUBST command on a drive that has already been joined.";

                case (int)ErrorCode.ERROR_IS_SUBSTED:
                    return @"An attempt was made to use a JOIN or SUBST command on a drive that has already been substituted.";

                case (int)ErrorCode.ERROR_NOT_JOINED:
                    return @"The system tried to delete the JOIN of a drive that is not joined.";

                case (int)ErrorCode.ERROR_NOT_SUBSTED:
                    return @"The system tried to delete the SUBST of a drive that is not substituted.";

                case (int)ErrorCode.ERROR_JOIN_TO_JOIN:
                    return @"The system tried to JOIN a drive to a directory on a joined drive.";

                case (int)ErrorCode.ERROR_SUBST_TO_SUBST:
                    return @"The system tried to SUBST a drive to a directory on a substituted drive.";

                case (int)ErrorCode.ERROR_JOIN_TO_SUBST:
                    return @"The system tried to JOIN a drive to a directory on a substituted drive.";

                case (int)ErrorCode.ERROR_SUBST_TO_JOIN:
                    return @"The system tried to SUBST a drive to a directory on a joined drive.";

                case (int)ErrorCode.ERROR_BUSY_DRIVE:
                    return @"The system cannot perform a JOIN or SUBST at this time.";

                case (int)ErrorCode.ERROR_SAME_DRIVE:
                    return @"The system cannot JOIN or SUBST a drive to or for a directory on the same drive.";

                case (int)ErrorCode.ERROR_DIR_NOT_ROOT:
                    return @"The directory is not a subdirectory of the root directory.";

                case (int)ErrorCode.ERROR_DIR_NOT_EMPTY:
                    return @"The directory is not empty.";

                case (int)ErrorCode.ERROR_IS_SUBST_PATH:
                    return @"The path specified is being used in a SUBST.";

                case (int)ErrorCode.ERROR_IS_JOIN_PATH:
                    return @"The path specified is being used in a JOIN.";

                case (int)ErrorCode.ERROR_PATH_BUSY:
                    return @"The path specified cannot be used at this time.";

                case (int)ErrorCode.ERROR_IS_SUBST_TARGET:
                    return @"An attempt was made to JOIN or SUBST a drive for which a directory on the drive is the target of a previous SUBST.";

                case (int)ErrorCode.ERROR_SYSTEM_TRACE:
                    return @"System trace information was not specified in CONFIG.SYS, or tracing is not allowed.";

                case (int)ErrorCode.ERROR_INVALID_EVENT_COUNT:
                    return @"The number of specified semaphore events is incorrect.";

                case (int)ErrorCode.ERROR_TOO_MANY_MUXWAITERS:
                    return @"DosMuxSemWait did not execute because too many semaphores are already set.";

                case (int)ErrorCode.ERROR_INVALID_LIST_FORMAT:
                    return @"The DosMuxSemWait list is incorrect.";

                case (int)ErrorCode.ERROR_LABEL_TOO_LONG:
                    return @"The volume label specified is too long and was truncated to 11 characters.";

                case (int)ErrorCode.ERROR_TOO_MANY_TCBS:
                    return @"The system cannot create another thread.";

                case (int)ErrorCode.ERROR_SIGNAL_REFUSED:
                    return @"The recipient process has refused the signal.";

                case (int)ErrorCode.ERROR_DISCARDED:
                    return @"The segment is already discarded and cannot be locked.";

                case (int)ErrorCode.ERROR_NOT_LOCKED:
                    return @"The segment is already unlocked.";

                case (int)ErrorCode.ERROR_BAD_THREADID_ADDR:
                    return @"The address for the thread ID is incorrect.";

                case (int)ErrorCode.ERROR_BAD_ARGUMENTS:
                    return @"The argument string is incorrect.";

                case (int)ErrorCode.ERROR_BAD_PATHNAME:
                    return @"The specified path is invalid.";

                case (int)ErrorCode.ERROR_SIGNAL_PENDING:
                    return @"A signal is already pending.";

                case (int)ErrorCode.ERROR_MAX_THRDS_REACHED:
                    return @"No more threads can be created in the system.";

                case (int)ErrorCode.ERROR_LOCK_FAILED:
                    return @"The system was unable to lock a region of a file.";

                case (int)ErrorCode.ERROR_BUSY:
                    return @"The requested resource is in use.";

                case (int)ErrorCode.ERROR_CANCEL_VIOLATION:
                    return @"A lock request was not outstanding for the supplied cancel region.";

                case (int)ErrorCode.ERROR_ATOMIC_LOCKS_NOT_SUPPORTED:
                    return @"The file system does not support atomic changes to the lock type.";

                case (int)ErrorCode.ERROR_INVALID_SEGMENT_NUMBER:
                    return @"The system detected an incorrect segment number.";

                case (int)ErrorCode.ERROR_INVALID_ORDINAL:
                    return @"The system cannot run something because of an invalid ordinal.";

                case (int)ErrorCode.ERROR_ALREADY_EXISTS:
                    return @"The system cannot create a file when it already exists.";

                case (int)ErrorCode.ERROR_INVALID_FLAG_NUMBER:
                    return @"An incorrect flag was passed.";

                case (int)ErrorCode.ERROR_SEM_NOT_FOUND:
                    return @"The specified system semaphore name was not found.";

                case (int)ErrorCode.ERROR_INVALID_STARTING_CODESEG:
                    return @"The system cannot run something because of an invalid starting code segment.";

                case (int)ErrorCode.ERROR_INVALID_STACKSEG:
                    return @"The system cannot run something because of an invalid stack segment.";

                case (int)ErrorCode.ERROR_INVALID_MODULETYPE:
                    return @"The system cannot run something because of an invalid module type.";

                case (int)ErrorCode.ERROR_INVALID_EXE_SIGNATURE:
                    return @"The system cannot run something because it cannot run under Windows NT/2000.";

                case (int)ErrorCode.ERROR_EXE_MARKED_INVALID:
                    return @"The system cannot run something because the EXE was marked as invalid.";

                case (int)ErrorCode.ERROR_BAD_EXE_FORMAT:
                    return @"The system cannot run something because it is an invalid Windows NT/2000 application.";

                case (int)ErrorCode.ERROR_ITERATED_DATA_EXCEEDS_64K:
                    return @"system cannot run something because the iterated data exceed 64KB.";

                case (int)ErrorCode.ERROR_INVALID_MINALLOCSIZE:
                    return @"The system cannot run something because of an invalid minimum allocation size.";

                case (int)ErrorCode.ERROR_DYNLINK_FROM_INVALID_RING:
                    return @"The system cannot run something because of a dynalink from an invalid ring.";

                case (int)ErrorCode.ERROR_IOPL_NOT_ENABLED:
                    return @"The system is not presently configured to run this application.";

                case (int)ErrorCode.ERROR_INVALID_SEGDPL:
                    return @"The system cannot run something because of an invalid segment DPL.";

                case (int)ErrorCode.ERROR_AUTODATASEG_EXCEEDS_64KB:
                    return @"The system cannot run something because the automatic data segment exceeds 64K.";

                case (int)ErrorCode.ERROR_RING2SEG_MUST_BE_MOVABLE:
                    return @"The code segment cannot be greater than or equal to 64KB.";

                case (int)ErrorCode.ERROR_RELOC_CHAIN_XEEDS_SEGLIM:
                    return @"The system cannot run something because the reallocation chain exceeds the segment limit.";

                case (int)ErrorCode.ERROR_INFLOOP_IN_RELOC_CHAIN:
                    return @"The system cannot run something because of an infinite loop in the reallocation chain.";

                case (int)ErrorCode.ERROR_ENVVAR_NOT_FOUND:
                    return @"The system could not find the specified environment variable.";

                case (int)ErrorCode.ERROR_NO_SIGNAL_SENT:
                    return @"No process in the command subtree has a signal handler.";

                case (int)ErrorCode.ERROR_FILENAME_EXCED_RANGE:
                    return @"The filename or extension is too long.";

                case (int)ErrorCode.ERROR_RING2_STACK_IN_USE:
                    return @"The ring 2 stack is busy.";

                case (int)ErrorCode.ERROR_META_EXPANSION_TOO_LONG:
                    return @"The global filename characters ( or ? ) are entered incorrectly, or too many global filename characters are specified.";

                case (int)ErrorCode.ERROR_INVALID_SIGNAL_NUMBER:
                    return @"The signal being posted is incorrect.";

                case (int)ErrorCode.ERROR_THREAD_1_INACTIVE:
                    return @"The signal handler cannot be set.";

                case (int)ErrorCode.ERROR_LOCKED:
                    return @"The segment is locked and cannot be reallocated.";

                case (int)ErrorCode.ERROR_TOO_MANY_MODULES:
                    return @"Too many dynamic link modules are attacked to this program or module.";

                case (int)ErrorCode.ERROR_NESTING_NOT_ALLOWED:
                    return @"Nestring of calls to LoadModule is not allowed.";

                case (int)ErrorCode.ERROR_BAD_PIPE:
                    return @"The pipe state is invalid.";

                case (int)ErrorCode.ERROR_PIPE_BUSY:
                    return @"All pipe instances are busy.";

                case (int)ErrorCode.ERROR_NO_DATA:
                    return @"The pipe is being closed.";

                case (int)ErrorCode.ERROR_PIPE_NOT_CONNECTED:
                    return @"No process exists on the other end of the pipe.";

                case (int)ErrorCode.ERROR_MORE_DATA:
                    return @"More data is available.";

                case (int)ErrorCode.ERROR_VC_DISCONNECTED:
                    return @"The session was cancelled.";

                case (int)ErrorCode.ERROR_INVALID_EA_NAME:
                    return @"The specified extended attribute name was invalid.";

                case (int)ErrorCode.ERROR_EA_LIST_INCONSISTENT:
                    return @"The extended attributes are inconsistent.";

                case (int)ErrorCode.ERROR_NO_MORE_ITEMS:
                    return @"No more data is available.";

                case (int)ErrorCode.ERROR_CANNOT_COPY:
                    return @"The Copy API cannot be used.";

                case (int)ErrorCode.ERROR_DIRECTORY:
                    return @"The directory name is invalid.";

                case (int)ErrorCode.ERROR_EAS_DIDNT_FIT:
                    return @"The extended attributes did not fit into the buffer.";

                case (int)ErrorCode.ERROR_EA_FILE_CORRUPT:
                    return @"The extended attribute file on the mounted file system is corrupt.";

                case (int)ErrorCode.ERROR_EA_TABLE_FULL:
                    return @"The extended attribute table file is full.";

                case (int)ErrorCode.ERROR_INVALID_EA_HANDLE:
                    return @"The specified extended attribute handle is invalid.";

                case (int)ErrorCode.ERROR_EAS_NOT_SUPPORTED:
                    return @"The mounted file system does not support extended attributes.";

                case (int)ErrorCode.ERROR_NOT_OWNER:
                    return @"The system attempted to release a mutex not owned by the caller.";

                case (int)ErrorCode.ERROR_TOO_MANY_POSTS:
                    return @"Too many posts were made to a semaphore.";

                case (int)ErrorCode.ERROR_MR_MID_NOT_FOUND:
                    return @"The system cannot find the message for the specified message number in the proper message file.";

                case (int)ErrorCode.ERROR_INVALID_ADDRESS:
                    return @"The system attempted to access an invalid address.";

                case (int)ErrorCode.ERROR_ARITHMETIC_OVERFLOW:
                    return @"An arithmetic overflow ( result > 32 bits ) occured.";

                case (int)ErrorCode.ERROR_PIPE_CONNECTED:
                    return @"A process already exists on the other end of the pipe.";

                case (int)ErrorCode.ERROR_PIPE_LISTENING:
                    return @"The system is waiting for a process to open on the other end of the pipe.";

                case (int)ErrorCode.ERROR_EA_ACCESS_DENIED:
                    return @"Access to the extended attribute was denied.";

                case (int)ErrorCode.ERROR_OPERATION_ABORTED:
                    return @"The I/O operation has been aborted because of either a thread exit or an application request.";

                case (int)ErrorCode.ERROR_IO_INCOMPLETE:
                    return @"The overlapped I/O event is not in a signalled state.";

                case (int)ErrorCode.ERROR_IO_PENDING:
                    return @"The overlapped I/O operation is in progress.";

                case (int)ErrorCode.ERROR_NOACCESS:
                    return @"An invalid access to a memory location was attempted.";

                case (int)ErrorCode.ERROR_SWAPERROR:
                    return @"An error performing an inpage operation occured.";

                case (int)ErrorCode.ERROR_STACK_OVERFLOW:
                    return @"The stack overflowed because recursion was too deep.";

                case (int)ErrorCode.ERROR_INVALID_MESSAGE:
                    return @"The window cannot act on the sent message.";

                case (int)ErrorCode.ERROR_CAN_NOT_COMPLETE:
                    return @"The function cannot be completed.";

                case (int)ErrorCode.ERROR_INVALID_FLAGS:
                    return @"Invalid flags were used.";

                case (int)ErrorCode.ERROR_UNRECOGNIZED_VOLUME:
                    return @"The disk volume does not contain a recognized file system.";

                case (int)ErrorCode.ERROR_FILE_INVALID:
                    return @"The disk volume for a file has been altered such that the opened file is no longer valid.";

                case (int)ErrorCode.ERROR_FULLSCREEN_MODE:
                    return @"The requested operation cannot be performed in full-screen mode.";

                case (int)ErrorCode.ERROR_NO_TOKEN:
                    return @"An attempt to reference a nonexistent token was made.";

                case (int)ErrorCode.ERROR_BADDB:
                    return @"The registry database is corrupt.";

                case (int)ErrorCode.ERROR_BADKEY:
                    return @"The registry key is invalid.";

                case (int)ErrorCode.ERROR_CANTOPEN:
                    return @"The registry key could not be opened.";

                case (int)ErrorCode.ERROR_CANTREAD:
                    return @"The registry key could not be read from.";

                case (int)ErrorCode.ERROR_CANTWRITE:
                    return @"The registry key could not be written to.";

                case (int)ErrorCode.ERROR_REGISTRY_RECOVERED:
                    return @"One of the registry database files was successfully recovered.";

                case (int)ErrorCode.ERROR_REGISTRY_CORRUPT:
                    return @"The registry is corrupt.The cause could be a corrupted registry database file, a corrupted image in system memory, or a failed attempt to recover the registry because of a missing or corrupted log.";

                case (int)ErrorCode.ERROR_REGISTRY_IO_FAILED:
                    return @"An I/O operation initiated by the registry failed unrecoverably.";

                case (int)ErrorCode.ERROR_NOT_REGISTRY_FILE:
                    return @"The system tried to load or restore a file into the registry, but that file is not in the proper file format.";

                case (int)ErrorCode.ERROR_KEY_DELETED:
                    return @"An illegal operation was attempted on a registry key marked for deletion.";

                case (int)ErrorCode.ERROR_NO_LOG_SPACE:
                    return @"The system could not allocate the required space in a registry log.";

                case (int)ErrorCode.ERROR_KEY_HAS_CHILDREN:
                    return @"A symbolic link in a registry key that already has subkeys or values cannot be created.";

                case (int)ErrorCode.ERROR_CHILD_MUST_BE_VOLATILE:
                    return @"A stable subkey cannot be created under a volatile parent key.";

                case (int)ErrorCode.ERROR_NOTIFY_ENUM_DIR:
                    return @"Because a notify change request is being completed and the information is not being returned in the caller's buffer, the caller must now enumerate the files to find the changes.";

                case (int)ErrorCode.ERROR_DEPENDENT_SERVICES_RUNNING:
                    return @"A stop control has been sent to a service which other running services are dependent on.";

                case (int)ErrorCode.ERROR_INVALID_SERVICE_CONTROL:
                    return @"The requested control is not valid for this service.";

                case (int)ErrorCode.ERROR_SERVICE_REQUEST_TIMEOUT:
                    return @"The service did not respond to the start or control request within the timeout period.";

                case (int)ErrorCode.ERROR_SERVICE_NO_THREAD:
                    return @"A thread could not be created for the service.";

                case (int)ErrorCode.ERROR_SERVICE_DATABASE_LOCKED:
                    return @"The service database is locked.";

                case (int)ErrorCode.ERROR_SERVICE_ALREADY_RUNNING:
                    return @"An instance of the service is already running.";

                case (int)ErrorCode.ERROR_INVALID_SERVICE_ACCOUNT:
                    return @"The account name for the service is invalid or nonexistent.";

                case (int)ErrorCode.ERROR_SERVICE_DISABLED:
                    return @"The specified service is disabled and cannot be started.";

                case (int)ErrorCode.ERROR_CIRCULAR_DEPENDENCY:
                    return @"A circular service dependency was specified.";

                case (int)ErrorCode.ERROR_SERVICE_DOES_NOT_EXIST:
                    return @"The specified service does not exist.";

                case (int)ErrorCode.ERROR_SERVICE_CANNOT_ACCEPT_CTRL:
                    return @"The service cannot accept control messages at this time.";

                case (int)ErrorCode.ERROR_SERVICE_NOT_ACTIVE:
                    return @"The service has not been started.";

                case (int)ErrorCode.ERROR_FAILED_SERVICE_CONTROLLER_CONNECT:
                    return @"The service process could not connect to the service controller.";

                case (int)ErrorCode.ERROR_EXCEPTION_IN_SERVICE:
                    return @"An exception occured in the service when handling the control request.";

                case (int)ErrorCode.ERROR_DATABASE_DOES_NOT_EXIST:
                    return @"The specified database does not exist.";

                case (int)ErrorCode.ERROR_SERVICE_SPECIFIC_ERROR:
                    return @"The service has returned a service-specific error code.";

                case (int)ErrorCode.ERROR_PROCESS_ABORTED:
                    return @"The process terminated unexpectedly.";

                case (int)ErrorCode.ERROR_SERVICE_DEPENDENCY_FAIL:
                    return @"The dependency service or group failed to start.";

                case (int)ErrorCode.ERROR_SERVICE_LOGON_FAILED:
                    return @"The service did not start due to a logon failure.";

                case (int)ErrorCode.ERROR_SERVICE_START_HANG:
                    return @"After starting, the service hung in a start-pending state.";

                case (int)ErrorCode.ERROR_INVALID_SERVICE_LOCK:
                    return @"The specified service database lock is invalid.";

                case (int)ErrorCode.ERROR_SERVICE_MARKED_FOR_DELETE:
                    return @"The specified service has been marked for deletion.";

                case (int)ErrorCode.ERROR_SERVICE_EXISTS:
                    return @"The specified service already exists.";

                case (int)ErrorCode.ERROR_ALREADY_RUNNING_LKG:
                    return @"The system is currently running with the last-known-good configuration.";

                case (int)ErrorCode.ERROR_SERVICE_DEPENDENCY_DELETED:
                    return @"The dependency service does not exist or has been marked for deletion.";

                case (int)ErrorCode.ERROR_BOOT_ALREADY_ACCEPTED:
                    return @"The current boot has already been accepted for use as the last-known-good control set.";

                case (int)ErrorCode.ERROR_SERVICE_NEVER_STARTED:
                    return @"No attempts to start the service have been made.";

                case (int)ErrorCode.ERROR_DUPLICATE_SERVICE_NAME:
                    return @"The name is already in use either as a service name or as a service display name.";

                case (int)ErrorCode.ERROR_END_OF_MEDIA:
                    return @"The physical end of the tape has been reached.";

                case (int)ErrorCode.ERROR_FILEMARK_DETECTED:
                    return @"A tape access reached a filemark.";

                case (int)ErrorCode.ERROR_BEGINNING_OF_MEDIA:
                    return @"The beginning of the tape or partition was encountered.";

                case (int)ErrorCode.ERROR_SETMARK_DETECTED:
                    return @"A tape access reached the end of a set of files.";

                case (int)ErrorCode.ERROR_NO_DATA_DETECTED:
                    return @"No more data is on the tape.";

                case (int)ErrorCode.ERROR_PARTITION_FAILURE:
                    return @"The tape could not be partitioned.";

                case (int)ErrorCode.ERROR_INVALID_BLOCK_LENGTH:
                    return @"When accessing a new tape of a multivolume partition, the current blocksize is incorrect.";

                case (int)ErrorCode.ERROR_DEVICE_NOT_PARTITIONED:
                    return @"The tape partition information could not be found.";

                case (int)ErrorCode.ERROR_UNABLE_TO_LOCK_MEDIA:
                    return @"The system was unable to lock the media eject mechanism.";

                case (int)ErrorCode.ERROR_UNABLE_TO_UNLOAD_MEDIA:
                    return @"The system was unable to unload the media.";

                case (int)ErrorCode.ERROR_MEDIA_CHANGED:
                    return @"The media in the drive may have changed.";

                case (int)ErrorCode.ERROR_BUS_RESET:
                    return @"The I/O bus was reset.";

                case (int)ErrorCode.ERROR_NO_MEDIA_IN_DRIVE:
                    return @"There is no media in the drive.";

                case (int)ErrorCode.ERROR_NO_UNICODE_TRANSLATION:
                    return @"No mapping for the Unicode character exists in the target multi-byte code page.";

                case (int)ErrorCode.ERROR_DLL_INIT_FAILED:
                    return @"A dynamic link library initialization routine failed.";

                case (int)ErrorCode.ERROR_SHUTDOWN_IN_PROGRESS:
                    return @"A system shutdown is in progress.";

                case (int)ErrorCode.ERROR_NO_SHUTDOWN_IN_PROGRESS:
                    return @"The system shutdown could not be aborted because no shutdown is in progress.";

                case (int)ErrorCode.ERROR_IO_DEVICE:
                    return @"The request could not be performed because of an device I/O error.";

                case (int)ErrorCode.ERROR_SERIAL_NO_DEVICE:
                    return @"No serial device was successfully initialized; the serial driver will therefore unload.";

                case (int)ErrorCode.ERROR_IRQ_BUSY:
                    return @"The system was unable to open a device sharing an interrupt request ( IRQ ) with other device( s ) because at least one of those devices is already opened.";

                case (int)ErrorCode.ERROR_MORE_WRITES:
                    return @"A serial I/O operation was completed by another write to the serial port.";

                case (int)ErrorCode.ERROR_COUNTER_TIMEOUT:
                    return @"A serial I/O operation completed because the time-out period expired.";

                case (int)ErrorCode.ERROR_FLOPPY_ID_MARK_NOT_FOUND:
                    return @"No ID address mark was found on the floppy disk.";

                case (int)ErrorCode.ERROR_FLOPPY_WRONG_CYLINDER:
                    return @"A mismatch exists between the floppy disk sector ID field and the floppy disk controller track address.";

                case (int)ErrorCode.ERROR_FLOPPY_UNKNOWN_ERROR:
                    return @"The floppy disk controller reported an unrecognized error.";

                case (int)ErrorCode.ERROR_FLOPPY_BAD_REGISTERS:
                    return @"The floppy disk controller returned inconsistent results in its registers.";

                case (int)ErrorCode.ERROR_DISK_RECALIBRATE_FAILED:
                    return @"While accessing a hard disk, a recalibrate operation failed, even after retries.";

                case (int)ErrorCode.ERROR_DISK_OPERATION_FAILED:
                    return @"While accessing a hard disk, a disk operation failed, even after retries.";

                case (int)ErrorCode.ERROR_DISK_RESET_FAILED:
                    return @"While accessing a hard disk, a disk controller reset was needed, but even that failed.";

                case (int)ErrorCode.ERROR_EOM_OVERFLOW:
                    return @"An EOM overflow occured.";

                case (int)ErrorCode.ERROR_NOT_ENOUGH_SERVER_MEMORY:
                    return @"Not enough server storage is available to process this command.";

                case (int)ErrorCode.ERROR_POSSIBLE_DEADLOCK:
                    return @"A potential deadlock condition has been detected.";

                case (int)ErrorCode.ERROR_MAPPED_ALIGNMENT:
                    return @"The base address or the file offset specified does not have the proper alignment.";


                #endregion
            }
            return "Unknown Error";
        }


        public enum ErrorCode : int
        {
            #region enum

            NO_ERROR = 0,
            ERROR_SUCCESS = 0,
            ERROR_INVALID_FUNCTION = 1,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_TOO_MANY_OPEN_FILES = 4,
            ERROR_ACCESS_DENIED = 5,
            ERROR_INVALID_HANDLE = 6,
            ERROR_ARENA_TRASHED = 7,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_INVALID_BLOCK = 9,
            ERROR_BAD_ENVIRONMENT = 10,
            ERROR_BAD_FORMAT = 11,
            ERROR_INVALID_ACCESS = 12,
            ERROR_INVALID_DATA = 13,
            ERROR_OUTOFMEMORY = 14,
            ERROR_INVALID_DRIVE = 15,
            ERROR_CURRENT_DIRECTORY = 16,
            ERROR_NOT_SAME_DEVICE = 17,
            ERROR_NO_MORE_FILES = 18,
            ERROR_WRITE_PROTECT = 19,
            ERROR_BAD_UNIT = 20,
            ERROR_NOT_READY = 21,
            ERROR_BAD_COMMAND = 22,
            ERROR_CRC = 23,
            ERROR_BAD_LENGTH = 24,
            ERROR_SEEK = 25,
            ERROR_NOT_DOS_DISK = 26,
            ERROR_SECTOR_NOT_FOUND = 27,
            ERROR_OUT_OF_PAPER = 28,
            ERROR_WRITE_FAULT = 29,
            ERROR_READ_FAULT = 30,
            ERROR_GEN_FAILURE = 31,
            ERROR_SHARING_VIOLATION = 32,
            ERROR_LOCK_VIOLATION = 33,
            ERROR_WRONG_DISK = 34,
            ERROR_SHARING_BUFFER_EXCEEDED = 36,
            ERROR_HANDLE_EOF = 38,
            ERROR_HANDLE_DISK_FULL = 39,
            ERROR_NOT_SUPPORTED = 50,
            ERROR_REM_NOT_LIST = 51,
            ERROR_DUP_NAME = 52,
            ERROR_BAD_NETPATH = 53,
            ERROR_NETWORK_BUSY = 54,
            ERROR_DEV_NOT_EXIST = 55,
            ERROR_TOO_MANY_CMDS = 56,
            ERROR_ADAP_HDW_ERR = 57,
            ERROR_BAD_NET_RESP = 58,
            ERROR_UNEXP_NET_ERR = 59,
            ERROR_BAD_REM_ADAP = 60,
            ERROR_PRINTQ_FULL = 61,
            ERROR_NO_SPOOL_SPACE = 62,
            ERROR_PRINT_CANCELLED = 63,
            ERROR_NETNAME_DELETED = 64,
            ERROR_NETWORK_ACCESS_DENIED = 65,
            ERROR_BAD_DEV_TYPE = 66,
            ERROR_BAD_NET_NAME = 67,
            ERROR_TOO_MANY_NAMES = 68,
            ERROR_TOO_MANY_SESS = 69,
            ERROR_SHARING_PAUSED = 70,
            ERROR_REQ_NOT_ACCEP = 71,
            ERROR_REDIR_PAUSED = 72,
            ERROR_FILE_EXISTS = 80,
            ERROR_CANNOT_MAKE = 82,
            ERROR_FAIL_I24 = 83,
            ERROR_OUT_OF_STRUCTURES = 84,
            ERROR_ALREADY_ASSIGNED = 85,
            ERROR_INVALID_PASSWORD = 86,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_NET_WRITE_FAULT = 88,
            ERROR_NO_PROC_SLOTS = 89,
            ERROR_TOO_MANY_SEMAPHORES = 100,
            ERROR_EXCL_SEM_ALREADY_OWNED = 101,
            ERROR_SEM_IS_SET = 102,
            ERROR_TOO_MANY_SEM_REQUESTS = 103,
            ERROR_INVALID_AT_INTERRUPT_TIME = 104,
            ERROR_SEM_OWNER_DIED = 105,
            ERROR_SEM_USER_LIMIT = 106,
            ERROR_DISK_CHANGE = 107,
            ERROR_DRIVE_LOCKED = 108,
            ERROR_BROKEN_PIPE = 109,
            ERROR_OPEN_FAILED = 110,
            ERROR_BUFFER_OVERFLOW = 111,
            ERROR_DISK_FULL = 112,
            ERROR_NO_MORE_SEARCH_HANDLES = 113,
            ERROR_INVALID_TARGET_HANDLE = 114,
            ERROR_INVALID_CATEGORY = 117,
            ERROR_INVALID_VERIFY_SWITCH = 118,
            ERROR_BAD_DRIVER_LEVEL = 119,
            ERROR_CALL_NOT_IMPLEMENTED = 120,
            ERROR_SEM_TIMEOUT = 121,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_INVALID_NAME = 123,
            ERROR_INVALID_LEVEL = 124,
            ERROR_NO_VOLUME_LABEL = 125,
            ERROR_MOD_NOT_FOUND = 126,
            ERROR_PROC_NOT_FOUND = 127,
            ERROR_WAIT_NO_CHILDREN = 128,
            ERROR_CHILD_NOT_COMPLETE = 129,
            ERROR_DIRECT_ACCESS_HANDLE = 130,
            ERROR_NEGATIVE_SEEK = 131,
            ERROR_SEEK_ON_DEVICE = 132,
            ERROR_IS_JOIN_TARGET = 133,
            ERROR_IS_JOINED = 134,
            ERROR_IS_SUBSTED = 135,
            ERROR_NOT_JOINED = 136,
            ERROR_NOT_SUBSTED = 137,
            ERROR_JOIN_TO_JOIN = 138,
            ERROR_SUBST_TO_SUBST = 139,
            ERROR_JOIN_TO_SUBST = 140,
            ERROR_SUBST_TO_JOIN = 141,
            ERROR_BUSY_DRIVE = 142,
            ERROR_SAME_DRIVE = 143,
            ERROR_DIR_NOT_ROOT = 144,
            ERROR_DIR_NOT_EMPTY = 145,
            ERROR_IS_SUBST_PATH = 146,
            ERROR_IS_JOIN_PATH = 147,
            ERROR_PATH_BUSY = 148,
            ERROR_IS_SUBST_TARGET = 149,
            ERROR_SYSTEM_TRACE = 150,
            ERROR_INVALID_EVENT_COUNT = 151,
            ERROR_TOO_MANY_MUXWAITERS = 152,
            ERROR_INVALID_LIST_FORMAT = 153,
            ERROR_LABEL_TOO_LONG = 154,
            ERROR_TOO_MANY_TCBS = 155,
            ERROR_SIGNAL_REFUSED = 156,
            ERROR_DISCARDED = 157,
            ERROR_NOT_LOCKED = 158,
            ERROR_BAD_THREADID_ADDR = 159,
            ERROR_BAD_ARGUMENTS = 160,
            ERROR_BAD_PATHNAME = 161,
            ERROR_SIGNAL_PENDING = 162,
            ERROR_MAX_THRDS_REACHED = 164,
            ERROR_LOCK_FAILED = 167,
            ERROR_BUSY = 170,
            ERROR_CANCEL_VIOLATION = 173,
            ERROR_ATOMIC_LOCKS_NOT_SUPPORTED = 174,
            ERROR_INVALID_SEGMENT_NUMBER = 180,
            ERROR_INVALID_ORDINAL = 182,
            ERROR_ALREADY_EXISTS = 183,
            ERROR_INVALID_FLAG_NUMBER = 186,
            ERROR_SEM_NOT_FOUND = 187,
            ERROR_INVALID_STARTING_CODESEG = 188,
            ERROR_INVALID_STACKSEG = 189,
            ERROR_INVALID_MODULETYPE = 190,
            ERROR_INVALID_EXE_SIGNATURE = 191,
            ERROR_EXE_MARKED_INVALID = 192,
            ERROR_BAD_EXE_FORMAT = 193,
            ERROR_ITERATED_DATA_EXCEEDS_64K = 194,
            ERROR_INVALID_MINALLOCSIZE = 195,
            ERROR_DYNLINK_FROM_INVALID_RING = 196,
            ERROR_IOPL_NOT_ENABLED = 197,
            ERROR_INVALID_SEGDPL = 198,
            ERROR_AUTODATASEG_EXCEEDS_64KB = 199,
            ERROR_RING2SEG_MUST_BE_MOVABLE = 200,
            ERROR_RELOC_CHAIN_XEEDS_SEGLIM = 201,
            ERROR_INFLOOP_IN_RELOC_CHAIN = 202,
            ERROR_ENVVAR_NOT_FOUND = 203,
            ERROR_NO_SIGNAL_SENT = 205,
            ERROR_FILENAME_EXCED_RANGE = 206,
            ERROR_RING2_STACK_IN_USE = 207,
            ERROR_META_EXPANSION_TOO_LONG = 208,
            ERROR_INVALID_SIGNAL_NUMBER = 209,
            ERROR_THREAD_1_INACTIVE = 210,
            ERROR_LOCKED = 212,
            ERROR_TOO_MANY_MODULES = 214,
            ERROR_NESTING_NOT_ALLOWED = 215,
            ERROR_BAD_PIPE = 230,
            ERROR_PIPE_BUSY = 231,
            ERROR_NO_DATA = 232,
            ERROR_PIPE_NOT_CONNECTED = 233,
            ERROR_MORE_DATA = 234,
            ERROR_VC_DISCONNECTED = 240,
            ERROR_INVALID_EA_NAME = 254,
            ERROR_EA_LIST_INCONSISTENT = 255,
            ERROR_NO_MORE_ITEMS = 259,
            ERROR_CANNOT_COPY = 266,
            ERROR_DIRECTORY = 267,
            ERROR_EAS_DIDNT_FIT = 275,
            ERROR_EA_FILE_CORRUPT = 276,
            ERROR_EA_TABLE_FULL = 277,
            ERROR_INVALID_EA_HANDLE = 278,
            ERROR_EAS_NOT_SUPPORTED = 282,
            ERROR_NOT_OWNER = 288,
            ERROR_TOO_MANY_POSTS = 298,
            ERROR_MR_MID_NOT_FOUND = 317,
            ERROR_INVALID_ADDRESS = 487,
            ERROR_ARITHMETIC_OVERFLOW = 534,
            ERROR_PIPE_CONNECTED = 535,
            ERROR_PIPE_LISTENING = 536,
            ERROR_EA_ACCESS_DENIED = 994,
            ERROR_OPERATION_ABORTED = 995,
            ERROR_IO_INCOMPLETE = 996,
            ERROR_IO_PENDING = 997,
            ERROR_NOACCESS = 998,
            ERROR_SWAPERROR = 999,
            ERROR_STACK_OVERFLOW = 1001,
            ERROR_INVALID_MESSAGE = 1002,
            ERROR_CAN_NOT_COMPLETE = 1003,
            ERROR_INVALID_FLAGS = 1004,
            ERROR_UNRECOGNIZED_VOLUME = 1005,
            ERROR_FILE_INVALID = 1006,
            ERROR_FULLSCREEN_MODE = 1007,
            ERROR_NO_TOKEN = 1008,
            ERROR_BADDB = 1009,
            ERROR_BADKEY = 1010,
            ERROR_CANTOPEN = 1011,
            ERROR_CANTREAD = 1012,
            ERROR_CANTWRITE = 1013,
            ERROR_REGISTRY_RECOVERED = 1014,
            ERROR_REGISTRY_CORRUPT = 1015,
            ERROR_REGISTRY_IO_FAILED = 1016,
            ERROR_NOT_REGISTRY_FILE = 1017,
            ERROR_KEY_DELETED = 1018,
            ERROR_NO_LOG_SPACE = 1019,
            ERROR_KEY_HAS_CHILDREN = 1020,
            ERROR_CHILD_MUST_BE_VOLATILE = 1021,
            ERROR_NOTIFY_ENUM_DIR = 1022,
            ERROR_DEPENDENT_SERVICES_RUNNING = 1051,
            ERROR_INVALID_SERVICE_CONTROL = 1052,
            ERROR_SERVICE_REQUEST_TIMEOUT = 1053,
            ERROR_SERVICE_NO_THREAD = 1054,
            ERROR_SERVICE_DATABASE_LOCKED = 1055,
            ERROR_SERVICE_ALREADY_RUNNING = 1056,
            ERROR_INVALID_SERVICE_ACCOUNT = 1057,
            ERROR_SERVICE_DISABLED = 1058,
            ERROR_CIRCULAR_DEPENDENCY = 1059,
            ERROR_SERVICE_DOES_NOT_EXIST = 1060,
            ERROR_SERVICE_CANNOT_ACCEPT_CTRL = 1061,
            ERROR_SERVICE_NOT_ACTIVE = 1062,
            ERROR_FAILED_SERVICE_CONTROLLER_CONNECT = 1063,
            ERROR_EXCEPTION_IN_SERVICE = 1064,
            ERROR_DATABASE_DOES_NOT_EXIST = 1065,
            ERROR_SERVICE_SPECIFIC_ERROR = 1066,
            ERROR_PROCESS_ABORTED = 1067,
            ERROR_SERVICE_DEPENDENCY_FAIL = 1068,
            ERROR_SERVICE_LOGON_FAILED = 1069,
            ERROR_SERVICE_START_HANG = 1070,
            ERROR_INVALID_SERVICE_LOCK = 1071,
            ERROR_SERVICE_MARKED_FOR_DELETE = 1072,
            ERROR_SERVICE_EXISTS = 1073,
            ERROR_ALREADY_RUNNING_LKG = 1074,
            ERROR_SERVICE_DEPENDENCY_DELETED = 1075,
            ERROR_BOOT_ALREADY_ACCEPTED = 1076,
            ERROR_SERVICE_NEVER_STARTED = 1077,
            ERROR_DUPLICATE_SERVICE_NAME = 1078,
            ERROR_END_OF_MEDIA = 1100,
            ERROR_FILEMARK_DETECTED = 1101,
            ERROR_BEGINNING_OF_MEDIA = 1102,
            ERROR_SETMARK_DETECTED = 1103,
            ERROR_NO_DATA_DETECTED = 1104,
            ERROR_PARTITION_FAILURE = 1105,
            ERROR_INVALID_BLOCK_LENGTH = 1106,
            ERROR_DEVICE_NOT_PARTITIONED = 1107,
            ERROR_UNABLE_TO_LOCK_MEDIA = 1108,
            ERROR_UNABLE_TO_UNLOAD_MEDIA = 1109,
            ERROR_MEDIA_CHANGED = 1110,
            ERROR_BUS_RESET = 1111,
            ERROR_NO_MEDIA_IN_DRIVE = 1112,
            ERROR_NO_UNICODE_TRANSLATION = 1113,
            ERROR_DLL_INIT_FAILED = 1114,
            ERROR_SHUTDOWN_IN_PROGRESS = 1115,
            ERROR_NO_SHUTDOWN_IN_PROGRESS = 1116,
            ERROR_IO_DEVICE = 1117,
            ERROR_SERIAL_NO_DEVICE = 1118,
            ERROR_IRQ_BUSY = 1119,
            ERROR_MORE_WRITES = 1120,
            ERROR_COUNTER_TIMEOUT = 1121,
            ERROR_FLOPPY_ID_MARK_NOT_FOUND = 1122,
            ERROR_FLOPPY_WRONG_CYLINDER = 1123,
            ERROR_FLOPPY_UNKNOWN_ERROR = 1124,
            ERROR_FLOPPY_BAD_REGISTERS = 1125,
            ERROR_DISK_RECALIBRATE_FAILED = 1126,
            ERROR_DISK_OPERATION_FAILED = 1127,
            ERROR_DISK_RESET_FAILED = 1128,
            ERROR_EOM_OVERFLOW = 1129,
            ERROR_NOT_ENOUGH_SERVER_MEMORY = 1130,
            ERROR_POSSIBLE_DEADLOCK = 1131,
            ERROR_MAPPED_ALIGNMENT = 1132
            #endregion
        }
    }
    #endregion

}
