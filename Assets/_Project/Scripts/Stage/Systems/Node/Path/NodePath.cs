using PathCreation;
using PathCreation.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    [SelectionBase]
    [RequireComponent(typeof(PathCreator))]
    public class NodePath : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] private int sleepingTimeToTraverse = 4;
        [SerializeField] private bool isSecretPath = false;

        [Header("Components")]
        [SerializeField] private PathCreator pathCreator;

        public event Action<int> OnSleepingTimeToTraverseChanged;

        public int SleepingTimeToTraverse
        {
            get
            {
                return sleepingTimeToTraverse;
            }
        }

        public bool IsSecretPath
        {
            get
            {
                return IsSecretPath;
            }
        }

        public PathCreator PathCreator
        {
            get
            {
                return pathCreator;
            }
        }

        public event Action<bool> OnSecretPathRevealed;

        private void OnValidate()
        {
            name = GetNodeName();
        }

        protected virtual string GetNodeName()
        {
            return $"NodePath_{transform.GetSiblingIndex()}";
        }

        public void RevealPath()
        {
            isSecretPath = true;

            OnSecretPathRevealed?.Invoke(isSecretPath);
        }

        public void SetSleepingTimeToTraverse(int value)
        {
            sleepingTimeToTraverse = value;

            OnSleepingTimeToTraverseChanged?.Invoke(value);
        }

        public void CenterTransform()
        {
            var bezierPath = pathCreator.bezierPath;

            Vector3 worldCentre = bezierPath.CalculateBoundsWithTransform(pathCreator.transform).center;
            Vector3 transformPos = pathCreator.transform.position;
            if (bezierPath.Space == PathSpace.xy)
            {
                transformPos = new Vector3(transformPos.x, transformPos.y, 0);
            }
            else if (bezierPath.Space == PathSpace.xz)
            {
                transformPos = new Vector3(transformPos.x, 0, transformPos.z);
            }
            Vector3 worldCentreToTransform = transformPos - worldCentre;

            if (worldCentre != pathCreator.transform.position)
            {
                if (worldCentreToTransform != Vector3.zero)
                {
                    Vector3 localCentreToTransform = MathUtility.InverseTransformVector(worldCentreToTransform, pathCreator.transform, bezierPath.Space);
                    for (int i = 0; i < bezierPath.NumPoints; i++)
                    {
                        bezierPath.SetPoint(i, bezierPath.GetPoint(i) + localCentreToTransform, true);
                    }
                }

                pathCreator.transform.position = worldCentre;
                bezierPath.NotifyPathModified();
            }
        }
    }
}