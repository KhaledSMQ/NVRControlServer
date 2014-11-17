using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.Net.Interface;
using NVRControlServer.Net.Model;

namespace NVRControlServer.Net.Control
{
    public class LoginHandle : ICommandHandle
    {
        private ClientSession clientSession;
        private ClientCommand clientCommand;
        private ICommandHandle netxCommandHandle;

        public LoginHandle()
        {

        }

        public void SetHandleModel(ClientSession session, ClientCommand command)
        {
            this.clientSession = session;
            this.clientCommand = command;
        }

        public void SetNextHandler(ICommandHandle nextHandle)
        {
            this.netxCommandHandle = nextHandle;
        }

        public void Process()
        {
            if (clientCommand.CommanKind == COMKIND.RequestLogin)
            {
                string welcomStr = "welcom";
                byte[] welcomStrByte = Utils.Transform.String2Byte(welcomStr);
                clientSession.Send(MSGKIND.ResponseLogin, EXERESULT.ExcuteSuccess, welcomStrByte);
            }
            else
            {
                if (netxCommandHandle != null)
                {
                    netxCommandHandle.Process();
                }
            }
        }


    }
}
