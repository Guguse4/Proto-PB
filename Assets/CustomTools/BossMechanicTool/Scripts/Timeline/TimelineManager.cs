using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction startTimelineAction;
    
    private PlayableDirector _playableDirector;
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        playerInput = GetComponent<PlayerInput>();
        startTimelineAction = playerInput.actions.FindAction("StartTimeline");
    }
    void Update()
    {
        if (startTimelineAction.IsPressed() && _playableDirector.state != PlayState.Playing)
        {
            _playableDirector.Play();
        }
    }
}
