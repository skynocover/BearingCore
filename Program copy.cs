using System;

namespace test2
{
    class Program2
    {
        static void Main2(string[] args)
        {
            d = 80;
            a = 100;
            b = 200;
            c = 100;
            E = 21000;

            Angle1 = 0;
            Angle2 = 0;
            Arrange1 = 3;
            Arrange2 = 1;
            p1 =539;
            p2 = 3867;

            P1P2_calculate();
            Rigidity_calculate();
            
            Ka = 500;
            //前軸承
            fr_1=fr;//徑向力
            b_1=0; //軸承類型
            bran_1 = 0; //軸承配置
            a_1=0; //接觸角類型
            c0_1=10000;//額定靜負荷
            cs_1=10000;//單顆額定動負荷
            b_con=2; //軸承個數
            fv=1000;//預壓力 
            //後軸承
            fr_2=fr1;//徑向力
            b_2=0; //軸承類型
            bran_2 = 0; //軸承配置
            a_2=0; //接觸角類型
            c0_2=10000;//額定靜負荷
            cs_2=10000;//單顆額定動負荷
            b_con1=2; //軸承個數
            fv1=1000;//預壓力 

            CP_calculate();
            Life_calculate();
            
            Console.WriteLine("fr="+ fr);
            Console.WriteLine("fr1="+fr1);
            Console.WriteLine(TotalR);
            Console.WriteLine(life);
            Console.WriteLine(life_year);
        }
        //input
        //共用參數
        public static double Ka ; //軸向力     
        //前軸承輸入參數
        public static double fr_1 ; //徑向力
        public static int b_1 ; //軸承類型
        public static int bran_1 ; //軸承配置
        public static int a_1 ; //接觸角類型
        public static double c0_1 ;//額定靜負荷
        public static double cs_1 ; //單顆額定動負荷
        public static int b_con;
        public static double fv;
        //後軸承輸入參數
        public static double fr_2 ; //徑向力
        public static int b_2 ; //軸承類型
        public static int bran_2 ; //軸承配置
        public static int a_2 ; //接觸角類型
        public static double c0_2 ;//額定靜負荷
        public static double cs_2 ; //單顆額定動負荷
        public static int b_con1;
        public static double fv1;




        public static double d ;
        public static double a;
        public static double b;
        public static double c;
        public static double E;
        public static int Angle1;
        public static int Angle2;
        public static int Arrange1 ;
        public static int Arrange2 ;
        public static double p1;
        public static double p2;


        #region 宣告
        //輸出
        public static double life;
        public static double life_year;
        public static string totallife;
        public static string TotalR;
        public static double fr;
        public static double fr1;



        public string[,] Plan = new string[5, 39]; //放置方案陣列 
        public string[] Bearing = new string[7];  //放置軸承陣列
        public int Plannum = 0; //當前共幾個方案
        public int Plannow = 0; //現在顯示的是第幾個方案
       
        //前軸承輸出
        public static double P_1;//徑向當量動負荷
        public static double C_1;//額定動負荷
        //後軸承輸出
        public static double P_2;//徑向當量動負荷
        public static double C_2;//額定動負荷
        #endregion

        #region 參數
        //失效概率
        public static double[] A1 = new double[6] { 0.25, 0.37, 0.47, 0.55, 0.64, 1 };
        //最大運行溫度
        public static double[] Ft = new double[4] { 1, 0.73, 0.42, 0.22 };
        //汙染係數
        public static double[,] Ec = new double[2, 7] { { 1,0.7,0.55,0.4,0.2,0.05,0},{1,0.85,0.75,0.5,0.3,0.05,0 } };

        //參考值
        public static double[] A_15 = new double[9] { 0.015, 0.029, 0.058, 0.087, 0.12, 0.17, 0.29, 0.44, 0.58 };
        public static double[] DG = new double[9] { 0.014, 0.028, 0.056, 0.085, 0.11, 0.17, 0.28, 0.42, 0.56 };

        //e值
        public static double[] Aee = new double[9] { 0.38, 0.4, 0.43, 0.46, 0.47, 0.5, 0.55, 0.56, 0.56 };
        public static double[] DGee = new double[9] { 0.23, 0.26, 0.3, 0.34, 0.36, 0.4, 0.45, 0.5, 0.52 };

