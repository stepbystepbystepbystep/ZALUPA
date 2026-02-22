using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class UrpAutoConfigurator
{
    private const string PipelineAssetPath = "Assets/Resources/Rendering/UniversalRenderPipelineAsset.asset";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ApplyRuntime()
    {
        if (GraphicsSettings.defaultRenderPipeline != null)
        {
            return;
        }

        var pipelineAsset = Resources.Load<UniversalRenderPipelineAsset>("Rendering/UniversalRenderPipelineAsset");
        if (pipelineAsset != null)
        {
            GraphicsSettings.defaultRenderPipeline = pipelineAsset;
            QualitySettings.renderPipeline = pipelineAsset;
        }
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void ApplyEditor()
    {
        var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelineAssetPath);
        if (pipelineAsset == null)
        {
            return;
        }

        if (GraphicsSettings.defaultRenderPipeline != pipelineAsset)
        {
            GraphicsSettings.defaultRenderPipeline = pipelineAsset;
        }

        for (var i = 0; i < QualitySettings.names.Length; i++)
        {
            if (QualitySettings.GetRenderPipelineAssetAt(i) == pipelineAsset)
            {
                continue;
            }

            QualitySettings.SetRenderPipelineAssetAt(i, pipelineAsset);
        }

        AssetDatabase.SaveAssets();
    }
#endif
}
