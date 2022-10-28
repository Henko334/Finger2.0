using libzkfpcsharp;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace Finger
{
    public partial class Form1 : Form
    {
        #region --------Variables---------
        Thread captureThread = null;
        const int REGISTER_FINGER_COUNT = 3;
        zkfp fpr = new zkfp();
        IntPtr FormHandle = IntPtr.Zero;

        bool bIsTimeToDie = false;
        bool IsRegister = false;
        bool bIdentify = true;

        public byte[][] RegTmps = new byte[REGISTER_FINGER_COUNT][];
        public byte[] CapTmp = new byte[2048];
        public byte[] RegTmp = new byte[2048];
        public byte[] FPBuffer;
        int cbCapTmp = 2048;
        int regTempLen = 0;
        int iFid = 1;
        int ucnt = 0;
        int timer = 0;
        private int mfpWidth = 0;
        private int mfpHeight = 0;
        int RegisterCount = 0;
        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;
        Enroll enroll = new Enroll();
        Bitmap bmp;
        #endregion

        public Form1()
        {
            InitializeComponent();
            InitDevice();
            timer1.Interval = 5000;
            this.WindowState = FormWindowState.Maximized;
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            TimerClear();
        }

        public void InitDevice()
        {
            int callBackCode = fpr.Initialize();
            if (zkfp.ZKFP_ERR_OK == callBackCode)
            {
                int nCount = fpr.GetDeviceCount();
                if (nCount == 0)
                {
                    label1.Text = "Uable to connect with the device!";
                    label1.BackColor = Color.FromArgb(231, 76, 60);
                    return;
                }
                int openDeviceCallBackCode = fpr.OpenDevice(0);
                if (zkfp.ZKFP_ERR_OK != openDeviceCallBackCode)
                {
                    label1.Text = "Uable to connect with the device!";
                    label1.BackColor = Color.FromArgb(231, 76, 60);
                    return;
                }

                RegisterCount = 0;
                regTempLen = 0;
                iFid = 1;

                for (int i = 0; i < REGISTER_FINGER_COUNT; i++)
                {
                    RegTmps[i] = new byte[2048];
                }
                byte[] paramValue = new byte[4];
                int size = 4;

                fpr.GetParameters(1, paramValue, ref size);
                zkfp2.ByteArray2Int(paramValue, ref mfpWidth);
                size = 4;
                fpr.GetParameters(2, paramValue, ref size);
                zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

                FPBuffer = new byte[mfpWidth * mfpHeight];
                captureThread = new Thread(new ThreadStart(DoCapture));
                captureThread.IsBackground = true;
                captureThread.Start();
                bIsTimeToDie = false;
                string devSN = fpr.devSn;
                label1.Text = "Device Connected ";
                label1.BackColor = Color.FromArgb(46,204,113);
            }
            else
            {
                label1.Text = "Could Not Connect to Device";
                label1.BackColor = Color.FromArgb(231, 76, 60);
            }

            StreamReader sr = new StreamReader(@"D:\Fingers.txt");
            int x = 0;
            string finger;
            while (((finger = sr.ReadLine()) != null))
            {
                if ((finger != null) && (finger != ""))
                {
                    if (x < 3)
                    {
                        RegTmps[x] = Convert.FromBase64String(finger);
                    }
                    if (x == 3)
                    {
                        x = 0;
                        iFid = Convert.ToInt32(finger);
                        GenerateRegisteredFingerPrint();
                        AddTemplateToMemory();
                        ucnt += 1;
                    }
                    else
                    {
                        x++;
                    }
                }
            }
            sr.Close();
            if (zkfp.ZKFP_ERR_OK == callBackCode)
            {
                label1.Text = "Device Connected: " + ucnt + " Users Loaded";
            }
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private void DoCapture()
        {
            try
            {
                while (!bIsTimeToDie)
                {
                    cbCapTmp = 2048;
                    int ret = fpr.AcquireFingerprint(FPBuffer, CapTmp, ref cbCapTmp);

                    if (ret == zkfp.ZKFP_ERR_OK)
                    {
                        //if (RegisterCount == 0)
                        //    btnEnroll.Invoke((Action)delegate
                        //    {
                        //        btnEnroll.Enabled = true;
                        //    });
                        SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                    }
                    Thread.Sleep(100);
                }
            }
            catch { }

        }

        private void FingerPrintControl_Load(object sender, EventArgs e) { FormHandle = this.Handle; }

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        DisplayFinger displayFinger = new DisplayFinger(mfpWidth, mfpHeight, FPBuffer);
                        this.picFPImg.Image = displayFinger.DisplayFingerPrintImage();
                        timer1.Dispose();
                        timer1.Start();
                        timer1.Tick += new EventHandler(OnTimedEvent);
                        if (IsRegister)
                        {
                            #region -------- IF REGISTERED FINGERPRINT --------

                            int ret = zkfp.ZKFP_ERR_OK;
                            int fid = 0, score = 0;
                            ret = fpr.Identify(CapTmp, ref fid, ref score);
                            if (zkfp.ZKFP_ERR_OK == ret)
                            {
                                int deleteCode = fpr.DelRegTemplate(fid);   // <---- REMOVE FINGERPRINT
                                if (deleteCode != zkfp.ZKFP_ERR_OK)
                                {
                                    label1.Text = "This finger is already registerd with id ";
                                    label1.BackColor = Color.Tomato;
                                    return;
                                }
                            }
                            if (RegisterCount > 0 && fpr.Match(CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                label1.Text = "Please press the same finger " + REGISTER_FINGER_COUNT + " times for enrollment";
                                return;
                            }
                            Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);

                            RegisterCount++;
                            if (RegisterCount >= REGISTER_FINGER_COUNT)
                            {

                                RegisterCount = 0;
                                ret = GenerateRegisteredFingerPrint();   // <--- GENERATE FINGERPRINT TEMPLATE
                                //string reg = "";
                                //reg = Convert.ToBase64String(RegTmp);
                                //StreamWriter sw = new StreamWriter(@"D:\Fingers.txt", true);
                                //sw.WriteLine(reg + Environment.NewLine);
                                //sw.Close();

                                if (zkfp.ZKFP_ERR_OK == ret)
                                {

                                    ret = AddTemplateToMemory();        //  <--- LOAD TEMPLATE TO MEMORY
                                    if (zkfp.ZKFP_ERR_OK == ret)         // <--- ENROLL SUCCESSFULL
                                    {
                                        string fingerPrintTemplate = string.Empty;
                                        zkfp.Blob2Base64String(RegTmp, regTempLen, ref fingerPrintTemplate);

                                        label1.Text = "You have successfully enrolled the user";
                                        label1.BackColor = Color.FromArgb(46, 204, 113);
                                        string img1 = Convert.ToBase64String(RegTmps[0]);
                                        string img2 = Convert.ToBase64String(RegTmps[1]);
                                        string img3 = Convert.ToBase64String(RegTmps[2]);
                                        StreamWriter sw = new StreamWriter(@"D:\Fingers.txt", true);
                                        sw.WriteLine(img1);
                                        sw.WriteLine(img2);
                                        sw.WriteLine(img3);
                                        sw.WriteLine(iFid);
                                        sw.WriteLine(Environment.NewLine);
                                        sw.Close();
                                    }
                                    else
                                    {
                                        label1.Text = "Failed to add the users template " + ret;
                                        label1.BackColor = Color.FromArgb(231, 76, 60);
                                    }
                                }
                                else
                                {
                                    label1.Text = "Unable to enroll the current user. " + ret;
                                    label1.BackColor = Color.FromArgb(231, 76, 60);
                                }

                                IsRegister = false;
                                return;
                            }
                            else
                            {
                                int remainingCont = REGISTER_FINGER_COUNT - RegisterCount;
                                label1.Text = "Please provide your fingerprint " + remainingCont + " more time(s)";
                                label1.BackColor = Color.FromArgb(41, 128, 185);
                            }
                            #endregion
                        }
                        else
                        {
                            #region ------- IF RANDOM FINGERPRINT -------
                            // If unidentified random fingerprint is applied

                            if (regTempLen <= 0)
                            {
                                label1.Text = "Un-identified fingerprint. Please enroll to register.";
                                label1.BackColor = Color.FromArgb(231, 76, 60);
                                return;
                            }

                            if (bIdentify)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = fpr.Identify(CapTmp, ref fid, ref score);
                                if (zkfp.ZKFP_ERR_OK == ret)
                                {
                                    label1.Text = "User Validated. Score: " + score + "ID: " + fid;
                                    label1.BackColor = Color.FromArgb(46, 204, 113);
                                    return;
                                }
                                else
                                {
                                    label1.Text = "Identification Failed. Score: " + ret;
                                    label1.BackColor = Color.FromArgb(231, 76, 60);
                                    return;
                                }
                            }
                            else
                            {
                                int ret = fpr.Match(CapTmp, RegTmp);
                                if (0 < ret)
                                {
                                    label1.Text = "Match Successfull. Score: " + ret;
                                    label1.BackColor = Color.FromArgb(231, 76, 60);
                                    return;
                                }
                                else
                                {
                                    label1.Text = "Match Failed. Score: " + ret;
                                    label1.BackColor = Color.FromArgb(231, 76, 60);
                                    return;
                                }
                            }
                            #endregion
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            OnDisconnect();
        }

        public void OnDisconnect()
        {
            bIsTimeToDie = true;
            RegisterCount = 0;
            ClearImage();
            Thread.Sleep(1000);
            int result = fpr.CloseDevice();

            if (fpr.GetDeviceCount() > 0)
            {
                captureThread.Abort();
                if (result == zkfp.ZKFP_ERR_OK)
                {
                    Thread.Sleep(1000);
                    result = fpr.Finalize();   // CLEAR RESOURCES

                    if (result == zkfp.ZKFP_ERR_OK)
                    {
                        regTempLen = 0;
                        IsRegister = false;

                        Cursor = Cursors.Default;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                        label1.Text = "Error Disconnecting the Device " + iFid;
                        label1.BackColor = Color.FromArgb(231, 76, 60);
                    }
                }
                label1.Text = "Device Disconnected";
                label1.BackColor = Color.FromArgb(231, 76, 60);
            }
            Cursor = Cursors.Default;
        }

        private void ClearImage()
        {
            picFPImg.Image = null;
            //pbxImage2.Image = null;
        }

        private void TimerClear()
        {
            timer1.Dispose();
            if (fpr.GetDeviceCount() > 0)
            {
                ClearImage();
                label1.Text = "Device Connected";
                label1.BackColor = Color.FromArgb(46, 204, 113);
            }
            else
            {
                label1.Text = " No Device Connected";
                label1.BackColor = Color.FromArgb(231, 76, 60);
            }
            timer1.Start();
        }



        private int GenerateRegisteredFingerPrint()
        {
            return fpr.GenerateRegTemplate(RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref regTempLen);
        }
        private int AddTemplateToMemory()
        {
            return fpr.AddRegTemplate(iFid, RegTmp);
        }

        


 
        #region ------MenuStrip Items-------

        public void enrollEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Enroll")
                {
                    isOpen = true;
                    f.Focus();
                    break;
                }
            }
            if (isOpen == false)
            {
                enroll.MdiParent = this;
                enroll.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnDisconnect();
            Application.Exit();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool openchild = false;

            if (!MdiChildren.Any())
            {
                openchild = true;
            }
            else
                this.ActiveMdiChild.Close();
        }

        public string LabelText
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public void LabelColor(Color color)
        {
            label1.BackColor = color;
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            employeesToolStripMenuItem.BackColor = Color.FromArgb(38, 111, 153);
            employeesToolStripMenuItem.ForeColor = Color.Black;
        }

        private void employeesToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            employeesToolStripMenuItem.BackColor = Color.FromArgb(38, 111, 153);
            employeesToolStripMenuItem.ForeColor = Color.White;
        }
        #endregion
    }
}
