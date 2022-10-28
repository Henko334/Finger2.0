using libzkfpcsharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;

namespace Finger
{
    public partial class Enroll : Form
    {
        Form1 form1;

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
        private int mfpWidth = 0;
        private int mfpHeight = 0;
        int RegisterCount = 0;
        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        public Enroll()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void FingerPrintControl_Load(object sender, EventArgs e) { FormHandle = this.Handle; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("No User Id Entered");
                return;
            }
            if (!IsRegister)
            {
                iFid = Convert.ToInt32(textBox1.Text);
                IsRegister = true;
                RegisterCount = 0;
                regTempLen = 0;
                ((Form1)this.ParentForm).LabelText = "Please press your finger 3 times!";
                ((Form1)this.ParentForm).LabelColor(Color.LightBlue);
            }
        }

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
                       // SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                    }
                    Thread.Sleep(100);
                }
            }
            catch { }

        }

       // public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        DisplayFingerPrintImage();
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
                                    ((Form1)this.ParentForm).LabelText = "This finger is already registerd with id " + fid;
                                    ((Form1)this.ParentForm).LabelColor(Color.Tomato);
                                    return;
                                }
                            }
                            if (RegisterCount > 0 && fpr.Match(CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                            {
                                ((Form1)this.ParentForm).LabelText = "Please press the same finger " + REGISTER_FINGER_COUNT + " times for enrollment";
                                ((Form1)this.ParentForm).LabelColor(Color.LightBlue);
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

                                        ((Form1)this.ParentForm).LabelText = "You have successfully enrolled the user";
                                        ((Form1)this.ParentForm).LabelColor(Color.Lime);
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
                                        ((Form1)this.ParentForm).LabelText = "Failed to add the users template " + ret;
                                        ((Form1)this.ParentForm).LabelColor(Color.Tomato);
                                    }
                                }
                                else
                                {
                                    ((Form1)this.ParentForm).LabelText = "Unable to enroll the current user. " + ret;
                                    ((Form1)this.ParentForm).LabelColor(Color.Tomato);
                                }

                                IsRegister = false;
                                return;
                            }
                            else
                            {
                                int remainingCont = REGISTER_FINGER_COUNT - RegisterCount;
                                ((Form1)this.ParentForm).LabelText = "Please provide your fingerprint " + remainingCont + " more time(s)";
                                ((Form1)this.ParentForm).LabelColor(Color.LightBlue);
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

        private int GenerateRegisteredFingerPrint()
        {
            return fpr.GenerateRegTemplate(RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref regTempLen);
        }

        private int AddTemplateToMemory()
        {
            return fpr.AddRegTemplate(iFid, RegTmp);
        }

        private void DisplayFingerPrintImage()
        {
            MemoryStream ms = new MemoryStream();
            GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
            WriteBitmap(FPBuffer, mfpWidth, mfpHeight);
            Bitmap bmp = new Bitmap(ms);

            Image img = bmp;
            img.Save(@"D:\finger.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            FileInfo img1 = new FileInfo(@"D:\finger.bmp");
            // string finger = ;
        }

        public struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public int bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public int bfOffBits;
        }

        public struct MASK
        {
            public byte redmask;
            public byte greenmask;
            public byte bluemask;
            public byte rgbReserved;
        }

        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        public static void RotatePic(byte[] BmpBuf, int width, int height, ref byte[] ResBuf)
        {
            int RowLoop = 0;
            int ColLoop = 0;
            int BmpBuflen = width * height;

            try
            {
                for (RowLoop = 0; RowLoop < BmpBuflen;)
                {
                    for (ColLoop = 0; ColLoop < width; ColLoop++)
                    {
                        ResBuf[RowLoop + ColLoop] = BmpBuf[BmpBuflen - RowLoop - width + ColLoop];
                    }

                    RowLoop = RowLoop + width;
                }
            }
            catch (Exception ex)
            {
                //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                //logger.Append();
            }
        }

        public static byte[] StructToBytes(object StructObj, int Size)
        {
            int StructSize = Marshal.SizeOf(StructObj);
            byte[] GetBytes = new byte[StructSize];

            try
            {
                IntPtr StructPtr = Marshal.AllocHGlobal(StructSize);
                Marshal.StructureToPtr(StructObj, StructPtr, false);
                Marshal.Copy(StructPtr, GetBytes, 0, StructSize);
                Marshal.FreeHGlobal(StructPtr);

                if (Size == 14)
                {
                    byte[] NewBytes = new byte[Size];
                    int Count = 0;
                    int Loop = 0;

                    for (Loop = 0; Loop < StructSize; Loop++)
                    {
                        if (Loop != 2 && Loop != 3)
                        {
                            NewBytes[Count] = GetBytes[Loop];
                            Count++;
                        }
                    }

                    return NewBytes;
                }
                else
                {
                    return GetBytes;
                }
            }
            catch (Exception ex)
            {
                //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                //logger.Append();

                return GetBytes;
            }
        }

        public static void GetBitmap(byte[] buffer, int nWidth, int nHeight, ref MemoryStream ms)
        {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight * 2];

            try
            {
                BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
                BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
                MASK[] ColorMask = new MASK[m_nColorTableEntries];

                int w = (((nWidth + 3) / 4) * 4);

                //Í¼Æ¬Í·ÐÅÏ¢
                BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
                BmpInfoHeader.biWidth = nWidth;
                BmpInfoHeader.biHeight = nHeight;
                BmpInfoHeader.biPlanes = 1;
                BmpInfoHeader.biBitCount = m_nBitCount;
                BmpInfoHeader.biCompression = 0;
                BmpInfoHeader.biSizeImage = 0;
                BmpInfoHeader.biXPelsPerMeter = 0;
                BmpInfoHeader.biYPelsPerMeter = 0;
                BmpInfoHeader.biClrUsed = m_nColorTableEntries;
                BmpInfoHeader.biClrImportant = m_nColorTableEntries;

                //ÎÄ¼þÍ·ÐÅÏ¢
                BmpHeader.bfType = 0x4D42;
                BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
                BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
                BmpHeader.bfReserved1 = 0;
                BmpHeader.bfReserved2 = 0;

                ms.Write(StructToBytes(BmpHeader, 14), 0, 14);
                ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader));

                //µ÷ÊÔ°åÐÅÏ¢
                for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
                {
                    ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                    ColorMask[ColorIndex].rgbReserved = 0;

                    ms.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])), 0, Marshal.SizeOf(ColorMask[ColorIndex]));
                }

                //Í¼Æ¬Ðý×ª£¬½â¾öÖ¸ÎÆÍ¼Æ¬µ¹Á¢µÄÎÊÌâ
                RotatePic(buffer, nWidth, nHeight, ref ResBuf);

                byte[] filter = null;
                if (w - nWidth > 0)
                {
                    filter = new byte[w - nWidth];
                }
                for (int i = 0; i < nHeight; i++)
                {
                    ms.Write(ResBuf, i * nWidth, nWidth);
                    if (w - nWidth > 0)
                    {
                        ms.Write(ResBuf, 0, w - nWidth);
                    }
                }
            }
            catch (Exception ex)
            {
                // ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                // logger.Append();
            }
        }

        public static void WriteBitmap(byte[] buffer, int nWidth, int nHeight)
        {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight];

            try
            {

                BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
                BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
                MASK[] ColorMask = new MASK[m_nColorTableEntries];
                int w = (((nWidth + 3) / 4) * 4);
                //Í¼Æ¬Í·ÐÅÏ¢
                BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
                BmpInfoHeader.biWidth = nWidth;
                BmpInfoHeader.biHeight = nHeight;
                BmpInfoHeader.biPlanes = 1;
                BmpInfoHeader.biBitCount = m_nBitCount;
                BmpInfoHeader.biCompression = 0;
                BmpInfoHeader.biSizeImage = 0;
                BmpInfoHeader.biXPelsPerMeter = 0;
                BmpInfoHeader.biYPelsPerMeter = 0;
                BmpInfoHeader.biClrUsed = m_nColorTableEntries;
                BmpInfoHeader.biClrImportant = m_nColorTableEntries;

                //ÎÄ¼þÍ·ÐÅÏ¢
                BmpHeader.bfType = 0x4D42;
                BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
                BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
                BmpHeader.bfReserved1 = 0;
                BmpHeader.bfReserved2 = 0;

                Stream FileStream = File.Open("finger.bmp", FileMode.Create, FileAccess.Write);
                BinaryWriter TmpBinaryWriter = new BinaryWriter(FileStream);

                TmpBinaryWriter.Write(StructToBytes(BmpHeader, 14));
                TmpBinaryWriter.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)));

                //µ÷ÊÔ°åÐÅÏ¢
                for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
                {
                    ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                    ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                    ColorMask[ColorIndex].rgbReserved = 0;

                    TmpBinaryWriter.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])));
                }

                //Í¼Æ¬Ðý×ª£¬½â¾öÖ¸ÎÆÍ¼Æ¬µ¹Á¢µÄÎÊÌâ
                RotatePic(buffer, nWidth, nHeight, ref ResBuf);

                //Ð´Í¼Æ¬
                //TmpBinaryWriter.Write(ResBuf);
                byte[] filter = null;
                if (w - nWidth > 0)
                {
                    filter = new byte[w - nWidth];
                }
                for (int i = 0; i < nHeight; i++)
                {
                    TmpBinaryWriter.Write(ResBuf, i * nWidth, nWidth);
                    if (w - nWidth > 0)
                    {
                        TmpBinaryWriter.Write(ResBuf, 0, w - nWidth);
                    }
                }

                FileStream.Close();
                TmpBinaryWriter.Close();
            }
            catch (Exception ex)
            {
                //ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
                //logger.Append();
            }
        }
    }
}
