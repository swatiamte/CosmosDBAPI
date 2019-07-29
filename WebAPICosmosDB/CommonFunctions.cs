using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICosmosDB
{
    public class CommonFunctions
    {
        public string ConvertDecToBinary(int val)
        {

            int num = Convert.ToInt32(val);
            string result = "";
            while (num > 1)
            {
                int remainder = num % 2;
                result = Convert.ToString(remainder) + result;
                num /= 2;
            }
            result = Convert.ToString(num) + result;
            return result;
        }


    }
}