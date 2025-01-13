using System.Collections;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.Networking;

public class WebRTCPublisher_1 : MonoBehaviour
{
    public string url = "http://localhost:8889/unity1/whip";
    public int videoWidth = 1280;
    public int videoHeight = 720;

    private RTCPeerConnection pc;
    private MediaStream videoStream;

    void Start()
    {
        pc = new RTCPeerConnection();
        Camera sourceCamera = gameObject.GetComponent<Camera>();
        videoStream = sourceCamera.CaptureStream(videoWidth, videoHeight);
        foreach (var track in videoStream.GetTracks())
        {
            pc.AddTrack(track);
        }

        StartCoroutine(WebRTC.Update());
        StartCoroutine(createOffer());
    }

    private IEnumerator createOffer()
    {
        var op = pc.CreateOffer();
        yield return op;
        if (op.IsError)
        {
            Debug.LogError("CreateOffer() failed");
            yield break;
        }

        yield return setLocalDescription(op.Desc);
    }

    private IEnumerator setLocalDescription(RTCSessionDescription offer)
    {
        var op = pc.SetLocalDescription(ref offer);
        yield return op;
        if (op.IsError)
        {
            Debug.LogError("SetLocalDescription() failed");
            yield break;
        }

        yield return postOffer(offer);
    }

    private IEnumerator postOffer(RTCSessionDescription offer)
    {
        var content = new System.Net.Http.StringContent(offer.sdp);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/sdp");
        var client = new System.Net.Http.HttpClient();

        var task = System.Threading.Tasks.Task.Run(async () => {
            var res = await client.PostAsync(new System.UriBuilder(url).Uri, content);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        });
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
            yield break;
        }

        yield return setRemoteDescription(task.Result);
    }

    private IEnumerator setRemoteDescription(string answer)
    {
        RTCSessionDescription desc = new RTCSessionDescription();
        desc.type = RTCSdpType.Answer;
        desc.sdp = answer;
        var op = pc.SetRemoteDescription(ref desc);
        yield return op;
        if (op.IsError)
        {
            Debug.LogError("SetRemoteDescription() failed");
            yield break;
        }

        yield break;
    }

    void OnDestroy()
    {
        pc?.Close();
        pc?.Dispose();
        videoStream?.Dispose();
    }
}