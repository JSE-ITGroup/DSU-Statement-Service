using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace StatmentWarehouseService.Models
{

    
    public class InstrumentMetaData
    {
 
        public int ISINID { get; set; }
        public string ISINCode { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentCode { get; set; }
        public string TransactionDate { get; set; }
        public Nullable<decimal> TransactionVolume { get; set; }
        public Nullable<decimal> TransactionValue { get; set; }
        public Nullable<decimal> Owned { get; set; }
        public Nullable<decimal> Available { get; set; }
        public Nullable<decimal> Pledge { get; set; }
        

    }

    
    public  class BrokerMetaData
    {
        public string BrokerCode { get; set; }
        public string BrokerID { get; set; }
        public string BrokerName { get; set; }
    }

   

    //[DataContract(IsReference = true)]
         public partial class AccountMetaData
        {

            public string AccountReference { get; set; }
            public Nullable<int> EquatorAccountNumber { get; set; }
            public string AccountType { get; set; }
            public string AccountCategory { get; set; }
            public Nullable<int> AccountNumber { get; set; }
            public string AccountNationality { get; set; }
            public string AccountOwnerList { get; set; }
            public string DateOfBirth { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string HomeTelephone { get; set; }
            public string WorkTelephone { get; set; }
            public string FaxNumber { get; set; }
            public string BrokerID { get; set; }
            public string BrokerCode { get; set; }
            public string BrokerName { get; set; }


            [JsonIgnore]
            public System.DateTime CreatedOn { get; set; }

            [JsonIgnore]
            public Nullable<System.DateTime> LastModifiedOn { get; set; }

            public Transaction Transaction { get; set; }

        }

       
        public class TransactionMetaData
        {
         

             [JsonIgnore]
            public long ID { get; set; }

            public Nullable<int> TransactionID { get; set; }
            public Nullable<int> TransactionDate { get; set; }
            public string AccountReference { get; set; }
            public string TransactionTypeID { get; set; }
            public string TransactionDescription { get; set; }
            public string BrokerID { get; set; }
            public string BrokerCode { get; set; }
            public string BrokerName { get; set; }
            public string InstrumentCode { get; set; }
            public string InstrumentISINCode { get; set; }
            public string InstrumentName { get; set; }
            public Nullable<decimal> TransactionPrice { get; set; }
            public Nullable<decimal> TransactionVolume { get; set; }
            public Nullable<decimal> TransactionValue { get; set; }
            public Nullable<decimal> Owned { get; set; }
            public Nullable<decimal> Available { get; set; }
            public Nullable<decimal> Pledge { get; set; }
                

            [JsonIgnore]
            public string TransactionReference { get; set; }
             [JsonIgnore]
            public string MatchReference { get; set; }
             
            public int TradeDate { get; set; }
        }

       
}
