﻿using DevExpress.Data.Extensions;
using DevExpress.Xpf.Bars;
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
                case ControlPanelCommand.Next:
                    ShowItemInImageVewer(GetNextItem((GalleryItem)imageViewer.Tag));
                    break;
                case ControlPanelCommand.Prior:
                    ShowItemInImageVewer(GetPriorItem((GalleryItem)imageViewer.Tag));
                    break;
                //case ControlPanelCommand.Play:
                //    slideViewer.Visibility = Visibility.Visible;
                //    slideViewer.NextSlideImageSource = ((PhotoInfo)gallery.Gallery.Groups[0].Items[0].Caption).Source;
                //    slideViewer.Tag = gallery.Gallery.Groups[0].Items[0];
                //    SlideMode = true;
                //    slideViewer.Play();
                //    break;
                case ControlPanelCommand.Print:
                    PrintCurrentImage();
                    break;
            }
        }
        private void ShowItemInImageVewer(GalleryItem item)
        {
            imageViewerTitle.Text = ((PhotoInfo)item.Caption).Caption;
            imageViewer.ImageSource = (BitmapSource)((PhotoInfo)item.Caption).Source;
            imageViewer.Tag = item;
            imageViewer.Rotation = Rotation.Rotate0;
            imageViewer.LayoutUpdated += new EventHandler(imageViewer_LayoutUpdated);
            FitImageInViewport();
        }
        GalleryItem GetNextItem(GalleryItem item)
        {
            return GetItem(item, true);
        }
        GalleryItem GetPriorItem(GalleryItem item)
        {
            return GetItem(item, false);
        }

        GalleryItem GetItem(GalleryItem item, bool next)
        {
            if (item == null)
                return null;
            var coeff = next ? 1 : -1;
            var itemIndex = item.Group.Items.IndexOf(item) + coeff;
            if (item.Group.Items.IsValidIndex(itemIndex))
                return item.Group.Items[itemIndex];

            var groups = item.Group.Gallery.Groups;
            var groupIndex = (groups.IndexOf(item.Group) + coeff + groups.Count) % groups.Count;
            var group = groups[groupIndex];
            return next ? group.Items.First() : group.Items.Last();
        }

        void imageViewer_LayoutUpdated(object sender, EventArgs e)
        {
            FitImageInViewport();
            imageViewer.LayoutUpdated -= new EventHandler(imageViewer_LayoutUpdated);
        }

        void FitImageInViewport()
        {
            if (imageViewer.ImageSource == null)
            {
                controlPanel.ZoomValue = 100;
                return;
            }
            double scaleWidth = Math.Min(1.0, (imageViewer.Viewport.ActualWidth - 20) / imageViewer.ImageSource.PixelWidth);
            double scaleHeight = Math.Min(1.0, (imageViewer.Viewport.ActualHeight - 20) / imageViewer.ImageSource.PixelHeight);
            controlPanel.ZoomValue = 100 * Math.Min(scaleWidth, scaleHeight);
        }

        public void PrintCurrentImage()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PhotoInfo photoInfo = (PhotoInfo)((GalleryItem)imageViewer.Tag).Caption;
                printDialog.PrintVisual(new Image() { Source = photoInfo.Source }, photoInfo.Caption);
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

        private void bntCloseImageViewer_Click(object sender, RoutedEventArgs e)
        {
            CloseImageViewPopup();
        }
        void CloseImageViewPopup()
        {
            //mainView.Effect = null;
            imageViewPopup.Visibility = Visibility.Collapsed;
        }
        private void imageViewer_MouseWheelZoom(object sender, EventArgs e)
        {
            controlPanel.ZoomValue = imageViewer.Scale * 100;
        }

        private void slideViewer_BeforeCurrentSlideLoading(object sender, EventArgs e)
        {
            GalleryItem item = GetNextItem((GalleryItem)slideViewer.Tag);
            slideViewer.Tag = item;
            slideViewer.NextSlideImageSource = ((PhotoInfo)item.Caption).Source;
        }
    }
    public class PhotoInfo
    {
        public ImageSource Source { get; set; }
        public string Caption { get; set; }
        public string SizeInfo { get; set; }
        public Size ViewSize { get; set; }
        public PhotoInfo()
        {
            ViewSize = new Size(double.NaN, double.NaN);
        }
    }
}
