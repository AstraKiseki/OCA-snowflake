using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercismLeap
{
    public class Year
    {
        public bool LeapCheck(int theYear)
        {   
            if ((theYear % 4 == 0 && theYear % 100 != 0) || theYear % 400 == 0)
            {
                    return true;
            }      
            return false;
        }
    }
}
