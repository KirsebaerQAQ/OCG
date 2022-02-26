using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using static WinFormsApp1.Interface1;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        string DeviceID;
        int quantity;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("CG.dll"))//check devcon
            {
                File.WriteAllBytes("CG.dll", Resource1.CG);//output devcon
            }
            
            
            string DisplayName = "";
            ManagementClass m = new ManagementClass("Win32_VideoController");
            ManagementObjectCollection mn = m.GetInstances();
            DisplayName = "显卡数量：" + mn.Count.ToString() + "  ";
            quantity = (int)mn.Count;//number of device
            if (quantity == 1)
            {
                MessageBox.Show("Because you only have one graphics card, You cannot use this program", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Check that you have " + quantity.ToString() + " core graphics card that may contain AMD or Intel", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_VideoController");//Win32_VideoController 显卡
            int count = 0;
            foreach (ManagementObject mo in mos.Get())
            {
                count++;
                //DisplayName += "第" + count.ToString() + "张显卡名称：" + mo["Name"].ToString() + "   ";
                if (mo["Name"].ToString().IndexOf("AMD Radeon(TM) Graphics") != 0 && mo["Name"].ToString().IndexOf("INTEL") != 0)
                {
                    comboBox1.Items.Add(mo["Name"].ToString());
                    comboBox1.SelectedIndex = 0;
                    DeviceID = mo["PNPDeviceID"].ToString();
                    DeviceStatus();
                }

            }
            //Console.WriteLine(DisplayName);
            mn.Dispose();
            m.Dispose();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            enable();
            Delay(3000);
            DeviceStatus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            disable();
            Delay(3000);
            DeviceStatus();
        }
        void disable()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cg.dll";
            processStartInfo.Arguments = "disable @" + "\"" + DeviceID + "\"";
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden; looks like it doesn't work in .NET 6 or Winform
            processStartInfo.CreateNoWindow = true;
            Process.Start(processStartInfo);
        }

        void enable()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cg.dll";
            processStartInfo.Arguments = "enable @" + "\"" + DeviceID + "\"";
            processStartInfo.CreateNoWindow = true;
            //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden; looks like it doesn't work in .NET 6 or Winform
            Process.Start(processStartInfo);
        }

        void DeviceStatus()
        {
            var dpk = new DEVPROPKEY();
            dpk.fmtid = new Guid("60b193cb-5276-4d0f-96fc-f173abad3ec6");
            dpk.pid = 2;

            var displayDevClass = new Guid(DisplayAdapter.ToString());
            var hDevInfo = SetupDiGetClassDevs(ref displayDevClass, null, IntPtr.Zero, DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE);
            if (hDevInfo != INVALID_HANDLE_VALUE)
            {
                uint i = 0;
                uint status;
                uint probl;

                var did = new SP_DEVINFO_DATA();
                did.cbSize = (uint)Marshal.SizeOf(did);
                SetupDiEnumDeviceInfo(hDevInfo, i, ref did);


                CM_Get_DevNode_Status(out status, out probl, did.devInst, 0);
                if (status == 25174026)// check device status,if not this number than disbale status
                {
                    if (quantity == 1)
                    {
                        button1.Enabled = false;
                        button2.Enabled = false;
                    }
                    else
                    {
                        button1.Enabled = false;
                        button2.Enabled = true;
                    }
                }
                else
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                }
            }
        }

        public static void Delay(int milliSecond)//no stuck delay
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }



        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShellAbout(this.Handle, "OCG", "Only Core Graphics \n" + "By:Kirsebaer kirsebaer.cn Version:1.0 C# version",this.Icon.Handle);
        }       
    }
}