using prbd_2324_a01.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a01.Utils
{
    public class StringBuilders
    {
        public static string GetDescription(Tricount tricount) {
            if (tricount.Description == null) {
                return "No Description";
            }
            return tricount.Description;
        }
    }
}
