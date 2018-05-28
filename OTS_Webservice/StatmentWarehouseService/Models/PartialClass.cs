using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StatmentWarehouseService.Models
{
    

        [MetadataType(typeof(InstrumentMetaData))]
        public partial class Instrument
        {

        }
        [MetadataType(typeof(BrokerMetaData))]
        public partial class Broker
        {

        }

        [MetadataType(typeof(AccountMetaData))]
        public partial class Account
        {

        }

        [MetadataType(typeof(TransactionMetaData))]
        public partial class Transaction
        {

        }
    
}
