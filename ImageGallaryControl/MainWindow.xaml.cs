using DevExpress.Data.Extensions;
using DevExpress.Xpf.Bars;
using ImageControlCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageGallaryControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ControlPanel_CommandClick(object sender, ControlPanelEventArgs e)
        {
            switch (e.Command)
            {
                case ControlPanelCommand.ZoomValueChanged:
                    imageViewer.ScaleCenter(controlPanel.ZoomValue / 100);
                    break;
                case ControlPanelCommand.RotateLeft:
                    RotateCounterclockwise();
                    break;
                case ControlPanelCommand.RotateRight:
                    RotateClockwise();
                    break;
                case ControlPanelCommand.ZoomToOriginalSize:
                    controlPanel.SetAndAnimateZoomValue(100);
                    break;
                case ControlPanelCommand.VerSize:
                    controlPanel.SetAndAnimateZoomValue(imageViewer.VerticalFitScale * 100);
                    break;
                case ControlPanelCommand.HorSize:
                    controlPanel.SetAndAnimateZoomValue(imageViewer.HorizontalFitScale * 100);
                    break;
                case ControlPanelCommand.AutoSize:
                    controlPanel.SetAndAnimateZoomValue(imageViewer.GetBestFitScale() * 100);
                    break;
            }
        }
        private void RotateClockwise()
        {
            switch (imageViewer.Rotation)
            {
                case Rotation.Rotate0:
                    imageViewer.Rotation = Rotation.Rotate90;
                    break;
                case Rotation.Rotate90:
                    imageViewer.Rotation = Rotation.Rotate180;
                    break;
                case Rotation.Rotate180:
                    imageViewer.Rotation = Rotation.Rotate270;
                    break;
                case Rotation.Rotate270:
                    imageViewer.Rotation = Rotation.Rotate0;
                    break;
            }
        }
        private void RotateCounterclockwise()
        {
            switch (imageViewer.Rotation)
            {
                case Rotation.Rotate0:
                    imageViewer.Rotation = Rotation.Rotate270;
                    break;
                case Rotation.Rotate90:
                    imageViewer.Rotation = Rotation.Rotate0;
                    break;
                case Rotation.Rotate180:
                    imageViewer.Rotation = Rotation.Rotate90;
                    break;
                case Rotation.Rotate270:
                    imageViewer.Rotation = Rotation.Rotate180;
                    break;
            }
        }
        private void imageViewer_MouseWheelZoom(object sender, EventArgs e)
        {
            controlPanel.ZoomValue = imageViewer.Scale * 100;
        }
    }
}
