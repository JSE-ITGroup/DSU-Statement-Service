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
using StatmentWarehouseService.Properties;

namespace StatmentWarehouseService.Controllers
{
   // [Route("Jse/AccountServices")]
    public class AccountServicesController : ApiController
    {



        private ServiceReference.RDServiceClient mid = new ServiceReference.RDServiceClient();
        private ServiceReference.ReturnInfo rt = new ServiceReference.ReturnInfo();
        string SessionID = String.Empty;



        string UserName = Properties.Settings.Default.DependUserName.ToString();
        string Password = Properties.Settings.Default.Password.ToString();
        string Participant = Properties.Settings.Default.Participant.ToString();


        ServiceOperations serviceoperation = null;


         [Route("Jse/AccountServices/GetCommittment/AccountNumber")]
        public List<ORDER_COMMITTMENT> GetCommittment(string AccountNumber)
        {


            serviceoperation = new ServiceOperations();
            SessionID = serviceoperation.Login(UserName, Password, Participant);


            if (string.IsNullOrEmpty(SessionID))
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + "Login failed. Terminating");

                throw new HttpResponseException(SetHttpResonse("LOGIN FAILED PLEASE VERIFY CREDENTAIL OR CONTACT ADMINISTRATOR", "ResponseCode", "2"));
            }

            if (string.IsNullOrEmpty(AccountNumber))
            {
                throw new HttpResponseException(SetHttpResonse("INVALID ACCOUNT NUMBER SUPLLIED", "ResponseCode", "8"));
            }
            string[] AccountList = AccountNumber.Split(',');
            

            //
            byte[] bs;
            string schema;

            bs = null;
            schema = null;


            var CommittmentOperationName = Settings.Default.LIST_COMMIT_OPNAME;
            var CommittmentTableName = Settings.Default.LIST_COMMIT_TABLE;
            var CommittmentQueryPath = Settings.Default.LIST_COMMIT_QUERY;
            var CommittmentRowCount = Properties.Settings.Default.DEFAULT_ROW_COUNT;

            //QueryTemplate
            var TemplateFileName = HttpContext.Current.Server.MapPath("~/" + CommittmentQueryPath);
            var StatementQuery = serviceoperation.FileToString(TemplateFileName);
            StatementQuery = StatementQuery.Replace("@ACCOUNT_ID", AccountNumber.Substring(0, 6).Trim());


            DataTable CommittmentStatement = serviceoperation.GetDataTable(SessionID, CommittmentOperationName, CommittmentTableName, CommittmentRowCount, StatementQuery, out schema, out bs);
            List<ORDER_COMMITTMENT> commitmentList = null;
            ORDER_COMMITTMENT order_committment = new ORDER_COMMITTMENT();

            if (CommittmentStatement != null)
            {
                if (CommittmentStatement.Rows.Count != 0)
                {

                    commitmentList = serviceoperation.BindData<ORDER_COMMITTMENT>(CommittmentStatement);

                }
                else
                {
                    commitmentList = new List<ORDER_COMMITTMENT>();
                    commitmentList.Add(order_committment);
                    return commitmentList;
                }
            }
            else
            {
                commitmentList = new List<ORDER_COMMITTMENT>();
                commitmentList.Add(order_committment);
                return commitmentList;
            }

