using System;
using System.IO;
using System.Collections.Generic;

namespace test
{
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
}