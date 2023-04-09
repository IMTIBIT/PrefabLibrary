using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabLibraryGUIHandler
{
    public int DisplayCategories(List<PrefabCategory> categories, ref Vector2 scrollPosition, float aspectRatio, int selectedCategoryIndex)
    {
        if (categories == null || categories.Count == 0)
        {
            return -1;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.BeginVertical("box");

        for (int i = 0; i < categories.Count; i++)
        {
            PrefabCategory category = categories[i];
            if (category == null)
            {
                continue;
            }

            if (category.categoryImage != null)
            {
                GUI.backgroundColor = (selectedCategoryIndex == i) ? Color.gray : Color.white;

                float imageWidth = EditorGUIUtility.currentViewWidth - 30;
                float imageHeight = imageWidth / aspectRatio;
                Rect imageRect = GUILayoutUtility.GetRect(imageWidth, imageHeight, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));

                // Highlight category image title when hovering over it
                if (Event.current.type == EventType.Repaint && imageRect.Contains(Event.current.mousePosition))
                {
                    GUIStyle titleStyle = new GUIStyle(EditorStyles.whiteLabel);
                    titleStyle.alignment = TextAnchor.MiddleCenter;
                    titleStyle.normal.textColor = Color.white;
                    titleStyle.hover.textColor = Color.blue;
                    Rect titleRect = new Rect(imageRect.x, imageRect.y + imageHeight, imageRect.width, 20);
                    EditorGUI.DropShadowLabel(titleRect, category.categoryImageTitle, titleStyle);
                }
                EditorGUI.DrawPreviewTexture(imageRect, category.categoryImage);

                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && imageRect.Contains(Event.current.mousePosition))
                {
                    selectedCategoryIndex = i;
                }
                GUI.backgroundColor = Color.white;
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        return selectedCategoryIndex;
    }

    public void DisplayPrefabs(List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");

        // Display prefabs as icons/previews from top down
        int prefabPerRow = Mathf.FloorToInt((EditorGUIUtility.currentViewWidth - 20) / 70f);
        int totalRows = Mathf.CeilToInt(prefabs.Count / (float)prefabPerRow);

        for (int row = 0; row < totalRows; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int column = 0; column < prefabPerRow; column++)
            {
                int index = row * prefabPerRow + column;

                if (index < prefabs.Count)
                {
                    if (prefabs[index] != null)
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(60), GUILayout.Height(80));
                        Rect prefabRect = GUILayoutUtility.GetRect(50, 50, GUILayout.Width(50), GUILayout.Height(50));
                        EditorGUI.DrawPreviewTexture(prefabRect, AssetPreview.GetAssetPreview(prefabs[index]), null, ScaleMode.ScaleToFit);
                        EditorGUILayout.LabelField(prefabs[index].name, EditorStyles.wordWrappedMiniLabel);
                        GUILayout.FlexibleSpace();

                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && prefabRect.Contains(Event.current.mousePosition))
                        {
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.objectReferences = new Object[] { prefabs[index] };
                            DragAndDrop.StartDrag(prefabs[index].name);
                        }

                        EditorGUILayout.EndVertical();
                    }
                }
                else
                {
                    GUILayout.FlexibleSpace();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

}

