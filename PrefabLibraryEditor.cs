using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabLibraryEditor : EditorWindow
{
    private List<PrefabCategory> categories;
    private Vector2 scrollPosition;
    private PrefabLibraryGUIHandler guiHandler;
    private bool showAddCategorySection;
    private int selectedCategoryIndex = -1;

    [MenuItem("Window/Prefab Library Editor")]
    public static void ShowWindow()
    {
        GetWindow<PrefabLibraryEditor>("Prefab Library Editor");
    }

    private void OnEnable()
    {
        if (categories == null)
        {
            categories = new List<PrefabCategory>();
        }

        guiHandler = new PrefabLibraryGUIHandler();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Categories", EditorStyles.boldLabel);

        if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
        {
            showAddCategorySection = !showAddCategorySection;
        }

        if (showAddCategorySection)
        {
            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < categories.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                categories[i] = (PrefabCategory)EditorGUILayout.ObjectField(categories[i], typeof(PrefabCategory), false);

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    categories.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Category", GUILayout.Width(100)))
            {
                categories.Add(null);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        selectedCategoryIndex = guiHandler.DisplayCategories(categories, ref scrollPosition, 21f / 9f, selectedCategoryIndex);

        if (selectedCategoryIndex >= 0 && selectedCategoryIndex < categories.Count)
        {
            guiHandler.DisplayPrefabs(categories[selectedCategoryIndex].prefabs);
        }

        // Process the picked GameObject from the Object Picker
        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            var pickedGameObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
            var targetCategoryID = EditorGUIUtility.GetObjectPickerControlID();

            if (pickedGameObject != null)
            {
                AddPrefabToCategory(pickedGameObject, targetCategoryID);
            }
            Repaint();
        }
    }

    private void AddPrefabToCategory(GameObject prefab, int categoryID)
    {
        var targetCategory = EditorUtility.InstanceIDToObject(categoryID) as PrefabCategory;

        if (targetCategory != null && !targetCategory.prefabs.Contains(prefab))
        {
            targetCategory.prefabs.Add(prefab);
            EditorUtility.SetDirty(targetCategory);
        }
    }
}
