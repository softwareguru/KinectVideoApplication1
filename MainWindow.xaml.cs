using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;

namespace KinectVideoApplication1
{
    public partial class MainWindow : Window
    {
        Runtime runtime = new Runtime();

        public MainWindow()
        {
            InitializeComponent();


            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Unloaded += new RoutedEventHandler(MainWindow_Unloaded);


            runtime.VideoFrameReady += new EventHandler<Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs>(runtime_VideoFrameReady);

            runtime.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(runtime_DepthFrameReady);

        }
                
        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            runtime.Uninitialize();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Inicializa el dispositivo
            runtime.Initialize(Microsoft.Research.Kinect.Nui.RuntimeOptions.UseColor | Microsoft.Research.Kinect.Nui.RuntimeOptions.UseDepth);

            //Esta rutina abre el stream de video para mostrar la camara RGB
            runtime.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
            //Esta rutina abre el stream de profundidad para mostrar las distintas profundidades del KINECT
            runtime.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution640x480, ImageType.Depth);
        }

        void runtime_VideoFrameReady(object sender, Microsoft.Research.Kinect.Nui.ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;

            BitmapSource source = BitmapSource.Create(image.Width, image.Height, 96, 96,
                PixelFormats.Bgr32, null, image.Bits, image.Width * image.BytesPerPixel);
            videoImage.Source = source;
        }

        void runtime_DepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            PlanarImage image = e.ImageFrame.Image;

            BitmapSource source = BitmapSource.Create(image.Width, image.Height, 96, 96,
                PixelFormats.Gray16, null, image.Bits, image.Width * image.BytesPerPixel);
            depthImage.Source = source;
        }

    }
}
