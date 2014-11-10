using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Utils;

namespace NVRControlServer.Net.Model
{
    [Serializable]
    class SendCell : IDataCell
    {
        private int messageId;
        private object messageData;

        public object MessageData
        {
            get { return messageData; }
            set { messageData = value; }
        }

        public int MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public SendCell()
        {
        }

        public SendCell(int messageId, object messageData)
        {
            this.messageId = messageId;
            this.messageData = messageData;
        }

        public byte[] ToBuffer()
        {
            byte[] data = BufferHelper.Serialize(messageData);
            byte[] id = BitConverter.GetBytes(messageId);
            byte[] buffer = new byte[data.Length + id.Length];
            Buffer.BlockCopy(id, 0, buffer, 0, id.Length);
            Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);
            return buffer;
        }

        public void FromBuffer(byte[] buffer)
        {
            messageId = BitConverter.ToInt32(buffer, 0);
            messageData = BufferHelper.Deserialize(buffer, 4);
        }
    }
}
