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

    [Header("Player")]
    public SkeletonAnimation playerAnimation;
    public AnimationReferenceAsset playerIdle;
    public AnimationReferenceAsset getLeaf;
    public AnimationReferenceAsset getLily;
    public AnimationReferenceAsset getFire;
    [Space(20)]

    public SpriteRenderer lilyPillar;
    public Sprite newPillar;

    public UnityEngine.Video.VideoPlayer VideoPlayer;

    private int birdAnimaitonIndex = 0;

    [SerializeField]private double videoLength;
    private float startPlayTime;
    private bool isPlayingFinal = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.NPCTalkDEvent.AddListener(TalkEventHandler);
        EventManager.instance.NPCTalkEndDEvent.AddListener(TalkEndEvenetHandler);
        EventManager.instance.DeerPillarEvent.AddListener(DeerPillarChange);
        EventManager.instance.BirdTalkEvent.AddListener(BirdTalkHandler);
        EventManager.instance.BirdFirstTalk.AddListener(BirdFirstTalkHandler);
        EventManager.instance.BirdTalkEnd.AddListener(BirdTalkEndHandler);
        EventManager.instance.GetRelicEvent.AddListener(GetRelicHandler);

        videoLength = VideoPlayer.length;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startPlayTime > videoLength && isPlayingFinal)
        {
            FindObjectOfType<GameManager>().transform.Find("ScM").GetComponent<ScenesManager>().MainMenu();
        }
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

    private void DeerPillarChange()
    {
        lilyPillar.sprite = newPillar;
    }
    private void BirdFirstTalkHandler()
    {
        birdAnimation.AnimationState.SetAnimation(0, birdBlueTalk, true);
    }
    private void BirdTalkEndHandler()
    {
        birdAnimation.AnimationState.SetAnimation(0, birdRedIdle, true);
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

    private void GetRelicHandler(string relicName)
    {
        switch (relicName)
        {
            case "Leaf":
                playerAnimation.AnimationState.SetAnimation(0, getLeaf, false);
                break;
            case "Lily":
                playerAnimation.AnimationState.SetAnimation(0, getLily, false);
                break;
            case "Fire":
                playerAnimation.AnimationState.SetAnimation(0, getFire, false);
                break;
        }
    }

    public void PlayFinal()
    {
        VideoPlayer.Stop();
        VideoPlayer.Play();
        startPlayTime = Time.time;
        isPlayingFinal = true;
    }
    public void PlayCH3BGM()
    {
        SoundManager.instance.PlayBGM(2);
    }
}
