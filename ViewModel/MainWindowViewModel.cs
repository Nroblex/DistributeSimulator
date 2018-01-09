using GitUtilSimulate.Model;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace GitUtilSimulate.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private string mExternalFtp;
        private string mExternalFtpUser;
        //private string mExternalFtpPass;
        private string mBuildNo;
        private string mBuildPath;
        

        private ICommand sendGitUtilCommand;

        private bool canExecute = true;

        private Action<object, Exception> callBack;

        public MainWindowViewModel()
        {
            SendGitUtilCommand = new DelegateCommand(DoSendGitJSON, param => this.canExecute);
        }

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


        public ICommand SendGitUtilCommand
        {
            get
            {
                //return new DelegateCommand(DoSendGitJSON);
                //return sendGitUtilCommand;
                /*
                sendGitUtilCommand= new  DelegateCommand(p =>
                {
                    var passwordBox = p as PasswordBox;
                    var password = passwordBox.Password;
                }, p=> {
                    var passwordBox = p as PasswordBox;

                    return !String.IsNullOrEmpty(passwordBox.Password);
                });
                */
                return sendGitUtilCommand;
            }
            set
            {
                sendGitUtilCommand = value;
            }
        }

        private void DoSendGitJSON(object obj)
        {
            GitUtilCommand command = new GitUtilCommand
            {
                BuildNumber = mBuildNo,
                PathToBuild = mBuildPath,
                CreatedAt = DateTime.Now,
                ExternalFTPPassword = (obj as PasswordBox).Password,
                ExternalFTPPath = mExternalFtp,
                ExternalFTPUser = mExternalFtpUser
            };

            

            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(GitUtilCommand));
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
