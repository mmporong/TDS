using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoBatchProcessor
{
    [MenuItem("Tools/Process Batch")]
    public static void ProcessBatch()
    {
        Debug.Log("=== 배치 처리 시작 ===");
        
        // 패키지 임포트 대기
        AssetDatabase.Refresh();
        
        // PackageAssetCopier가 있다면 실행
        var copierType = System.Type.GetType("PackageAssetCopier");
        if (copierType != null)
        {
            var method = copierType.GetMethod("CopyFilesFromPackage", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                Debug.Log("PackageAssetCopier.CopyFilesFromPackage 실행");
                method.Invoke(null, null);
            }
        }
        
        // 최종 Asset Database 갱신
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        
        Debug.Log("=== 배치 처리 완료 ===");
    }
}
