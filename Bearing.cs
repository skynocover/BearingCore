using System;
using System.IO;
using System.Collections.Generic;

namespace test
{
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
        private double ka;
        public double Ka{
            set{
                ka=value;
            }
            get{
                return ka;
            }
        }
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
        public double realka{ //軸向負荷計算

            get{
                 if (ka > 3 * realfv)
            {
                return ka;
            }
            else
            {
                return realfv + 0.67 * ka;
            }
            }

        }

        public double[] xy{
            get{
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

                double parameter = realnum * realka / c0; //相對軸向負荷

                if (type == 0) //主軸軸承
                {
                    if (angle == 2) //25度接觸角計算
                    {
                        int p1 = arrange;//第一維:配置
                        int p2;
                        if (realka / fr <= 0.68) { p2 = 0; } else { p2 = 1; } //第二維:參數

                        x = A_25[p1, p2, 0];
                        y = A_25[p1, p2, 1];
                    }else if (angle == 1)//20度接觸角計算
                    {
                        int p1 = arrange; //第一維:配置
                        int p2;
                        if (realka / fr <= 0.57) { p2 = 0; } else { p2 = 1; }//第二維:參數

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
                        if (realka / fr <= ee) { p2 = 0; } else { p2 = 1; }//大小
                        x = A_15X[p1, p2];
                        y = A_15Y[p1, p2, kk];
                    }
                }else //深溝軸承
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
                    if (realka / fr <= ee) { p2 = 0; } else { p2 = 1; }//大小
                    x= DGX[p1, p2];
                    y = DGY[p1, p2, kk];
                }
            return new double[2]{x,y};
            }
        }
        public double C{
            get{
                return Math.Pow(realnum, 0.7) * cs;
            }
        }
        public double P{ //當量的軸承負荷計算
            get{
                var xyarr = xy;
                return xyarr[0] * fr + xyarr[1] * realka;
            }
        }
        private double rpm;
        public double Rpm{
            get{
                return Rpm;
            }
            set{
                rpm = value;
            }
        }
        public double life{
            get{
                 return 1000000 / 60 / rpm * Math.Pow(C / P, 3);
            }
           
        }
    }
}