using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.NVR.Control;
using System.Threading;

namespace NVRControlServer.Nvr.Modle
{
    public class NVROffLineState : NVRState
    {
        private NVRControler nvrControler;

        public NVROffLineState(NVRControler nvrControler)
        {
            this.nvrControler = nvrControler;
        }

        public void Dispense()
        {
            nvrControler.Login();
            //if (nvrControler.NvrStatus == NVRStatus.Offline)
            //{
            //    Thread loginNVRThread = new Thread(LoginNVR);
            //    loginNVRThread.Start(nvrControler);
            //}
        }

        //private void LoginNVR(object obj)
        //{
        //    NVRControler nvrControler = (NVRControler)obj as NVRControler;
        //    while (true)
        //    {
        //        nvrControler.Login();
        //        Thread.Sleep(10000);
        //        if (nvrControler.NvrStatus == NVRStatus.Online)
        //            break;
        //    }
        //}


        public void NVRChannelRealPlay(int selectChannelIndex)
        {
            
        }

        public void TurnToNextState()
        {
            
        }
    }
}
