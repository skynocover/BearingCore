using System;
using System.IO;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        #region 未來參數
        public string[,] Plan = new string[5, 39]; //放置方案陣列 
        public string[] Bearing = new string[7];  //放置軸承陣列
        public int Plannum = 0; //當前共幾個方案
        public int Plannow = 0; //現在顯示的是第幾個方案
        #endregion

        static void Main()
        {
            var read = new UI("spindle");
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
                Rpm = spindle.rpm,
                Ka = spindle.ka,
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
                Rpm = spindle.rpm,
                Ka = spindle.ka,
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

           
            double life = Math.Round(bearing1.life, 0);
            double life_year = hour2year(life);

            
            double life2 = Math.Round(bearing2.life, 0);
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

}
