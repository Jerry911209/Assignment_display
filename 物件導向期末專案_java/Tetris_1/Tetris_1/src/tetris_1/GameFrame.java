/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package tetris_1;

import java.awt.BorderLayout;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;   
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.Timer;
import java.util.TimerTask;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import static tetris_1.Tetris_1.main;

/**
 *
 * @author user
 */

 public class GameFrame extends JFrame{
	
    public GameFrame() {
        this.setTitle("Tetris");//設定標題
        this.setSize(700, 550);//設定尺寸
        this.setLayout(new BorderLayout());
        this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);//點選關閉按鈕是關閉程式
        this.setLocationRelativeTo(null);   //設定視窗顯示居中           
        this.setResizable(false); //不允許修改介面大小
        PanelGame game1=new PanelGame();
        this.setContentPane( game1);    //把PanelGame()設定成為frame的內容面板
        this.setVisible(true);
        

//        //鍵盤監聽
        addKeyListener(new KeyAdapter()
        {
            public void keyPressed(KeyEvent e)
            {
                int code = e.getKeyCode();
                switch(code){
                    case KeyEvent.VK_UP:
                        System.out.println("key_up");
                        game1.key_up();
                        break;
                    case KeyEvent.VK_DOWN:
                        System.out.println("key_down");
                        game1.key_down();
                        break;
                    case KeyEvent.VK_LEFT:
                        System.out.println("key_left");
                        game1.key_left();
                        break;                    
                    case KeyEvent.VK_RIGHT:
                        System.out.println("key_right");
                        game1.key_right();
                        break;                    
                    case KeyEvent.VK_C:
                        System.out.println("key_c");
                        game1.key_c();
                        break;                    
                    case KeyEvent.VK_SPACE:
                        System.out.println("key_space");
                        game1.key_space();
                        break;
                }

            }
        });
    }

    
    
    
    public void timer(GameFrame game) throws InterruptedException{

        Timer time=new Timer();
        //在特定 delay 後重複執行
        time.schedule(new TimerTask(){
            @Override
            public void run() {
               game.repaint();  //重繪
            }
        },1000,200);    //period -> 速率
    }
}   

     

