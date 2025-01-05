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
using Microsoft.Win32;
using System.IO;

using OpenCvSharp.Extensions;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Drawing.Imaging;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

using OpenCvSharp.Extensions; // 如果擴充方法在這裡
using Drawing = System.Drawing;
using System.Collections.Generic;
using static AForge.Math.FourierTransform;
using System.Diagnostics;
using OpenCvSharp;
using System.Runtime.InteropServices;

//Process currentProcess = Process.GetCurrentProcess();
//currentProcess.PriorityClass = ProcessPriorityClass.High;

namespace 影像期末
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        // 獲取用戶選擇的檔案路徑
        string selectedFile = "";
        private DispatcherTimer frameCaptureTimer; // 用於定時捕捉畫面的計時器
        Bitmap originalBitmap;
        public int sencond = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private Bitmap BitmapFromSource(BitmapSource source)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(ms);
                return new Bitmap(ms);
            }
        }

        private BitmapSource BitmapToSource(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }
        private void input_image_Click(object sender, RoutedEventArgs e)
        {
            // 創建 OpenFileDialog 對象
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "選擇影片檔案",
                Filter = "影片檔案 (*.mp4;*.avi;*.mov;*.wmv)|*.mp4;*.avi;*.mov;*.wmv|所有檔案 (*.*)|*.*"
            };

            // 顯示對話框，並檢查用戶是否選擇了檔案
            if (openFileDialog.ShowDialog() == true)
            {
                // 獲取用戶選擇的檔案路徑
                selectedFile = openFileDialog.FileName;

                // 確保檔案存在
                if (File.Exists(selectedFile))
                {
                    // 設置 MediaElement 的來源
                    before_media.Source = new Uri(selectedFile, UriKind.Absolute);
                }

            }
        }
        private void BeforeMedia_MediaOpened(object sender, RoutedEventArgs e)//監聽影片
        {
            // 確保影片已加載完成，並且有有效的寬度和高度
            if (before_media.NaturalVideoWidth > 0 && before_media.NaturalVideoHeight > 0)
            {
                // 初始化計時器，用於定時捕捉畫面
                frameCaptureTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(30) // 每 30 毫秒捕捉一次

                };
                frameCaptureTimer.Tick += CaptureFrame;
                frameCaptureTimer.Start();
            }
            else
            {
                MessageBox.Show("影片加載失敗或無效。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Start_image_Click(object sender, RoutedEventArgs e)
        {
            // 確保檔案存在
            if (File.Exists(selectedFile))
            {
                sencond = 0;
                // 播放影片
                before_media.Position = TimeSpan.Zero;
                before_media.Play();


            }
            else
            {
                // 顯示錯誤訊息，如果檔案不存在
                MessageBox.Show("選擇的檔案不存在，請重試。", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Retrieve_image_Click(object sender, RoutedEventArgs e)
        {
            cut_image2.Source = after_image1.Source;

        }
        // 定時器回呼函數：捕捉畫面
        private void CaptureFrame(object sender, EventArgs e)
        {
            //try
            //{
            // 渲染 MediaElement 畫面到 RenderTargetBitmap
            var renderTarget = new RenderTargetBitmap(
                (int)before_media.ActualWidth,
                (int)before_media.ActualHeight,
                96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            renderTarget.Render(before_media);

            // 將 RenderTargetBitmap 轉換為 BitmapSource
            BitmapSource bitmapSource = renderTarget;
            originalBitmap = BitmapFromSource(bitmapSource);


            // 使用影像處理判斷直線
            BitmapSource processedImage = ApplyEdgeDetection(bitmapSource);

            // 將處理後的影像顯示在 after_image1 控件中
            after_image1.Source = processedImage;

            // 在 originalBitmap 上顯示 processedImage 紅色的地方
            Bitmap highlightedBitmap = HighlightRedRegions(originalBitmap, processedImage);

            cut_image2.Source = BitmapToSource(highlightedBitmap);
            //}
            //catch (Exception ex)
            //{
            //    // 捕捉任何錯誤，並停止計時器
            //    MessageBox.Show($"捕捉畫面時出錯：{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            //    frameCaptureTimer?.Stop();
            //}
        }
        private Bitmap HighlightRedRegions(Bitmap original, BitmapSource processed)
        {
            // 将 BitmapSource 转换为 Bitmap
            Bitmap processedBitmap = BitmapFromSource(processed);

            // 确保 originalBitmap 的格式支持 SetPixel
            Bitmap writableOriginal = ConvertTo24bppBitmap(original);

            // 遍历 processedBitmap 的像素
            for (int y = 0; y < processedBitmap.Height; y++)
            {
                for (int x = 0; x < processedBitmap.Width; x++)
                {
                    // 获取像素颜色
                    Drawing.Color processedColor = processedBitmap.GetPixel(x, y);

                    // 如果是红色区域
                    if (processedColor.R > 200 && processedColor.G < 50 && processedColor.B < 50)
                    {
                        // 在 writableOriginal 上标记红色
                        writableOriginal.SetPixel(x, y, Drawing.Color.Red);
                    }
                }
            }

            return writableOriginal;
        }
        private Bitmap ConvertTo24bppBitmap(Bitmap source)
        {
            // 创建目标格式的 Bitmap
            Bitmap target = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // 使用 Graphics 将原始图像绘制到目标图像
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Drawing.Rectangle(0, 0, target.Width, target.Height));
            }

            return target;
        }
        private BitmapSource ApplyEdgeDetection(BitmapSource source)
        {
            sencond++;

            // 將 BitmapSource 轉換為 System.Drawing.Bitmap
            Bitmap bitmap = BitmapFromSource(source);


            // 將影像轉換為灰階格式
            AForge.Imaging.Filters.Grayscale grayscaleFilter = new AForge.Imaging.Filters.Grayscale(0.2126, 0.7152, 0.0722); // 使用標準灰階轉換係數
            Bitmap grayBitmap = grayscaleFilter.Apply(bitmap);

            // 對比度增強(自適應值方圖)
            AForge.Imaging.Filters.HistogramEqualization histogramFilter = new AForge.Imaging.Filters.HistogramEqualization();
            Bitmap contrastEnhancedBitmap = histogramFilter.Apply(grayBitmap);
            originalBitmap = contrastEnhancedBitmap;

            // 初始化銳化過濾器
            Sharpen sharpenFilter = new Sharpen();

            // 應用銳化過濾器
            Bitmap sharpenedImage = sharpenFilter.Apply(originalBitmap);
            // 高斯模糊
            AForge.Imaging.Filters.GaussianBlur blurFilter = new AForge.Imaging.Filters.GaussianBlur(1, 3); // 減小核大小
            Bitmap blurredBitmap = blurFilter.Apply(sharpenedImage);

            //拉普拉絲邊緣
            //Bitmap edgeLaplacianBitmap = ApplyLaplacianFilter(blurredBitmap);

            // 初始化 Canny 邊緣檢測過濾器
            AForge.Imaging.Filters.CannyEdgeDetector cannyFilter = new AForge.Imaging.Filters.CannyEdgeDetector
            {
                LowThreshold = 30,   // 設定低閾值
                HighThreshold = 90  // 設定高閾值
            };

            // 對影像應用 Canny 邊緣檢測
            Bitmap edgeBitmap = cannyFilter.Apply(blurredBitmap);

            cut_image3.Source = BitmapToSource(edgeBitmap);

            // 二值化影像
            Threshold binaryFilter = new Threshold(20);
            Bitmap binaryImage = binaryFilter.Apply(edgeBitmap);

            // 使用十字形核心進行膨脹
            short[,] crossKernel = new short[,]
{
    {0, 0, 1, 0 ,0},
    {0, 0, 1, 0 ,0},
    {1, 1, 1, 1 ,1},
    {0, 0, 1, 0 ,0},
    {0, 0, 1, 0 ,0}
};
            short[,] crossKernel_3 = new short[,]
{
    {0, 1, 0},
    {1, 1, 1},
    {0, 1, 0,}
};


            // 創建膨脹濾鏡，並指定結構元素
            Dilatation dilationFilter = new Dilatation(crossKernel_3);
            Bitmap dilationImage = dilationFilter.Apply(binaryImage);
            dilationImage = dilationFilter.Apply(dilationImage);


            // 創建侵蝕濾鏡，並指定結構元素
            Erosion erosionFilter = new Erosion(crossKernel);
            Bitmap erosionImage = erosionFilter.Apply(dilationImage);

            Erosion erosionFilter_3 = new Erosion(crossKernel_3);
            erosionImage = erosionFilter_3.Apply(erosionImage);
            dilationImage = dilationFilter.Apply(erosionImage);
            dilationImage = dilationFilter.Apply(dilationImage);
            erosionImage = erosionFilter_3.Apply(dilationImage);
            //erosionImage = erosionFilter_3.Apply(dilationImage);

            Bitmap resule = dilationImage;
            //Bitmap resule = grayBitmap;
            //Bitmap CentralImage = ApplyCentralWeightedMedianFilter(erosionImage, 3);

            // 10.進行連通性分析
            List<Blob> blobs = PerformBlobAnalysis(resule);

            // 11. 繪製連通區域（可選）
            Bitmap roiImage = DrawBlobsOnImage(resule, blobs);

            cut_image1.Source = BitmapToSource(roiImage);


            Bitmap after_image = grayBitmap;

            //12 在after_image中把連通區域內白色的piexl畫成紅色

            // 創建一個副本來存放結果影像（確保格式為 24bppRgb）
            Bitmap resultImage = new Bitmap(erosionImage.Width, erosionImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(resultImage))
            {
                g.DrawImage(erosionImage, new Drawing.Rectangle(0, 0, erosionImage.Width, erosionImage.Height));
            }
            // 第12步：將連通區域內的白色像素繪製為紅色
            if (sencond > 12 && sencond < 23)
            {
                try
                {
                    Blob thirdLargestBlob = blobs[3];

                    Drawing.Rectangle rect = thirdLargestBlob.Rectangle;

                    // 獲取連通區域內像素，將白色像素繪製為紅色
                    for (int y = rect.Top; y < rect.Bottom; y++)
                    {
                        for (int x = rect.Left; x < rect.Right; x++)
                        {
                            if (x >= 0 && x < erosionImage.Width && y >= 0 && y < erosionImage.Height)
                            {
                                Drawing.Color pixelColor = erosionImage.GetPixel(x, y);

                                // 如果是白色像素，將其設為紅色
                                if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                                {
                                    resultImage.SetPixel(x, y, Drawing.Color.Red);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            else if (sencond > 23 && sencond < 30) {
                Blob thirdLargestBlob = blobs[2];

                Drawing.Rectangle rect = thirdLargestBlob.Rectangle;

                // 獲取連通區域內像素，將白色像素繪製為紅色
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        if (x >= 0 && x < erosionImage.Width && y >= 0 && y < erosionImage.Height)
                        {
                            Drawing.Color pixelColor = erosionImage.GetPixel(x, y);

                            // 如果是白色像素，將其設為紅色
                            if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                            {
                                resultImage.SetPixel(x, y, Drawing.Color.Red);
                            }
                        }
                    }
                }
            }
            else if (sencond > 30 && sencond < 43)
            {
                Blob thirdLargestBlob = blobs[1];

                Drawing.Rectangle rect = thirdLargestBlob.Rectangle;

                // 獲取連通區域內像素，將白色像素繪製為紅色
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        if (x >= 0 && x < erosionImage.Width && y >= 0 && y < erosionImage.Height)
                        {
                            Drawing.Color pixelColor = erosionImage.GetPixel(x, y);

                            // 如果是白色像素，將其設為紅色
                            if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                            {
                                resultImage.SetPixel(x, y, Drawing.Color.Red);
                            }
                        }
                    }
                }
            }
            else if (sencond>43)
            {
                Drawing.Rectangle rect;
                Blob largestBlob = blobs.OrderByDescending(blob => blob.Rectangle.Width * blob.Rectangle.Height).First();
                rect = largestBlob.Rectangle;

                // 獲取連通區域內像素，將白色像素繪製為紅色
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    for (int x = rect.Left; x < rect.Right; x++)
                    {
                        if (x >= 0 && x < erosionImage.Width && y >= 0 && y < erosionImage.Height)
                        {
                            Drawing.Color pixelColor = erosionImage.GetPixel(x, y);

                            // 如果是白色像素，將其設為紅色
                            if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                            {
                                resultImage.SetPixel(x, y, Drawing.Color.Red);
                            }
                        }
                    }
                }
            }
            
           


            //////擷取ROI(大圖中間1/3)
            //Drawing.Rectangle roi = GetCentralROI(resultImage);
            //Bitmap roiBitmap = CropBitmap(resultImage, roi);



            return BitmapToSource(resultImage);
        }
        private System.Drawing.Rectangle GetCentralROI(Bitmap image)
        {
            // 寬度保持不變
            int width = image.Width / 3;

            // 高度為原本的 1/3
            int height = image.Height;

            // 起始點 X 為 0（寬度不變，所以從最左邊開始）
            int startX = image.Width / 3;

            // 起始點 Y 保證從圖像高度的中間部分開始
            int startY = 0;

            return new System.Drawing.Rectangle(startX, startY, width, height);
        }

        private Bitmap CropBitmap(Bitmap source, System.Drawing.Rectangle roi)
        {
            // 確保裁剪範圍在影像內
            if (roi.X < 0 || roi.Y < 0 || roi.X + roi.Width > source.Width || roi.Y + roi.Height > source.Height)
                throw new ArgumentException("裁剪範圍超出影像邊界");

            // 裁剪區域
            return source.Clone(roi, source.PixelFormat);
        }

       


        // 拉普拉斯
        private Bitmap ApplyLaplacianFilter(Bitmap grayBitmap)
        {
            // 定義 Laplacian 卷積核
            int[,] laplacianKernel = new int[,]
            {
        { 0, -1, 0 },
        { -1, 4, -1 },
        { 0, -1, 0 }
            };

            // 創建結果圖像
            Bitmap resultBitmap = new Bitmap(grayBitmap.Width, grayBitmap.Height);

            // 遍歷每個像素（跳過邊界像素）
            for (int y = 1; y < grayBitmap.Height - 1; y++)
            {
                for (int x = 1; x < grayBitmap.Width - 1; x++)
                {
                    int gradient = 0;

                    // 卷積操作
                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int pixelValue = grayBitmap.GetPixel(x + kx, y + ky).R; // 灰階值
                            gradient += pixelValue * laplacianKernel[ky + 1, kx + 1];
                        }
                    }

                    // 將梯度結果限制在 [0, 255]
                    gradient = Math.Max(0, Math.Min(255, gradient));

                    // 設置結果像素值
                    resultBitmap.SetPixel(x, y, Drawing.Color.FromArgb(gradient, gradient, gradient));
                }
            }

            return resultBitmap;
        }

        //自適應2值
        private Bitmap ApplyAdaptiveThreshold(Bitmap source, int blockSize, int c)
        {
            if (blockSize % 2 == 0)
            {
                throw new ArgumentException("Block size must be an odd number.");
            }

            // 將影像轉換為灰階格式（此處假設輸入已是灰階）
            Bitmap grayBitmap = source;

            // 建立積分影像
            int[,] integralImage = new int[grayBitmap.Width + 1, grayBitmap.Height + 1];

            for (int y = 1; y <= grayBitmap.Height; y++)
            {
                for (int x = 1; x <= grayBitmap.Width; x++)
                {
                    int pixelValue = grayBitmap.GetPixel(x - 1, y - 1).R;
                    integralImage[x, y] = pixelValue +
                                          integralImage[x - 1, y] +
                                          integralImage[x, y - 1] -
                                          integralImage[x - 1, y - 1];
                }
            }

            // 建立結果影像
            Bitmap result = new Bitmap(grayBitmap.Width, grayBitmap.Height);
            int radius = blockSize / 2;

            // 計算二值化
            for (int y = 0; y < grayBitmap.Height; y++)
            {
                for (int x = 0; x < grayBitmap.Width; x++)
                {
                    // 確定鄰域範圍
                    int x1 = Math.Max(0, x - radius);
                    int x2 = Math.Min(grayBitmap.Width - 1, x + radius);
                    int y1 = Math.Max(0, y - radius);
                    int y2 = Math.Min(grayBitmap.Height - 1, y + radius);

                    // 使用積分影像快速計算鄰域總和
                    int sum = integralImage[x2 + 1, y2 + 1]
                              - integralImage[x1, y2 + 1]
                              - integralImage[x2 + 1, y1]
                              + integralImage[x1, y1];

                    int count = (x2 - x1 + 1) * (y2 - y1 + 1);
                    int mean = sum / count;

                    // 計算自適應閾值
                    int threshold = mean - c;

                    // 二值化
                    int pixelValue = grayBitmap.GetPixel(x, y).R;
                    result.SetPixel(x, y, pixelValue > threshold ? Drawing.Color.White : Drawing.Color.Black);
                }
            }

            return result;
        }
        //灰階反轉
        private Bitmap InvertBrightness(Bitmap source)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    // 獲取原始像素值
                    Drawing.Color originalColor = source.GetPixel(x, y);

                    // 計算亮暗反轉的像素值
                    int invertedR = 255 - originalColor.R;
                    int invertedG = 255 - originalColor.G;
                    int invertedB = 255 - originalColor.B;

                    // 設置反轉後的像素值
                    Drawing.Color invertedColor = Drawing.Color.FromArgb(originalColor.A, invertedR, invertedG, invertedB);
                    result.SetPixel(x, y, invertedColor);
                }
            }

            return result;
        }

        // 中值法後處理
        private Bitmap ApplyMedianFilter(Bitmap source, int blockSize)
        {
            if (blockSize % 2 == 0)
            {
                throw new ArgumentException("Block size 必須是奇數。");
            }

            Bitmap result = new Bitmap(source.Width, source.Height);
            int radius = blockSize / 2;

            // 遍歷每個像素
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    // 獲取鄰域
                    List<int> neighborhood = new List<int>();
                    for (int yy = Math.Max(0, y - radius); yy <= Math.Min(source.Height - 1, y + radius); yy++)
                    {
                        for (int xx = Math.Max(0, x - radius); xx <= Math.Min(source.Width - 1, x + radius); xx++)
                        {
                            neighborhood.Add(source.GetPixel(xx, yy).R);
                        }
                    }

                    // 計算中值
                    neighborhood.Sort();
                    int median = neighborhood[neighborhood.Count / 2];

                    // 將中值應用到結果影像
                    result.SetPixel(x, y, Drawing.Color.FromArgb(median, median, median));
                }
            }

            return result;
        }
        //中央加權中值法
        private Bitmap ApplyCentralWeightedMedianFilter(Bitmap source, int blockSize)
        {
            if (blockSize % 2 == 0)
            {
                throw new ArgumentException("Block size 必須是奇數。");
            }

            Bitmap result = new Bitmap(source.Width, source.Height);
            int radius = blockSize / 2;

            // 中央加權矩陣（可調整）
            int[,] weights = GenerateCentralWeights(blockSize);

            // 遍歷每個像素
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    List<int> weightedNeighborhood = new List<int>();

                    // 收集鄰域像素值及其加權
                    for (int yy = -radius; yy <= radius; yy++)
                    {
                        for (int xx = -radius; xx <= radius; xx++)
                        {
                            int neighborX = x + xx;
                            int neighborY = y + yy;

                            // 確保鄰域像素在影像範圍內
                            if (neighborX >= 0 && neighborX < source.Width && neighborY >= 0 && neighborY < source.Height)
                            {
                                int weight = weights[xx + radius, yy + radius]; // 獲取加權值
                                int pixelValue = source.GetPixel(neighborX, neighborY).R;

                                // 將像素值按權重添加到清單
                                for (int w = 0; w < weight; w++)
                                {
                                    weightedNeighborhood.Add(pixelValue);
                                }
                            }
                        }
                    }

                    // 計算加權中值
                    weightedNeighborhood.Sort();
                    int median = weightedNeighborhood[weightedNeighborhood.Count / 2];

                    // 將結果應用到影像
                    result.SetPixel(x, y, Drawing.Color.FromArgb(median, median, median));
                }
            }

            return result;
        }

        // 中央加權矩陣
        private int[,] GenerateCentralWeights(int blockSize)
        {
            int[,] weights = new int[blockSize, blockSize];
            int center = blockSize / 2;

            for (int y = 0; y < blockSize; y++)
            {
                for (int x = 0; x < blockSize; x++)
                {
                    // 距離越遠，權重越小
                    int distance = Math.Abs(center - x) + Math.Abs(center - y);
                    weights[x, y] = Math.Max(1, center + 1 - distance);
                }
            }

            return weights;
        }

        //8byte轉24
        private static Bitmap ConvertToGrayscale(Bitmap inputBitmap)
        {
            // 將圖像轉換為灰階格式（8bpp）
            AForge.Imaging.Filters.Grayscale grayscaleFilter = new AForge.Imaging.Filters.Grayscale(0.2126, 0.7152, 0.0722); // 標準灰階轉換
            return grayscaleFilter.Apply(inputBitmap);
        }
        private List<Blob> PerformBlobAnalysis(Bitmap binaryImage)//連通判斷
        {
            BlobCounter blobCounter = new BlobCounter
            {
                FilterBlobs = true,
                MinWidth = 5,  // 過濾最小寬度
                MaxWidth = 100,
                MinHeight = 20, // 過濾最小高度
                ObjectsOrder = ObjectsOrder.Size // 按大小排序
            };

            blobCounter.ProcessImage(binaryImage);

            // 獲取所有連通區域
            Blob[] blobs = blobCounter.GetObjectsInformation();

            return blobs.ToList();
        }
        private Bitmap DrawBlobsOnImage(Bitmap sourceImage, List<Blob> blobs)
        {
            Bitmap resultImage = new Bitmap(sourceImage);

            using (Graphics g = Graphics.FromImage(resultImage))
            {
                //if (sencond > 44)
                //{
                //    if (blobs != null && blobs.Count > 0)
                //    {
                //        // 找到最大連通區域
                //        Blob largestBlob = blobs.OrderByDescending(blob => blob.Rectangle.Width * blob.Rectangle.Height).First();

                //        // 繪製最大連通區域的矩形邊框
                //        Drawing.Rectangle rect = largestBlob.Rectangle;
                //        g.DrawRectangle(Pens.Red, rect);
                //    }
                //}
                //else 
                //{
                    // 定義一組顏色集合
                    Drawing.Color[] colors = { Drawing.Color.Red, Drawing.Color.Blue, Drawing.Color.Green, Drawing.Color.Orange, Drawing.Color.Purple, Drawing.Color.Yellow };

                    int colorCount = colors.Length; // 顏色數量
                    int blobIndex = 0; // 用於計算當前 blob 的索引

                    foreach (var blob in blobs)
                    {
                        // 繪製連通區域的矩形邊框
                        Drawing.Rectangle rect = blob.Rectangle;

                        // 動態選擇顏色（使用餘數來循環顏色）
                        Drawing.Color currentColor = colors[blobIndex % colorCount];

                        // 創建畫筆
                        using (Drawing.Pen pen = new Drawing.Pen(currentColor))
                        {
                            g.DrawRectangle(pen, rect);
                        }

                        blobIndex++; // 遞增索引
                    //}

                   
                    //if (sencond > 15 && sencond < 23)
                    //{
                    //    try {
                    //        Blob thirdLargestBlob = blobs[3];

                    //        Drawing.Rectangle rect = thirdLargestBlob.Rectangle;
                    //        g.DrawRectangle(Pens.Red, rect);
                    //    }
                    //    catch { }
                        
                    //}
                    //if (sencond>23&& sencond <30)
                    //{
                    //    Blob thirdLargestBlob = blobs[2];

                    //    Drawing.Rectangle rect = thirdLargestBlob.Rectangle;
                    //    g.DrawRectangle(Pens.Red, rect);
                    //}
                    //if(sencond>30&&sencond<45)
                    //{
                    //    Blob thirdLargestBlob = blobs[1];

                    //    Drawing.Rectangle rect = thirdLargestBlob.Rectangle;
                    //    g.DrawRectangle(Pens.Red, rect);
                    //}
                   
                }
                    //if (blobs != null && blobs.Count > 0)
                    //{
                    //    // 找到最大連通區域
                    //    Blob largestBlob = blobs.OrderByDescending(blob => blob.Rectangle.Width * blob.Rectangle.Height).First();

                    //    // 繪製最大連通區域的矩形邊框
                    //    Drawing.Rectangle rect = largestBlob.Rectangle;
                    //    g.DrawRectangle(Pens.Red, rect);
                    //}
                    //all
                   
            }

            return resultImage;
        }

        private Bitmap CropBitmap_blob(Bitmap source, Drawing.Rectangle rect)
        {
            // 確保裁剪範圍在原始影像內
            rect.Intersect(new Drawing.Rectangle(0, 0, source.Width, source.Height));

            if (rect.Width <= 0 || rect.Height <= 0)
                return null; // 無效範圍返回 null

            Bitmap croppedBitmap = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.DrawImage(source, 0, 0, rect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        private void Second_text_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}