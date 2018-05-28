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



namespace StatmentWarehouseService.Controllers
{
    //[Route("Jse/StatementServices")]
    public class StatementServicesController : ApiController
    {
      


        private ServiceReference.RDServiceClient mid = new ServiceReference.RDServiceClient();
        private ServiceReference.ReturnInfo rt = new ServiceReference.ReturnInfo();
        string SessionID = String.Empty;



        string UserName = Properties.Settings.Default.DependUserName.ToString();
        string Password = Properties.Settings.Default.Password.ToString();
        string Participant = Properties.Settings.Default.Participant.ToString();

        // GET api/Statements/5
        [Route("Jse/StatementServices/GetStatement/{AccountNumber}/{StartDate}/{EndDate}")]
        public IEnumerable<Transaction> GetStatement(string AccountNumber, int StartDate, int EndDate)
        {

            try
            {
                //var transactions;

                if (string.IsNullOrEmpty(AccountNumber) || string.IsNullOrEmpty(StartDate.ToString()) || string.IsNullOrEmpty(EndDate.ToString()))
                {
                    throw new HttpResponseException(SetHttpResonse("ONE OR MORE INVALID PARAMETER SUPPLIED,PLEASE CHECK PARAMETER AND RESUMBIT REQUEST", "ResponseCode", "2"));
                }

                DateTime StrDt = new DateTime();
                DateTime EnDt = new DateTime();
                /*
                if (!(((DateTime.TryParse(StartDate.ToString(), out StrDt)) && (DateTime.TryParse(EndDate.ToString(), out EnDt)))))
                {
                    throw new HttpResponseException(SetHttpResonse("ONE OR MORE INVALID DATE PARAMETER SUPPLIED,PLEASE CHECK PARAMETER AND RESUMBIT REQUEST", "ResponseCode", "2"));
                }
                 */ 

                SessionID = Login(UserName, Password, Participant);
                if (string.IsNullOrEmpty(SessionID))
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(DateTime.Now.ToString() + "\t" + "Login failed. Terminating");

                    throw new HttpResponseException(SetHttpResonse("LOGIN FAILED PLEASE VERIFY CREDENTAIL OR CONTACT ADMINISTRATOR", "ResponseCode", "2"));
                }


                else
                {


                    var transactions = GetTransactions(AccountNumber, StartDate.ToString(), EndDate.ToString());



                    return transactions;

                }

            }
            catch (Exception EXP)
            {
            }
            return null;
        }
        private string Login(string UserName, string Password, string Participant)
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


