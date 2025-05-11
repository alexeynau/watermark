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
            // Не завершаем приложение при скрытии окна
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Настраиваем трей и оверлей
            SetupTrayIcon();
            _overlay = new MainWindow();
            _overlay.Show();
        }

        private void SetupTrayIcon()
        {
            _trayMenu = new ContextMenuStrip();
            _trayMenu.Items.Add("Показать/Спрятать", null, (s, e) => ToggleOverlay());
            _trayMenu.Items.Add("Цвет", null, (s, e) => ChangeTextColor());
            _trayMenu.Items.Add("Текст", null, (s, e) => ChangeText());
            _trayMenu.Items.Add("Непрозрачность", null, (s, e) => ChangeTextOpacity());
            _trayMenu.Items.Add("Выход", null, (s, e) => ExitApp());


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

        private void ChangeTextColor()
        {
            using (var dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Конвертация System.Drawing.Color в System.Windows.Media.Color
                    var mediaColor = System.Windows.Media.Color.FromArgb(
                        dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B);
                    _overlay.Dispatcher.Invoke(() =>
                    {
                        _overlay.OverlayText.Foreground = new System.Windows.Media.SolidColorBrush(mediaColor);
                    });
                }
            }
        }

        private void ChangeText() // Изменение текста оверлея
        {
            using (var inputDialog = new Form())
            {
                inputDialog.Text = "Поменять текст";
                inputDialog.Width = 400;
                inputDialog.Height = 150;

                var label = new Label { Left = 10, Top = 20, Text = "Новый текст" };
                var textBox = new TextBox { Left = 10, Top = 50, Width = 360 };
                var confirmation = new Button { Text = "OK", Left = 280, Width = 90, Top = 80, DialogResult = DialogResult.OK };

                inputDialog.Controls.Add(label);
                inputDialog.Controls.Add(textBox);
                inputDialog.Controls.Add(confirmation);
                inputDialog.AcceptButton = confirmation;

                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    var newText = textBox.Text;
                    _overlay.Dispatcher.Invoke(() =>
                    {
                        _overlay.OverlayText.Text = newText;
                    });
                }
            }
        }

        private void ChangeTextOpacity()
        {
            using (var inputDialog = new Form())
            {
                inputDialog.Text = "Изменить непрозрачность";
                inputDialog.Width = 400;
                inputDialog.Height = 150;

                var label = new Label { Left = 10, Top = 20, Text = "От 0,0 до 1,0" };
                var textBox = new TextBox { Left = 10, Top = 50, Width = 360 };
                var confirmation = new Button { Text = "OK", Left = 280, Width = 90, Top = 80, DialogResult = DialogResult.OK };

                inputDialog.Controls.Add(label);
                inputDialog.Controls.Add(textBox);
                inputDialog.Controls.Add(confirmation);
                inputDialog.AcceptButton = confirmation;

                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    if (double.TryParse(textBox.Text, out double opacity) && opacity >= 0 && opacity <= 1)
                    {
                        _overlay.Dispatcher.Invoke(() =>
                        {
                            _overlay.OverlayText.Opacity = opacity;
                        });
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Введите значение от 0 до 1.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
            _overlay.Close();
            Shutdown();
        }
    }
}