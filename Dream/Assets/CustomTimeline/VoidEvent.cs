using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "TimelineEvent", menuName = "Developer/TimelineEvent", order = 0)]
public class VoidEvent : ScriptableObject 
{
    public event UnityAction action;
    public void Execute()
    {
        if(action!=null)
            action.Invoke();
    }
    private void OnDisable()
    {
        action = null;
    }
}