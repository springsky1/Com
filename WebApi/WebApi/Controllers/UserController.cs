using Comlib.ModelValidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Controllers
{
    [Route("")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("Api/User/Login")]
        public HttpResponseMessage Login()
        {

            UserInfo userInfo = new UserInfo();
            userInfo.UserName = "测试";
            userInfo.Age = "10";
            userInfo.Address = "上海";
            String aa = userInfo.Validate();

            HttpResponseMessage httpResponse = new HttpResponseMessage();

            httpResponse.StatusCode = HttpStatusCode.OK;
            httpResponse.Content = new StringContent("OKK");

            return httpResponse;
        }
    }


    public class UserInfo
    {
        [Request("名称不能为空")]
        public string UserName { get; set; }
        [Rang(1, 3, "范围不正确 请输入1-3位数的值")]
        public string Age { get; set; }
        [Regex("[a-zA-Z]{1,10}", "不符合要求")]
        public string Address { get; set; }
    }

}
