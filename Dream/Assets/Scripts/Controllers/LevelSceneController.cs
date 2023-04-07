using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class LevelSceneController : MonoBehaviour
{
    [Header("Snake")]
    public SkeletonAnimation snakeAnimation;
    public AnimationReferenceAsset snakeTalk;
    public AnimationReferenceAsset snakeIdle;

    [Header("Deer")]
    public SkeletonAnimation deerAnimation;
    public AnimationReferenceAsset deerIdle;
    public AnimationReferenceAsset deerTalk;

    [Header("Eagle")]
    public SkeletonAnimation eagleAnimation;
    public AnimationReferenceAsset eagleIdle;
    public AnimationReferenceAsset eagleTalk;

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
        deerAnimation.AnimationState.SetAnimation(0, deerTalk, true);
    }
    private void TalkEndEvenetHandler()
    {
        snakeAnimation.AnimationState.SetAnimation(0, snakeIdle, true);
        deerAnimation.AnimationState.SetAnimation(0, deerIdle, true);
    }
}
