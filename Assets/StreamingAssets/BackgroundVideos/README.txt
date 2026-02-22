Drop background videos in this folder.
Supported formats: .mp4, .mov, .m4v, .webm

How it works:
- The game loads videos from Assets/StreamingAssets/BackgroundVideos.
- The first video starts automatically after scene load.
- Audio is played with the video.
- If there are several videos, they are played one by one in a loop.

Build note:
- In a built game, use <BuildName>_Data/StreamingAssets/BackgroundVideos
  (this folder is generated during build and may not exist in source tree).
