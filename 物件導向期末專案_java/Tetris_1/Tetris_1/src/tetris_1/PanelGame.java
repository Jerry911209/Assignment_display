/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package tetris_1;

//import static com.sun.java.accessibility.util.AWTEventMonitor.addKeyListener;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.event.KeyEvent;
//import java.awt.event.KeyListener;
import javax.swing.JPanel;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import javax.swing.JFrame;


/**
 *
 * @author user
 */
public class PanelGame extends JPanel{
    
//    int x=400,y=0,change=0,paint=0;
    int x=0,y=0,change=0,now_x=0;
    int now_x_Index=0,now_y_Index=0;
    int block_height = 0;   //方塊列高
    int block_weight = 0;   //方塊行寬
    int bottom_y_Index = 0; 
    int DeleteLine = 0; //消除行數
    int block_topline = 0;  //方塊頂邊高度    
    int block_bottomline = 0;   //方塊底邊高度    
    int block_leftline = 0;  //方塊左邊界
    int block_rightline = 0;   //方塊右邊界    
    int block_bottomline_weight = 0;   //方塊底邊寬度    
    int block_bottomline_FirstIndex = 0;   //方塊底邊Index


    int bg_block[][]=new int [20][15];  //背景格子
//    public final int  bg_block[][]={
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},	
//        {0,0,0,0,0,	0,0,0,0,0,	0,0,0,0,0},
//        {0,0,0,0,0,	0,0,0,0,0,	1,1,1,1,1},	
//        {1,1,1,0,0,	1,1,1,1,1,	1,1,1,1,1}};
    int block[][]=new int[4][4];
    int rand=(int)(Math.random()*7);
//    int rand=0;   //測試用
    int block_PreTemp=-1;
    int block_NewTemp=-1;
    int block_count=1;
    block b=new block();      
    
    
    
