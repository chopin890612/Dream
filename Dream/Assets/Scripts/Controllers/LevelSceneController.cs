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
    public SkeletonAnimation birdAnimation;
    public AnimationReferenceAsset birdRedIdle;
    public AnimationReferenceAsset birdBlueTalk;

    public AnimationReferenceAsset birdBlue2Red;
    public AnimationReferenceAsset birdRedGiveFire;
    public AnimationReferenceAsset birdRedTalk;
    [Space(20)]

    public SpriteRenderer lilyPillar;
    public Sprite newPillar;

    private int birdAnimaitonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.NPCTalkDEvent.AddListener(TalkEventHandler);
        EventManager.instance.NPCTalkEndDEvent.AddListener(TalkEndEvenetHandler);
        EventManager.instance.DeerPillarEvent.AddListener(DeerPillarChange);
        EventManager.instance.BirdTalkEvent.AddListener(BirdTalkHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TalkEventHandler()
    {
        snakeAnimation.AnimationState.SetAnimation(0, snakeTalk, true);
        deerAnimation.AnimationState.SetAnimation(0, deerTalk, true);
        birdAnimation.AnimationState.SetAnimation(0, birdBlueTalk, true);
    }
    private void TalkEndEvenetHandler()
    {
        snakeAnimation.AnimationState.SetAnimation(0, snakeIdle, true);
        deerAnimation.AnimationState.SetAnimation(0, deerIdle, true);
        birdAnimation.AnimationState.SetAnimation(0, birdRedIdle, true);
    }

    private void DeerPillarChange()
    {
        lilyPillar.sprite = newPillar;
    }

    private void BirdTalkHandler()
    {
        switch (birdAnimaitonIndex)
        {
            case 0:
                birdAnimation.AnimationState.SetAnimation(0, birdBlue2Red, false);
                break;
            case 1:
                birdAnimation.AnimationState.SetAnimation(0, birdRedGiveFire, false);
                break;
            case 2:
                birdAnimation.AnimationState.SetAnimation(0, birdRedTalk, true);
                break;
        }
        birdAnimaitonIndex++;
    }
}
