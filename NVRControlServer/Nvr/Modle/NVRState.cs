using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Nvr.Modle
{
    public interface NVRState
    {

        void TurnToNextState();
        void NVRChannelRealPlay(int selectChannelIndex);
        void Dispense();
    }
}
