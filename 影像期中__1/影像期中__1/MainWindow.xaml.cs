using Microsoft.Win32; 
using System; 
using System.IO; 
using System.Windows; 
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 影像期中__1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Lode_image_Click(object sender, RoutedEventArgs e) // 載入圖片
        {
            // 打開檔案選取對話框
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                // 建立 BitmapImage 並載入圖片
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                // 將圖片顯示在 before_image 控件上
                before_image.Source = bitmap;
            }
        }
        private void save_before_image_Click(object sender, RoutedEventArgs e) // 存圖
        {
            if (after_image.Source is BitmapSource bitmapSource)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_processed.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                    }
                    MessageBox.Show("Image saved successfully!\nPath: " + saveFileDialog.FileName, "Save Image", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void Gray_transfer_Click(object sender, RoutedEventArgs e) // 灰階
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var grayBitmap = ConvertToGrayscale(bitmapSource);
                after_image.Source = grayBitmap;

                // 添加儲存按鈕
               // SaveImage(grayBitmap, "_grayscale");
            }
        }
        // 灰階化函數
        private BitmapSource ConvertToGrayscale(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            source.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte gray = (byte)(pixels[i] * 0.3 + pixels[i + 1] * 0.59 + pixels[i + 2] * 0.11);
                pixels[i] = gray;       // B
                pixels[i + 1] = gray;   // G
                pixels[i + 2] = gray;   // R
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        }

        private void channel_transfer_Click(object sender, RoutedEventArgs e) // 通道轉換
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var redChannel = ExtractChannel(bitmapSource, 2); // R 通道
                var greenChannel = ExtractChannel(bitmapSource, 1); // G 通道
                var blueChannel = ExtractChannel(bitmapSource, 0); // B 通道

                // 顯示三個通道圖像在三個不同的控件中
                R_image.Source = redChannel;
                G_image.Source = greenChannel;
                B_image.Source = blueChannel;

                //// 儲存三個通道圖像
                //SaveImage(redChannel, "_red_channel");
                //SaveImage(greenChannel, "_green_channel");
                //SaveImage(blueChannel, "_blue_channel");
            }
        }

        // 通道提取函數 (提取指定的通道: 0 = B, 1 = G, 2 = R)
        private BitmapSource ExtractChannel(BitmapSource source, int channelIndex)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            source.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                byte channelValue = pixels[i + channelIndex];
                pixels[i] = channelValue;       // B
                pixels[i + 1] = channelValue;   // G
                pixels[i + 2] = channelValue;   // R
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        }



        // 儲存圖像函數
        private void SaveImage(BitmapSource bitmapSource, string suffix)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + suffix + ".png";

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                }
                MessageBox.Show("Image saved successfully!\nPath: " + saveFileDialog.FileName, "Save Image", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void binarization_Click(object sender, RoutedEventArgs e) // 二值化
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                int threshold = (int)slider_255.Value;
                var binarizedBitmap = BinarizeImageManual(bitmapSource, threshold);
                after_image.Source = binarizedBitmap;
            }
        }

        private void slider_255_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)//用slider的值調整二值化的閥值
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                value.Text = Convert.ToString((int)e.NewValue);
                int threshold = (int)e.NewValue;
                var binarizedBitmap = BinarizeImageManual(bitmapSource, threshold);
                after_image.Source = binarizedBitmap;
            }
        }
        // 二值化函數
        private BitmapSource BinarizeImageManual(BitmapSource source, int threshold)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            source.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // 計算灰階值
                byte gray = (byte)(pixels[i] * 0.3 + pixels[i + 1] * 0.59 + pixels[i + 2] * 0.11);

                //閾值
                byte binaryValue = (gray >= threshold) ? (byte)255 : (byte)0;

                // 將每個通道設置為二值化結果
                pixels[i] = binaryValue;       // B
                pixels[i + 1] = binaryValue;   // G
                pixels[i + 2] = binaryValue;   // R
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, pixels, stride);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (after_image.Source is BitmapSource bitmapSource)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image|*.png";
                saveFileDialog.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_processed.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                    }
                    MessageBox.Show("Image saved successfully!\nPath: " + saveFileDialog.FileName, "Save Image", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Erode_Click(object sender, RoutedEventArgs e) // 侵蝕
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                int kernelSize = 9; // 可以更改為5或其他奇數大小
                var erodedBitmap = ErodeImage(bitmapSource, kernelSize);
                erodedBitmap = ErodeImage(erodedBitmap, kernelSize);
                after_image.Source = erodedBitmap;
            }
        }

        private void Dilate_Click(object sender, RoutedEventArgs e) // 擴張
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                int kernelSize = 9; // 可以更改為5或其他奇數大小
                var dilatedBitmap = DilateImage(bitmapSource, kernelSize);
                dilatedBitmap = DilateImage(dilatedBitmap, kernelSize);
                after_image.Source = dilatedBitmap;
            }
        }

        // 侵蝕函數，允許設定卷積核大小
        private BitmapSource ErodeImage(BitmapSource source, int kernelSize)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 複製原始圖像的像素數據
            source.CopyPixels(pixels, stride, 0);

            int offset = kernelSize / 2;

            // 遍歷每個像素，跳過邊界像素
            for (int y = offset; y < height - offset; y++)
            {
                for (int x = offset; x < width - offset; x++)
                {
                    int index = (y * stride) + (x * 4);
                    byte minValue = 255;

                    // 遍歷卷積核範圍，十字形結構元素
                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        if (ky == 0) // 如果是 y 軸為中心，則遍歷整行
                        {
                            for (int kx = -offset; kx <= offset; kx++)
                            {
                                int neighborIndex = ((y + ky) * stride) + ((x + kx) * 4);
                                byte value = pixels[neighborIndex]; // 二值化後的值只有 0 或 255
                                if (value < minValue)
                                {
                                    minValue = value;
                                }
                            }
                        }
                        else // 如果不是 y 軸為中心，則只取 x 為 0 的位置
                        {
                            int neighborIndex = ((y + ky) * stride) + (x * 4);
                            byte value = pixels[neighborIndex]; // 二值化後的值只有 0 或 255
                            if (value < minValue)
                            {
                                minValue = value;
                            }
                        }
                    }

                    // 將最小值賦給當前像素的 RGB 通道
                    resultPixels[index] = minValue;
                    resultPixels[index + 1] = minValue;
                    resultPixels[index + 2] = minValue;
                    resultPixels[index + 3] = 255; // Alpha channel
                }
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }


        private BitmapSource DilateImage(BitmapSource source, int kernelSize)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 複製原始圖像的像素數據
            source.CopyPixels(pixels, stride, 0);

            int offset = kernelSize / 2;

            // 遍歷每個像素，跳過邊界像素
            for (int y = offset; y < height - offset; y++)
            {
                for (int x = offset; x < width - offset; x++)
                {
                    int index = (y * stride) + (x * 4);
                    byte maxValue = 0;

                    // 遍歷卷積核範圍，十字形結構元素
                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        if (ky == 0) // 如果是 y 軸為中心，則遍歷整行
                        {
                            for (int kx = -offset; kx <= offset; kx++)
                            {
                                int neighborIndex = ((y + ky) * stride) + ((x + kx) * 4);
                                byte value = pixels[neighborIndex]; // 二值化後的值只有 0 或 255
                                if (value > maxValue)
                                {
                                    maxValue = value;
                                }
                            }
                        }
                        else // 如果不是 y 軸為中心，則只取 x 為 0 的位置
                        {
                            int neighborIndex = ((y + ky) * stride) + (x * 4);
                            byte value = pixels[neighborIndex]; // 二值化後的值只有 0 或 255
                            if (value > maxValue)
                            {
                                maxValue = value;
                            }
                        }
                    }

                    // 將最大值賦給當前像素的 RGB 通道
                    resultPixels[index] = maxValue;
                    resultPixels[index + 1] = maxValue;
                    resultPixels[index + 2] = maxValue;
                    resultPixels[index + 3] = 255; // Alpha channel
                }
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }

        private void Denoise_Click(object sender, RoutedEventArgs e) // 去雜訊 (中值法 kernel=3)
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var denoisedBitmap = DenoiseImageMedian(bitmapSource, 3);
                denoisedBitmap = DenoiseImageMedian(denoisedBitmap, 3);

                after_image.Source = denoisedBitmap;
            }
        }
        private BitmapSource DenoiseImageMedian(BitmapSource source, int kernelSize)//中執法
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            source.CopyPixels(pixels, stride, 0);

            int offset = kernelSize / 2;//定義卷積核的半徑

            for (int y = offset; y < height - offset; y++)
            {
                for (int x = offset; x < width - offset; x++)
                {
                    int index = (y * stride) + (x * 4);//BGRA 格式
                    List<byte> neighbors = new List<byte>();

                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        for (int kx = -offset; kx <= offset; kx++)
                        {
                            int neighborIndex = ((y + ky) * stride) + ((x + kx) * 4);
                            byte gray = (byte)(pixels[neighborIndex] * 0.3 + pixels[neighborIndex + 1] * 0.59 + pixels[neighborIndex + 2] * 0.11);//轉灰階
                            neighbors.Add(gray);
                        }
                    }

                    neighbors.Sort();//排序
                    byte medianValue = neighbors[neighbors.Count / 2];//中值

                    resultPixels[index] = medianValue;
                    resultPixels[index + 1] = medianValue;
                    resultPixels[index + 2] = medianValue;
                    resultPixels[index + 3] = 255;
                }
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }

        private void Sharpen_Click(object sender, RoutedEventArgs e) // 銳化 (拉普拉斯)
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var sharpenedBitmap = ApplyLaplaceSharpen(bitmapSource);
                after_image.Source = sharpenedBitmap;
            }
        }

        // 拉普拉斯銳化函數（適用於三通道）
        private BitmapSource ApplyLaplaceSharpen(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 複製原始圖像的像素數據
            source.CopyPixels(pixels, stride, 0);

            // 定義拉普拉斯卷積核
            int[,] laplaceKernel =
            {
        { 0, -1, 0 },
        { -1, 5, -1 },
        { 0, -1, 0 }
    };

            int offset = 1;

            // 遍歷每個像素，跳過邊界像素
            for (int y = offset; y < height - offset; y++)
            {
                for (int x = offset; x < width - offset; x++)
                {
                    int index = (y * stride) + (x * 4);
                    int blueValue = 0;
                    int greenValue = 0;
                    int redValue = 0;

                    // 遍歷卷積核範圍，計算卷積和
                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        for (int kx = -offset; kx <= offset; kx++)
                        {
                            int neighborIndex = ((y + ky) * stride) + ((x + kx) * 4);
                            blueValue += pixels[neighborIndex] * laplaceKernel[ky + offset, kx + offset];
                            greenValue += pixels[neighborIndex + 1] * laplaceKernel[ky + offset, kx + offset];
                            redValue += pixels[neighborIndex + 2] * laplaceKernel[ky + offset, kx + offset];
                        }
                    }

                    // 將顏色值限制在0到255之間
                    blueValue = Math.Clamp(blueValue, 0, 255);
                    greenValue = Math.Clamp(greenValue, 0, 255);
                    redValue = Math.Clamp(redValue, 0, 255);

                    // 將最終結果賦值給當前像素的 RGB 通道
                    resultPixels[index] = (byte)blueValue;
                    resultPixels[index + 1] = (byte)greenValue;
                    resultPixels[index + 2] = (byte)redValue;
                    resultPixels[index + 3] = 255; // Alpha channel
                }
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }


        private void edge_Click(object sender, RoutedEventArgs e) // 邊緣檢測 (Sobel)
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var edgeBitmap = ApplySobelEdgeDetection(bitmapSource);
                after_image.Source = edgeBitmap;
            }
        }

        // Sobel 邊緣檢測函數
        private BitmapSource ApplySobelEdgeDetection(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 複製原始圖像的像素數據
            source.CopyPixels(pixels, stride, 0);

            // 定義 Sobel 卷積核
            int[,] sobelX =
            {
        { -1, 0, 1 },
        { -2, 0, 2 },
        { -1, 0, 1 }
    };

            int[,] sobelY =
            {
        { -1, -2, -1 },
        { 0, 0, 0 },
        { 1, 2, 1 }
    };

            int offset = 1;

            // 遍歷每個像素，跳過邊界像素
            for (int y = offset; y < height - offset; y++)
            {
                for (int x = offset; x < width - offset; x++)
                {
                    int index = (y * stride) + (x * 4);
                    int gradientX = 0;
                    int gradientY = 0;

                    // 計算 Sobel 卷積和
                    for (int ky = -offset; ky <= offset; ky++)
                    {
                        for (int kx = -offset; kx <= offset; kx++)
                        {
                            int neighborIndex = ((y + ky) * stride) + ((x + kx) * 4);
                            byte gray = (byte)(pixels[neighborIndex] * 0.3 + pixels[neighborIndex + 1] * 0.59 + pixels[neighborIndex + 2] * 0.11);
                            gradientX += gray * sobelX[ky + offset, kx + offset];
                            gradientY += gray * sobelY[ky + offset, kx + offset];
                        }
                    }

                    // 計算梯度強度
                    int gradient = (int)Math.Sqrt((gradientX * gradientX) + (gradientY * gradientY));
                    gradient = Math.Clamp(gradient, 0, 255);
                    byte finalValue = (byte)gradient;

                    // 亮度反轉
                    byte invertedValue = (byte)(255 - finalValue);

                    // 將最終結果賦值給當前像素的 RGB 通道
                    resultPixels[index] = invertedValue;
                    resultPixels[index + 1] = invertedValue;
                    resultPixels[index + 2] = invertedValue;
                    resultPixels[index + 3] = 255; // Alpha channel
                }
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }

        private void Histogram_Click(object sender, RoutedEventArgs e) // 直方圖等化
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
                var equalizedBitmap = EqualizeHistogram(bitmapSource);
                after_image.Source = equalizedBitmap;
            }
        }

        // 直方圖等化函數（3 通道）
        private BitmapSource EqualizeHistogram(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 複製原始圖像的像素數據
            source.CopyPixels(pixels, stride, 0);

            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];

            // 統計pixel
            for (int i = 0; i < pixels.Length; i += 4)
            {
                histogramB[pixels[i]]++;
                histogramG[pixels[i + 1]]++;
                histogramR[pixels[i + 2]]++;
            }

            // 計算累積
            int[] cdfR = new int[256];
            int[] cdfG = new int[256];
            int[] cdfB = new int[256];

            cdfR[0] = histogramR[0];
            cdfG[0] = histogramG[0];
            cdfB[0] = histogramB[0];

            for (int i = 1; i < 256; i++)
            {
                cdfR[i] = cdfR[i - 1] + histogramR[i];
                cdfG[i] = cdfG[i - 1] + histogramG[i];
                cdfB[i] = cdfB[i - 1] + histogramB[i];
            }

            // 算CDF加直方圖均衡化
            for (int i = 0; i < pixels.Length; i += 4)
            {
                resultPixels[i] = (byte)((cdfB[pixels[i]] - cdfB[0]) * 255 / (width * height - cdfB[0]));
                resultPixels[i + 1] = (byte)((cdfG[pixels[i + 1]] - cdfG[0]) * 255 / (width * height - cdfG[0]));
                resultPixels[i + 2] = (byte)((cdfR[pixels[i + 2]] - cdfR[0]) * 255 / (width * height - cdfR[0]));
                resultPixels[i + 3] = 255; // Alpha channel
            }

            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }

        private void Spin_circles_Click(object sender, RoutedEventArgs e) // 轉圈圈
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {

                var rotatedBitmap = RotateImageWithBilinearInterpolation(bitmapSource, 150);

                value_circles.Text = Convert.ToString(Convert.ToInt32(150));

                after_image.Source = rotatedBitmap;
            }
        }


        private BitmapSource RotateImageWithBilinearInterpolation(BitmapSource source, double angle)//轉圈圈
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];
            byte[] resultPixels = new byte[height * stride];

            // 角度轉為弧度
            double radians = angle * Math.PI / 180.0;

            // 計算旋轉的 cos 和 sin 值
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);

            // 圖像的中心點
            int centerX = width / 2;
            int centerY = height / 2;

           
            source.CopyPixels(pixels, stride, 0);

           


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 計算中心點的偏移量
                    double offsetX = x - centerX;
                    double offsetY = y - centerY;

                    // 使用旋轉矩陣
                    double originalX = cosTheta * offsetX - sinTheta * offsetY + centerX;
                    double originalY = sinTheta * offsetX + cosTheta * offsetY + centerY;

                    
                    int nearestX = (int)Math.Round(originalX);
                    int nearestY = (int)Math.Round(originalY);

                    // 確保位置在圖像範圍內
                    if (nearestX >= 0 && nearestX < width && nearestY >= 0 && nearestY < height)
                    {
                        for (int channel = 0; channel < 4; channel++)
                        {
                            resultPixels[(y * stride) + (x * 4) + channel] = pixels[(nearestY * stride) + (nearestX * 4) + channel];
                        }
                    }
                }
            }
            

            

            // 創建旋轉後的 BitmapSource
            return BitmapSource.Create(width, height, source.DpiX, source.DpiY, PixelFormats.Bgra32, null, resultPixels, stride);
        }


        private void circles_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)//轉圈圈slider
        {
            if (before_image.Source is BitmapSource bitmapSource)
            {
               
                var rotatedBitmap = RotateImageWithBilinearInterpolation(bitmapSource, circles_Slider.Value);//轉圈圈
                DenoiseImageMedian(bitmapSource, 3);//中值綠波
                value_circles.Text=Convert.ToString(Convert.ToInt32(circles_Slider.Value));

                after_image.Source = rotatedBitmap;
            }
        }
    }
}
