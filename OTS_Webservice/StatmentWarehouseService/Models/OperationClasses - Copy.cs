using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace StatmentWarehouseService.Models
{

    public class ISINClass
    {
        public string ISIN_ID { get; set; }
        public string ISIN_CODE { get; set; }
        public string ISIN_CCY { get; set; }
        public string ISIN_FULL_NAME { get; set; }
        public string ISIN_SHORT_NAME { get; set; }
    }
    public class Commitments
    {
        public string CommitmentID { get; set; }
        public int CommitmentDate { get; set; }
        public string CommitmentDesc { get; set; }
        public decimal CommitmentQty { get; set; }

        public string IsinCode { get; set; }
        public int IsinID { get; set; }
        public string IsinShortName { get; set; }
        public string IsinFullName { get; set; }

        public string AccountID { get; set; }
        public string Account_Reference { get; set; }

    }

    public class Balances
    {
        public string BalanceAccount { get; set; }
        public string BalanceAccountReference { get; set; }
        public string BalTimeBalance { get; set; }
        public string BalTimeISIN { get; set; }
        public string BalTimePledge { get; set; }
        public string ISIN_CODE { get; set; }
        public string ISIN_SHORTNAME { get; set; }
        public string BALTIMEFree { get; set; }
    }
    public class AccountNames
    {
         public string FullName { get; set; }
         public string Sequence { get; set; }
         public string WorkTelephone { get; set; }
         public string AddressLine1 { get; set; }
         public string AddressLine2 { get; set; }
         public string PostalCode { get; set; }
         public string State { get; set; }
         public string Country { get; set; }
         public string City { get; set; }
         public string AccountID { get; set; }
         public string AccountReference { get; set; }
        
        
    }

    public class ORDER_COMMITTMENT
    {

      
        [JsonProperty(PropertyName = "CommittmentType")] 
        public string COMMIT_TYPE { get; set; }

        
        [JsonProperty(PropertyName = "CommittmentParentID")] 
        public string COMMIT_PARENT { get; set; }

       
        [JsonProperty(PropertyName = "CommittmentRefererence")] 
        public string COMMIT_REFERENCE { get; set; }

        
        [JsonProperty(PropertyName = "CommittmentDescription")] 
        public string COMMIT_DESCRIPTION { get; set; }

       
        [JsonProperty(PropertyName = "CommittmentIsin")] 
        public string COMMIT_ISIN { get; set; }

        
        [JsonProperty(PropertyName = "CommittmentQuantity")] 
        public string COMMIT_QTY { get; set; }

       
        [JsonProperty(PropertyName = "CommittmentAccount")] 
        public string COMMIT_ACCOUNT { get; set; }

        
        [JsonProperty(PropertyName = "CommittmentDate")] 
        public string COMMIT_DATE { get; set; }

        
        [JsonProperty(PropertyName = "CommittmentRemarks")] 
        public string COMMIT_REMARKS { get; set; }

 
       
        [JsonProperty(PropertyName = "IsinID")] 
        public string ISIN_ID { get; set; }
        
        
        [JsonProperty(PropertyName = "IsinCode")] 
        public string ISIN_CODE { get; set; }

        
        [JsonProperty(PropertyName = "IsinShortName")] 
        public string ISIN_SHORT_NAME { get; set; }

        
        [JsonProperty(PropertyName = "ISIN_FULL_NAME")] 
        public string ISIN_FULL_NAME { get; set; }

     
        [JsonProperty(PropertyName = "AccountID")] 
        public string ACCOUNT_ID { get; set; }

       
        [JsonProperty(PropertyName = "CommittmentReference")] 
        public string ACCOUNT_REFERENCE { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "AccountOwnerList")] 
        public string ACCOUNT_OWNER_LIST { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "AccountResidence")] 
        public string ACCOUNT_RESIDENCE { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "NAME_ID_CODE")] 
        public string NAME_ID_CODE { get; set; }

    }

}