        //XY參數
        public static double[,] A_15X = new double[2, 2] { { 1, 0.44 }, { 1, 0.72 } };
        public static double[,,] A_15Y = new double[2, 2, 9] { { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 1.47, 1.4, 1.3, 1.23, 1.19, 1.12, 1.02, 1, 1 } },
                                                        { { 1.65, 1.57, 1.46, 1.38, 1.34, 1.26, 1.14, 1.12, 1.12 }, { 2.39, 2.28, 2.11, 2, 1.93, 1.82, 1.66, 1.63, 1.63 } } };
        public static double[,,] A_20 = new double[2, 2, 2] { { { 1, 0 }, { 0.43, 1 } }, { { 1, 1.09 }, { 0.7, 1.63 } } };
        public static double[,,] A_25 = new double[2, 2, 2] { { { 1, 0 }, { 0.41, 0.87 } }, { { 1, 0.92 }, { 0.67, 1.41 } } };
        public static double[,] DGX = new double[2, 2] { { 1, 0 }, { 1, 0.78 } };
        public static double[,,] DGY = new double[2, 2, 9] { { { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 2.3, 1.99, 1.71, 1.55, 1.45, 1.31, 1.15, 1.04, 1 } },
                                                      { { 2.78, 2.4, 2.07, 1.87, 1.75, 1.58, 1.39, 1.26, 1.21 }, { 3.74, 3.23, 2.78, 2.52, 2.36, 2.13, 1.87, 1.69, 1.63 } } };
        #endregion

