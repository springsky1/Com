using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.ModelValidate
{
    /// <summary>
    /// 是否必填校验
    /// </summary>
    public class RequestAttribute : BaseAttribute
    {
        //   private String message;
        public override string error { get => base.error; set => base.error = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message">错误提示</param>
        public RequestAttribute(String Message)
        {
            //message = Message;
            error = Message;
        }


        public override bool Validate(object value)
        {
            return (value != null);
        }
    }
}
