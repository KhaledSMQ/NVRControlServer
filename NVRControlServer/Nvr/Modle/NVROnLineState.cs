using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.NVR.Control;

namespace NVRControlServer.Nvr.Modle
{
    public class NVROnLineState : NVRState
    {
        private NVRControler nvrControler;

        public NVROnLineState(NVRControler nvrControler)
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
