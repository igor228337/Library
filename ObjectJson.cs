using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ObjectJson
    {
        public List<Dictionary<string, List<string>>> allDizain;
        public int load;
        public string login;
        public string password;
    }
    static public class ObjectJsonStatic
    {
        static public List<Dictionary<string, List<string>>> allDizain { get; set; }
        static public int load { get; set; }
        static public string login { get; set; }
        static public string password { get; set; }
    }
}
