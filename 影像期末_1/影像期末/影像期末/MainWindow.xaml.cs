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
                    Interval = TimeSpan.FromMilliseconds(16) // 每 100 毫秒捕捉一次

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

                // 使用影像處理方法執行邊緣檢測
                BitmapSource processedImage = ApplyEdgeDetection(bitmapSource);

                // 將處理後的影像顯示在 after_image1 控件中
                after_image1.Source = processedImage;
            //}
            //catch (Exception ex)
            //{
            //    // 捕捉任何錯誤，並停止計時器
            //    MessageBox.Show($"捕捉畫面時出錯：{ex.Message}", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            //    frameCaptureTimer?.Stop();
            //}
        }


        private BitmapSource ApplyEdgeDetection(BitmapSource source)
        {
            // 將 BitmapSource 轉換為 System.Drawing.Bitmap
            Bitmap bitmap = BitmapFromSource(source);
            //Bitmap originalBitmap = ;

            // 將影像轉換為灰階格式
            Grayscale grayscaleFilter = new Grayscale(0.2126, 0.7152, 0.0722); // 使用標準灰階轉換係數
            Bitmap grayBitmap = grayscaleFilter.Apply(bitmap);

            // 對比度增強(自適應值方圖)
            HistogramEqualization histogramFilter = new HistogramEqualization();
            Bitmap contrastEnhancedBitmap = histogramFilter.Apply(grayBitmap);

            // 高斯模糊
            GaussianBlur blurFilter = new GaussianBlur(2, 5); // 減小核大小
            Bitmap blurredBitmap = blurFilter.Apply(contrastEnhancedBitmap);

            ////Sobel
            //Edges edgeFilter = new Edges();
            //Bitmap edgeEnhancedBitmap = edgeFilter.Apply(blurredBitmap);

            //拉普拉絲邊緣
            Bitmap edgeLaplacianBitmap = ApplyLaplacianFilter(blurredBitmap);

            // 使用中央加權中值法進一步處理
            Bitmap centralWeightedBitmap = ApplyCentralWeightedMedianFilter(edgeLaplacianBitmap, 5);

            //// 使用中值法進一步處理
            //Bitmap medianProcessedBitmap = ApplyMedianFilter(edgeLaplacianBitmap, 3);

            // 使用自適應二值化
            Bitmap adaptiveBitmap = ApplyAdaptiveThreshold(centralWeightedBitmap, 11, 2);

            //反轉
            Bitmap invertedBitmap = InvertBrightness(adaptiveBitmap);

            // 將結果轉回 BitmapSource
            return BitmapToSource(invertedBitmap);
        }
        // 將中值濾波結果作為遮罩應用到原圖

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

        //灰階反轉
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

        // 生成中央加權矩陣
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

        private Bitmap HighlightLongestLine(Bitmap binaryBitmap)
        {
            // 使用 Drawing.Color
            Drawing.Color white = Drawing.Color.White;

            // 定義方向向量（8 個方向）
            int[,] directions = new int[,]
            {
        { -1, -1 }, { 0, -1 }, { 1, -1 }, // 左上、上、右上
        { -1,  0 },          { 1,  0 },  // 左、中、右
        { -1,  1 }, { 0,  1 }, { 1,  1 }  // 左下、下、右下
            };

            int maxContinuity = 0;
            Drawing.Point startPoint = Drawing.Point.Empty;
            List<Drawing.Point> longestLinePoints = new List<Drawing.Point>(); // 使用 Drawing.Point

            HashSet<Drawing.Point> visited = new HashSet<Drawing.Point>();

            for (int y = 0; y < binaryBitmap.Height; y++)
            {
                for (int x = 0; x < binaryBitmap.Width; x++)
                {
                    if (binaryBitmap.GetPixel(x, y) == white && !visited.Contains(new Drawing.Point(x, y)))
                    {
                        Queue<(Drawing.Point, int)> queue = new Queue<(Drawing.Point, int)>();
                        queue.Enqueue((new Drawing.Point(x, y), -1));
                        visited.Add(new Drawing.Point(x, y));

                        List<Drawing.Point> currentLinePoints = new List<Drawing.Point>();
                        currentLinePoints.Add(new Drawing.Point(x, y));

                        int continuity = 0;

                        while (queue.Count > 0)
                        {
                            var (current, prevDirection) = queue.Dequeue();
                            continuity++;

                            for (int i = 0; i < directions.GetLength(0); i++)
                            {
                                int newX = current.X + directions[i, 0];
                                int newY = current.Y + directions[i, 1];

                                if (newX >= 0 && newY >= 0 && newX < binaryBitmap.Width && newY < binaryBitmap.Height)
                                {
                                    Drawing.Point newPoint = new Drawing.Point(newX, newY);

                                    if (binaryBitmap.GetPixel(newX, newY) == white && !visited.Contains(newPoint))
                                    {
                                        queue.Enqueue((newPoint, i));
                                        visited.Add(newPoint);
                                        currentLinePoints.Add(newPoint);
                                    }
                                }
                            }
                        }

                        if (continuity > maxContinuity)
                        {
                            maxContinuity = continuity;
                            startPoint = new Drawing.Point(x, y);
                            longestLinePoints = new List<Drawing.Point>(currentLinePoints);
                        }
                    }
                }
            }

            Bitmap outputBitmap = (Bitmap)binaryBitmap.Clone();
            using (Drawing.Graphics g = Drawing.Graphics.FromImage(outputBitmap))
            {
                using (Drawing.Pen redPen = new Drawing.Pen(Drawing.Color.Red, 2))
                {
                    for (int i = 0; i < longestLinePoints.Count - 1; i++)
                    {
                        g.DrawLine(redPen, longestLinePoints[i], longestLinePoints[i + 1]);
                    }
                }
            }

            return outputBitmap;
        }
    }

}