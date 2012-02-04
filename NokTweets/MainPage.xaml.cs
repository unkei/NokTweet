using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.Phone.Tasks;

namespace NokTweets
{
    public partial class MainPage : PhoneApplicationPage
    {
		private string TwitterId = "Nokia";
        private SyndicationFeed myTweet = null;
        private SyndicationFeed othersTweet = null;
        bool isDownloading_myTweet = false;
        bool isDownloading_othersTweet = false;
		
        public MainPage()
        {
            InitializeComponent();
            updateTwitterId();
			GetTweets(TwitterId);
            GetTweetsAboutMe(TwitterId);
            piv.LoadingPivotItem += new EventHandler<PivotItemEventArgs>(piv_LoadingPivotItem);
        }

        void piv_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            setContext();
        }

        void setContext()
        {
            if (myTweet != null)
                List1.ItemsSource = myTweet.Items;
            if (othersTweet != null)
                List2.ItemsSource = othersTweet.Items;
        }

        void setDownloading()
        {
            if (isDownloading_myTweet == false && isDownloading_othersTweet == false)
            {
                progressBar1.IsIndeterminate = false;
                piv.Opacity = 1.0f;
            }
            else
            {
                progressBar1.IsIndeterminate = true;
                piv.Opacity = 0.3f;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (updateTwitterId() == true)
            {
                GetTweets(TwitterId);
                GetTweetsAboutMe(TwitterId);
            }

        }

        private bool updateTwitterId()
        {
            Settings settings = new Settings();
            bool isUpdated = false;

            if (TwitterId != settings.TwitterIdSetting)
            {
                TwitterId = settings.TwitterIdSetting;
                piv.Title = "@" + TwitterId;
                pi1.Header = TwitterId + " tweets";
                isUpdated = true;
            }

            return isUpdated;
        }
    
        private void GetTweets(string strId)
	    {
			Uri u = new Uri(String.Format(
				"http://twitter.com/statuses/user_timeline/{0}.atom", strId), UriKind.Absolute);
			WebClient cli = new WebClient();
            cli.DownloadStringCompleted += new DownloadStringCompletedEventHandler(cli_DownloadStringCompleted_myTweet);
			cli.DownloadStringAsync(u);
            isDownloading_myTweet = true;
            setDownloading();
        }

        void cli_DownloadStringCompleted_myTweet(object sender, DownloadStringCompletedEventArgs e)
        {
            isDownloading_myTweet = false;
            setDownloading();

            if (e.Error != null)
            {
                if (e.Error.Message.Contains("NotFound") == false)
                {
                    MessageBox.Show("通信エラーが発生しました。\r\n" + e.Error.Message);
                }
                return;
            }

            StringReader sr = new StringReader(e.Result);
            XmlReader xr = XmlReader.Create(sr);
            SyndicationFeed sf = SyndicationFeed.Load(xr);
            myTweet = sf;
            setContext();
        }

        void cli_DownloadStringCompleted_othersTweet(object sender, DownloadStringCompletedEventArgs e)
        {
            isDownloading_othersTweet = false;
            setDownloading();
            if (e.Error != null)
            {
                MessageBox.Show("通信エラーが発生しました。\r\n" + e.Error.Message);
                return;
            }
            StringReader sr = new StringReader(e.Result);
            XmlReader xr = XmlReader.Create(sr);
            SyndicationFeed sf = SyndicationFeed.Load(xr);
            othersTweet = sf;
            setContext();
        }
        private void menuSearch_Click(object sender, System.EventArgs e)
		{
			GetTweets(TwitterId);
			// TODO: ここにイベント ハンドラーのコードを追加します。
		}

		private void menuTweetsAboutMe_Click(object sender, System.EventArgs e)
		{
			GetTweetsAboutMe(TwitterId);
			// TODO: ここにイベント ハンドラーのコードを追加します。
		}
		
		private void GetTweetsAboutMe(string strId)
		{
			Uri u = new Uri("http://search.twitter.com/search.atom?rpp=50&q=" + HttpUtility.UrlEncode(strId));
			WebClient cli = new WebClient();
            cli.DownloadStringCompleted += new DownloadStringCompletedEventHandler(cli_DownloadStringCompleted_othersTweet);
			cli.DownloadStringAsync(u);
            isDownloading_othersTweet = true;
            setDownloading();
        }

        private void menuReload_Click(object sender, EventArgs e)
        {
            GetTweets(TwitterId);
            GetTweetsAboutMe(TwitterId);
        }

        private void userSetting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}
