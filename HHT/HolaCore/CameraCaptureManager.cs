using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsMobile.Forms;
using System.IO;

namespace HolaCore
{
    public static class CameraCaptureManager
    {
          private static CameraCaptureDialog cameraCapture = new CameraCaptureDialog();
        /// <summary>
        /// 获取拍摄对象
        /// </summary>
        public static CameraCaptureDialog CameraCapture
        {
            get { return CameraCaptureManager.cameraCapture; }
        }

        public static string FileDirectory
        {
            get
            {
                string dir = @"\Program Files\hhtiii\photos";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        static CameraCaptureManager()
        {
            cameraCapture.InitialDirectory = FileDirectory;
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public static string FileName
        {
            get
            {
                return cameraCapture.FileName;
            }
        }

        /// <summary>
        /// 拍摄
        /// </summary>
        /// <returns></returns>
        public static bool Shoot()
        {
            try
            {
                //cameraCapture.DefaultFileName = Guid.NewGuid().ToString();

                // Displays the "Camera Capture" dialog box

                //cameraCapture.Mode = CameraCaptureMode.Still;                  //拍摄方式
                cameraCapture.StillQuality = CameraCaptureStillQuality.High;     //图片质量
                cameraCapture.Resolution = new System.Drawing.Size(1248,832);  	 //图片大小

                if (cameraCapture.ShowDialog() == DialogResult.OK)
                {
                    //获取文件路径
                    // If it is a video we rename the file so that it has the user entered
                    // default filename and the correct extension.

                    return true;
                }
                else
                {
                    return false;   
                }
            }
            catch (ArgumentException ex)
            {
                // An invalid argument was specified.
                MessageBox.Show(ex.Message, "拍摄出错", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            catch (OutOfMemoryException ex)
            {
                // There is not enough memory to save the image or video.
                MessageBox.Show(ex.Message, "拍摄出错", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                // An unknown error occurred.
                MessageBox.Show(ex.Message, "拍摄出错", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return false;
            }
        }

    }
}
