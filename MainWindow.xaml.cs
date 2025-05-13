using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace OverlayApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Регистрируем хоткей после инициализации окна
            SourceInitialized += OnSourceInitialized;
            Closed += OnWindowClosed;
            Hide();
        }

        private (int scaleX, int scaleY) GetScaleFactors()
        {
            // PresentationSource для окна
            var source = PresentationSource.FromVisual(this);
            if (source?.CompositionTarget != null)
            {
            // матрица преобразования от логических единиц к физическим
            Matrix m = source.CompositionTarget.TransformToDevice;
            // m.M11 == масштаб по горизонтали, m.M22 — по вертикали
            int scaleX = (int)(m.M11 * 100);
            int scaleY = (int)(m.M22 * 100);
            return (scaleX, scaleY);
            }
            return (100, 100); // Default scale factors if not available
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            int scaleX, scaleY;
            (scaleX, scaleY) = GetScaleFactors();
             this.Dispatcher.Invoke(() =>
                    {
                        this.Left = this.Left * 100 / scaleX;
                        this.Top = this.Top * 100 / scaleY;
                        this.OverlayText.FontSize = this.OverlayText.FontSize * 100 / scaleY;
                    });
        }


        private void ToggleVisibility()
        {
            if (IsVisible) Hide();
            else Show();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
        }
    }
}