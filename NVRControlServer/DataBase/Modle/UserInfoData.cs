using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.DataBase.Modle
{

    [Serializable]
    public class UserInfoData
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public int uesrLevel { get; set; }
    }
}
