using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BossMechanicTool;
using BossMechanicTool.Timeline.Mechanic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    private Dictionary<int, List<MechanicControlTrack>> _groupedTracks = new Dictionary<int, List<MechanicControlTrack>>();
    private List<MechanicControlTrack> _mutedTracks = new List<MechanicControlTrack>();
    
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        SetupGroupedTracks();
        SelectTracks();
        _playableDirector.Play(); //_playableDirect.RebuildGraph();
        StartCoroutine(WaitAndUnmute());
    }
    
    /*
     * Coroutine to wait X seconds before unmuting tracks (and select new ones)
     */
    IEnumerator WaitAndUnmute()
    {
        yield return new WaitForSeconds(2);
        UnMuteTracks();
    }

    /*
     * Function to unmute all the tracks that were muted for future pull (then clear itself to not stack previous tracks)
     */
    void UnMuteTracks()
    {
        foreach (var track in _mutedTracks)
        {
            track.muted = false;
        }
        _mutedTracks.Clear();
    }

    /*
     * Function called once when entering the instance that set up the Dictionary "Dictionary<int, List<MechanicControlTrack>> _groupedTracks"
     */
    void SetupGroupedTracks()
    {
        var asset = _playableDirector.playableAsset as TimelineAsset;
        if (asset == null)
        {
            return;
        }
        
        _groupedTracks = new Dictionary<int, List<MechanicControlTrack>>();
        List<MechanicControlTrack> mechanicTracks = asset.GetOutputTracks().OfType<MechanicControlTrack>().ToList();
        foreach (var mechanicTrack in mechanicTracks)
        {
            if(mechanicTrack.randomGroup == -1)
                continue;
            
            if (!_groupedTracks.ContainsKey(mechanicTrack.randomGroup))
                _groupedTracks[mechanicTrack.randomGroup] = new List<MechanicControlTrack>();

            _groupedTracks[mechanicTrack.randomGroup].Add(mechanicTrack);
        }
    }

    /*
     * Function to call each time before a pull to calculate a new set of random suite of mechanics/tracks
     */
    void SelectTracks()
    {
        foreach (var group in _groupedTracks)
        {
            List<MechanicControlTrack> currentGroupTracks = group.Value;
            
            if(currentGroupTracks.Count < 1)
                continue;
            
            int randomIndex = UnityEngine.Random.Range(0, currentGroupTracks.Count);
            MechanicControlTrack chosenTrack = currentGroupTracks[randomIndex];

            foreach (var track in currentGroupTracks)
            {
                if (track != chosenTrack)
                {
                    track.muted = true;
                    _mutedTracks.Add(track);
                }
            }
        }
    }
}
