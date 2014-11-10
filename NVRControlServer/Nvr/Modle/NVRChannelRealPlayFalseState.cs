using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.NVR.Control;

namespace NVRControlServer.Nvr.Modle
{
    public class NVRChannelRealPlayFalseState : NVRState
    {
        private NVRControler nvrControler;

        public NVRChannelRealPlayFalseState(NVRControler nvrControler)
        {
            this.nvrControler = nvrControler;
        }

        public void Dispense()
        {
            throw new NotImplementedException();
        }

        public void NVRChannelRealPlay(int selectChannelIndex)
        {
            
        }

        public void TurnToNextState()
        {
            throw new NotImplementedException();
        }
    }
}
