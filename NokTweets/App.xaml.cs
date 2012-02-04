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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NokTweets
{
    public partial class App : Application
    {
        /// <summary>
        /// Phone アプリケーションのルート フレームへの簡単なアクセスを提供します。
        /// </summary>
        /// <returns>Phone アプリケーションのルート フレーム。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application オブジェクトのコンストラクター
        /// </summary>
        public App()
        {
            // キャッチされていない例外のグローバル ハンドラー。
            //  ApplicationBarItem.Click によりスローされる例外はここでキャッチされないことに注意。
            UnhandledException += Application_UnhandledException;

            // デバッグ中にグラフィック プロファイル情報を表示します。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 現在のフレーム レート カウンターを表示します。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 各フレーム内で再描画されているアプリケーションの領域を表示します。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 非稼動分析視覚化モードを有効にして、
                // 色付きオーバーレイで GPU 加速化されているページの領域を示します。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // 標準の Silverlight 初期化
            InitializeComponent();

            // Phone 固有の初期化
            InitializePhoneApplication();
        }

        // アプリケーションの起動時に実行されるコード (例: 開始時)
        // このコードは、アプリケーションが再アクティブ化されるときには実行されません。
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // アプリケーションがアクティブ化 (前景に移動) されるときに実行されるコード
        // このコードは、アプリケーションの最初の起動時には実行されません
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // アプリケーションが非アクティブ化 (背景に移動) されるときに実行されるコード
        // このコードは、アプリケーションの終了時には実行されません
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // アプリケーションの終了時に実行されるコード (例: ユーザーが [戻る] を押したとき)
        // このコードは、アプリケーションが非アクティブ化されたときには実行されません
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // 移動に失敗した場合に実行されるコード
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 移動に失敗しました; 中断してデバッガーを起動します
                System.Diagnostics.Debugger.Break();
            }
        }

        // ハンドルされていない例外に対して実行されるコード
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ハンドルされていない例外が発生しました; 中断してデバッガーを起動します
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone アプリケーションの初期化

        // 二重初期化を回避します
        private bool phoneApplicationInitialized = false;

        // このメソッドに別のコードを追加しないでください
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // フレームを作成して、RootVisual に設定しないでください; これによりスプラッシュ
            // 画面が、アプリケーションでレンダリングの準備ができるまでアクティブのままになります。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 移動のエラーをハンドルします
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 再度初期化しないようにします
            phoneApplicationInitialized = true;
        }

        // このメソッドに別のコードを追加しないでください
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // ルートを visual に設定してアプリケーションのレンダリングを許可します
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 必要なくなったので、このハンドラーを削除します
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}