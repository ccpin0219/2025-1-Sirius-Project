using UnityEngine;
using UnityEditor;
using System.IO;

public class SkillArtAutoAssigner
{
    [MenuItem("Tools/Assign Skill Art Sprites Automatically")]
    public static void AssignArtSprites()
    {
        string skillPath = "Assets/Resources/Skills";
        string spritePath = "Assets/Resources/SkillArts";

        string[] skillGUIDs = AssetDatabase.FindAssets("t:Skill", new[] { skillPath });
        int assignedCount = 0;

        foreach (string guid in skillGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Skill skill = AssetDatabase.LoadAssetAtPath<Skill>(assetPath);

            if (skill == null) continue;

            // 스킬 이름으로 sprite 찾기
            string spriteAssetPath = $"{spritePath}/{skill.skillName}.png";
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);

            if (sprite != null)
            {
                skill.artSprite = sprite;
                EditorUtility.SetDirty(skill);
                assignedCount++;
                Debug.Log($"✅ {skill.skillName} → {sprite.name} 연결됨");
            }
            else
            {
                Debug.LogWarning($"⚠️ Sprite 파일 없음: {skill.skillName}.png");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"🎉 완료: {assignedCount}개의 스킬에 artSprite가 설정됨");
    }
}
