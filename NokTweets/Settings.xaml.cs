using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace NokTweets
{
    public partial class Settings : PhoneApplicationPage
    {
        IsolatedStorageSettings settings;

        const string TwitterIdSettingKeyName = "TwitterIdSetting";
        const string TwitterIdSettingDefault = "Nokia";

        public Settings()
        {
            InitializeComponent();

            settings = IsolatedStorageSettings.ApplicationSettings;
            ApplicationTitle.Text = "@" + TwitterIdSetting;
            userNameBox.Text = TwitterIdSetting;
        }

        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            if (settings.Contains(Key))
            {
                if (settings[Key] != value)
                {
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public void Save()
        {
            settings.Save();
        }

        public string TwitterIdSetting
        {
            get
            {
                return GetValueOrDefault<string>(TwitterIdSettingKeyName, TwitterIdSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(TwitterIdSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            TwitterIdSetting = userNameBox.Text;
            NavigationService.GoBack();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            userNameBox.Select(0, userNameBox.Text.Length);
            userNameBox.Focus();
        }

        private void userNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TwitterIdSetting = userNameBox.Text;
                NavigationService.GoBack();
            }
        }
    }
}