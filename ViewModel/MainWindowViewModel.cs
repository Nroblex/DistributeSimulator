using GitUtilSimulate.Model;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using IOPath = System.IO.Path;

namespace GitUtilSimulate.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private string mExternalFtp;
        private string mExternalFtpUser;
        private string mBuildNo;
        private string mBuildPath;

        private string selectedPath;
        
        private ICommand sendGitUtilCommand;
        private ICommand browseForFolder;

        private bool canExecute = true;

        private Action<object, Exception> callBack;

        public MainWindowViewModel()
        {
            SendGitUtilCommand = new DelegateCommand(DoSendGitJSON, param => this.canExecute);
            BrowseForFolder = new DelegateCommand(DoBrowseForFolder, param => canExecute);
        }

        private void DoBrowseForFolder(object obj)
        {
            string s = ";";
        }

        public string SelectedPath
        {
            get => selectedPath;
            private set
            {
                selectedPath = value;
            }
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

        public ICommand BrowseForFolder
        {
            get { return browseForFolder; }
            set { browseForFolder = value; }
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

        private void DoBrowseForFolder()
        {
            String x = "a";
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
