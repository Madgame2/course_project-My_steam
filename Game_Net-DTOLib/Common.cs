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
        UnKnowError,

        EmailAlredyTaken,
        UserNotfound,

        WrongPassword,

        InvalidRefreshToken,

        WrongGoodId,

        ObjectAleradyExist,

        noMoreProducts,

        PurchouseAlredyExist
    }

    public enum ServerStatus
    {
        Unknown = 0,
        Online = 1,
        Service = 2
    }

    public enum UserRole
    {
        General,
        Publisher,
        Admin
    }


    public enum ReportToppic
    {
        Complaint,
        ReQuest_for_publisher
    }
}
