using System;
using System.IO;
using System.Collections.Generic;

namespace test
{
    class UI{
        public string filename;
        public UI(string Filename){
            filename = Filename;
        }

        public List<string> file(){
            StreamReader sr = new StreamReader(filename);

            List<string> line = new List<string>();
            var readline = (sr.ReadLine());
            while(readline != null){
                line.Add(readline);
                readline = (sr.ReadLine());
            }
            sr.Close();
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
    }   
}