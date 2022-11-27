using System;
using UnityEngine.Playables;

namespace CustomTimeline
{
    [Serializable]
    public class TimelineEventBehaviour : PlayableBehaviour
    {
        public VoidEvent eventScript;
        private bool clipPlayed = false;
        private bool pauseScheduled = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!clipPlayed && info.weight > 0f)
            {
                eventScript.Execute();
                clipPlayed = true;
            }
        }
    }

}