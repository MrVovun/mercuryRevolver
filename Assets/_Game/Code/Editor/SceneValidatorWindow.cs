#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace MR.EditorTools
{
    public class SceneValidatorWindow : EditorWindow
    {
        [MenuItem("Tools/MR/Scene Validator")]
        public static void Open() => GetWindow<SceneValidatorWindow>("MR Scene Validator");

        private Vector2 _scroll;
        private List<ValidationResult> _results = new();
        private bool _validateAllBuildScenes = false;

        private class ValidationResult
        {
            public string sceneName;
            public List<string> errors = new();
            public List<string> warnings = new();
        }

        private void OnGUI()
        {
            GUILayout.Label("Scene Validator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Checks Bootstrap/HQ/Case scenes for required and forbidden components.", MessageType.Info);

            _validateAllBuildScenes = EditorGUILayout.Toggle("Validate All Build Scenes", _validateAllBuildScenes);

            if (GUILayout.Button("Run Validation"))
            {
                RunValidation();
            }

            EditorGUILayout.Space();
            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            if (_results.Count == 0)
            {
                GUILayout.Label("No results yet.");
            }
            else
            {
                foreach (var r in _results)
                {
                    GUILayout.Space(8);
                    GUILayout.Label($"Scene: {r.sceneName}", EditorStyles.boldLabel);

                    if (r.errors.Count == 0 && r.warnings.Count == 0)
                        GUILayout.Label("✓ No issues found.", EditorStyles.helpBox);

                    foreach (var e in r.errors) EditorGUILayout.HelpBox(e, MessageType.Error);
                    foreach (var w in r.warnings) EditorGUILayout.HelpBox(w, MessageType.Warning);
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            if (GUILayout.Button("Open Build Settings")) EditorWindow.GetWindow(typeof(BuildPlayerWindow));
        }

        private void RunValidation()
        {
            _results.Clear();

            var paths = new List<string>();
            if (_validateAllBuildScenes)
            {
                paths.AddRange(EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path));
            }
            else
            {
                var active = EditorSceneManager.GetActiveScene();
                if (active.IsValid()) paths.Add(active.path);
            }

            foreach (var path in paths)
            {
                var result = new ValidationResult { sceneName = System.IO.Path.GetFileNameWithoutExtension(path) };
                // Open additively (in-memory) to inspect without losing current
                var opened = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                ValidateScene(opened, result);
                _results.Add(result);
            }
        }

        private static void ValidateScene(Scene scene, ValidationResult result)
        {
            var name = scene.name;
            // Heuristics: determine scene role by name
            bool isBootstrap = name.ToLower().Contains("bootstrap");
            bool isHQ = name.ToLower().Contains("hq");
            bool isCase = !isBootstrap && !isHQ; // default to "case" if not matching above

            // Helpers
            T[] FindAll<T>() where T : Component => Object.FindObjectsOfType<T>(true);
            bool Any<T>() where T : Component => FindAll<T>().Length > 0;

            // Components we care about
            var hasGame                  = Any<MR.Core.Game>();
            var hasServices              = Any<MR.Core.Services>();
            var hasInputService          = Any<MR.Systems.Input.InputService>();
#if ENABLE_INPUT_SYSTEM
            var hasInputAdapter          = Any<MR.Systems.Input.InputSystemAdapter>();
#else
            var hasInputAdapter          = false;
#endif
            var hasWeaponHotbar          = Any<MR.Systems.Selection.WeaponHotbar>();
            var hasItemWheel             = Any<MR.Systems.Selection.ItemWheel>();
            var hasManifest              = Any<MR.Systems.AI.ManifestController>();
            var hasRitual                = Any<MR.Systems.Ritual.ReposeRitualController>();
            var hasNavAgent              = Any<NavMeshAgent>();

            // --- Rules ---
            if (isBootstrap)
            {
                if (!hasGame)        result.errors.Add("Bootstrap must contain MR.Core.Game.");
                if (!hasServices)    result.errors.Add("Bootstrap must contain MR.Core.Services (assign ItemDatabase).");
                if (!hasInputService)result.errors.Add("Bootstrap must contain MR.Systems.Input.InputService.");
#if ENABLE_INPUT_SYSTEM
                if (!hasInputAdapter)result.warnings.Add("Bootstrap should contain MR.Systems.Input.InputSystemAdapter (assign actions asset).");
#endif
                if (hasWeaponHotbar) result.warnings.Add("Bootstrap should NOT have WeaponHotbar (belongs in HQ/Case).");
                if (hasItemWheel)    result.warnings.Add("Bootstrap should NOT have ItemWheel (belongs in HQ/Case).");
                if (hasManifest)     result.warnings.Add("Bootstrap should NOT have ManifestController (case-only).");
                if (hasRitual)       result.warnings.Add("Bootstrap should NOT have ReposeRitualController (case-only).");
            }
            else if (isHQ)
            {
                if (!hasWeaponHotbar) result.errors.Add("HQ should contain a Player with WeaponHotbar.");
                if (!hasItemWheel)    result.errors.Add("HQ should contain a Player with ItemWheel.");
                if (hasManifest)      result.warnings.Add("HQ should NOT have ManifestController.");
                if (hasRitual)        result.warnings.Add("HQ should NOT have ReposeRitualController.");
            }
            else // Case
            {
                if (!hasManifest) result.errors.Add("Case scene must contain a ManifestController.");
                if (!hasNavAgent) result.warnings.Add("Case scene should have a NavMeshAgent (Manifest needs it).");
                if (!hasRitual)   result.errors.Add("Case scene must contain ReposeRitualController (finale).");

                if (hasGame)      result.warnings.Add("Case scene should NOT include MR.Core.Game (provided by Bootstrap).");
                if (hasServices)  result.warnings.Add("Case scene should NOT include MR.Core.Services (provided by Bootstrap).");
                if (hasInputService) result.warnings.Add("Case scene should NOT include InputService (provided by Bootstrap).");
#if ENABLE_INPUT_SYSTEM
                if (hasInputAdapter) result.warnings.Add("Case scene should NOT include InputSystemAdapter (provided by Bootstrap).");
#endif
            }
        }
    }
}
#endif