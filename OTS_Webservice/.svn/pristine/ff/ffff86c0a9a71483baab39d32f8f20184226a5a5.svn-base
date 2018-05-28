using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatmentWarehouseService.Models
{

  

      
  
    public class ClientPortfolio
    {
   
        public AccountDetails AccountDetails { get; set; }
        public BrokerInfo Broker { get; set; }
        public List<InstrumentInfo> InstrumentInfo { get; set; }
        public List<GrandTotal> GrandTotal { get; set; }
    }
    public class AccountDetails
    {
        public string Name { get; set; }
        public string Account { get; set; }
        //public string Address { get; set; }
    }

    public class BrokerInfo
    {
        //public string BrokerID { get; set; }
        public string BrokerCode { get; set; }
        public string BrokerName { get; set; }

    }


    public class TransactionActivity
    {
        public string TransactionID { get; set; }

        public string TransactionDate { get; set; }

        public string TransType { get; set; }

        public string Quantity { get; set; }

        public string TransCurrency { get; set; }
    }

    public class InstrumentInfo
    {
        public string InstrumentName { get; set; }
        public string InstrumentISINCode { get; set; }
        public string InstrumentCode { get; set; }

        public string BroughtForwardDate { get; set; }
        public string BroughtForwardAmount { get; set; }
        public List<TransactionActivity> TransactionActivity = new List<TransactionActivity>();
        public string NetActivityAmount { get; set; }

        public string CarryForwardDate { get; set; }
        public string CarryForwardAmount { get; set; }

        public string Price { get; set; }
        public string Value { get; set; }

        public string PlegeStartingBalance { get; set; }

        public string PledgeEndingBalance { get; set; }

        public string ISINCurrency { get; set; }

    }
    public class GrandTotal
    {
        public string GrandTotalAmount { get; set; }
        public string GrandTotalCurrency { get; set; }

    }
}
