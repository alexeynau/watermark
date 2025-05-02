using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace OverlayApp
{
    public partial class App : Application
    {
        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private MainWindow _overlay;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Приложение живёт до Shutdown() вручную
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Инициализируем трей
            SetupTrayIcon();
            // Создаём окно, но не показываем его
            _overlay = new MainWindow();
        }

        private void SetupTrayIcon()
        {
            _trayMenu = new ContextMenuStrip();
            _trayMenu.Items.Add("Show Overlay", null, (s, e) => ToggleOverlay());
            _trayMenu.Items.Add("Exit", null, (s, e) => ExitApp());

            Icon icon;
            try
            {
                var uri = new Uri("pack://application:,,,/Resources/Icon.ico");
                var stream = GetResourceStream(uri).Stream;
                icon = new Icon(stream);
            }
            catch
            {
                icon = SystemIcons.Application;
            }

            _trayIcon = new NotifyIcon
            {
                Text = "OverlayApp",
                Icon = icon,
                ContextMenuStrip = _trayMenu,
                Visible = true
            };
            _trayIcon.DoubleClick += (s, e) => ToggleOverlay();
        }

        private void ToggleOverlay()
        {
            if (_overlay.IsVisible)
                _overlay.Hide();
            else
                _overlay.Show();
        }

        private void ExitApp()
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            // Закрываем окно, если открыто
            _overlay.Close();
            Shutdown();
        }
    }
}