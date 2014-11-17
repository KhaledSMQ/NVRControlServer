using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NVRControlServer.Net.Model
{
    public class PlayBackVideoSendQueue
    {
        private Queue<string> videoFileQueue;

        public PlayBackVideoSendQueue()
        {
            videoFileQueue = new Queue<string>();
        }

        public void Enqueue(string videoFile)
        {
            lock (((ICollection)videoFileQueue).SyncRoot)
            {
                videoFileQueue.Enqueue(videoFile);
            }
        }

        public string Dequeue()
        {
            lock (((ICollection)videoFileQueue).SyncRoot)
            {
                if (videoFileQueue.Count != 0)
                {
                    return videoFileQueue.Dequeue();
                }
                else
                    return null;
            }
        }

        public int Count()
        {
            lock (((ICollection)videoFileQueue).SyncRoot)
                return videoFileQueue.Count;
        }

        public void Clear()
        {
            lock (((ICollection)videoFileQueue).SyncRoot)
                videoFileQueue.Clear();
        }

        public void Dispose()
        {

            Enqueue(null);
        }

    }
}
