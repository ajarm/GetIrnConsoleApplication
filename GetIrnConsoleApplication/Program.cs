using GetIrnConsoleApplication.DBoperations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetIrnConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    DataSet ds = new DataSet();
                    string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    DBOperation dBOperation = new DBOperation();

                    ds = dBOperation.GetDate(date);

                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        MainProgram.MainProgram.Execute();
                        DBOperation dBOperation1 = new DBOperation();
                        ds = dBOperation1.GetDate(date);

                        count = ds.Tables[0].Rows.Count;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }
        }    
    }
}
