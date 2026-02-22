using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundVideoPlayer : MonoBehaviour
{
    private static readonly string[] SupportedExtensions = { ".mp4", ".mov", ".m4v", ".webm" };

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private string[] videos;
    private int currentIndex;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (FindObjectOfType<BackgroundVideoPlayer>() != null)
            return;

        var host = new GameObject("BackgroundVideoPlayer");
        DontDestroyOnLoad(host);
        host.AddComponent<BackgroundVideoPlayer>();
    }

    private void Awake()
    {
        videos = FindVideos();

        if (videos.Length == 0)
        {
            Debug.LogWarning("BackgroundVideoPlayer: no videos found. Check StreamingAssets/BackgroundVideos or build folder paths.");
            return;
        }

        InitializeComponents();
        StartCoroutine(WaitForCameraAndPlay());
    }

    private string[] FindVideos()
    {
        foreach (string directory in GetCandidateDirectories())
        {
            if (!Directory.Exists(directory))
                continue;

            string[] found = Directory
                .GetFiles(directory)
                .Where(path => SupportedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                .OrderBy(path => path)
                .ToArray();

            if (found.Length > 0)
            {
                Debug.Log($"BackgroundVideoPlayer: using videos from {directory}");
                return found;
            }
        }

        return Array.Empty<string>();
    }

    private IEnumerable<string> GetCandidateDirectories()
    {
        yield return Path.Combine(Application.streamingAssetsPath, "BackgroundVideos");

        string dataPath = Application.dataPath;
        if (!string.IsNullOrEmpty(dataPath))
        {
            string executableDirectory = Path.GetDirectoryName(dataPath);
            if (!string.IsNullOrEmpty(executableDirectory))
            {
                yield return Path.Combine(executableDirectory, "BackgroundVideos");
                yield return Path.Combine(executableDirectory, "StreamingAssets", "BackgroundVideos");
                yield return Path.Combine(executableDirectory, "CubeFortress_Data", "StreamingAssets", "BackgroundVideos");
            }
        }
    }

    private IEnumerator WaitForCameraAndPlay()
    {
        while (Camera.main == null)
            yield return null;

        videoPlayer.targetCamera = Camera.main;
        StartPlayback(0);
    }

    private void InitializeComponents()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;

        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.aspectRatio = VideoAspectRatio.FitHorizontally;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void StartPlayback(int index)
    {
        if (videos == null || videos.Length == 0)
            return;

        currentIndex = index;
        videoPlayer.url = videos[currentIndex];
        videoPlayer.Prepare();
        StartCoroutine(PlayWhenPrepared());
    }

    private IEnumerator PlayWhenPrepared()
    {
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
    }

    private void OnLoopPointReached(VideoPlayer source)
    {
        int nextIndex = (currentIndex + 1) % videos.Length;
        StartPlayback(nextIndex);
    }

    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogWarning($"Background video playback error: {message}");
        int nextIndex = (currentIndex + 1) % videos.Length;
        StartPlayback(nextIndex);
    }

    private void OnDestroy()
    {
        if (videoPlayer == null)
            return;

        videoPlayer.errorReceived -= OnVideoError;
        videoPlayer.loopPointReached -= OnLoopPointReached;
    }
}
