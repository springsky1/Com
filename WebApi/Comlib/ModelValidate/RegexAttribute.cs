using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Comlib.ModelValidate
{
    public class RegexAttribute : BaseAttribute
    {
        private String regexPatter;
        public override string error { get => base.error; set => base.error = value; }
        public RegexAttribute(String regex1, String message)
        {
            regexPatter = regex1;
            error = message;
        }

        public override bool Validate(object value)
        {
            Regex regex = new Regex(regexPatter);
            return regex.IsMatch(value.ToString());
        }
    }
}
