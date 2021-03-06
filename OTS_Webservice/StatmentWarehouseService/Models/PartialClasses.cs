﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatmentWarehouseService.Models
{
   
     public partial class Transaction
    {
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
        public string TransactionReference { get; set; }
        public string MatchReference { get; set; }
        public Nullable<int> TradeDate { get; set; }
    }

 public partial class Instrument
    {
        public int ISINID { get; set; }
        public string ISINCode { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentCode { get; set; }
        public string UBuys { get; set; }
        public string USells { get; set; }
        public string HeadRoom { get; set; }
        public Nullable<int> TransactionDate { get; set; }
        public Nullable<decimal> TransactionVolume { get; set; }
        public Nullable<decimal> TransactionValue { get; set; }
        public Nullable<decimal> Owned { get; set; }
        public Nullable<decimal> Available { get; set; }
        public Nullable<decimal> Pledge { get; set; }
    }
 public partial class Broker
{
        public string BrokerCode { get; set; }
        public string BrokerID { get; set; }
        public string BrokerName { get; set; }
    }

  public partial class Balance
    {
        public long ID { get; set; }
        public string AccountReference { get; set; }
        public string InstrumentCode { get; set; }
        public string BalanceDate { get; set; }
        public string InstrumentName { get; set; }
        public string ISINCode { get; set; }
        public Nullable<int> ISINID { get; set; }
        public decimal StockBalance { get; set; }
        public Nullable<decimal> Available { get; set; }
        public Nullable<decimal> Pledge { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
 public partial class AccountOwnership
    {
        public string HolderOwnership { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountReference { get; set; }
    }

 public partial class Account
    {
        public long ID { get; set; }
        public Nullable<int> Occurence { get; set; }
        public string AccountReference { get; set; }
        public Nullable<int> EquatorAccountNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountCategory { get; set; }
        public Nullable<int> AccountNumber { get; set; }
        public string AccountNationality { get; set; }
        public string AccountHolderName { get; set; }
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
        public Nullable<System.DateTime> DateCreated { get; set; }
    }
}
