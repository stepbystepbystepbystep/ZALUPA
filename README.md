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
    - ищет видео в нескольких путях (включая build-папки),
    - выводит видео на задний фон через камеру,
    - воспроизводит звук,
    - прокручивает несколько видео по кругу.

## Как использовать видео

### В Unity Editor
1. Положите видеофайлы в `Assets/StreamingAssets/BackgroundVideos`.
2. Запустите сцену — видео начнёт играть автоматически.

### В готовом build
Используйте один из путей рядом с билдом:

- `build/CubeFortress_Data/StreamingAssets/BackgroundVideos`
- `build/BackgroundVideos`

> Важно: если вы добавили видео в `Assets/StreamingAssets` после сборки, нужно либо пересобрать игру, либо положить видео в один из путей рядом с готовым билдом выше.
