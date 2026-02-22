using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class UrpAutoConfigurator
{
    private const string PipelineAssetPath = "Assets/Resources/Rendering/UniversalRenderPipelineAsset.asset";
    private const string PipelineResourcePath = "Rendering/UniversalRenderPipelineAsset";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ApplyRuntime()
    {
        if (GraphicsSettings.defaultRenderPipeline != null)
            return;

        RenderPipelineAsset pipelineAsset = Resources.Load<RenderPipelineAsset>(PipelineResourcePath);
        if (pipelineAsset == null)
            return;

        GraphicsSettings.defaultRenderPipeline = pipelineAsset;
        QualitySettings.renderPipeline = pipelineAsset;
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void ApplyEditor()
    {
        RenderPipelineAsset pipelineAsset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(PipelineAssetPath);
        if (pipelineAsset == null)
            return;

        if (GraphicsSettings.defaultRenderPipeline != pipelineAsset)
            GraphicsSettings.defaultRenderPipeline = pipelineAsset;

        if (QualitySettings.renderPipeline != pipelineAsset)
            QualitySettings.renderPipeline = pipelineAsset;

        AssetDatabase.SaveAssets();
    }
#endif
}
