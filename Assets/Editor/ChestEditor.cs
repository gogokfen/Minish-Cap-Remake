using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chest))]
public class ChestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the Chest instance
        Chest chest = (Chest)target;

        // Disable GUI to start with
        EditorGUI.BeginDisabledGroup(true);
        base.OnInspectorGUI();
        EditorGUI.EndDisabledGroup();

        // Start custom GUI
        EditorGUILayout.LabelField("Chest Type", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        
        // Checkboxes for each chest type
        bool keyChest = EditorGUILayout.Toggle("Key Chest", chest.keyChest);
        bool bossKeyChest = EditorGUILayout.Toggle("Boss Key Chest", chest.bossKeyChest);
        bool mapChest = EditorGUILayout.Toggle("Map Chest", chest.mapChest);
        bool compassChest = EditorGUILayout.Toggle("Compass Chest", chest.compassChest);
        bool gustJarChest = EditorGUILayout.Toggle("Gust Jar Chest", chest.gustJarChest);
        bool heartPieceChest = EditorGUILayout.Toggle("Heart Piece Chest", chest.heartPieceChest);
        bool rupeeChest = EditorGUILayout.Toggle("Rupee Chest", chest.rupeeChest);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(chest, "Chest Type Change");

            // Ensure only one chest type is selected at a time
            chest.keyChest = keyChest;
            chest.bossKeyChest = bossKeyChest;
            chest.mapChest = mapChest;
            chest.compassChest = compassChest;
            chest.gustJarChest = gustJarChest;
            chest.heartPieceChest = heartPieceChest;
            chest.rupeeChest = rupeeChest;

            // Ensure only one boolean is true at a time
            if (keyChest)
            {
                chest.bossKeyChest = false;
                chest.mapChest = false;
                chest.compassChest = false;
                chest.gustJarChest = false;
                chest.heartPieceChest = false;
                chest.rupeeChest = false;
            }
            else if (bossKeyChest)
            {
                chest.keyChest = false;
                chest.mapChest = false;
                chest.compassChest = false;
                chest.gustJarChest = false;
                chest.heartPieceChest = false;
                chest.rupeeChest = false;
            }
            else if (mapChest)
            {
                chest.keyChest = false;
                chest.bossKeyChest = false;
                chest.compassChest = false;
                chest.gustJarChest = false;
                chest.heartPieceChest = false;
                chest.rupeeChest = false;
            }
            else if (compassChest)
            {
                chest.keyChest = false;
                chest.bossKeyChest = false;
                chest.mapChest = false;
                chest.gustJarChest = false;
                chest.heartPieceChest = false;
                chest.rupeeChest = false;
            }
            else if (gustJarChest)
            {
                chest.keyChest = false;
                chest.bossKeyChest = false;
                chest.mapChest = false;
                chest.compassChest = false;
                chest.heartPieceChest = false;
                chest.rupeeChest = false;
            }
            else if (heartPieceChest)
            {
                chest.keyChest = false;
                chest.bossKeyChest = false;
                chest.mapChest = false;
                chest.compassChest = false;
                chest.gustJarChest = false;
                chest.rupeeChest = false;
            }
            else if (rupeeChest)
            {
                chest.keyChest = false;
                chest.bossKeyChest = false;
                chest.mapChest = false;
                chest.compassChest = false;
                chest.gustJarChest = false;
                chest.heartPieceChest = false;
            }

            EditorUtility.SetDirty(chest);
        }
    }
}
