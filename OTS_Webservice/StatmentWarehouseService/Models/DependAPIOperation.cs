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
using System.Web;

namespace StatmentWarehouseService.Models
{
    static class DependAPIOperation
    {
       

      

        public static string FileToString(string FileName)
        {

            System.IO.StreamReader file = new System.IO.StreamReader(FileName);
            string fileContent = file.ReadToEnd();
            return fileContent;

        }


        public static List<ISINClass> GetInstruments(DataTable Instruments)
        {
            List<ISINClass> ISINList = new List<ISINClass>();

            try
            {
                

                var TransList = from Trans in Instruments.AsEnumerable()
                                select new
                                {
                                    ISIN_CODE = Trans.Field<string>("ISIN_CODE"),
                                    ISIN_CCY = Trans.Field<string>("ISIN_CCY"),
                                    ISIN_FULL_NAME = Trans.Field<string>("ISIN_FULL_NAME"),
                                    ISIN_SHORT_NAME = Trans.Field<string>("ISIN_SHORT_NAME"),
                                    ISIN_ID = Trans.Field<int?>("ISIN_ID")
                                };


                foreach (var isins in TransList.ToList())
                {
                    ISINClass isinclass = new ISINClass();

                    isinclass.ISIN_CCY = isins.ISIN_CCY;
                    isinclass.ISIN_CODE = isins.ISIN_CODE;
                    isinclass.ISIN_FULL_NAME = isins.ISIN_FULL_NAME;
                    isinclass.ISIN_SHORT_NAME = isins.ISIN_SHORT_NAME;
                    isinclass.ISIN_ID = isins.ISIN_ID.ToString();
                    ISINList.Add(isinclass);

                }
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);


            }
            return ISINList;

        }


     public static List<ISINClass> GetCommitments(DataTable CommittmentDT)
        {
            List<ISINClass> ISINList = new List<ISINClass>();

            try
            {


                var TransList = from Trans in CommittmentDT.AsEnumerable()
                                select new
                                {
                                    ISIN_CODE = Trans.Field<string>("ISIN_CODE"),
                                    ISIN_CCY = Trans.Field<string>("ISIN_CCY"),
                                    ISIN_FULL_NAME = Trans.Field<string>("ISIN_FULL_NAME"),
                                    ISIN_SHORT_NAME = Trans.Field<string>("ISIN_SHORT_NAME"),
                                    ISIN_ID = Trans.Field<int?>("ISIN_ID")
                                };


                foreach (var isins in TransList.ToList())
                {
                    ISINClass isinclass = new ISINClass();

                    isinclass.ISIN_CCY = isins.ISIN_CCY;
                    isinclass.ISIN_CODE = isins.ISIN_CODE;
                    isinclass.ISIN_FULL_NAME = isins.ISIN_FULL_NAME;
                    isinclass.ISIN_SHORT_NAME = isins.ISIN_SHORT_NAME;
                    isinclass.ISIN_ID = isins.ISIN_ID.ToString();
                    ISINList.Add(isinclass);

                }
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);


            }
            return ISINList;

        } 


     

    
    }
}