        #region 主功能        
        private static void CP_calculate() //CP計算
        {
            //共用參數
            double Ka = 50; //軸向力

            #region 前軸承    
            double fv_1 = Calculate_fv(fv, bran_1);
            int i_1 = Getnum(b_con);
            //前軸承計算
            double x_1;  //x係數
            double y_1;  //y係數
            double fa_1 =  Calculate_F(Ka,fv_1); //軸向負荷計算
            Calculate_XY(i_1, fa_1, c0_1, b_1, bran_1, a_1, fr_1, out x_1, out y_1); //XY係數計算
            C_1 =Calculate_C(i_1, cs_1); //額定動負荷計算
            P_1 = Calculate_P(x_1,y_1,fr_1,fa_1); //當量軸承負荷計算
            #endregion

            #region 後軸承
            
            double fv_2 = Calculate_fv(fv1, bran_2); //預壓力 
            int i_2 = Getnum(b_con1); //軸承個數
            //後軸承計算
            double x_2;  //x係數
            double y_2;  //y係數
            double fa_2 = Calculate_F(Ka,fv_2); //軸向負荷計算
            Calculate_XY(i_2, fa_2, c0_2, b_2, bran_2, a_2, fr_2, out x_2, out y_2); //XY係數計算
            C_2 = Calculate_C(i_2, cs_2); //額定動負荷計算
            P_2 = Calculate_P(x_2, y_2, fr_2, fa_2); //當量軸承負荷計算
            #endregion

            
        }
        private static void Life_calculate() //壽命計算
        {
            double Rpm = Convert.ToDouble(500);//轉速

            //前軸承計算
            double life_1 = 1000000 / 60 / Rpm * Math.Pow(C_1 / P_1, 3); ; //額定壽命計算
            double year_1 = Calculate_life(life_1); //換算年
           
            life = Math.Round(life_1, 0);//顯示基本額定壽命(小時)
            life_year=Math.Round(year_1, 2); //輸出年
            //後軸承計算
            double life_2 = 1000000 / 60 / Rpm * Math.Pow(C_2 / P_2, 3); ; //額定壽命計算
            double year_2 = Calculate_life(life_2); //換算年
            /*
            if (uselife2.IsChecked == true) 
            {
                double aiso = Convert.ToDouble(life_p1.Text);
                double life2_2 = life_2 * aiso * Ft[Tempture.SelectedIndex] * A1[lost_chance.SelectedIndex];
                life3.Text = Math.Round(life2_2, 0).ToString();
                year_2 = Calculate_life(life2_2);
            }
            life1.Text = Math.Round(life_2, 0).ToString();//顯示基本額定壽命(小時)
            Life_year1.Text = Math.Round(year_2, 2).ToString(); //輸出年
            */
            totallife = "總壽命："+ Math.Round(Calculate_totallife(year_1, year_2),1).ToString() + "年";
        } 
        private static double Calculate_totallife(double y1,double y2) //總壽命計算
        {
            double p1 = Math.Pow(y1, 1.1);
            double p2 = Math.Pow(y2, 1.1);

            return 1 / (Math.Pow((1 / p1 + 1 / p2), 1 / 1.1));
        }
        private static double Calculate_life(double hour)  //壽命換算時間
        {
            double p1 = 24; //hours day
            double p2 = 365; //day year
            return hour / p1 / p2;
        }
        private static void Calculate_XY(int i, double fa, double c0, int b,int bran, int a, double fr, out double x, out double y) //XY係數
        {
            //i 軸承個數
            //fa 軸向負荷
            //c0 額定靜負荷
            //b 軸承類型
            //bran 軸承配置
            //a 接觸角類型
            //fr 徑向力
            if (bran >1)
            {
                bran = 1;
            }

            double parameter = i * fa / c0; //相對軸向負荷

            if (b == 0) //主軸軸承
            {
                if (a == 2) //25度接觸角計算
                {
                    int p1 = bran;//第一維:配置
                    int p2;
                    if (fa / fr <= 0.68) { p2 = 0; } else { p2 = 1; } //第二維:參數

                    x = A_25[p1, p2, 0];
                    y = A_25[p1, p2, 1];
                }
                else if (a == 1)//20度接觸角計算
                {
                    int p1 = bran; //第一維:配置
                    int p2;
                    if (fa / fr <= 0.57) { p2 = 0; } else { p2 = 1; }//第二維:參數

                    x = A_20[p1, p2, 0];   //第三維第一項:X
                    y = A_20[p1, p2, 1]; //第三維第二項:Y
                }
                else //15度接觸角計算
                {
                    //抓出最接近的數字
                    double min = 999;
                    int kk = 0;
                    for (int ii = 0; ii < 9; ii++)
                    {
                        if (Math.Abs(parameter - A_15[ii]) < min)
                        {
                            min = Math.Abs(parameter - A_15[ii]);
                            kk = ii;
                        }
                    }
                    //抓出ee值
                    double ee = Aee[kk];

                    int p1 = bran;//軸承配置
                    if (p1 > 1)
                    {
                        p1 = 1;
                    }
                    int p2;
                    if (fa / fr <= ee) { p2 = 0; } else { p2 = 1; }//大小
                    x = A_15X[p1, p2];
                    y = A_15Y[p1, p2, kk];
                }
            }
            else //深溝軸承
            {
                //抓出最接近的數字
                double min = 999;
                int kk = 0;
                for (int ii = 0; ii < 9; ii++)
                {
                    if (Math.Abs(parameter - DG[ii]) < min)
                    {
                        min = Math.Abs(parameter - DG[ii]);
                        kk = ii;
                    }
                }
                //抓出ee值
                double ee = DGee[kk];

                int p1 = bran;//軸承配置
                int p2;
                if (fa / fr <= ee) { p2 = 0; } else { p2 = 1; }//大小
                x= DGX[p1, p2];
                y = DGY[p1, p2, kk];
            }
        }
        private static double Calculate_C(double ii, double cs) //額定動負荷計算
        {
            return Math.Pow(ii, 0.7) * cs;
        }
        private static double Calculate_P(double x,double y , double fr,double fa) //當量的軸承負荷計算
        {
            return x * fr + y * fa;
        }
        private static double Calculate_F(double ka, double fv) //軸向負荷計算
        {
            if (ka > 3 * fv)
            {
                return ka;
            }
            else
            {
                return fv + 0.67 * ka;
            }
        }
        private static double Calculate_fv(double fv, int arrange) //取得正確的預壓力
        {
            switch (arrange)
            {
                case 2:
                    return fv * 1.35;
                case 3:
                    return fv * 2;
                default:
                    return fv;
            }
        }
        private static int Getnum(int index) //取得軸承數
        {
            switch (index)
            {
                case 0:
                    return 2;
                case 1:
                    return 2;
                case 2:
                    return 3;
                default:
                    return 4;
            }
        }
        private static void All_calculate() //P1P2計算 CP計算 life計算 剛性計算
        {
            P1P2_calculate();
            CP_calculate();
            Life_calculate();
            Rigidity_calculate();
        }
        #endregion
         private static void Rigidity_calculate()
        {   
            double I = Math.PI *  (Math.Pow(d,4)) / 64;

            double K1 = 1000/9.81*calculate_BR(Angle1,Arrange1 , Convert.ToDouble(159));
            double K2 = 1000/9.81*calculate_BR(Angle2,Arrange2, Convert.ToDouble(140));
            int p = 1;

            double ys = p * Math.Pow(a, 2) / 3 / E / I * (a + b)*1000;
            double yz = p*1000 / K1 * ((1 + K1 / K2) * Math.Pow(a, 2) / Math.Pow(b, 2) + 2 * a / b + 1);
            TotalR = "總剛性：" + Math.Round(p / (ys + yz), 2).ToString() + "kg/um";
        }
        private static double calculate_BR(int angle , int arrange , double rigidity)//軸承剛性計算
        {
            double p1;
            double p2;
            switch (angle) //角度
            {
                case -1:
                    p1 = 1;
                    break;
                case 0:
                    p1 = 6;
                    break;
                case 1:
                    p1 = 4;
                    break;
                default:
                    p1 = 2;
                    break;
            }
            switch (arrange) //配置
            {
                case 0:
                    p2 = 1;
                    break;
                case 1:
                    p2 = 1;
                    break;
                case 2:
                    p2 = 1.45;
                    break;
                default:
                    p2 = 2;
                    break;
            }
            return rigidity * p1 * p2;
        }

