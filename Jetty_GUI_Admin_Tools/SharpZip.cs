using System;
using System.Linq;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Diagnostics;
using Microsoft.Win32;

namespace Jetty_GUI_Admin_Tools
{
    public class SharpZip
    {

        #region 压缩多个文件

        /// <summary>  
        ///  压缩多个文件  
        /// </summary>  
        /// <param name="files">文件名</param>  
        /// <param name="ZipedFileName">压缩包文件名</param>  
        /// <param name="Password">解压码</param>  
        /// <returns></returns>  
        public static void Zip(string[] files, string ZipedFileName, string Password)
        {
            files = files.Where(f => File.Exists(f)).ToArray();
            if (files.Length == 0) throw new FileNotFoundException("未找到指定打包的文件");
            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFileName));
            s.SetLevel(6);
            if (!string.IsNullOrEmpty(Password.Trim())) s.Password = Password.Trim();
            ZipFileDictory(files, s);
            s.Finish();
            s.Close();
        }

        /// <summary>  
        ///  压缩多个文件  
        /// </summary>  
        /// <param name="files">文件名</param>  
        /// <param name="ZipedFileName">压缩包文件名</param>  
        /// <returns></returns>  
        public static void Zip(string[] files, string ZipedFileName)
        {
            Zip(files, ZipedFileName, string.Empty);
        }

        private static void ZipFileDictory(string[] files, ZipOutputStream s)
        {
            ZipEntry entry = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();
            try
            {
                //创建当前文件夹  
                entry = new ZipEntry("/");  //加上 “/” 才会当成是文件夹创建  
                s.PutNextEntry(entry);
                s.Flush();
                foreach (string file in files)
                {
                    //打开压缩文件  
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    entry = new ZipEntry("/" + Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);
                    s.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (entry != null)
                    entry = null;
                GC.Collect();
            }
        }

        #endregion 压缩多个文件


        #region  解压文件 包括.rar 和zip

        /// <summary>
        ///解压文件
        /// </summary>
        /// <param name="fileFromUnZip">解压前的文件路径（绝对路径）</param>
        /// <param name="fileToUnZip">解压后的文件目录（绝对路径）</param>
        public static void UnpackFileRarOrZip(string fileFromUnZip, string fileToUnZip)
        {
            //获取压缩类型
            string unType = fileFromUnZip.Substring(fileFromUnZip.LastIndexOf(".") + 1, 3).ToLower();

            switch (unType)
            {
                case "rar":
                    UnRar(fileFromUnZip, fileToUnZip);
                    break;
                default:
                    UnZip(fileFromUnZip, fileToUnZip);
                    break;

            }
        }


        #endregion



        #region  解压文件 .rar文件

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="unRarPatch"></param>
        /// <param name="rarPatch"></param>
        /// <param name="rarName"></param>
        /// <returns></returns>
        public static void UnRar(string fileFromUnZip, string fileToUnZip)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;

            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                //the_rar = the_rar.Substring(1, the_rar.Length - 7);

                if (Directory.Exists(fileToUnZip) == false)
                {
                    Directory.CreateDirectory(fileToUnZip);
                }
                the_Info = "x " + Path.GetFileName(fileFromUnZip) + " " + fileToUnZip + " -y";

                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = Path.GetDirectoryName(fileFromUnZip);//获取压缩包路径

                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return unRarPatch;
        }

        #endregion



        #region  解压文件 .zip文件

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">指定解压目标目录</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }

            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }

            ICSharpCode.SharpZipLib.Zip.ZipInputStream s = null;
            ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry = null;

            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(FileToUpZip));
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹

                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }


        #endregion
    }
}