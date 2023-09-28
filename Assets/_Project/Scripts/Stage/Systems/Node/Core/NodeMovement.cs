using DG.Tweening;
using PathCreation;
using System;
using System.Collections;
using UnityEngine;

namespace DreamQuiz
{
    public class NodeMovement : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] protected float speed = 6f;
        [SerializeField] protected Vector2 initialDirection = Vector2.up;

        [Header("Options")]
        [SerializeField] protected float timeToAdjustPositionInSpline = 0.2f;

        public Vector2 Direction { get; protected set; }
        public bool IsMovementLocked { get; protected set; }
        public bool IsMoving { get; protected set; }

        public event Action<NodeBase, NodePath> OnArrival;

        private void OnEnable()
        {
            SetDirection(initialDirection);
        }

        public void LockMovement(bool active)
        {
            IsMovementLocked = active;
        }

        public bool TryMoveToNode(NodeBase fromNode, NodeBase toNode)
        {
            if (IsMovementLocked == true)
            {
                Debug.Log($"Pawn {gameObject.name} movement locked");
                return false;
            }

            if (IsMoving == true)
            {
                Debug.Log($"Pawn {gameObject.name} already moving");
                return false;
            }

            if (fromNode == toNode)
            {
                Debug.Log($"Pawn {gameObject.name} already in node {toNode.gameObject.name}");
                return false;
            }

            if (NodeHelper.HasConnectionToNode(fromNode, toNode, out NodeConnection currentNodeConnection) == false)
            {
                Debug.Log($"Node {fromNode.gameObject.name} doesn't have a connection to node {toNode.gameObject.name}");
                return false;
            }

            MoveToNode(fromNode, toNode, currentNodeConnection);

            return true;
        }

        private void MoveToNode(NodeBase fromNode, NodeBase toNode, NodeConnection connection)
        {
            NodeHelper.GetStartPositionInSpline(
                transform.position,
                connection.Path.PathCreator,
                out Vector3 startPosition,
                out int speedMultiplier,
                out float startDistance,
                out NodeHelper.ConditionToEnd conditionToEnd);

            StartCoroutine(MoveThroughSpline(fromNode, toNode, speedMultiplier, startDistance, conditionToEnd, connection.Path));
        }

        private void SetDirection(Vector2 newDirection)
        {
            Direction = NodeHelper.GetDirectionFromVector(newDirection);
        }

        private IEnumerator MoveThroughSpline(NodeBase fromNode, NodeBase toNode, int speedMultiplier, float distanceTravelled, NodeHelper.ConditionToEnd conditionToEnd, NodePath nodePath)
        {
            IsMoving = true;

            if (fromNode != null)
            {
                transform.DOMove(fromNode.transform.position, timeToAdjustPositionInSpline);
                yield return new WaitForSeconds(timeToAdjustPositionInSpline);
            }

            float lenght = nodePath.PathCreator.path.length;

            Vector3 previousPosition = transform.position;
            Vector3 currentPosition;
            Vector3 direction;

            while (conditionToEnd(distanceTravelled, lenght) == false)
            {
                distanceTravelled += speed * speedMultiplier * Time.deltaTime;

                currentPosition = nodePath.PathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                direction = currentPosition - previousPosition;

                transform.position = currentPosition;
                SetDirection(direction);

                previousPosition = currentPosition;

                yield return null;
            }

            transform.DOMove(toNode.transform.position, timeToAdjustPositionInSpline);
            yield return new WaitForSeconds(timeToAdjustPositionInSpline);

            IsMoving = false;

            OnArrival?.Invoke(toNode, nodePath);
        }
    }
}