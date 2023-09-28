using PathCreation;
using UnityEngine;

namespace DreamQuiz
{
    public static class NodeHelper
    {
        private const float minValueToConsiderDirection = 0.3f;

        public delegate bool ConditionToEnd(float distance, float lenght);

        public static bool HasConnectionToNode(NodeBase from, NodeBase to, out NodeConnection currentNodeConnection)
        {
            foreach (NodeConnection connection in from.Connections)
            {
                if (connection.Node == to)
                {
                    currentNodeConnection = connection;

                    return true;
                }
            }

            currentNodeConnection = default;
            return false;
        }

        public static void GetStartPositionInSpline(Vector3 playerPosition, PathCreator pathCreator, out Vector3 startPosition, out int speedMultiplier, out float startDistance, out ConditionToEnd conditionToEnd)
        {
            Vector3 startOfSpline = pathCreator.path.GetPointAtTime(0, EndOfPathInstruction.Stop);
            Vector3 endOfSpline = pathCreator.path.GetPointAtTime(1, EndOfPathInstruction.Stop);

            if (SquaredDistance(playerPosition, startOfSpline) <= SquaredDistance(playerPosition, endOfSpline))
            {
                startPosition = startOfSpline;
                speedMultiplier = 1;
                startDistance = 0;
                conditionToEnd = ConditionFromStartOfSpline;
            }
            else
            {
                startPosition = endOfSpline;
                speedMultiplier = -1;
                startDistance = pathCreator.path.length;
                conditionToEnd = ConditionFromEndOfSpline;
            }
        }

        public static Vector2 GetDirectionFromVector(Vector2 vector)
        {
            vector.Normalize();

            int vectorX = EvaluateDirectionValue(vector.x, minValueToConsiderDirection);
            int vectorY = EvaluateDirectionValue(vector.y, minValueToConsiderDirection);

            return new Vector2(vectorX, vectorY);
        }

        private static int EvaluateDirectionValue(float value, float minValueToConsiderDirection)
        {
            if (Mathf.Abs(value) >= minValueToConsiderDirection)
            {
                return Mathf.RoundToInt(Mathf.Sign(value));
            }

            return 0;
        }

        private static bool ConditionFromStartOfSpline(float distance, float lenght)
        {
            return distance >= lenght;
        }

        private static bool ConditionFromEndOfSpline(float distance, float lenght)
        {
            return distance <= 0;
        }

        public static float SquaredDistance(Vector3 position1, Vector3 position2)
        {
            return (position1 - position2).sqrMagnitude;
        }

        public static void CreateInverseConnection(NodeBase currentNode, NodeConnection currentConnection)
        {
            NodeBase otherNode = currentConnection.Node;

            if (otherNode == null)
            {
                return;
            }

            for (int i = 0; i < otherNode.Connections.Count; i++)
            {
                NodeConnection connection = otherNode.Connections[i];

                if (connection.Node == currentNode)
                {
                    if (connection.Path != currentConnection.Path)
                    {
                        otherNode.UpdateConnectionAt(new NodeConnection(currentNode, currentConnection.Path), i);
                    }

                    return;
                }
            }

            otherNode.SetConnection(new NodeConnection(currentNode, currentConnection.Path));
        }
    }
}