using GetIrnConsoleApplication.DBoperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.AppClasses
{
    class Common
    {
        public static bool InsertExceptionIntoTable(string message, string stackTrace, int custid, int userid, string controllerName)
        {
           
            DBOperation dBOperations = new DBOperation();
            return dBOperations.InsertExceptionIntoTable(message, stackTrace, custid, userid, controllerName);
        }
        


    }
}
