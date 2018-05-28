using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatmentWarehouseService.Models
{
    public class StatementReportModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DateRun { get; set; }

        public String UserRunFor { get; set; }

        public String AccountNumber { get; set; }

        public String ClientName { get; set; }

        public List<StockModel> Stocks { get; set; }

        public CustomerModel customer { get; set; }

        public string GrandTotal { get; set; }

        public string GrandTotal2 { get; set; }
    }

    public class StockModel
    {
        public String InstrumentsSINCode { get; set; }

        public String InstrumentName { get; set; }

        public String InstrumentCode { get; set; }

        public Decimal NetActivity { get; set; }

        public Decimal CarriedForward { get; set; }

        public Decimal Price { get; set; }

        public Decimal Value { get; set; }

        public Decimal PledgeStartingValue { get; set; }

        public Decimal PledgeEndingValue { get; set; }

        public List<TransactionModel> Transactions { get; set; }

        public decimal BroughtForwardAmount { get; set; }

        public DateTime BroughtForwardDate { get; set; }
    }

    public class TransactionModel
    {
        public String TransactionNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public String Description { get; set; }

        public Int32 Quantity { get; set; }
    }

    public class CustomerModel
    {
        public string AccountNumber { get; set; }

        public string AccountHolder { get; set; }
    }
}