using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace eApi.Setup
{
    class setup
    {
        public bool portsAreReady = false;
        private DateTime setupTimestamp;
        private DateTime lastTry = DateTime.Now;

        public void start()
        {

            if (!(File.Exists("C:\\Program Files (x86)\\com0com\\setupc.exe") && File.Exists("C:\\Program Files (x86)\\com0com\\setupCom0Com.bat")))
            {
                copyCom0Com();

                while (portsAreReady == false)
                {
                    connectPorts();
                }
            }
            else
            {
                if (doAllPortsExist() == false)
                {
                    while (portsAreReady == false)
                    {
                        connectPorts();
                    }
                }
                else
                {
                    portsAreReady = true;
                }
            }
        }

        private void connectPorts()
        {
            // if setup is finished look if the ports exist. If they don't exist, create them

            if (System.Diagnostics.Process.GetProcessesByName("setup_com0com_signed").Length == 0 && DateTime.Now.Subtract(setupTimestamp) > new TimeSpan(0,0,0,0,100))
            {
                lastTry = DateTime.Now;
                if (setupTimestamp == null)
                {
                    setupTimestamp = DateTime.Now;
                }
                else if (DateTime.Now.Subtract(setupTimestamp) > new TimeSpan(0, 0, 0, 2, 0))
                {
                    if (doAllPortsExist())
                    {
                        // stop
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        proc.StartInfo.FileName = "C:\\Program Files (x86)\\com0com\\configCom0Com.bat";  
                        proc.Start();

                        DateTime waitStart = DateTime.Now;
                        while (DateTime.Now.Subtract(waitStart) < new TimeSpan(0, 0, 0, 2, 0)) ;
                        hideCOMAPI();

                        portsAreReady = true;
                    }
                    else
                    {
                        initPorts();
                    }
                }
            }
        }


        private void copyCom0Com()
        {
            Directory.CreateDirectory("C:\\Program Files (x86)\\com0com");
            if (!File.Exists("C:\\Program Files (x86)\\com0com\\setupc.exe") && System.Diagnostics.Process.GetProcessesByName("setup_com0com_signed").Length == 0)
            {
                // copy com0com setup into temp directory
                string path = Path.Combine(Path.GetTempPath(), "com0com");
                Directory.CreateDirectory(path);
                File.WriteAllBytes(Path.Combine(path, "setup.exe"), Properties.Resources.setup_com0com_signed);

                // run setup
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = Path.Combine(path, "setup.exe");
                proc.StartInfo.Arguments = "/S";
                Environment.SetEnvironmentVariable("CNC_INSTALL_START_MENU_SHORTCUTS", "NO");
                Environment.SetEnvironmentVariable("CNC_INSTALL_CNCA0_CNCB0_PORTS", "NO");
                Environment.SetEnvironmentVariable("CNC_INSTALL_COMX_COMX_PORTS", "NO");
                Environment.SetEnvironmentVariable("CNC_INSTALL_SKIP_SETUP_PREINSTALL", "NO");
                proc.Start();
            }

            if (!File.Exists("C:\\Program Files (x86)\\com0com\\configCom0Com.bat"))
                File.WriteAllText("C:\\Program Files (x86)\\com0com\\configCom0Com.bat", Properties.Resources.configCom0Com);

            if (!File.Exists("C:\\Program Files (x86)\\com0com\\setupCom0Com.bat"))
                File.WriteAllText("C:\\Program Files (x86)\\com0com\\setupCom0Com.bat", Properties.Resources.setupCom0Com);

            if (!File.Exists("C:\\Program Files (x86)\\com0com\\delCom0Com.bat"))
                File.WriteAllText("C:\\Program Files (x86)\\com0com\\delCom0Com.bat", Properties.Resources.delCom0Com);
        }

        private bool configRunFlag = false;
        private void initPorts()
        {
            // config Com0Com
            if (configRunFlag == false)
            {
                if (!(File.Exists("C:\\Program Files (x86)\\com0com\\configCom0Com.bat")) && Directory.Exists("C:\\Program Files (x86)\\com0com"))
                    File.WriteAllText("C:\\Program Files (x86)\\com0com\\configCom0Com.bat", Properties.Resources.configCom0Com);

                if (!(File.Exists("C:\\Program Files (x86)\\com0com\\setupCom0Com.bat")) && Directory.Exists("C:\\Program Files (x86)\\com0com"))
                    File.WriteAllText("C:\\Program Files (x86)\\com0com\\setupCom0Com.bat", Properties.Resources.setupCom0Com);

                if (!(File.Exists("C:\\Program Files (x86)\\com0com\\delCom0Com.bat")) && Directory.Exists("C:\\Program Files (x86)\\com0com"))
                    File.WriteAllText("C:\\Program Files (x86)\\com0com\\delCom0Com.bat", Properties.Resources.delCom0Com);

                configRunFlag = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "C:\\Program Files (x86)\\com0com\\setupCom0Com.bat";
                proc.Start();
            }
        }

        public bool doAllPortsExist()
        {
            // Count every port which contains portBaseName
            string[] sArr = System.IO.Ports.SerialPort.GetPortNames();
            int portCounter = 0;

            SerialPort sp = new SerialPort();
            sp.BaudRate = 115200;
            for (uint i = 2; i < 7; i++)
            {
                try
                {
                    sp.PortName = "COMAN" + i.ToString();
                    sp.Open();
                    sp.Close();
                    portCounter++;
                }
                catch { }
            }
            try
            {
                sp.PortName = "COMAPI";
                sp.Open();
                sp.Close();
                portCounter++;
            }
            catch { }

            if (portCounter == 6)
                return true;
            else
                return false;
        }

        public static void showCOMAPI()
        {
            string path = Path.Combine(Path.GetTempPath(), "com0com");
            Directory.CreateDirectory(path);
            File.WriteAllText("C:\\Program Files (x86)\\com0com\\showCom0Com.bat", Properties.Resources.showCOMAPI);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Program Files (x86)\\com0com\\showCom0Com.bat";
            proc.Start();

            DateTime wait = DateTime.Now;
            while (DateTime.Now.Subtract(wait) > new TimeSpan(0, 0, 0, 0, 50)) ;
        }

        public static void hideCOMAPI()
        {
            string path = Path.Combine(Path.GetTempPath(), "com0com");
            Directory.CreateDirectory(path);
            File.WriteAllText("C:\\Program Files (x86)\\com0com\\hideCom0Com.bat", Properties.Resources.hideCOMAPI);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "C:\\Program Files (x86)\\com0com\\hideCom0Com.bat";
            proc.Start();

            DateTime wait = DateTime.Now;
            while (DateTime.Now.Subtract(wait) > new TimeSpan(0, 0, 0, 0, 50)) ;
        }

        public void Close()
        {
            portsAreReady = false;

            // delete Com0Com ports
            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = "C:\\Program Files (x86)\\com0com\\delCom0Com.bat";
            //proc.Start();
        }
    }
}
