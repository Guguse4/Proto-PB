using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playableDirector.Play();
    }
}
