using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using StatmentWarehouseService.Models;
using System.Web.Routing;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using NLog;


namespace StatmentWarehouseService.Models
{
    public class ServiceOperations
    {

        private ServiceReference.RDServiceClient mid = new ServiceReference.RDServiceClient();
        private ServiceReference.ReturnInfo rt = new ServiceReference.ReturnInfo();
        string SessionID = String.Empty;


        public DataTable GetDataTable(string SessionID, string OperationName, string TableName, int RowCount, string Holderfilter, out string schema, out byte[] bs)
        {
            DataTable Table = new DataTable();
            schema = null;
            bs = null;

            try
            {


                rt = mid.DataSetListZIP(SessionID, OperationName, RowCount, Holderfilter, out schema, out bs);

                if (rt.HasError)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(DateTime.Now.ToString() + "\t" + rt.ErrorInfo.ErrorReference + "\t" + rt.ErrorInfo.ErrorText);


                }
                else
                {
                    DataSet opListDS = Utils.unZipDS(bs, schema);

                    //opListDS.WriteXml(@"C:\data\DEPOOPMEMBER.xml");
                    Table = opListDS.Tables[TableName];

                }




            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);


            }

            return Table;

        }

        public string Login(string UserName, string Password, string Participant)
        {
            string sessionID = String.Empty;
            Console.WriteLine();
            try
            {
                /* Call actual login. password is sent in MD5 */
                rt = mid.DependLogin(UserName, Utils.MD5(Password), Assembly.GetExecutingAssembly().GetName().Name + " Ver: " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), Participant, out sessionID);
                if (rt.HasError)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(DateTime.Now.ToString() + "\t" + String.Format("Login error: {0}\r\n{1}", rt.ErrorInfo.ErrorReference, rt.ErrorInfo.ErrorText));


                }

                else
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(DateTime.Now.ToString() + "\t" + String.Format("Login successful.\r\nSessionID:{0}", sessionID));

                }
            }
            catch (Exception ex) //catch unexpected stuff that is not able to set "rt" (like network failure)
            {
                // Console.WriteLine(String.Format("Login exception:\r\n{0}", ex.Message));
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + "Login exception: \t" + ex.Message);
            }
            finally
            {
                //always close once done.
                //if (mid != null)
                //mid.Close();
            }
            return sessionID;
        }

        public List<T> BindData<T>(DataTable dt)
        {
            DataRow dr = dt.Rows[0];

            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object

            List<T> objList = new List<T>();

            foreach (DataRow datarw in dt.Rows)
            {
                //foreach (var item in dr.ItemArray)
                // {
                //Console.WriteLine(item);
                // }

                var ob = Activator.CreateInstance<T>();


                // Get all fields
                var fields = typeof(T).GetFields();
                foreach (var fieldInfo in fields)
                {
                    if (columns.Contains(fieldInfo.Name))
                    {
                        // Fill the data into the field
                        fieldInfo.SetValue(ob, datarw[fieldInfo.Name]);
                    }
                }

                // Get all properties
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {
                    if (columns.Contains(propertyInfo.Name))
                    {
                        // Fill the data into the property
                        //Convert all to String
                        propertyInfo.SetValue(ob, datarw[propertyInfo.Name].ToString());
                    }
                }

                objList.Add(ob);
            }//  Next Row

            return objList;
        }

        public string FileToString(string FileName)
        {
            var file = new StreamReader(FileName);
            var fileContent = file.ReadToEnd();
            return fileContent;
        }
    }
}
