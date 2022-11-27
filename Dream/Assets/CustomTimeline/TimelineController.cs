using UnityEngine;
using UnityEngine.Playables;
public class TimelineController : MonoBehaviour {
    public VoidEvent pauseEvent;
    public VoidEvent resumeEvent;

    private void Start() 
    {
        //LoadScene();
        if(pauseEvent!=null)
            pauseEvent.action += Pause;
        if(resumeEvent!=null)
            resumeEvent.action += Resume;
    }
    private void OnDestroy() 
    {
        if(pauseEvent!=null)
            pauseEvent.action -= Pause;
        if(resumeEvent!=null)
            resumeEvent.action -= Resume;
    }
    void Pause()
    {
        GetComponent<PlayableDirector>().Pause();
    }  
    void Resume()
    {
        GetComponent<PlayableDirector>().Resume();
    }  
    void Init(){
        GetComponent<PlayableDirector>().Play();
    }
    //void LoadScene(){
    //    if(LoadSceneManager.ins != null){  
    //        Pause();
    //        LoadSceneManager.ins.SceneLoadOver(Resume);
    //    }
    //}
}