//    @Override
    public void paintComponent(Graphics g)
    {
        Set_BlockType();    //設定方塊造型
        Draw_BackgroundLine(g); //繪製背景線條
        BlockHeight_Compute();  //計算方塊高度
        BlockWeight_Compute();  //計算方塊寬度
        
        
        block_topline = y;
        block_bottomline = y + block_height;
        block_leftline = x;
        block_rightline = x + block_weight;

////////////////////////////////////////////////////////////////////////////////
        //關閉程式
        for(int i=0;i<15;i++)
        {
            if(bg_block[0][i]==1)
            {
                System.exit(0);
            }

        }


        //繪製方塊-16格為一組
        for(int i=0;i<16;i++,x++)
        {   
            //移動至下一列
            if(i % 4 == 0)  
            {
                if(i!=0){   y ++;   }   
                x = block_leftline;  //x恢復預設值
            }
            
            //方塊超出範圍
            if(block_rightline > 15 )
            {
                x=15-block_weight;
                block_leftline=x;
                break;
            }
            else if(block_bottomline > 20)
            {
                Draw_NewOne();  //繪製新方塊
                break;
            }
            //此格為方塊的一部分
            else if(block[change][i] == 1)
            {
                g.setColor(Color.red);  //設定方塊顏色
                g.fillRect(200+x*20, 20+y*20,20, 20); //繪製實心方塊
//                g.setColor(Color.yellow);   //設定方塊邊線顏色
//                g.drawRect(x,y,20,20);   //繪製空心方框
//                block_bottomline = y+20;  //紀錄方塊bottomline的高度
            }
        }   
        y = block_topline+1;    //方塊落下 
        x = block_leftline;  //x恢復預設值

        check_record();
        Draw_bg_block(g);   //繪製已存在之方塊
    }
    
    //鍵盤動作
    public void key_up()
    {
        change ++;
        if(change%4==0){
            change=0;
        }
    }
    public void key_down() 
    {
        if(block_bottomline<20)
        {
            y++;
//            System.out.println("block_bottomline="+block_bottomline);
        }
    }
    public void key_left() 
    {
        if(block_leftline > 0)
        {
            x--;
            now_x_Index--;  
//            block_leftline--;
//            block_rightline--;
        }
    }
    public void key_right() 
    {
        
        if(block_rightline < 19)
        {   
            x++;
            now_x_Index++; 
//            block_leftline++;
//            block_rightline++;
        }
    }
    public void key_c() 
    {
        if(block_PreTemp==-1)
        {
            block_PreTemp = rand;
            rand=(int)(Math.random()*7);    //隨機產生新樣式
        }
        else
        {
            block_NewTemp = rand;
            rand = block_PreTemp;
            block_PreTemp = block_NewTemp;
        }
    }    
    public void key_space() 
    {
    }
    
    //計算方塊高度
    public void BlockHeight_Compute()
    {
        //高4
        if(!(block[change][12] == 0 && block[change][13] == 0 && block[change][14] == 0 && block[change][15] == 0))
        {   block_height = 4;   }
        //高3
        else if(!(block[change][8] == 0 && block[change][9] == 0 && block[change][10] == 0 && block[change][11] == 0))
        {   block_height = 3;   }
        //高2
        else if(!(block[change][4] == 0 && block[change][5] == 0 && block[change][6] == 0 && block[change][7] == 0))
        {   block_height = 2;   }
        //高1
        else
        {   block_height = 1;   }
    }
    
    //計算方塊寬度
    public void BlockWeight_Compute()
    {
        //寬4
        if(!(block[change][3] == 0 && block[change][7] == 0 && block[change][11] == 0 && block[change][15] == 0))
        {   block_weight = 4;   }
        //寬3
        else if(!(block[change][2] == 0 && block[change][6] == 0 && block[change][10] == 0 && block[change][14] == 0))
        {   block_weight = 3;   }
        //寬2
        else if(!(block[change][1] == 0 && block[change][5] == 0 && block[change][9] == 0 && block[change][13] == 0))
        {   block_weight = 2;   }
        //寬1
        else
        {   block_weight = 1;   }
    }
            
    //設定方塊造型
    public void Set_BlockType()
    {
        switch(rand)
        {
            case 0:
                block=b.Iblock;
                break;
            case 1:
                block=b.Jblock;
                break;
            case 2:
                block=b.Lblock;
                break;
            case 3:
                block=b.Tblock;
                break;
            case 4:
                block=b.xblock;
                break;
            case 5:
                block=b.xzblock;
                break;
            case 6:
                block=b.zblock;
                break;
        }
    }
    
    //繪製背景線條
    public void Draw_BackgroundLine(Graphics g)
    {
        //繪製外框線
        g.drawLine(180, 0, 180, 440);   //left line
        g.drawLine(520, 0, 520, 440);   //right line
        g.drawLine(180, 440, 520, 440);   //bottom line
        g.drawLine(180, 0, 520, 0);   //top line

        //繪製背景格子
        int x1=200,y1=20;
        for(int i=0;i<20;i++)
        {
            for(int j=0;j<15;j++)
            {
                g.drawRect(x1,y1,20,20); //繪製矩形
                x1=x1+20;
            }
            y1=y1+20;
            x1=200;
        }
    }
    
    //繪製新方塊
    public void Draw_NewOne()
    {
        System.out.println("next shape");
        rand=(int)(Math.random()*7);    //隨機樣式
//        rand=0;    //測試
        y=0;    //高度重置
//        x-=3;   //x回到原位---DELETE
        
        BlockHeight_Compute();  //計算方塊高度
        BlockWeight_Compute();  //計算方塊寬度
        
        block_topline = y;
        block_bottomline = y + block_height;
        block_leftline = x;
        block_rightline = x + block_weight;
        
        block_count++;
    }
    
    //檢查是否要被記錄
    public void check_record()
    {
        now_x_Index=0;        
        now_y_Index=0;

        //  恰好落到底
        if(block_bottomline == 20) 
        {
            block_record(); //紀錄此落下方塊最後位置
            Draw_NewOne();  //繪製新方塊
        }          
        else if(block_bottomline < 20)
        {
            switch(block_height)
            {
                case 1: block_bottomline = y +2;    break;
                case 2: block_bottomline = y +3;    break;
                case 3: block_bottomline = y +4;    break;
                case 4: block_bottomline = y +5;    break;                
            }
//            BlockBottomlineWeight_Compute();    //計算方塊底邊寬度 

            switch(block_weight)
            {
                case 1:
                    if(bg_block[block_bottomline-2][block_leftline]==1)
                    {
                        block_record(); //紀錄此落下方塊最後位置
                        Draw_NewOne();  //繪製新方塊
                    }
                    break;
                case 2:
                    if(change==0&&rand==1||change==0&&rand==2||rand==4){
                        if(bg_block[block_bottomline-2][block_leftline]==1||bg_block[block_bottomline-2][block_leftline+1]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(change==2&&rand==1||change==1&&rand==6||change==3&&rand==6){
                        if(bg_block[block_bottomline-2][block_leftline]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(change==2&&rand==2||change==1&&rand==3||change==3&&rand==3||change==1&&rand==5||change==3&&rand==5){
                        if(bg_block[block_bottomline-2][block_leftline+1]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    break;
                case 3:
                    if(change==3&&rand==1 || change==1&&rand==2||change==3&&rand==5||change==0&&rand==3)
                    {
                        if(bg_block[block_bottomline-2][block_leftline]==1 || bg_block[block_bottomline-2][block_leftline+1]==1 ||bg_block[block_bottomline-2][block_leftline+2]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(rand==5){
                        if(bg_block[block_bottomline-2][block_leftline]==1 || bg_block[block_bottomline-2][block_leftline+1]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(rand==6){
                        if(bg_block[block_bottomline-2][block_leftline+1]==1 || bg_block[block_bottomline-2][block_leftline+2]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(change==1&&rand==1){
                        if(bg_block[block_bottomline-2][block_leftline+2]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(change==3&&rand==3){
                        if(bg_block[block_bottomline-2][block_leftline+1]==1)
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    else if(change==3&&rand==2){
                        if(bg_block[block_bottomline-2][block_leftline]==1 )
                        {
                            block_record(); //紀錄此落下方塊最後位置
                            Draw_NewOne();  //繪製新方塊
                        }
                    }
                    break;
                case 4:
                    if(bg_block[block_bottomline-2][block_leftline]==1 || bg_block[block_bottomline-2][block_leftline+1]==1 ||bg_block[block_bottomline-2][block_leftline+2]==1 || bg_block[block_bottomline-2][now_x_Index+3]==1  )
                    {
                        block_record(); //紀錄此落下方塊最後位置
                        Draw_NewOne();  //繪製新方塊
                    }
                    break;             
            }
            
        }
    }
    
    //紀錄此落下方塊最後位置
    public void block_record()
    {
        Show_bg_block();
        for(int Index=0;Index<16;Index++)
            {
                if(Index % 4 == 0)  
                {
                    if(Index!=0){   now_y_Index ++; }   //移動至下一列
                    now_x_Index = x - (Index%4);  //x恢復預設值
                }
                if(block[change][Index] == 1)
                {
                    bg_block[block_topline +now_y_Index][block_leftline+now_x_Index-x]=1;
                    System.out.println("\n\nblock_record()!\nblock_count="+block_count);
                    System.out.println("bg_block["+(block_topline +now_y_Index)+"]["+(block_leftline+now_x_Index-x)+"]=1");
                    System.out.println("block_record()\tEND\n\n");
                }
                now_x_Index++;
            }
    }
    
    //繪製已存在之方塊
    public void Draw_bg_block(Graphics g)
    {
        for(int count=0,r=0;r<20;r++)
        {
            count=0;
            for(int c=0;c<15;c++)
            {
                if(bg_block[r][c]==1)
                {
                    g.setColor(Color.blue);  //設定方塊顏色
                    g.fillRect(20*c+200,20*r+20,20, 20); //繪製實心方塊
                    count++;
                }
                if(count==15)
                {
                    for(int clear=0;clear<15;clear++)
                    {
                        bg_block[r][clear]=0;
                        g.clearRect(20*c+200,20*r+20,20, 20); //清除實心方塊
                    }
                    DeleteLine++;   //消除行數+1
                    for(int row=r;row>=0;row--)
                    {
                        for(int column=0;column<15;column++)
                        {
                            bg_block[row][column]=bg_block[row-1][column];
                        }
                    }
                }
            }
        }
    }
    
    //printf方塊紀錄
    public void Show_bg_block()
    {
        System.out.print("\nbg_block:\n");
        for(int r=0;r<20;r++)
        {
            for(int c=0;c<15;c++)
            {
                System.out.print(bg_block[r][c]+",");
                if(c==4||c==9||c==14){
                    System.out.print("\t");
                }
            }
            System.out.print("\n");
        }
        System.out.print("\n\n\n\n\n");
    }
     
    //計算方塊底邊寬度 
    public void BlockBottomlineWeight_Compute()        
    {
        int count=0;
        for(int i=0;i<4;i++)
        {
            if(block[block_bottomline][block_leftline+i]==1)
            {   
                count++;
                if(count==1){   block_bottomline_FirstIndex=count;  }    }  //方塊底邊Index
        }
        
        switch(count){
            case 1:
                block_bottomline_weight=1;
                break;
            case 2:
                block_bottomline_weight=2;
                break;
            case 3:
                block_bottomline_weight=3;
                break;
            case 4:
                block_bottomline_weight=4;
                break;
        }
    }
    
    
   
}








































































