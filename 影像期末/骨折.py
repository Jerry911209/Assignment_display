import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk
import cv2
import numpy as np
from datetime import datetime

class ImageViewer:
    
    def __init__(self, root):
        self.root = root
        self.root.title("圖片處理程式")

        # 設置視窗的大小
        root.geometry("1000x800")

        # 創建一個Label用於顯示圖片，將寬高設為600x800，邊框為2px，邊框顏色為黑色
        self.image_label = tk.Label(self.root, bd=2, relief="solid", width=500, height=750, highlightbackground="black")
        self.image_label.place(x=0, y=0, width=500, height=750)

        # 創建兩個按鈕
        self.load_button = tk.Button(self.root, text="載入圖片", command=self.load_image)
        self.process_button = tk.Button(self.root, text="處理圖片", command=self.process_image)

        # 使用 pack 或 place 配置
        self.load_button.place(x=530, y=30)  # 相對於視窗底部中央
        self.process_button.place(x=530, y=80)  # 相對於視窗底部中央

    def load_image(self):
        # 打開文件對話框選擇圖片文件
        self.file_path = filedialog.askopenfilename(filetypes=[("Image files", "*.png;*.jpg;*.jpeg;*.gif")])
        self.image_uplode()

    def process_image(self):
        name = "Sobel邊緣檢測+Morphing"
        if self.file_path:
            # 打開圖像文件
            original_image = Image.open(self.file_path)

            # 將原始圖像轉換為灰度圖像
            gray_image = original_image.convert('L')

            save_image=gray_image
            
            # 高斯濾波
            blurred_image = self.apply_gaussian_blur(gray_image)

            # 使用Sobel邊緣檢測器
            sobel_image = self.sobel_edge_detection(blurred_image)
            
            #sharpening_image=self.apply_sharpening(original_image)

            # 進行Morphing的變形
            morphed_image = self.morph_image(original_image, sobel_image)
            morphed_np_array = np.array(morphed_image)

            #骨頭
            bone_image=self.toke_Bone(original_image)
            bone_np_array = np.array(bone_image)

            

            #自適應直方圖等話
            
            # 二值化
            binary_image = morphed_image.point(lambda p: 255 if p > 115 else 0)
            binary_image_np_array=np.array(binary_image)
            
            #拿骨頭和2值化相加
            morphed_and_toke_bone= cv2.addWeighted(bone_np_array, 0.6,binary_image_np_array,1, 20)
            morphed_and_toke_bone_image = Image.fromarray(morphed_and_toke_bone)
            
            #直方圖等
            #adaptive_histogram =cv2.equalizeHist(morphed_and_toke_bone)
            #adaptive_histogram_image=Image.fromarray(adaptive_histogram)
            
            adaptive_histogram=self.apply_adaptive_histogram(morphed_and_toke_bone_image)
            adaptive_histogram_image=adaptive_histogram
            # 二值化
            binary_image = adaptive_histogram_image.point(lambda p: 255 if p > 105 else 0)
            binary_image_np_array=np.array(binary_image)
            
            # 擴張
            dilated_image = self.apply_dilation(binary_image, iterations=5)
            
            # 侵蝕
            eroded_image = self.apply_erosion(dilated_image, iterations=5)
            
            
            
            # 連通區域標記並畫方形
            
            #原圖
            result_image_orange = self.label_and_draw_rectangles(original_image, eroded_image)
            #2值化的圖
            result_image_two_ware= self.label_and_draw_rectangles(eroded_image, eroded_image)

            # 將結果圖像轉換為PIL格式
            result_image_orange_pil = Image.fromarray(result_image_orange)
            result_image_two_ware_pil = Image.fromarray(result_image_two_ware)


            # 使用當前時間作為檔案名稱
            current_time = datetime.now().strftime("%Y%m%d%H%M%S")
            save_path = f"C:/Users/user/Desktop/骨折/處理後photo/{name}_{current_time}.png"
            
            # 保存結果圖像                  
            #圖像    
            save_image=result_image_two_ware_pil
            
            save_image.save(save_path)

            self.file_path = save_path
            self.image_uplode()

    # 邊緣檢測
    def sobel_edge_detection(self, gray_image):
        # 轉換為 NumPy 陣列
        gray_array = np.array(gray_image)

        # 使用Sobel邊緣檢測器
        sobel_x = cv2.Sobel(gray_array, cv2.CV_64F, 1, 0, ksize=3)
        sobel_y = cv2.Sobel(gray_array, cv2.CV_64F, 0, 1, ksize=3)

        # 計算邊緣強度和方向
        edge_strength = np.sqrt(sobel_x**2 + sobel_y**2)
        edge_direction = np.arctan2(sobel_y, sobel_x)

        # 歸一化到範圍[0, 255]
        edge_strength_normalized = ((edge_strength - np.min(edge_strength)) / (np.max(edge_strength) - np.min(edge_strength))) * 255

        # 轉換為灰度圖像
        edge_image = Image.fromarray(edge_strength_normalized.astype(np.uint8))

        return edge_image
    
    #拿骨頭
    def toke_Bone(self, original_image):
        
         # 二值化
            binary_image = original_image.point(lambda p: 255 if p > 115 else 0)

            # 將二值化圖像轉換為 NumPy 陣列
            binary_array = np.array(binary_image)

            # 創建遮罩，保留二值化後為1的區域
            mask_array = binary_array == 255

            # 將原始圖像中不在二值化後為1的區域的像素過濾為0
            result_array = np.array(original_image)
            result_array[~mask_array] = 0

            # 將結果陣列轉換回 PIL 圖像
            result_image = Image.fromarray(result_array)
            # 边缘检测
            cv_image = cv2.cvtColor(np.array(result_array), cv2.COLOR_RGB2BGR)
            edges = cv2.Canny(cv_image, 100, 150)  # 调整阈值可以影响检测结果

             # 将边缘检测结果转回PIL图像
            edges_image = Image.fromarray(edges)
            
            dilated_image = self.apply_dilation(edges_image, iterations=2)
            eroded_image = self.apply_erosion(dilated_image, iterations=2)

            return eroded_image 
        
    # 高斯
    def apply_gaussian_blur(self, image):
        # 轉換為 NumPy 陣列
        image_array = np.array(image)

        # 高斯濾波
        blurred_array = cv2.GaussianBlur(image_array, (5, 5), 0)

        # 轉換回PIL圖像
        blurred_image = Image.fromarray(blurred_array)

        return blurred_image

    # 變形
    def morph_image(self, original_image, edge_image):
        # 將原始圖像和邊緣圖像轉換為 NumPy 陣列
        original_array = np.array(original_image)
        edge_array = np.array(edge_image)

        # 進行Morphing的變形
        morphed_array = cv2.addWeighted(original_array, 0.7, edge_array, 0.3, 0)

        # 轉換回PIL圖像
        morphed_image = Image.fromarray(morphed_array)

        return morphed_image
    # 新增自適應直方圖均衡化方法
    def apply_adaptive_histogram(self, image):
        # 轉換為 NumPy 陣列
        image_array = np.array(image)

        # 自適應直方圖均衡化
        clahe = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(8, 8))
        equalized_array = clahe.apply(image_array)

        # 轉換回PIL圖像
        equalized_image = Image.fromarray(equalized_array)

        return equalized_image
    
    # 新增邊緣增強方法
    def enhance_edges(self, image):
        # 轉換為 NumPy 陣列
        image_array = np.array(image)

        # 增強邊緣
        enhanced_array = cv2.equalizeHist(image_array)

        # 轉換回PIL圖像
        enhanced_image = Image.fromarray(enhanced_array)

        return enhanced_image
    
    # 新增直方圖等化方法
    def apply_histogram_equalization(self, image):
        # 轉換為 NumPy 陣列
        image_array = np.array(image)

        # 直方圖等化
        equalized_array = cv2.equalizeHist(image_array)

        # 轉換回PIL圖像
        equalized_image = Image.fromarray(equalized_array)

        return equalized_image
 # 擴張
    def apply_dilation(self, binary_image, iterations=1):
        binary_array = np.array(binary_image)

        # 使用方形的核心
        kernel = np.ones((3, 3), np.uint8)

        # 進行擴張
        dilated_array = cv2.dilate(binary_array, kernel, iterations=iterations)

        # 轉換回PIL圖像
        dilated_image = Image.fromarray(dilated_array)

        return dilated_image
    
    # 侵蝕
    def apply_erosion(self, binary_image, iterations=1):
        binary_array = np.array(binary_image)

        # 使用方形的核心
        kernel = np.ones((3, 3), np.uint8)

        # 進行侵蝕
        eroded_array = cv2.erode(binary_array, kernel, iterations=iterations)

        # 轉換回PIL圖像
        eroded_image = Image.fromarray(eroded_array)

        return eroded_image
    
    # 連通區域標記並畫方形
    def label_and_draw_rectangles(self, original_image, binary_image):
        # 連通區域標記
        labeled_image, num_labels, stats, centroids = cv2.connectedComponentsWithStats(np.array(binary_image), connectivity=8)

        # 提取總標籤數量（包括背景）
        num_labels = int(stats.shape[0])  # 使用 stats 陣列的行數

        # 轉換為3通道的彩色影像
        colored_image = cv2.cvtColor(np.array(original_image), cv2.COLOR_RGB2BGR)

        # 在每個不同標籤中間點為中心畫一個100*100的方形
        square_size = 100
        for i in range(1, num_labels):  # 從1開始，跳過背景標籤
            center_x, center_y = int(centroids[i][0]), int(centroids[i][1])
            x1 = max(0, center_x - square_size // 2)
            y1 = max(0, center_y - square_size // 2)
            x2 = min(colored_image.shape[1], center_x + square_size // 2)
            y2 = min(colored_image.shape[0], center_y + square_size // 2)
            # 畫紅色矩形
            cv2.rectangle(colored_image, (x1, y1), (x2, y2), (0, 0, 255), 2)  # 注意這裡使用 (0, 0, 255) 代表紅色
            cv2.circle(colored_image, (center_x, center_y), 5, (255, 0, 0), -1)  # 注意這裡使用 (255, 0, 0) 代表藍色

        return colored_image
    # 新增銳化方法
    def apply_sharpening(self, image):
        # 轉換為 NumPy 陣列
        image_array = np.array(image)

        # 定義銳化濾波器核心
        kernel = np.array([[-1, -1, -1],
                        [-1, 9, -1],
                        [-1, -1, -1]])

        # 使用濾波器進行銳化
        sharpened_array = cv2.filter2D(image_array, -1, kernel)

        # 轉換回PIL圖像
        sharpened_image = Image.fromarray(sharpened_array)

        return sharpened_image


    def image_uplode(self):
        if self.file_path:
            # 打開圖像文件
            original_image = Image.open(self.file_path)

            # 計算新的寬度和高度，保持寬高比，同時限制最大寬高
            base_width = min(original_image.width, 500)
            base_height = min(original_image.height, 750)

            # 縮小圖像，使用 Image.BICUBIC 替代 Image.ANTIALIAS
            resized_image = original_image.resize((base_width, base_height), Image.BICUBIC)

            # 將PIL圖像轉換為Tkinter PhotoImage對象
            tk_image = ImageTk.PhotoImage(resized_image)

            # 調整Label的大小以符合原始圖片的寬高比
            self.image_label.configure(image=tk_image, bd=0, highlightthickness=0)
            self.image_label.image = tk_image
    

if __name__ == "__main__":
    root = tk.Tk()
    app = ImageViewer(root)
    root.mainloop()
