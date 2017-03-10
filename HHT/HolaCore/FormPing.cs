using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
namespace HolaCore
{
    public partial class FormPing : Form
    {
        public FormPing()
        {
            InitializeComponent();
            doLayout();
        }
        Regex reg = new Regex(@"^\d+$");
        private int TaskBarHeight = 0;

        private void doLayout()
        {
            int srcWidth = 240, srcHeight = 320;
            int dstWidth = Screen.PrimaryScreen.Bounds.Width, dstHeight = Screen.PrimaryScreen.Bounds.Height;
            TaskBarHeight = dstHeight - Screen.PrimaryScreen.WorkingArea.Height;
            float xTime = dstWidth / srcWidth, yTime = dstHeight / srcHeight;
            dstHeight -= TaskBarHeight;
            try
            {
                SuspendLayout();
                Goback.Top = dstHeight - Goback.Height;
                test.Top = dstHeight - test.Height;
                ResumeLayout(false);
            }
            catch (Exception)
            {
            }
        }

        private List<string> MileSeconds;

        //Ping服务器次数
        private int times = 20;

        //Ping丢失次数
        private int miss = 0;

        private delegate void InvokeDelegate();

        private void FormPing_Load(object sender, EventArgs e)
        {
            try
            {
                MileSeconds =new List<string>();
                IPServer.Text = Config.IPServer;
                IPLocal.Text = Config.IPLocal;
               
            }
            catch (Exception)
            {
            }
        }

        private void PingcallBack(string MileSecond)
        {
            this.Invoke(new InvokeDelegate(() =>
                {
                    times--;
                    IPLocal.Text = Config.IPLocal;
                   
                    if (MileSecond.Equals("miss"))
                    {
                        miss++;
                        MileSeconds.Add("1000");
                    }
                    else
                    {
                        MileSeconds.Add(MileSecond);
                    }
                    if (times > 0)
                    {
                        show.Text = "网络监测中……" + (times / 2).ToString();
                    }
                     if (times <= 0)
                     {
                         int result=getAveMileSecond(MileSeconds);
                         int missPer=miss * 5;
                         if ( missPer< 20)
                         {
                             PingMS.ForeColor = System.Drawing.Color.Green;
                             PingMS.Text = "网络良好！";
                         }
                         else if (missPer >= 20 && missPer<40)
                         {
                             PingMS.ForeColor = System.Drawing.Color.Red;
                             PingMS.Text = "网络较差！";
                         }
                         else if (missPer >= 40 && missPer < 60)
                         {
                             PingMS.ForeColor = System.Drawing.Color.Red;
                             PingMS.Text = "网络差！";
                         }
                         else if (missPer >= 60 && missPer < 80)
                         {
                             PingMS.ForeColor = System.Drawing.Color.Red;
                             PingMS.Text = "网络很差！";
                         }
                         else if(missPer >= 80)
                         {
                             PingMS.ForeColor = System.Drawing.Color.Red;
                             PingMS.Text = "网络不通！";
                         }
                         lbMiss.Text = "丢失:"+(miss * 5).ToString() + "%";
                         test.Enabled = true;
                         times = 20;
                         Borting();
                         MileSeconds.Clear();
                         miss = 0;
                     }
                     
                }));
        }

        private int getAveMileSecond(List<string> ss)
        {
            int result = 0;
            try
            {
                for (int i=0; i < ss.Count; i++)
                {
                    if (reg.IsMatch(ss[i]))
                    {
                        result += int.Parse(ss[i]);
                    }
                }
                result = result / ss.Count;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }
            return result;
        }

        private void Goback_Click(object sender, EventArgs e)
        {
            try
            {
                Borting();
                Close();
            }
            catch (Exception)
            {
            }
        }

        private void test_Click(object sender, EventArgs e)
        {
            try
            {
                Start = true;
                BeginPing(Config.IPServer);
                test.Enabled = false;
                show.Text = "网络监测中……"+(times/2).ToString();
                PingMS.Text = "";
                lbMiss.Text = "";
            }
            catch (Exception)
            {
            }
        }

        #region Ping APIs
        [DllImport("iphlpapi.dll")]
        private static extern Int32 IcmpSendEcho(IntPtr icmpHandle, Int32 destinationAddress, String requestData, Int32 requestSize, ref ICMP_OPTIONS requestOptions, ref ICMP_ECHO_REPLY replyBuffer, Int32 replySize, Int32 timeout);
        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern IntPtr IcmpCreateFile();
        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern bool IcmpCloseHandle(IntPtr handle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct ICMP_OPTIONS
        {
            public Byte Ttl;
            public Byte Tos;
            public Byte Flags;
            public Byte OptionsSize;
            public IntPtr OptionsData;
        };
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct ICMP_ECHO_REPLY
        {
            public int Address;
            public int Status;
            public int RoundTripTime;
            public Int16 DataSize;
            public Int16 Reserved;
            public IntPtr DataPtr;
            public ICMP_OPTIONS Options;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 250)]
            public String Data;
        }

        private  Thread th = null;
        private bool Start = true;
        private IPAddress IP;
        private void Ping()
        {
            try
            {
                while (Start)
                {
                    IntPtr ICMPHandle;
                    String sData;
                    ICMP_OPTIONS oICMPOptions = new ICMP_OPTIONS();
                    ICMP_ECHO_REPLY ICMPReply = new ICMP_ECHO_REPLY();
                    Int32 iReplies;
                    ICMPHandle = IcmpCreateFile();
                    Int32 iIP = BitConverter.ToInt32(IP.GetAddressBytes(), 0);
                    sData = "x";
                    oICMPOptions.Ttl = 255;

                    iReplies = IcmpSendEcho(ICMPHandle, iIP,
                        sData, sData.Length, ref oICMPOptions, ref ICMPReply,
                        Marshal.SizeOf(ICMPReply), 30); IcmpCloseHandle(ICMPHandle);

                    //状态为0表示连通
                    if (ICMPReply.Status == 0)
                    {
                        PingcallBack(ICMPReply.RoundTripTime.ToString());
                    }
                    else
                    {
                        PingcallBack("miss");
                    }


                    Thread.Sleep(500);
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {

            }
        }

        private void BeginPing(string ip)
        {
            try
            {
                IPAddress ipa = IPAddress.Parse(ip);
                IP = ipa;
                th = new Thread(new ThreadStart(Ping));
                th.Start();

            }
            catch (Exception)
            {
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Borting()
        {
            try
            {
                Start = false;
                th.Abort();
                th = null;
            }
            catch (Exception)
            {
                GC.Collect();
            }
        }

        #endregion

    }
}