         private static void P1P2_calculate()
        {
            double bforce = (-p1 * (a + b) + p2 * c) / b; //前軸承
            double b1force = (p1 * a - p2 * (b + c)) / b; //後軸承

            fr = Math.Round(bforce, 1);
            fr1 = Math.Round(b1force, 1);
        }
    /*
        #region 額外壽命
        private void Aiso_calculate() //壽命調整係數計算
        {
            CP_calculate();
            //前軸承
            //輸入
            double v1_1 = Convert.ToDouble(v1.Text); //前軸承參考黏度            
            double dm_1 = Convert.ToDouble(dm.Text); //前軸承dm
            double c0_1 = Convert.ToDouble(c0.Text); //前軸承靜負荷
            //計算
            double k_1 = k_calculate(v1_1); //黏度比計算
            double eccup_1 = eccup_calculate(dm_1, c0_1, P_1); //前軸承eccup計算
            //顯示
            k.Text = Math.Round(k_1, 2).ToString();
            eccup.Text = Math.Round(eccup_1, 2).ToString();

            //後軸承
            //輸入
            double v1_2 = Convert.ToDouble(v3.Text); //前軸承參考黏度            
            double dm_2 = Convert.ToDouble(dm1.Text); //前軸承dm
            double c0_2 = Convert.ToDouble(c01.Text); //前軸承靜負荷
            //計算
            double k_2 = k_calculate(v1_2); //黏度比計算
            double eccup_2 = eccup_calculate(dm_2, c0_2, P_2); //前軸承eccup計算
            //顯示
            k1.Text = Math.Round(k_2, 2).ToString();
            eccup1.Text = Math.Round(eccup_2, 2).ToString();
        }
        private double k_calculate(double v1) //黏度比計算
        {
            double p1 = Math.Log(Convert.ToDouble(v40.Text) / Convert.ToDouble(v100.Text), Math.E);
            double p2 = (1948.1 / (Convert.ToDouble(tb.Text) + 273) - 6.22);
            double p3 = Math.Pow(Math.E, p2 * p1);
            double v = Convert.ToDouble(v40.Text) * p3;

            return v / v1;
        }
        private double eccup_calculate(double dm, double c0, double p) //調整係數計算
        {
            double cu;
            double ec;
            if (dm <= 100)
            {
                cu = c0 / 22;
                ec = Ec[0, clean.SelectedIndex];
            }
            else
            {
                cu = c0 / 22 * (Math.Pow(100 / dm, 0.5));
                ec = Ec[1, clean.SelectedIndex];
            }
            return ec * cu / p;
        }
        private void checkornot(bool b)//確認是否使用延長壽命
        {
            lost_chance.IsEnabled = b;
            Tempture.IsEnabled = b;
            clean.IsEnabled = b;
            life_p.IsEnabled = b;
            life2.IsEnabled = b;
            life2.Text = "";
            life3.IsEnabled = b;
           life3.Text = "";
            v40.IsEnabled = b;
            v100.IsEnabled = b;
            tb.IsEnabled = b;
            v1.IsEnabled = b;
            v3.IsEnabled = b;
            k.IsEnabled = b;
            eccup.IsEnabled = b;
            life_p.IsEnabled = b;
            k1.IsEnabled = b;
            eccup1.IsEnabled = b;
            life_p1.IsEnabled = b;
            aisoC_btm.IsEnabled = b;

        }
        #endregion*/
    }
}
