using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class LevelSceneController : MonoBehaviour
{
    public SkeletonAnimation snakeAnimation;
    public AnimationReferenceAsset snakeTalk;
    public AnimationReferenceAsset snakeIdle;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.NPCTalkDEvent.AddListener(TalkEventHandler);
        EventManager.instance.NPCTalkEndDEvent.AddListener(TalkEndEvenetHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TalkEventHandler()
    {
        snakeAnimation.AnimationState.SetAnimation(0, snakeTalk, true);
    }
    private void TalkEndEvenetHandler()
    {
        snakeAnimation.AnimationState.SetAnimation(0, snakeIdle, true);
    }
}
