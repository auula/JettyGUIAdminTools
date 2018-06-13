using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jetty_GUI_Admin_Tools
{
    class utils
    {
        /// <summary>
        /// 根据路径删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                File.Delete(path);
            }
        }
        /// <summary>
        /// 写出jetty资源文件
        /// </summary>
        public static void outfile() {
            byte[] temp = Properties.Resources.jetty;
            System.IO.FileStream fileStream = new System.IO.FileStream(@"C:\\jetty.zip", System.IO.FileMode.CreateNew);
            fileStream.Write(temp, 0, (int)(temp.Length));
            fileStream.Close();

            string path = @"C:\\jetty";
            if (Directory.Exists(path))
            {
                //Console.WriteLine("此文件夹已经存在，无需创建！");
            }
            else
            {
                Directory.CreateDirectory(path);
                //Console.WriteLine(path + " 创建成功!");
            }

            SharpZip.UnZip(@"C:\\jetty.zip", @"C:\\jetty");

            utils.DeleteFile(@"C:\\jetty.zip");
            Set_OS_Path.SetSysEnvironment("Jetty_PATH", "C:\\jetty\\jetty");
        }

        /// <summary>
        /// 杀掉进程
        /// </summary>
        /// <param name="P_id">JETTY进度</param>

        public static void kill_ID(string P_id)
        {
            Process stopProcess = new Process();
            stopProcess.StartInfo.FileName = @"cmd";
            stopProcess.StartInfo.Arguments = "/c \"TASKKILL /F /PID " + P_id + " /T\"";
            stopProcess.StartInfo.UseShellExecute = false;
            stopProcess.StartInfo.CreateNoWindow = true;
            stopProcess.StartInfo.RedirectStandardOutput = true;
            stopProcess.StartInfo.RedirectStandardError = true;
            stopProcess.Start();
            stopProcess.BeginOutputReadLine();
            stopProcess.BeginErrorReadLine();
        }
        /// <summary>
        /// 部署war使用的函数
        /// </summary>
        /// <param name="warpath">war路径</param>
        /// <param name="filename">war名字</param>
        public static bool deploy_war(string warpath,string filename)
        {
            bool flag = false;
            string pLocalFilePath = warpath;//要复制的文件路径
            string pSaveFilePath = @"C:\\jetty\\jetty\\webapps\\"+filename+".war";//指定存储的路径
            if (File.Exists(pLocalFilePath))//必须判断要复制的文件是否存在
            {
                File.Copy(pLocalFilePath, pSaveFilePath, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                //其实我们可以实现热部署
                flag = true;
            }
            return flag;
        }


        /// <summary>
        /// 复制文件夹中的所有内容
        /// </summary>
        /// <param name="sourceDirPath">源文件夹目录</param>
        /// <param name="saveDirPath">指定文件夹目录</param>
        public void CopyDirectory(string sourceDirPath, string saveDirPath)
        {
            try
            {
                if (!Directory.Exists(saveDirPath))
                {
                    Directory.CreateDirectory(saveDirPath);
                }
                string[] files = Directory.GetFiles(sourceDirPath);
                foreach (string file in files)
                {
                    string pFilePath = saveDirPath + "\\" + Path.GetFileName(file);
                    if (File.Exists(pFilePath))
                        continue;
                    File.Copy(file, pFilePath, true);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, saveDirPath + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                new Exception();
            }
        }
        /// <summary>
        /// 拷贝oldlab的文件到newlab下面
        /// </summary>
        /// <param name="sourcePath">lab文件所在目录(@"~\labs\oldlab")</param>
        /// <param name="savePath">保存的目标目录(@"~\labs\newlab")</param>
        /// <returns>返回:true-拷贝成功;false:拷贝失败</returns>
        public bool CopyOldLabFilesToNewLab(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #region //拷贝labs文件夹到savePath下
            try
            {
                string[] labDirs = Directory.GetDirectories(sourcePath);//目录
                string[] labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    for (int i = 0; i < labFiles.Length; i++)
                    {
                        if (Path.GetFileName(labFiles[i]) != ".lab")//排除.lab文件
                        {
                            File.Copy(sourcePath + "\\" + Path.GetFileName(labFiles[i]), savePath + "\\" + Path.GetFileName(labFiles[i]), true);
                        }
                    }
                }
                if (labDirs.Length > 0)
                {
                    for (int j = 0; j < labDirs.Length; j++)
                    {
                        Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(labDirs[j]));

                        //递归调用
                        CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(labDirs[j]), savePath + "\\" + Path.GetFileName(labDirs[j]));
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion
            return true;
        }
    }
}
