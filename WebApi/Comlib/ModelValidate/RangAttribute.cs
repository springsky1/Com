using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.ModelValidate
{
    /// <summary>
    /// 长度范围校验
    /// </summary>
    public class RangAttribute : BaseAttribute
    {
        private int startInt;
        private int endInt;

        public override string error { get => base.error; set => base.error = value; }
        /// <summary>
        /// 长度范围校验
        /// </summary>
        /// <param name="StartInt">最小长度</param>
        /// <param name="EndInt">最大长度</param>
        /// <param name="Message">错误提示</param>
        public RangAttribute(int minInt, int MaxInt, String Message)
        {
            startInt = minInt;
            endInt = MaxInt;
            error = Message;
        }
        public override bool Validate(object value)
        {
            if (value != null && value.ToString().Length >= startInt && value.ToString().Length <= endInt)
            {
                return true;
            }
            else { return false; }
        }
    }
}