        private IEnumerable<Transaction> GetTransactions(string AccountNumber, string StartDate, string EndDate)
        {
            byte[] bs;
            string schema;

            string StartdateTemplate = Properties.Settings.Default.StartDate_Template.ToString();
            string EnddateTemplate = Properties.Settings.Default.EndDate_Template.ToString();
            
           

            string TimeZone = Properties.Settings.Default.TimeInfo.ToString();

            string strtDate = StartDate + TimeZone;
            string edDate = EndDate + TimeZone;

            string TransQuery = Properties.Settings.Default.LIST_TRANSACTION_QUERY.ToString();

            string ISINQuery = Properties.Settings.Default.LIST_ISIN_QUERY.ToString();

            string TransactionOperationName = Properties.Settings.Default.LIST_TRANSACTION_OPNAME.ToString();
            string TransactionTableName = Properties.Settings.Default.LIST_TRANSACTION_TABLE.ToString();

            int RowCount = 10;
            RowCount = Properties.Settings.Default.DEFAULT_ROW_COUNT;

           
            StartDate = StartDate.Insert(6, "-");
            StartDate = StartDate.Insert(4, "-");
            StartDate = string.Format(StartdateTemplate, StartDate);

            EndDate = EndDate.Insert(6, "-");
            EndDate = EndDate.Insert(4, "-");
            EndDate = string.Format(EnddateTemplate, EndDate);

           

            DateTime Start = DateTime.Parse(StartDate);
            DateTime End = DateTime.Parse(EndDate);

            string TemplateFileName = HttpContext.Current.Server.MapPath("~/" + TransQuery);
            string Transfilter = FileToString(TemplateFileName);

            Transfilter = Transfilter.Replace("@ACCOUNT_REFERENCE", AccountNumber);
           

            TemplateFileName = HttpContext.Current.Server.MapPath("~/" + ISINQuery);
            string ISINfilter = FileToString(TemplateFileName);

            List<ISINClass> ISINList   = null;
            List<Transaction> TransactionList = new List<Transaction>();
            List<Commitments> CommitmentList = new List<Commitments>();

            try
            {

                rt = mid.DataSetListZIP(SessionID, TransactionOperationName, RowCount, Transfilter, out schema, out bs);


               
                if (rt.HasError) // lets see what server thinks about that
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Error(DateTime.Now.ToString() + "\t" + rt.ErrorInfo.ErrorReference + "\t" + rt.ErrorInfo.ErrorText);


                }
                else
                {
                    DataSet opListDS = Utils.unZipDS(bs, schema);
                    DataTable Transactions = opListDS.Tables[TransactionTableName];
                    string expression = null;

                    //Get Symbols
                    schema = null;
                    bs = null;
                    rt = null;
                    
                    rt = mid.DataSetListZIP(SessionID, "LIST_ISIN.1", RowCount, ISINfilter, out schema, out bs);
                    if (rt.HasError)
                    {
                        Logger logger = LogManager.GetCurrentClassLogger();
                        logger.Error(DateTime.Now.ToString() + "\t" + rt.ErrorInfo.ErrorReference + "\t" + rt.ErrorInfo.ErrorText);
                    }
                    else
                    {
                         opListDS = Utils.unZipDS(bs, schema);
                        DataTable ISINS = opListDS.Tables["LIST_ISIN"];

                        try
                        {
                            ISINList = DependAPIOperation.GetInstruments(ISINS);
                        }
                        catch (Exception ex)
                        {
                            ISINList = null;
                            Logger logger = LogManager.GetCurrentClassLogger();
                            logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);

                        }
                    }
                    string Commitmentfilter = null;
                    try
                    {
                       
                         Commitmentfilter = FileToString( HttpContext.Current.Server.MapPath("~/" + Properties.Settings.Default.LIST_COMMIT_QUERY.ToString()));

                         string AccountID = AccountNumber.Substring(0, AccountNumber.Length - 1);
                         Commitmentfilter = Commitmentfilter.Replace("@ACCOUNT_ID", AccountID);
                    }
                    catch (Exception exp)
                    {
                        Commitmentfilter = "";

                    }

                    string CommittmentOpName = Properties.Settings.Default.LIST_COMMIT_OPNAME.ToString();
                    //List<Commitments> commitmentslist
                        CommitmentList = GetCommitments(SessionID, CommittmentOpName, RowCount, Commitmentfilter, out schema, out bs,Start,End);

                        
                    var TransList = from Trans in Transactions.AsEnumerable()
                                    .Where(t=>t.Field<DateTime>("BALCHANGE_DATE") >= Start
                                         && t.Field<DateTime>("BALCHANGE_DATE") <= End)
                                    select new
                                    {
                                        TransactionID = Trans.Field<int>("TRANS_ID"),
                                        TransactionQty = Trans.Field<decimal>("BALCHANGE_QTY"),
                                        TransactionISIN = Trans.Field<int?>("BALCHANGE_ISIN"),
                                        TransactionAccount = Trans.Field<int?>("BALCHANGE_ACCOUNT"),
                                        TransactionDate = Trans.Field<DateTime?>("BALCHANGE_DATE"),
                                        TransactionType = Trans.Field<string>("TRANS_TYPE"),
                                        TransactionTypeDesc = Trans.Field<string>("TRANTYPE_DESCRIPTION"),
                                        ISINCode = Trans.Field<string>("ISIN_CODE"),
                                        ISINSHORTName = Trans.Field<string>("ISIN_SHORT_NAME"),
                                        TransactionPrice = Trans.Field<decimal?>("TRANS_PRICE"),
                                        TransactionValue = Trans.Field<decimal?>("TRANS_VALUE"),
                                        BalTimeBal = Trans.Field<decimal?>("BALTIME_BALANCE"),
                                        BrokerCode = Trans.Field<string>("TRANS_BROKER"),
                                        TransactionMatch = Trans.Field<int?>("TRANS_MATCH")
                                    };

                   

                    foreach(var Trans in TransList.ToList())
                    {
                        Transaction transaction = new Transaction();
                        transaction.AccountReference = AccountNumber.Trim();
                        transaction.TransactionID = Trans.TransactionID;
                        transaction.TransactionPrice = Trans.TransactionPrice==null ? null : Trans.TransactionPrice;
                        transaction.TransactionValue = Trans.TransactionValue == null ? null : Trans.TransactionValue; 
                        transaction.TransactionVolume = Trans.TransactionQty;
                     

                        transaction.BrokerCode = Trans.BrokerCode;
                        transaction.TransactionTypeID = Trans.TransactionType;
                        transaction.InstrumentCode = Trans.ISINSHORTName;
                        transaction.InstrumentISINCode = Trans.ISINCode;
                        transaction.Available = Trans.BalTimeBal;
                        transaction.Owned = Trans.BalTimeBal;
                        transaction.TradeDate = Convert.ToInt32(Convert.ToDateTime(Trans.TransactionDate).ToString("yyyyMMdd"));

                        /* Should be Removed. */
                        transaction.BrokerID = "";
                        transaction.BrokerCode = "";
                        transaction.BrokerName="";

                        if (ISINList == null || ISINList.Count == 0)
                        {
                            transaction.InstrumentName = "";
                        }
                        else
                        {
                            try
                            {
                                string IsINName = ISINList.First(a => a.ISIN_CODE == transaction.InstrumentISINCode).ISIN_FULL_NAME;
                                transaction.InstrumentName = IsINName;
                            }
                            catch (Exception ex)
                            {
                                Logger logger = LogManager.GetCurrentClassLogger();
                                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);
                                transaction.InstrumentName = "";
                            }
                        }                         
                        
                        transaction.TransactionDate = Convert.ToInt32(Convert.ToDateTime(Trans.TransactionDate).ToString("yyyyMMdd"));
                        transaction.TransactionDescription = Trans.TransactionTypeDesc.ToString();
                       
                        TransactionList.Add(transaction);
                    }


                }
            }
            catch (Exception ex)
            {

                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);


            }
            finally
            {

                mid.Close();
            }
            //Merging List
            return MergeTransactionAndCommitment(TransactionList, CommitmentList);
          //  return TransactionList;
        }

        private List<Commitments> GetCommitments(string SessionID, string OpName, int RowCount, string Commitmentfilter, out string schema, out byte[] bs,DateTime StartDate,DateTime EndDate)
        {

            int Postiveperiod = Properties.Settings.Default.SettlementPeriod;
            int Negperiod = Postiveperiod * (-1);

            //EndDate =  EndDate.AddDays(Properties.Settings.Default.SettlementPeriod);
            EndDate = Utils.AddBusinessDays(EndDate, Postiveperiod);

           
            List<Commitments> CommitmentList = new List<Commitments>();

            rt = mid.DataSetListZIP(SessionID, OpName, RowCount, Commitmentfilter, out schema, out bs);


             if (rt.HasError) // lets see what server thinks about that
             {
                 Logger logger = LogManager.GetCurrentClassLogger();
                 logger.Error(DateTime.Now.ToString() + "\t" + rt.ErrorInfo.ErrorReference + "\t" + rt.ErrorInfo.ErrorText);


             }
             else
             {
                 DataSet opListDS = Utils.unZipDS(bs, schema);
                 DataTable CommitmentDT = opListDS.Tables["LIST_COMMIT"];

                 try
                 {


                     var commitmentlist = from commitment in CommitmentDT.AsEnumerable()
                                          .Where(t => t.Field<DateTime>("COMMIT_DATE") >= StartDate
                                         && t.Field<DateTime>("COMMIT_DATE") <= EndDate)

                                     select new
                                     {
                                         CommitmentID = commitment.Field<int>("COMMIT_PARENT"),
                                         CommitmentDate = commitment.Field<DateTime?>("COMMIT_DATE"),
                                         CommitmentDesc = commitment.Field<string>("COMTYP_DESCRIPTION"),
                                         //ComTypeDesc = commitment.Field<string>("COMTYP_DESCRIPTION"),
                                         CommitmentQty = commitment.Field<decimal?>("COMMIT_QTY"),
                                         IsinID = commitment.Field<int?>("ISIN_ID"),
                                         IsinCode = commitment.Field<string>("ISIN_CODE"),
                                         IsinShortName = commitment.Field<string>("ISIN_SHORT_NAME"),
                                         IsinFullName = commitment.Field<string>("ISIN_FULL_NAME"),
                                         Account_Reference = commitment.Field<string>("ACCOUNT_REFERENCE")
                                     };


                     foreach (var c in commitmentlist.ToList())
                     {
                         Commitments commitments = new Commitments();


                         int CommittmentDate = Convert.ToInt32(Utils.AddBusinessDays(Convert.ToDateTime(c.CommitmentDate), Negperiod).ToString("yyyyMMdd"));

                         commitments.Account_Reference = c.Account_Reference.ToString();
                         commitments.CommitmentDate = CommittmentDate;                 
                         commitments.CommitmentDesc = c.CommitmentDesc;
                         commitments.CommitmentID = c.CommitmentID.ToString();
                         commitments.CommitmentQty = (decimal) c.CommitmentQty;
                         commitments.IsinCode = c.IsinCode;
                         commitments.IsinFullName = c.IsinFullName;
                         commitments.IsinID = (Int32) c.IsinID;
                         commitments.IsinShortName = c.IsinShortName;
                         CommitmentList.Add(commitments);

                     }
                 }
                 catch (Exception ex)
                 {
                     Logger logger = LogManager.GetCurrentClassLogger();
                     logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);


                 }




             }
             return CommitmentList;
            
        }

       
        public string FileToString(string FileName)
        {

            System.IO.StreamReader file = new System.IO.StreamReader(FileName);
            string fileContent = file.ReadToEnd();
            return fileContent;

        }
        protected override void Dispose(bool disposing)
        {
           
            base.Dispose(disposing);
        }
        public HttpResponseMessage SetHttpResonse(string Message, string MessageDetail, string Code)
        {
            HttpError myCustomError = new HttpError(Message) { { MessageDetail, Code } };
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, myCustomError);
        }

        public List<Transaction> MergeTransactionAndCommitment( List<Transaction>  transactionList, List<Commitments> CommitmentsList)
        {
            //List<Transaction> TransactionList = new List<Transaction>();

            if (transactionList == null)
                transactionList = new List<Transaction>();

            if (CommitmentsList == null)
                return transactionList;


            foreach (Commitments c in CommitmentsList)
            {
                Transaction trans= new Transaction();
                trans.TransactionID = Convert.ToInt32(c.CommitmentID);
                trans.TransactionDate = c.CommitmentDate;
                trans.TransactionVolume = c.CommitmentQty;
                trans.TransactionDescription = c.CommitmentDesc;
                trans.InstrumentCode = c.IsinShortName;
                trans.InstrumentName = c.IsinFullName;
                trans.InstrumentISINCode = c.IsinCode;
                trans.ID = c.IsinID;
                trans.AccountReference = c.Account_Reference;
                trans.TradeDate= c.CommitmentDate;

                transactionList.Add(trans);
            }
             //var t = transactionList.OrderByDescending(p =>p.TransactionDate);
            return transactionList;

        }

    }
}