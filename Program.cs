using System;
using System.IO;
using System.Collections.Generic;

namespace test
{
    class UI{
        public string filename;
        public List<string> file(){
            StreamReader sr = new StreamReader(filename);

            List<string> line = new List<string>();
            var readline = (sr.ReadLine());
            while(readline != null){
                line.Add(readline);
                readline = (sr.ReadLine());
            }
            return line;
        }
        public string find(List<string> item,string input){
            for (int i = 0; i < item.Count; i++)
            {
                var name = item[i].Split("=");
                if (name[0] == input)
                {
                    return name[1];
                }
            }
            return null;
        }
    }    class Program
    {
        # region 未來參數
        public string[,] Plan = new string[5, 39]; //放置方案陣列 
        public string[] Bearing = new string[7];  //放置軸承陣列
        public int Plannum = 0; //當前共幾個方案
        public int Plannow = 0; //現在顯示的是第幾個方案
        #endregion
        static void Main()
        {
            var read = new UI{
                filename = "spindle",
            };
            var line = read.file();

            Spindle spindle = new Spindle{
                d = Convert.ToDouble(read.find(line,"d")),
                a = Convert.ToDouble(read.find(line,"a")),
                b = Convert.ToDouble(read.find(line,"b")),
                c = Convert.ToDouble(read.find(line,"c")),
                E = Convert.ToDouble(read.find(line,"E")),
                ka = Convert.ToDouble(read.find(line,"ka")),
                rpm = Convert.ToDouble(read.find(line,"rpm")),
                p1 = Convert.ToDouble(read.find(line,"p1")),
                p2 = Convert.ToDouble(read.find(line,"p2")),
            };
            Bearing bearing1 = new Bearing{
                fr = Math.Abs(spindle.P1P2()[0]),
                type = Convert.ToInt32(read.find(line,"b1type")),
                arrange =Convert.ToInt32(read.find(line,"b1arrange")),
                angle = Convert.ToInt32(read.find(line,"b1angle")),
                c0 = Convert.ToDouble(read.find(line,"b1c0")),
                cs = Convert.ToDouble(read.find(line,"b1cs")),
                fv = Convert.ToDouble(read.find(line,"b1fv")),
                rigidity = Convert.ToDouble(read.find(line,"b1rigidity")),
            };
            Bearing bearing2 = new Bearing{
                fr = Math.Abs(spindle.P1P2()[1]),
                type = Convert.ToInt32(read.find(line,"b2type")),
                arrange =Convert.ToInt32(read.find(line,"b2arrange")),
                angle = Convert.ToInt32(read.find(line,"b2angle")),
                c0 = Convert.ToDouble(read.find(line,"b2c0")),
                cs = Convert.ToDouble(read.find(line,"b2cs")),
                fv = Convert.ToDouble(read.find(line,"b2fv")),
                rigidity = Convert.ToDouble(read.find(line,"b2rigidity")),
            };

            var TotalR = "總剛性：" + Math.Round(spindle.Rigidity(bearing1,bearing2),2).ToString() + "kg/um";

            var x = bearing1.xy(bearing1.realf(spindle.ka))[0];
            var y = bearing1.xy(bearing1.realf(spindle.ka))[1];
            var P = bearing1.P(x ,y ,bearing1.realf(spindle.ka));
            double life = Math.Round(bearing1.life(bearing1.C,P,spindle.rpm), 0);
            double life_year = hour2year(life);

            var x2 = bearing2.xy(bearing2.realf(spindle.ka))[0];
            var y2 = bearing2.xy(bearing2.realf(spindle.ka))[1];
            var P2 = bearing2.P(x2,y2,bearing2.realf(spindle.ka));
            double life2 = Math.Round(bearing2.life(bearing2.C,P2,spindle.rpm), 0);
            double life_year2 = hour2year(life2);
            
            Console.WriteLine("前軸承受力: "+ bearing1.fr);
            Console.WriteLine("後軸承受力: "+ bearing2.fr);
            Console.WriteLine(TotalR);
            Console.WriteLine("總壽命: "+spindle.Life(life_year,life_year2).ToString("0.0")+"年");
            Console.ReadKey();
            Console.Clear();
            Main();
        }
        
        
        private static double hour2year(double hour)  //壽命換算時間
        {
            double p1 = 24; //hours day
            double p2 = 365; //day year
            return hour / p1 / p2;
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
    
    class Spindle{
        public double d ;
        public double a ;
        public double b ;
        public double c ;
        public double E ;
        public double ka;
        public double p1 ;
        public double p2 ;

        public double rpm;

        public double Rigidity(Bearing bearing1 , Bearing bearing2) //主軸剛性計算
        {   
            double I = Math.PI *  (Math.Pow(d,4)) / 64;

            double K1 = 1000/9.81*bearing1.ComRigidity;
            double K2 = 1000/9.81*bearing2.ComRigidity;
            int p = 1;

            double ys = p * Math.Pow(a, 2) / 3 / E / I * (a + b)*1000;
            double yz = p*1000 / K1 * ((1 + K1 / K2) * Math.Pow(a, 2) / Math.Pow(b, 2) + 2 * a / b + 1);

            return (p / (ys + yz));
        }
        public double[] P1P2()
         {
            double bforce = (-p1 * (a + b) + p2 * c) / b; //前軸承
            double b1force = (p1 * a - p2 * (b + c)) / b; //後軸承
            
            double[] force = new double[]{bforce,b1force};
            return force;
        }
        public double Life(double y1,double y2) //總壽命計算
        {
            double p1 = Math.Pow(y1, 1.1);
            double p2 = Math.Pow(y2, 1.1);

            return 1 / (Math.Pow((1 / p1 + 1 / p2), 1 / 1.1));
        }
    }
    class Bearing{
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
        public double fr ; //徑向力
        public int type ; //軸承類型
        public int arrange ; //軸承配置
        public int angle ; //接觸角類型
        public double c0 ;//額定靜負荷
        public double cs ; //單顆額定動負荷
        public double fv; //預壓力
        public double rigidity; //剛性
        public int realnum{ //取得軸承數
            get{
                switch (arrange)
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
        }

        public double ComRigidity{
            get{
                double p1;
                double p2;
                switch (angle) //角度
                {
                case -1: //深溝軸承
                    p1 = 1;
                    break;
                case 0: //15度
                    p1 = 6;
                    break;
                case 1: //18度
                    p1 = 4;
                    break;
                default: //25度
                    p1 = 2;
                    break;
                }
                switch (arrange) //配置
                {
                case 0: //並聯
                    p2 = 1;
                    break;
                case 1: //DB
                    p2 = 1;
                    break;
                case 2: //DBD
                    p2 = 1.45;
                    break;
                default: //DBB
                    p2 = 2;
                    break;
                }
            return rigidity * p1 * p2;
            }            
        }
        public double realfv{ //取得正確的預壓力
            get{
                switch (arrange){
                case 2:
                    return fv * 1.35;
                case 3:
                    return fv * 2;
                default:
                    return fv;
                }
            }            
        }
        
        public double realf(double ka){ //軸向負荷計算
            if (ka > 3 * realfv)
            {
                return ka;
            }
            else
            {
                return realfv + 0.67 * ka;
            }
        }
        public double[] xy(double fa){
            double x ;
            double y;
            //i 軸承個數
            //fa 軸向負荷
            //c0 額定靜負荷
            //b 軸承類型
            //bran 軸承配置
            //a 接觸角類型
            //fr 徑向力
            if (arrange >1)
            {
                arrange = 1; //若為DBD或DBB 都使用DB/DF計算
            }

            double parameter = realnum * fa / c0; //相對軸向負荷

            if (type == 0) //主軸軸承
            {
                if (angle == 2) //25度接觸角計算
                {
                    int p1 = arrange;//第一維:配置
                    int p2;
                    if (fa / fr <= 0.68) { p2 = 0; } else { p2 = 1; } //第二維:參數

                    x = A_25[p1, p2, 0];
                    y = A_25[p1, p2, 1];
                }
                else if (angle == 1)//20度接觸角計算
                {
                    int p1 = arrange; //第一維:配置
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

                    int p1 = arrange;//軸承配置
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

                int p1 = arrange;//軸承配置
                int p2;
                if (fa / fr <= ee) { p2 = 0; } else { p2 = 1; }//大小
                x= DGX[p1, p2];
                y = DGY[p1, p2, kk];
            }
            double[] xy = new double[2]{x,y};
            return xy;
        }

        public double C{
            get{
                return Math.Pow(realnum, 0.7) * cs;
            }
        }
        public double P(double x,double y ,double fa) //當量的軸承負荷計算
        {
            return x * fr + y * fa;
        }
        public double life(double C , double P,double rpm){
            return 1000000 / 60 / rpm * Math.Pow(C / P, 3);
        }
    }

}