            return commitmentList;




        }

      
       [Route("Jse/AccountServices/GetAccountInfo/AccountNumber")]
        public List<AccountInfoView> GetAccountInfo(string AccountNumber)
        {
            List<AccountInfoView> accountinfoviewList = new List<AccountInfoView>();



            serviceoperation = new ServiceOperations();
            SessionID = serviceoperation.Login(UserName, Password, Participant);


            if (string.IsNullOrEmpty(SessionID))
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(DateTime.Now.ToString() + "\t" + "Login failed. Terminating");

                throw new HttpResponseException(SetHttpResonse("LOGIN FAILED PLEASE VERIFY CREDENTAIL OR CONTACT ADMINISTRATOR", "ResponseCode", "2"));
            }

            if (string.IsNullOrEmpty(AccountNumber))
            {
                throw new HttpResponseException(SetHttpResonse("INVALID ACCOUNT NUMBER SUPLLIED", "ResponseCode", "8"));
            }
            string[] AccountList = AccountNumber.Split(',');
            


           


                accountinfoviewList = GetHolder(AccountList);



                return accountinfoviewList;

        



        }

        private List<AccountInfoView> GetHolder(string[] AccountList)
        {

            List<AccountInfoView> AccountInfoViewList = new List<AccountInfoView>();


            foreach (string account in AccountList)
            {

                try
                {

                    ServiceOperations svrOperation = new ServiceOperations();

                    byte[] bs;
                    string schema;

                    bs = null;
                    schema = null;

                    string Account = account;

                    string OperationName = Properties.Settings.Default.LIST_ACCOUNT_OPNAME.ToString();
                    string TableName = Properties.Settings.Default.LIST_ACCOUNT_TABLE.ToString();
                    string AccountQueryName = Properties.Settings.Default.LIST_ACCOUNT_QUERY.ToString();
                    int RowCount = Properties.Settings.Default.DEFAULT_ROW_COUNT;

                    //ResetSet
                    RowCount = Properties.Settings.Default.DEFAULT_ROW_COUNT;

                    //QueryTemplate
                    string TemplateFileName = HttpContext.Current.Server.MapPath("~/" + AccountQueryName);
                    string filter = serviceoperation.FileToString(TemplateFileName);
                    filter = filter.Replace("@ACCOUNT_REFERENCE", Account);

                    string Account_Participant_Code = null;
                    DataTable AccountDT = svrOperation.GetDataTable(SessionID, OperationName, TableName, RowCount, filter, out schema, out bs);

                    if (AccountDT == null || AccountDT.Rows.Count == 0)
                    {
                        continue;
                    }

                    //Get Account Information

                    var AcList = from Ac in AccountDT.AsEnumerable()
                                 select new
                                 {
                                     AccountID = Ac.Field<int?>("ACCOUNT_ID"),
                                     AccountNumber = Ac.Field<int?>("ACCOUNT_NUMBER"),
                                     AccountReference = Ac.Field<string>("ACCOUNT_REFERENCE"),
                                     //Nationality = Ac.Field<string>("NATIONALITY"),
                                     AccountCategory = Ac.Field<string>("ACCOUNT_CATEGORY"),
                                     AccountType = Ac.Field<string>("ACTYPE_DESCRIPTION"),
                                     AccountOwner = Ac.Field<string>("ACCOUNT_OWNER_LIST"),
                                     AccountParticipantCode = Ac.Field<string>("ACCOUNT_PARTICIPANT_CODE"),

                                 };


                    string AccountID = AcList.Single(a => a.AccountReference == Account).AccountID.ToString();
                    Account_Participant_Code = AcList.Single().AccountParticipantCode.ToString();


                    string AccountNamesQuery = Properties.Settings.Default.LIST_ACCOUNT_NAMES_QUERY.ToString();
                    OperationName = Properties.Settings.Default.LIST_ACCOUNT_NAMES_OPNAME.ToString();
                    TableName = Properties.Settings.Default.LIST_ACCOUNT_NAMES_TABLE.ToString();
                    TemplateFileName = HttpContext.Current.Server.MapPath("~/" + AccountNamesQuery);
                    filter = serviceoperation.FileToString(TemplateFileName);
                    filter = filter.Replace("@ACCOUNT_ID", AccountID);//--Account);

                    DataTable AccountNameDT = svrOperation.GetDataTable(SessionID, OperationName, TableName, RowCount, filter, out schema, out bs);




                    string BrokerQuery = Properties.Settings.Default.LIST_PARTICIPANT_QUERY.ToString();
                    OperationName = Properties.Settings.Default.LIST_PARTICIPANT_OPNAME.ToString();
                    TableName = Properties.Settings.Default.LIST_PARTICIPANT_TABLE.ToString();
                    TemplateFileName = HttpContext.Current.Server.MapPath("~/" + BrokerQuery);
                    filter = serviceoperation.FileToString(TemplateFileName);
                    filter = filter.Replace("@DEPOPART_CODE", Account_Participant_Code);

                    DataTable BrokerDT = svrOperation.GetDataTable(SessionID, OperationName, TableName, RowCount, filter, out schema, out bs);

                    var BrokerList = from broker in BrokerDT.AsEnumerable()
                                     select new
                                     {
                                         BrokerCode = broker.Field<string>("DEPOPART_CODE") == null ? "" : broker.Field<string>("DEPOPART_CODE"),
                                         BrokerID = broker.Field<int?>("DEPOPART_ID"),
                                         BrokerName = broker.Field<string>("DEPOPART_NAME") == null ? "" : broker.Field<string>("DEPOPART_NAME")

                                     };


                    Broker Brokercs = new Broker();
                    Brokercs.BrokerCode = BrokerList.Single().BrokerCode.ToString();
                    Brokercs.BrokerID = BrokerList.Single().BrokerID.ToString();
                    Brokercs.BrokerName = BrokerList.Single().BrokerName.ToString();


                    var AcNameList = from AcName in AccountNameDT.AsEnumerable()
                                     select new
                                     {
                                         FullName = AcName.Field<string>("NAME_TEXT"),
                                         DOB = AcName.Field<DateTime?>("NAME_BIRTHDAY"),
                                         Sequence = AcName.Field<int?>("NAME_SEQUENCE"),
                                         WorkTelephone = AcName.Field<string>("NAME_WORK_TEL") == null ? "" : AcName.Field<string>("NAME_WORK_TEL"),
                                         AddresLine1 = AcName.Field<string>("NAME_ADDRESS_1") == null ? "" : AcName.Field<string>("NAME_ADDRESS_1"),
                                         Addressline2 = AcName.Field<string>("NAME_ADDRESS_2") == null ? "" : AcName.Field<string>("NAME_ADDRESS_2"),
                                         PostCode = AcName.Field<string>("NAME_POSTCODE") == null ? "" : AcName.Field<string>("NAME_POSTCODE"),
                                         State = AcName.Field<string>("NAME_STATE") == null ? "" : AcName.Field<string>("NAME_STATE"),
                                         Country = AcName.Field<string>("NAME_COUNTRY") == null ? "" : AcName.Field<string>("NAME_COUNTRY"),
                                         CITY = AcName.Field<string>("NAME_CITY") == null ? "" : AcName.Field<string>("NAME_CITY"),
                                         AccountReference = AcName.Field<string>("ACCOUNT_REFERENCE"),
                                         Nationality = AcName.Field<string>("NAME_NATIONALITY") == null ? "" : AcName.Field<string>("NAME_NATIONALITY")
                                     };



                    //
                    AccountInfoView AccInfoView = new AccountInfoView();
                    AccInfoView.AccountCategory = AcList.Single().AccountCategory;
                    AccInfoView.AccountOwnerList = AcList.Single().AccountOwner;
                    AccInfoView.AccountReference = AcList.Single().AccountReference;
                    AccInfoView.AccountType = AcList.Single().AccountType;
                    AccInfoView.AccountNumber = AcList.Single().AccountNumber;

                    AccInfoView.AddressLine1 = AcNameList.Single(a => a.Sequence == 1).AddresLine1.ToString();
                    AccInfoView.AddressLine2 = AcNameList.Single(a => a.Sequence == 1).Addressline2.ToString();
                    AccInfoView.City = AcNameList.Single(a => a.Sequence == 1).CITY.ToString();
                    AccInfoView.Country = AcNameList.Single(a => a.Sequence == 1).Country.ToString();
                    AccInfoView.DateOfBirth = Convert.ToDateTime(AcNameList.Single(a => a.Sequence == 1).DOB).ToString("yyyyMMdd");
                    AccInfoView.PostalCode = AcNameList.Single(a => a.Sequence == 1).PostCode.ToString();
                    AccInfoView.HomeTelephone = "";
                    AccInfoView.AccountNationality = AcNameList.Single(a => a.Sequence == 1).Nationality.ToString();
                    AccInfoView.WorkTelephone = AcNameList.Single(a => a.Sequence == 1).WorkTelephone.ToString();


                    List<AccountOwnership> OwnerList = new List<AccountOwnership>();


                    foreach (var owner in AcNameList.ToList())
                    {
                        AccountOwnership AccOwner = new AccountOwnership();

                        AccOwner.AccountReference = owner.AccountReference;
                        AccOwner.AccountHolderName = owner.FullName;
                        if (owner.Sequence == 1)
                        {
                            AccOwner.HolderOwnership = "PrimaryOwner";
                        }
                        else
                        {
                            AccOwner.HolderOwnership = "JointOwner";
                        }

                        OwnerList.Add(AccOwner);
                    }


                    AccInfoView.OwnershipList = OwnerList;
                    AccInfoView.Broker = Brokercs;







                    string ISINQUERY = Properties.Settings.Default.LIST_ISIN_QUERY.ToString();
                    OperationName = Properties.Settings.Default.LIST_ISIN_OPNAME.ToString();
                    TableName = Properties.Settings.Default.LIST_ISIN_TABLE.ToString();
                    TemplateFileName = HttpContext.Current.Server.MapPath("~/" + ISINQUERY);
                    string Holderfilter = serviceoperation.FileToString(TemplateFileName);

                    DataTable ISINSDT = svrOperation.GetDataTable(SessionID, OperationName, TableName, RowCount, null, out schema, out bs);

                    if (ISINSDT == null)
                    {

                    }
                    var IsinList = from isins in ISINSDT.AsEnumerable()
                                   select new
                                   {
                                       ISIN_CODE = isins.Field<string>("ISIN_CODE"),
                                       ISIN_CCY = isins.Field<string>("ISIN_CCY"),
                                       ISIN_FULL_NAME = isins.Field<string>("ISIN_FULL_NAME"),
                                       ISIN_SHORT_NAME = isins.Field<string>("ISIN_SHORT_NAME"),
                                       ISIN_ID = isins.Field<int?>("ISIN_ID")
                                   };





                    string BalanceQuery = Properties.Settings.Default.LIST_BALANCE_QUERY.ToString();
                    OperationName = Properties.Settings.Default.LIST_BALANCE_OPNAME.ToString();
                    TableName = Properties.Settings.Default.LIST_BALANCE_TABLE.ToString();
                    TemplateFileName = HttpContext.Current.Server.MapPath("~/" + BalanceQuery);
                    Holderfilter = serviceoperation.FileToString(TemplateFileName);
                    Holderfilter = Holderfilter.Replace("@ACCOUNT_ID", AccountID);

                    DataTable Balances = svrOperation.GetDataTable(SessionID, OperationName, TableName, -1, Holderfilter, out schema, out bs);


                    var BalanceList = from Bal in Balances.AsEnumerable()
                                      select new
                                      {
                                          Account_ID = Bal.Field<int>("BALTIME_ACCOUNT"),
                                          Owned = Bal.Field<decimal>("BALTIME_BALANCE"),
                                          IsinID = Bal.Field<int>("BALTIME_ISIN"),
                                          Pledge = Bal.Field<decimal?>("BALTIME_PLEDGED"),
                                          Buys = Bal.Field<decimal?>("BALTIME_UBUYS"),
                                          Sells = Bal.Field<decimal>("BALTIME_USELLS"),
                                          HeadRoom = Bal.Field<decimal?>("BALTIME_UBUYS") + Bal.Field<decimal?>("BALTIME_FREE"),
                                          AccountReference = Bal.Field<string>("ACCOUNT_REFERENCE"),
                                          IsinCode = Bal.Field<string>("ISIN_CODE"),
                                          InstrumentCode = Bal.Field<string>("ISIN_SHORT_NAME"),
                                          Free = Bal.Field<decimal?>("BALTIME_FREE"),
                                          BalTime = Bal.Field<DateTime?>("BALTIME_START")
                                      };


                    List<Instrument> InstrunmentList = new List<Instrument>();
                    foreach (var Bal in BalanceList.ToList())
                    {

                        Instrument instrument = new Instrument();


                        // instrument.InstrumentName = Bal.InstrumentName;
                        instrument.InstrumentCode = Bal.InstrumentCode;
                        instrument.ISINCode = Bal.IsinCode;
                        instrument.ISINID = Bal.IsinID;
                        instrument.TransactionDate = Convert.ToInt32(Convert.ToDateTime(Bal.BalTime).ToString("yyyyMMdd"));
                        instrument.UBuys = Bal.Buys.ToString();
                        instrument.USells = Bal.Sells.ToString();
                        instrument.HeadRoom = Bal.HeadRoom.ToString();
                        instrument.Available = Bal.Free;
                        instrument.Pledge = Bal.Pledge;
                        instrument.TransactionValue = null;
                        instrument.TransactionVolume = null;
                        instrument.Owned = Bal.Owned;
                        instrument.TransactionValue = Bal.Owned;

                        if (ISINSDT == null || ISINSDT.Rows.Count == 0)
                        {
                            instrument.InstrumentName = "";

                        }
                        else
                        {
                            try
                            {
                                instrument.InstrumentName = IsinList.Single(a => a.ISIN_ID == Bal.IsinID).ISIN_FULL_NAME.ToString();
                            }
                            catch (Exception ex)
                            {
                                //Skipped Delisted
                                Logger logger = LogManager.GetCurrentClassLogger();
                                logger.Error(DateTime.Now.ToString() + "\t" + ex.Message);
                                instrument.InstrumentName = "";
                                continue;


                            }

                        }

                        InstrunmentList.Add(instrument);



                    }

                    AccInfoView.Instruments = InstrunmentList;
                    AccountInfoViewList.Add(AccInfoView);
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
            }
            return AccountInfoViewList;
        }

        public HttpResponseMessage SetHttpResonse(string Message, string MessageDetail, string Code)
        {
            HttpError myCustomError = new HttpError(Message) { { MessageDetail, Code } };
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, myCustomError);
        }
       

    }
}
