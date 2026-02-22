# Cube Tower — технические изменения

Краткий журнал изменений в проекте.

## Что уже сделано

- Проект подготовлен под **URP (Universal Render Pipeline)**:
  - добавлен пакет URP в `Packages/manifest.json`;
  - добавлены URP-ассеты рендера в `Assets/Resources/Rendering`;
  - добавлен автоконфиг `UrpAutoConfigurator`, который назначает URP pipeline при старте (Editor/Runtime), если он не назначен.

- Добавлено фоновое воспроизведение видео со звуком:
  - создана папка для видео: `Assets/StreamingAssets/BackgroundVideos`;
  - добавлен скрипт `BackgroundVideoPlayer`, который автоматически:
    - ищет видео в `StreamingAssets/BackgroundVideos` (`.mp4`, `.mov`, `.m4v`, `.webm`),
    - выводит видео на задний фон через камеру,
    - воспроизводит звук,
    - прокручивает несколько видео по кругу.

## Как использовать папку с видео

1. Положите ваши видеофайлы в `Assets/StreamingAssets/BackgroundVideos`.
2. Запустите сцену — видео начнёт играть автоматически на фоне игры.
