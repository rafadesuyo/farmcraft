using PathCreation;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DreamQuiz
{
    public class SplinePath : MonoBehaviour
    {
        //Components
        [Header("Components")]
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private PathMeshCreator pathMeshCreator;

        [Space(10)]

        [SerializeField] private bool isSecretPath = false;
        [SerializeField] private TMP_Text energyToTraverse;

        //Variables
        [Header("Variables")]
        [SerializeField] private PathInfoSO pathInfo;

        //Getters
        public PathCreator PathCreator => pathCreator;
        public PathMeshCreator PathMeshCreator => pathMeshCreator;
        public PathInfoSO PathInfo => pathInfo;
        public bool IsSecretPath => isSecretPath;

        public void RevealPath()
        {
            isSecretPath = true;
            UpdatePathVisibility();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateTexture();

            if (gameObject.scene.IsValid())
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    pathMeshCreator.TriggerUpdate();

                    UpdatePathVisibility();
                };
            }
        }
#endif

        private void Awake()
        {
            pathMeshCreator.TriggerUpdate();
            pathMeshCreator.AssignMaterialsRuntime();
        }

        private void UpdatePathVisibility()
        {
            energyToTraverse.enabled = !isSecretPath;
            pathMeshCreator.gameObject.SetActive(!isSecretPath);
        }

        private void UpdateTexture()
        {
            if (pathInfo == null)
            {
                return;
            }

            Material newMaterial = pathInfo.PathMaterial;

            if (pathMeshCreator.PathMaterial != newMaterial)
            {
                pathMeshCreator.SetMaterial(newMaterial, pathInfo.TextureTilingMultiplier);
            }
        }
    }
}