//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        sergey_stoyan@yahoo.com
//        http://www.cliversoft.com
//        26 September 2006
//Copyright: (C) 2006, Sergey Stoyan
//********************************************************************************************
using System.IO;
using System.Security.AccessControl;

namespace Cliver.Win
{
    abstract public class AppSettings : Settings
    {
        /// <summary>
        /// By defult Environment.SpecialFolder.CommonApplicationData does not have writting permission
        /// </summary>
        /// <param name="userIdentityName"></param>
        public static void AllowReadWriteConfig(string userIdentityName = null)
        {
            if (userIdentityName == null)
                userIdentityName = UserRoutines.GetCurrentUserName3();
            DirectoryInfo di = new DirectoryInfo(Cliver.AppSettings.StorageDir);
            DirectorySecurity ds = di.GetAccessControl();
            ds.AddAccessRule(new FileSystemAccessRule(
                userIdentityName,
                FileSystemRights.Modify,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));
            di.SetAccessControl(ds);
        }
    }
}