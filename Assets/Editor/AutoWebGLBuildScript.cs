using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoWebGLBuildScript
{
    [MenuItem("Build/Auto Build WebGL (Player Settings)")]
    public static void BuildWebGLWithPlayerSettings()
    {
        Debug.Log("=== WebGL Player Settings ë°˜ì˜ ë¹Œë“œ ì‹œì‘ ===");
        
        // í˜„ì¬ Player Settings ì •ë³´ ì¶œë ¥
        LogCurrentPlayerSettings();
        
        // ë¹Œë“œ ì¶œë ¥ ê²½ë¡œ ì„¤ì • (Product Name ê¸°ë°˜)
        string buildPath = @"E:/TDS/Builds/WebGL";
        
        // Product Nameì´ ì„¤ì •ë˜ì–´ ìˆë‹¤ë©´ ê²½ë¡œì— ë°˜ì˜
        if (!string.IsNullOrEmpty(PlayerSettings.productName))
        {
            string safeName = PlayerSettings.productName.Replace(" ", "_");
            // íŠ¹ìˆ˜ë¬¸ì ì œê±°
            safeName = System.Text.RegularExpressions.Regex.Replace(safeName, @"[^\w\-_]", "");
            buildPath = Path.Combine(Path.GetDirectoryName(buildPath), safeName);
        }
        
        // ì¶œë ¥ ë””ë ‰í† ë¦¬ ìƒì„±
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
            Debug.Log($"ë¹Œë“œ ì¶œë ¥ ë””ë ‰í† ë¦¬ ìƒì„±: {buildPath}");
        }
        
        // ë¹Œë“œí•  ì”¬ë“¤ ê°€ì ¸ì˜¤ê¸° (Build Settingsì—ì„œ í™œì„±í™”ëœ ì”¬ë§Œ)
        string[] scenes = GetBuildScenes();
        if (scenes.Length == 0)
        {
            Debug.LogError("ë¹Œë“œí•  ì”¬ì´ ì—†ìŠµë‹ˆë‹¤. Build Settingsì—ì„œ ì”¬ì„ ì¶”ê°€í•˜ì„¸ìš”.");
            return;
        }
        
        // WebGL ë¹Œë“œ ì˜µì…˜ ì„¤ì • (Player Settings ì™„ì „ ë°˜ì˜)
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        
        // ë¹Œë“œ ì˜µì…˜ì„ Player Settingsì— ë”°ë¼ ì„¤ì •
        buildPlayerOptions.options = GetBuildOptionsFromPlayerSettings();
        
        // WebGL íŠ¹ìˆ˜ ì„¤ì • ì ìš©
        ApplyWebGLSettings();
        
        Debug.Log($"ğŸŒ WebGL ë¹Œë“œ ì‹œì‘");
        Debug.Log($"ğŸ“ ë¹Œë“œ ê²½ë¡œ: {buildPlayerOptions.locationPathName}");
        Debug.Log($"ğŸ® ì œí’ˆëª…: {PlayerSettings.productName}");
        Debug.Log($"ğŸ¢ íšŒì‚¬ëª…: {PlayerSettings.companyName}");
        Debug.Log($"ğŸ“‹ ë²„ì „: {PlayerSettings.bundleVersion}");
        
        // WebGL ë¹Œë“œ ì‹¤í–‰
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        // ë¹Œë“œ ê²°ê³¼ í™•ì¸
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log($"âœ… WebGL ë¹Œë“œ ì„±ê³µ!");
            Debug.Log($"ğŸ“¦ ë¹Œë“œ í¬ê¸°: {FormatBytes(report.summary.totalSize)}");
            Debug.Log($"â±ï¸ ë¹Œë“œ ì‹œê°„: {report.summary.totalTime}");
            Debug.Log($"ğŸ“ ë¹Œë“œ ê²½ë¡œ: {buildPath}");
            Debug.Log($"ğŸŒ WebGL ë¹Œë“œ ì™„ë£Œ!");
        }
        else
        {
            Debug.LogError($"âŒ WebGL ë¹Œë“œ ì‹¤íŒ¨: {report.summary.result}");
            if (report.summary.totalErrors > 0)
            {
                Debug.LogError($"ì—ëŸ¬ ìˆ˜: {report.summary.totalErrors}");
            }
            if (report.summary.totalWarnings > 0)
            {
                Debug.LogWarning($"ê²½ê³  ìˆ˜: {report.summary.totalWarnings}");
            }
        }
        
        Debug.Log("=== WebGL Player Settings ë°˜ì˜ ë¹Œë“œ ì™„ë£Œ ===");
    }
    
    private static void LogCurrentPlayerSettings()
    {
        Debug.Log("=== í˜„ì¬ WebGL Player Settings ===");
        Debug.Log($"ğŸ® ì œí’ˆëª…: {PlayerSettings.productName}");
        Debug.Log($"ğŸ¢ íšŒì‚¬ëª…: {PlayerSettings.companyName}");
        Debug.Log($"ğŸ“‹ ë²„ì „: {PlayerSettings.bundleVersion}");
        Debug.Log($"ğŸ–¼ï¸ ê¸°ë³¸ ì•„ì´ì½˜: {(PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown) != null ? "ì„¤ì •ë¨" : "ì—†ìŒ")}");
        
        // WebGL ì „ìš© ì„¤ì •ë“¤
        Debug.Log($"ğŸŒ WebGL í…œí”Œë¦¿: {PlayerSettings.WebGL.template}");
        Debug.Log($"ğŸ’¾ WebGL ë©”ëª¨ë¦¬ í¬ê¸°: {PlayerSettings.WebGL.memorySize}MB");
        Debug.Log($"ğŸ“¦ WebGL ì••ì¶• í¬ë§·: {PlayerSettings.WebGL.compressionFormat}");
        Debug.Log($"âš ï¸ WebGL ì˜ˆì™¸ ì§€ì›: {PlayerSettings.WebGL.exceptionSupport}");
        Debug.Log($"ğŸ’½ WebGL ë°ì´í„° ìºì‹±: {PlayerSettings.WebGL.dataCaching}");
        Debug.Log($"ğŸ”§ WebGL ë§ì»¤ íƒ€ê²Ÿ: {PlayerSettings.WebGL.linkerTarget}");
        Debug.Log($"ğŸ¯ WebGL ì½”ë“œ ìµœì í™”: {PlayerSettings.WebGL.codeOptimization}");
        Debug.Log("=====================================");
    }
    
    private static BuildOptions GetBuildOptionsFromPlayerSettings()
    {
        BuildOptions options = BuildOptions.None;
        
        // Development Build ì„¤ì • í™•ì¸
        if (EditorUserBuildSettings.development)
        {
            options |= BuildOptions.Development;
            Debug.Log("âœ… Development Build ëª¨ë“œ í™œì„±í™”");
        }
        
        // Script Debugging ì„¤ì • í™•ì¸
        if (EditorUserBuildSettings.allowDebugging)
        {
            options |= BuildOptions.AllowDebugging;
            Debug.Log("âœ… Script Debugging í™œì„±í™”");
        }
        
        // Profiler ì„¤ì • í™•ì¸
        if (EditorUserBuildSettings.connectProfiler)
        {
            options |= BuildOptions.ConnectWithProfiler;
            Debug.Log("âœ… Profiler ì—°ê²° í™œì„±í™”");
        }
        
        // Deep Profiling ì„¤ì • í™•ì¸
        if (EditorUserBuildSettings.buildWithDeepProfilingSupport)
        {
            options |= BuildOptions.EnableDeepProfilingSupport;
            Debug.Log("âœ… Deep Profiling ì§€ì› í™œì„±í™”");
        }
        
        // Auto Run Player ì„¤ì • í™•ì¸
        if (EditorUserBuildSettings.autoRunPlayer)
        {
            options |= BuildOptions.AutoRunPlayer;
            Debug.Log("âœ… ë¹Œë“œ í›„ ìë™ ì‹¤í–‰ í™œì„±í™”");
        }
        
        return options;
    }
    
    private static void ApplyWebGLSettings()
    {
        Debug.Log("ğŸŒ WebGL íŠ¹ìˆ˜ ì„¤ì • ì ìš© ë° ê²€ì¦ ì¤‘...");
        
        Debug.Log($"ğŸŒ WebGL í…œí”Œë¦¿ ì‚¬ìš©: {PlayerSettings.WebGL.template}");
        Debug.Log($"ğŸ’¾ WebGL ë©”ëª¨ë¦¬ í¬ê¸°: {PlayerSettings.WebGL.memorySize}MB");
        Debug.Log($"ğŸ“¦ WebGL ì••ì¶• í¬ë§·: {PlayerSettings.WebGL.compressionFormat}");
        Debug.Log($"âš ï¸ WebGL ì˜ˆì™¸ ì§€ì›: {PlayerSettings.WebGL.exceptionSupport}");
        Debug.Log($"ğŸ’½ WebGL ë°ì´í„° ìºì‹±: {PlayerSettings.WebGL.dataCaching}");
        
        // WebGL ìµœì í™” ì„¤ì • í™•ì¸ ë° ê¶Œì¥ì‚¬í•­
        if (PlayerSettings.WebGL.memorySize < 256)
        {
            Debug.LogWarning("âš ï¸ WebGL ë©”ëª¨ë¦¬ í¬ê¸°ê°€ 256MB ë¯¸ë§Œì…ë‹ˆë‹¤. ê³¼í•™ì‹¤í—˜ ì‹œë®¬ë ˆì´ì…˜ì—ëŠ” 512MB ì´ìƒ ê¶Œì¥í•©ë‹ˆë‹¤.");
        }
        else if (PlayerSettings.WebGL.memorySize >= 512)
        {
            Debug.Log("âœ… WebGL ë©”ëª¨ë¦¬ í¬ê¸°ê°€ ì ì ˆí•©ë‹ˆë‹¤ (512MB ì´ìƒ).");
        }
        
        if (string.IsNullOrEmpty(PlayerSettings.WebGL.template) || PlayerSettings.WebGL.template == "APPLICATION:Default")
        {
            Debug.LogWarning("âš ï¸ WebGL í…œí”Œë¦¿ì´ ê¸°ë³¸ê°’ì…ë‹ˆë‹¤. êµìœ¡ìš© í…œí”Œë¦¿ ì‚¬ìš©ì„ ê¶Œì¥í•©ë‹ˆë‹¤.");
        }
        else
        {
            Debug.Log($"âœ… WebGL í…œí”Œë¦¿ ì„¤ì •ë¨: {PlayerSettings.WebGL.template}");
        }
        
        // WebGL ì••ì¶• ì„¤ì • í™•ì¸
        if (PlayerSettings.WebGL.compressionFormat == WebGLCompressionFormat.Disabled)
        {
            Debug.LogWarning("âš ï¸ WebGL ì••ì¶•ì´ ë¹„í™œì„±í™”ë˜ì–´ ìˆìŠµë‹ˆë‹¤. íŒŒì¼ í¬ê¸°ê°€ í´ ìˆ˜ ìˆìŠµë‹ˆë‹¤.");
        }
        else
        {
            Debug.Log($"âœ… WebGL ì••ì¶• í™œì„±í™”: {PlayerSettings.WebGL.compressionFormat}");
        }
        
        // ê³¼í•™ì‹¤í—˜ ì‹œë®¬ë ˆì´ì…˜ì— ìµœì í™”ëœ ì„¤ì • ê¶Œì¥ì‚¬í•­
        Debug.Log("ğŸ“š ê³¼í•™ì‹¤í—˜ ì‹œë®¬ë ˆì´ì…˜ ìµœì í™” ê¶Œì¥ì‚¬í•­:");
        Debug.Log("  - ë©”ëª¨ë¦¬: 512MB ì´ìƒ");
        Debug.Log("  - ì••ì¶•: Gzip ë˜ëŠ” Brotli");
        Debug.Log("  - ì˜ˆì™¸ ì§€ì›: ExplicitlyThrownExceptionsOnly");
        Debug.Log("  - ë°ì´í„° ìºì‹±: í™œì„±í™”");
    }
    
    private static string[] GetBuildScenes()
    {
        // Build Settingsì—ì„œ í™œì„±í™”ëœ ì”¬ë“¤ë§Œ ê°€ì ¸ì˜¤ê¸°
        var enabledScenes = new System.Collections.Generic.List<string>();
        
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                enabledScenes.Add(scene.path);
            }
        }
        
        Debug.Log($"ğŸ“‹ ë¹Œë“œí•  ì”¬ ìˆ˜: {enabledScenes.Count}");
        foreach (var scene in enabledScenes)
        {
            Debug.Log($"  - {scene}");
        }
        
        return enabledScenes.ToArray();
    }
    
    private static string FormatBytes(ulong bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
