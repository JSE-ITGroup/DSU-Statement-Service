using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StatmentWarehouseService.Models
{
    public class AccountInfoView
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
       


        public List<AccountOwnership> OwnershipList { get; set; }
         
        public List<Instrument> Instruments { get; set; }
          
        public Broker Broker { get; set; }
           
    }
}
