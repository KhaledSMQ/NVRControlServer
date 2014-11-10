using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.NVR.Control;

namespace NVRControlServer.Nvr.Modle
{
    public class NVRRefreshChannelSuccessState : NVRState
    {
          private NVRControler nvrControler;

          public NVRRefreshChannelSuccessState(NVRControler nvrControler)
        {
            this.nvrControler = nvrControler;
        }

        public void TurnToNextState()
        {

        }

        public void NVRChannelRealPlay(int selectChannelIndex)
        {
          
        }

        public void Dispense()
        {
            
        }
    }
}
