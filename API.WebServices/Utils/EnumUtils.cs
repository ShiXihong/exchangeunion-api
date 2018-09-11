using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.WebServices.Utils
{
    public class EnumUtils
    {

        public enum Category
        {
            ExchangeUnion = 1,
            DigitalFinanceGroup = 2,
            ETCLabs = 3
        }

        public enum EmailType
        {
            Newsletter = 1,
            Contact = 2
        }

        public enum Status
        {
            Active = 1,
            DeActive = 2
        }

        public enum Language
        {
            All = 0,
            English = 1,
            Chinese = 2
        }
    }
}
