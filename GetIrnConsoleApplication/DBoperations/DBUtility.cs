using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.DBoperations
{
    class DBUtility
    {
        public static bool GetGSTINAuthenticationDetails(string GSTIN, out string strUsername, out string strAppKey, out string strEncryptedSek, out string strAuthToken)
        {
            DBOperation dBOperations = new DBOperation();
            return dBOperations.GetGSTINAuthenticationDetails(GSTIN, out strUsername, out strAppKey, out strEncryptedSek, out strAuthToken);
        }
    }
}
