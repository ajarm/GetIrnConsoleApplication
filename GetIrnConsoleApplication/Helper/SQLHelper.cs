using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication.Helper
{
    class SQLHelper
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        public static DataTable dt = new DataTable();


        public SQLHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static int FindStringPosition(string x, char FindChar, int NoofOccurance, int Direction) //Direction :0-leftToRight 1-RightToLeft
        {
            //string s="";
            int Occurance = 1, CharPosition = 0;

            x.ToCharArray();
            if (Direction == 0)
            {
                for (int i = 0; i <= x.Length - 1; i++)
                {
                    if ((x[i] == FindChar) && (Occurance <= NoofOccurance))
                    {
                        Occurance++;
                        CharPosition = (i + 1);
                    }
                }
            }
            else if (Direction == 1)
            {

                for (int i = x.Length - 1; i >= 0; i--)
                {
                    if ((x[i] == FindChar) && (Occurance <= NoofOccurance))
                    {
                        Occurance++;
                        CharPosition = (i + 1);
                    }
                }

            }
            return CharPosition;
        }


        public static string mid(string x, int startlength, int length)
        {
            string s = "";
            x.ToCharArray();
            for (int i = startlength - 1; i <= startlength - 1 + length - 1; i++)
                s += x[i];
            return s;
        }


        public static string left(string x, int length)
        {
            string s = "";
            x.ToCharArray();
            for (int i = 0; i <= length - 1; i++)
                s += x[i];
            return s;
        }


        public static string right(string x, int length)
        {
            string s = "";
            x.ToCharArray();
            for (int i = x.Length - length; i <= x.Length - 1; i++)
                s += x[i];
            return s;
        }


        public static string SqlDate(string IndianDate)
        {
            string day = "", Month = "", Year = "";
            day = SQLHelper.left(IndianDate, SQLHelper.FindStringPosition(IndianDate, '/', 1, 0) - 1);
            Month = SQLHelper.mid(IndianDate, SQLHelper.FindStringPosition(IndianDate, '/', 1, 0) + 1, SQLHelper.FindStringPosition(IndianDate, '/', 2, 0) - (SQLHelper.FindStringPosition(IndianDate, '/', 1, 0) + 1));
            Year = SQLHelper.right(IndianDate, IndianDate.Length - SQLHelper.FindStringPosition(IndianDate, '/', 2, 0));
            return Month + "/" + day + "/" + Year;
        }





        public static string InsertIntoTableWithIdentity(string table, ArrayList FieldName, ArrayList FieldValue, ArrayList FieldSeparator)
        {
            string Query = "";
            SqlConnection con = SQLHelper.con();
            Query = "Insert into " + table + "(";
            int i;
            for (i = 0; i <= FieldName.Count - 2; i++)
                Query += FieldName[i] + ",";
            Query += FieldName[i] + ") values (";
            for (i = 0; i <= FieldValue.Count - 2; i++)
            {
                Query += FieldSeparator[i];
                Query += FieldValue[i];
            }
            Query += FieldSeparator[i];
            Query += FieldValue[i];
            Query += FieldSeparator[i + 1];

            Query += ";Select SCOPE_IDENTITY()";

            //		Query += FieldSeparator[i]+ FieldValue[i] + "')";
            //
            SqlCommand com = new SqlCommand(Query);
            com.Connection = con;
            //com.CommandText =Query;
            com.Connection.Open();
            string IdentityColumnValue = com.ExecuteScalar().ToString();
            com.Connection.Close();

            return IdentityColumnValue;
            //    return Query;





        }

        public static string InsertIntoTable(string table, ArrayList FieldName, ArrayList FieldValue, ArrayList FieldSeparator)
        {
            string Query = "";

            Query = "Insert into " + table + "(";
            int i;
            for (i = 0; i <= FieldName.Count - 2; i++)
                Query += FieldName[i] + ",";
            Query += FieldName[i] + ") values (";
            for (i = 0; i <= FieldValue.Count - 2; i++)
            {
                Query += FieldSeparator[i];
                Query += FieldValue[i];
            }
            Query += FieldSeparator[i];
            Query += FieldValue[i];
            Query += FieldSeparator[i + 1];

            //Query += ")" ; 

            //		Query += FieldSeparator[i]+ FieldValue[i] + "')";
            //
            SqlCommand com = new SqlCommand(Query, SQLHelper.con());
            com.Connection.Open();
            com.ExecuteNonQuery();
            com.Connection.Close();
            return Query;
        }

        public static void EINV_AUTH_DataAdapter(string strGSTINNo, out string strUsername, out string strEncAppKey, out string strEncpassword, out bool strForceRefreshToken)
        {
            string Username = "", EncryptedAppKey = "", EncryptedPassword = "";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlDataAdapter adt = new SqlDataAdapter("Select * from TBL_AUTH_KEYS_EINVOICE where GSTIN = '" + strGSTINNo + "'", con);
                DataTable dt = new DataTable();
                adt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Username = dt.Rows[0]["Username"].ToString();
                    EncryptedAppKey = dt.Rows[0]["Appkey"].ToString();
                    EncryptedPassword = dt.Rows[0]["Password"].ToString();
                    //ForceRefreshToken = (bool)dt.Rows[0]["Force_Refresh_Accesstoken"];
                }
                con.Close();
            }
            strUsername = Username;
            strEncAppKey = EncryptedAppKey;
            strEncpassword = EncryptedPassword;
            strForceRefreshToken = false;
        }

        public static string InsertIntoTable(string table, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "", ParameterValues = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                ParameterNames += sp.ParameterName.Replace("@", "") + ",";
                ParameterValues += sp.ParameterName + ",";
                // ParameterValues += sp.Value + ",";
            }
            com.CommandText = "Insert into " + table + " (" + ParameterNames.Remove(ParameterNames.Length - 1, 1) + ") values (" + ParameterValues.Remove(ParameterValues.Length - 1, 1) + "); Select SCOPE_IDENTITY()";
            string IdentityColumnValue = com.ExecuteScalar().ToString();
            return IdentityColumnValue;

        }

        private static object threadLock = new object();
        internal static void InsertIntoTableWithLock(string table, SqlCommand com)
        {
            lock (threadLock)
            {
                try
                {
                    //string sqlQuery = "insert into Test Values (GetDate())";

                    string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        #region commented

                        com.Connection = connection;
                        string ParameterNames = "", ParameterValues = "";
                        foreach (SqlParameter sp in com.Parameters)
                        {
                            ParameterNames += sp.ParameterName.Replace("@", "") + ",";
                            ParameterValues += sp.ParameterName + ",";
                            // ParameterValues += sp.Value + ",";
                        }
                        com.CommandText = "Insert into " + table + " (" + ParameterNames.Remove(ParameterNames.Length - 1, 1) + ") values (" + ParameterValues.Remove(ParameterValues.Length - 1, 1) + "); Select SCOPE_IDENTITY()";
                        string IdentityColumnValue = com.ExecuteScalar().ToString();

                        #endregion
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        public static string InsertIntoTable(string Query)
        {
            SqlConnection con = SQLHelper.con();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            if (Query.Length > 0)
            {
                com.Connection.Open();
                com.CommandText = Query;
                string IdentityColumnValue = com.ExecuteScalar().ToString();
                com.Connection.Close();
                con.Close();
                return IdentityColumnValue;
            }
            else
                return "";
        }

        public static void UpdateTable(string table, string fieldname, string fieldvalue, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where " + fieldname + "= '" + fieldvalue + "'";
            com.ExecuteNonQuery();
        }
        public static void UpdateEWBTable(string table, string fieldname, string fieldvalue, string fieldname1, string fieldvalue1, string fieldname2, string fieldvalue2, string fieldname3, string fieldvalue3, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where " + fieldname + "= '" + fieldvalue + "' and " + fieldname1 + "= '" + fieldvalue1 + "' and " + fieldname2 + "= '" + fieldvalue2 + "' and " + fieldname2 + "= '" + fieldvalue2 + "'";
            com.ExecuteNonQuery();
        }
        public static void UpdateTable(string table, string fieldname, long? fieldvalue, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where " + fieldname + "= '" + fieldvalue + "'";
            com.ExecuteNonQuery();
        }

        public static void UpdateTable(string table, string docnum, string docdate, string doctype, string suptyp, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where DocNo='" + docnum + "'and DocDt='" + docdate + "' and DocType='" + doctype + "'and SupTyp='" + suptyp + "'";
            com.ExecuteNonQuery();
        }
        public static void UpdateEwbTable(string table, string ewbid, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where EwbId='" + ewbid + "'";
            com.ExecuteNonQuery();
        }

        public static void UpdateTable(string table, string einvoiceid, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            //ParameterNames = ParameterNames.Replace("@", "");
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where Einvoiceid='" + einvoiceid + "'";
            com.ExecuteNonQuery();
        }

        public static void InsertInTable(string table, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName +/*.Replace("@", "") + " =" + sp.ParameterName +*/ ",";
            }
            //ParameterNames = ParameterNames.Replace("@", "");
            com.CommandText = "INSERT INTO " + table + "(" + ParameterNames.Remove(ParameterNames.Length - 1, 1).Replace("@", "") + ")VALUES (" + ParameterNames.Remove(ParameterNames.Length - 1, 1) + ")";
            com.ExecuteNonQuery();
        }

        //public static void UpdateTableModified(string table, string fieldname, string fieldvalue, SqlCommand command, SqlConnection con)
        //{
        //    string ParameterNames = "";
        //    command.Connection = con;
        //    foreach (SqlParameter sp in command.Parameters)
        //    {
        //        //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
        //        ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
        //    }
        //    command.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where " + fieldname + "= '" + fieldvalue + "'";
        //    command.ExecuteNonQuery();
        //}




        public static void UpdateTable(string query)
        {
            SqlConnection con = SQLHelper.con();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.Connection.Open();
            com.CommandText = query;
            com.ExecuteNonQuery();
            com.Connection.Close();
            con.Close();
        }






        public static DataTable GetData(string table, string fieldname, string fieldvalue, bool searchYesNo)//search is yes or no option
        {
            //cmd.CommandText = "select * from SalesRequest where SalesRequestID = " + SalesID; 
            string query = "select * from " + table + " where DeleteFlag='N'";
            SqlConnection con1 = SQLHelper.con();
            con1.Open();
            if (fieldvalue != "*")
            {
                if (searchYesNo == false)
                {
                    query += " and " + fieldname + "= '" + fieldvalue + "'";
                }
                else if (searchYesNo == true)
                {
                    query += " and " + fieldname + " like '%" + fieldvalue + "%' ";
                }
            }
            //query = query + query;
            SqlDataAdapter adap = new SqlDataAdapter(query, con1);
            DataTable ds = new DataTable();
            adap.Fill(ds);
            con1.Close();
            return ds;
        }



        public static DataTable GetData(string query)
        {
            SqlConnection con1 = SQLHelper.con();
            con1.Open();
            SqlDataAdapter adap = new SqlDataAdapter(query, con1);
            DataTable ds = new DataTable();
            adap.Fill(ds);
            con1.Close();
            return ds;
        }




        public static string GetScalarvalue(string IDColumn, string table, string fieldname, string fieldvalue)
        {
            SqlCommand com = new SqlCommand();
            com.CommandText = "select " + IDColumn + " from " + table + " where " + fieldname + "= '" + fieldvalue + "'";// and  DeleteFlag='N'";
            SqlConnection con = SQLHelper.con();
            com.Connection = con;
            com.Connection.Open();
            string s = com.ExecuteScalar().ToString();
            com.Connection.Close();
            return s;
        }

        //Con
        public static SqlConnection con()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            return con;
        }
        // function for Getting the PO Reference no from database 

        //public static bool Rights(DataTable Dt,int AccessLevelId,int LinkId)
        //{
        //    //int val=0;
        //    //SqlCommand com = new SqlCommand("select AccessLinkId from AccessLinks where DeleteFlag='N' and AccessNameId=" + AccessLevelId +" and Linkid=" + LinkId,SQLHelper.con());
        //    DataRow []dr = Dt.Select("LinkId=" + LinkId);
        //    //com.Connection.Open();
        //    //val= Convert.ToInt32(com.ExecuteScalar());
        //    if (dr.Length>0)
        //        return true;
        //    else
        //        return false;
        //    //com.Connection.Close();
        //    //if (val!=0)				

        //}
        public static void UpdateGetTable(string table, string fieldname, string fieldvalue, SqlCommand com, SqlConnection con)
        {
            com.Connection = con;
            string ParameterNames = "";
            foreach (SqlParameter sp in com.Parameters)
            {
                //   ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName.Replace(sp.ParameterName, "?") + ",";
                ParameterNames += sp.ParameterName.Replace("@", "") + " =" + sp.ParameterName + ",";
            }
            com.CommandText = "Update " + table + " set " + ParameterNames.Remove(ParameterNames.Length - 1, 1) + " where " + fieldname + "= '" + fieldvalue + "'";
            com.ExecuteNonQuery();
        }

        public static bool Rights(DataTable Dt, int LinkId)
        {
            //int val=0;
            //SqlCommand com = new SqlCommand("select AccessLinkId from AccessLinks where DeleteFlag='N' and AccessNameId=" + AccessLevelId +" and Linkid=" + LinkId,SQLHelper.con());
            DataRow[] dr = Dt.Select("LinkId=" + LinkId);
            //com.Connection.Open();
            //val= Convert.ToInt32(com.ExecuteScalar());
            if (dr.Length > 0)
                return true;
            else
                return false;
            //com.Connection.Close();
            //if (val!=0)				

        }


        public static bool ModuleRights(DataTable Dt, int GroupId)
        {

            DataRow[] dr = Dt.Select("GroupId=" + GroupId);

            if (dr.Length > 0)
                return true;
            else
                return false;
        }


        public static bool RightsBackUp(int AccessLevelId, int LinkId)
        {
            int val = 0;
            SqlCommand com = new SqlCommand("select AccessLinkId from AccessLinks where DeleteFlag='N' and AccessNameId=" + AccessLevelId + " and Linkid=" + LinkId, SQLHelper.con());
            com.Connection.Open();
            val = Convert.ToInt32(com.ExecuteScalar());
            com.Connection.Close();
            if (val != 0)
                return true;
            else
                return false;

        }





        public static DataSet GetDetails1(string StoredProcedureName, string Requestid)
        {
            SqlConnection con = SQLHelper.con();
            SqlCommand com = new SqlCommand(StoredProcedureName, con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add("@id", SqlDbType.Int);
            com.Parameters["@id"].Value = Requestid;
            DataSet ds = new DataSet();
            SqlDataAdapter apt = new SqlDataAdapter(com);
            dt.Clear();
            apt.Fill(ds);
            con.Close();
            com.Dispose();
            return ds;
        }
        public static DataTable GetDetailsWOP(string StoredProcedureName)
        {
            SqlCommand com = new SqlCommand(StoredProcedureName, SQLHelper.con());
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter apt = new SqlDataAdapter(com);
            dt.Clear();
            apt.Fill(dt);
            con().Close();
            return dt;
        }

        public static string UpdateOrInsertTable(string Query)
        {
            SqlCommand com = new SqlCommand(Query, con());
            com.Connection.Open();
            com.ExecuteNonQuery();
            com.Connection.Close();
            con().Close();
            return Query;
        }
        public static string UpdateOrInsertTable(string Query, SqlConnection connection)
        {
            SqlCommand com = new SqlCommand(Query, connection);
            com.Connection.Open();
            com.ExecuteNonQuery();
            com.Connection.Close();
            connection.Close();
            return Query;
        }


        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        public static string NumberToText(string amount)
        {
            decimal numberrs = Convert.ToInt64(Math.Floor(Convert.ToDouble(amount)));
            CultureInfo ci = new CultureInfo("en-IN");
            string aaa = String.Format("{0:#,##0.##}", numberrs);
            aaa = aaa + " " + ci.NumberFormat.CurrencySymbol.ToString();

            string input = amount;
            string a = "";
            string b = "";
            string strWords1 = "";
            // take decimal part of input. convert it to word. add it at the end of method.
            string decimals = "";

            if (input.Contains(".00"))
            {
                decimals = "";
                input = input.Remove(input.IndexOf("."));
                strWords1 = NumbersToWords(Convert.ToInt64(input));
            }

            else if (!input.Contains(".00"))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                // remove decimal part from input
                input = input.Remove(input.IndexOf("."));
            }
            string strWords = NumbersToWords(Convert.ToInt64(input));



            if (amount.Contains(".00"))
            {
                a = strWords1 + "Rupees Only";
            }

            else if (numberrs == 0)
            {
                strWords1 = "";
                a = "";

            }
            else if (numberrs != 0)
            {
                a = strWords + " Rupees";
            }

            if (decimals == "")
            {
                b = "";
            }
            if (decimals.Length > 0)
            {
                // if there is any decimal part convert it to words and add it to strWords.
                string strwords2 = NumbersToWords(Convert.ToInt64(decimals));
                b = " and " + strwords2 + " Paisa Only ";
            }

            if (numberrs == 0)
            {
                string strwords2 = NumbersToWords(Convert.ToInt64(decimals));
                b = strwords2 + "Paisa Only";
            }
            string c = a + b;

            return c;
        }

        public static string NumbersToWords(long num)
        {

            if (num == 0)
                return "Zero";

            if (num < 0)
                return "Not supported";

            var words = "";
            string[] strones = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] strtens = { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            long trillion = 0, billion = 0, crore = 0, lakhs = 0, thousands = 0, hundreds = 0, tens = 0, single = 0;


            trillion = num / 100000000000;
            num = num - trillion * 100000000000;

            crore = num / 10000000;
            num = num - crore * 10000000;

            lakhs = num / 100000;
            num = num - lakhs * 100000;

            thousands = num / 1000;
            num = num - thousands * 1000;

            hundreds = num / 100;
            num = num - hundreds * 100;

            if (num > 19)
            {
                tens = num / 10; num = num - tens * 10;
            }
            single = num;
            if (trillion > 0)
            {
                if (billion > 19)
                    words += NumbersToWords(trillion) + " Crore";
                else
                    words += strones[trillion - 1] + " Crore";
            }
            if (crore > 0)
            {
                if (crore > 19)
                    words += NumbersToWords(crore) + "Crore ";
                else
                    words += strones[crore - 1] + " Crore ";
            }

            if (lakhs > 0)
            {
                if (lakhs > 19)
                    words += NumbersToWords(lakhs) + "Lakh ";
                else
                    words += strones[lakhs - 1] + " Lakh ";
            }

            if (thousands > 0)
            {
                if (thousands > 19)
                    words += NumbersToWords(thousands) + "Thousand ";
                else
                    words += strones[thousands - 1] + " Thousand ";
            }

            if (hundreds > 0)
                words += strones[hundreds - 1] + " Hundred ";

            if (tens > 0)
                words += strtens[tens - 2] + " ";

            if (single > 0)
                words += strones[single - 1] + " ";

            return words;
        }






        public static void LogFile(string fil, string page, int NextLine)
        {

            string filename = fil + ".txt";
            //FileInfo f = new FileInfo(@"\\10.10.10.16\LogFiles\" + filename);
            FileInfo f = new FileInfo(ConfigurationManager.AppSettings["ServerName"] + filename);
            StreamWriter sw = f.AppendText();
            if (NextLine == 1)
            {
                sw.WriteLine("");
                sw.WriteLine("");
            }
            else
                sw.Write("\t");
            sw.Write(page + "---" + DateTime.Now);

            sw.Close();

        }

        public static void InternetLogFile(string IP, string fil, string page, int NextLine)
        {

            //if (IP==ConfigurationSettings.AppSettings["ServerInternetIP"].ToString())
            if (IP == ConfigurationManager.AppSettings["ServerInternetIP"].ToString())
            {
                string filename = fil + ".txt";
                //FileInfo f = new FileInfo(@"\\10.10.10.16\LogFiles\" + filename);
                FileInfo f = new FileInfo(ConfigurationManager.AppSettings["InternetLogPath"] + filename);

                StreamWriter sw = f.AppendText();
                if (NextLine == 1)
                {
                    sw.WriteLine("");
                    sw.WriteLine("");
                }
                else
                    sw.Write("\t");
                sw.Write(page + "---" + DateTime.Now);

                sw.Close();
            }

        }

    }
}
