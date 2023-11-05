using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

[Overlay(typeof(SceneView), "Scene Selector/DropdownList", true)]
public class XRGSceneSelectorShortcut : ToolbarOverlay
{
    public const string homeBtnIcon = "Assets/SceneSelectorToolTip/Icons/homeIcon.png";
    public const string dropdown_icon = "Assets/SceneSelectorToolTip/Icons/sceneIcon.png";

    XRGSceneSelectorShortcut() : base(BaseSceneButton.baseButton_id, SceneDropDownButton.dropdownSelector_id) { }

    [EditorToolbarElement(dropdownSelector_id, typeof(SceneView))]

    class SceneDropDownButton : EditorToolbarDropdown, IAccessContainerWindow
    {
        public const string dropdownSelector_id = "SceneSelectionOverlay/SceneDropdownToggle";

        public EditorWindow containerWindow { get; set; }
        SceneDropDownButton()
        {
            text = "Scene Dropdown";
            tooltip = "Go to base scene";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(XRGSceneSelectorShortcut.dropdown_icon);
            clicked += ShowSceneMenu;
        }

        private void ShowSceneMenu()
        {
            GenericMenu menu = new GenericMenu();

            Scene currentScene = EditorSceneManager.GetActiveScene();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string path = EditorBuildSettings.scenes[i].path;

                string name = Path.GetFileNameWithoutExtension(path);

                menu.AddItem(new GUIContent(name), string.Compare(currentScene.name, name) == 0, () => OpenScene(currentScene, path));
            }
            menu.ShowAsContext();
        }

        private void OpenScene(Scene currentScene, string path)
        {
            if (currentScene.isDirty)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(path);
            }
            else
                EditorSceneManager.OpenScene(path);
        }
    }

    [EditorToolbarElement(baseButton_id, typeof(SceneView))]
    class BaseSceneButton : EditorToolbarButton, IAccessContainerWindow
    {
        public const string baseButton_id = "SceneSelectionOverlay/BaseSceneButton";
        public EditorWindow containerWindow { get; set; }
        
        BaseSceneButton()
        {
            text = "Base Scene";
            tooltip = "Select a scene to load";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(XRGSceneSelectorShortcut.homeBtnIcon);
            clicked += OpenFirstBuildScene;
        }

        [Shortcut("SceneSelector/Go to base scene", KeyCode.BackQuote, ShortcutModifiers.Control)]
        public static void OpenFirstBuildScene()
        {
            Scene currentScene = EditorSceneManager.GetActiveScene();
            if (currentScene.isDirty)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
            }
            else
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
        }
    }
}


