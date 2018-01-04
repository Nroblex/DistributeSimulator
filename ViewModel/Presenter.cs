using GitUtilSimulate.Model;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Input;

namespace GitUtilSimulate.ViewModel
{
    public class Presenter : ObservableObject
    {
        private string mExternalFtp;
        private string mExternalFtpUser;
        private string mExternalFtpPass;
        private string mBuildNo;
        private string mBuildPath;

        private Action<object, Exception> callBack;

        public string ExternalFtp
        {
            get { return mExternalFtp; }
            set
            {
                mExternalFtp = value;
                RaisePropertyChanged("ExternalFtp");
            }
        }

        public string ExternalFtpUser
        {
            get { return mExternalFtpUser; }
            set
            {
                mExternalFtpUser = value;
                RaisePropertyChanged("ExternalFtpUser");
            }
        }

        public string ExternalFtpPassWord
        {
            get { return mExternalFtpPass; }
            set
            {
                mExternalFtpPass = value;
                RaisePropertyChanged("ExternalFtpPassword");
            }
        }

        public string BuildNo
        {
            get { return mBuildNo; }
            set
            {
                mBuildNo = value;
                RaisePropertyChanged("BuildNo");
            }
        }

        public string BuildPath
        {
            get { return mBuildPath; }
            set
            {
                mBuildPath= value;
                RaisePropertyChanged("BuildPath");
            }
        }


        public ICommand SendGitUtil
        {
            get
            {
                return new DelegateCommand(DoSendGitJSON);
            }
        }

        private void DoSendGitJSON()
        {
            GitUtilCommnad command = new GitUtilCommnad
            {
                BuildNumber = mBuildNo,
                PathToBuild = mBuildPath,
                CreatedAt = DateTime.Now,
                ExternalFTPPassword = mExternalFtpPass,
                ExternalFTPPath = mExternalFtp,
                ExternalFTPUser = mExternalFtpUser
            };

            

            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(GitUtilCommnad));
            MemoryStream memstream = new MemoryStream();
            serializer.WriteObject(memstream, command);

            string json = Encoding.UTF8.GetString(memstream.ToArray(), 0, (int)memstream.Length);

            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Encoding = Encoding.UTF8;
            //webClient.UploadString("http://localhost:8081/Distribute/UpgradeSoftware", "POST", json);
            string guid = Guid.NewGuid().ToString();
            //webClient.UploadStringAsync(new Uri("http://localhost:8081/Distribute/UpgradeSoftware"), "POST", json, guid);

            webClient.UploadStringCompleted += WebClient_UploadStringCompleted;
            webClient.UploadStringAsync(new Uri("http://localhost:8081/Distribute/UpgradeSoftware"), "POST", json, guid);

        }

        private void WebClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            object recievedFromServer = e.UserState;
        }
    }
}
