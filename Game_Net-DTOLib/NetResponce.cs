using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public enum ResultCode
    {
        Success,

        EmailAlredyTaken,
        UserNotfound,

        WrongPassword,

        InvalidRefreshToken
    }

    public class NetResponse<T>
    {
        public bool Success {  get; set; }
        public ResultCode resultCode { get; set; } = ResultCode.Success;
        public string Message { get; set; }
        public T data { get; set; }
    }
}
