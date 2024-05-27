using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Infrastructure.Messaging
{
    public abstract class ServiceResponseBase
    {
        public BusinessStatusCodeEnum StatusCode { get; set; }
        protected ServiceResponseBase()
        {
            StatusCode = BusinessStatusCodeEnum.Success;
        }
        protected ServiceResponseBase(BusinessStatusCodeEnum statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
