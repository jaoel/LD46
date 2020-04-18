using System.Collections.Generic;
using UnityEngine;

namespace Verlet {
    public class VerletPhysicsManager : MonoBehaviour {
        private static VerletPhysicsManager instance = null;
        public static VerletPhysicsManager Instance {
            get {
                if (instance == null) {
                    VerletPhysicsManager manager = FindObjectOfType<VerletPhysicsManager>();
                    if (manager != null) {
                        instance = manager;
                    } else {
                        instance = new GameObject("VerletPhysicsManager", typeof(VerletPhysicsManager)).GetComponent<VerletPhysicsManager>();
                    }
                }
                return instance;
            }
        }

        private int iterations = 10;
        public int Iterations { get { return iterations; } set { iterations = Mathf.Clamp(value, 1, 100); } }

        [System.NonSerialized] public List<Point> points = new List<Point>();
        [System.NonSerialized] public List<EdgeConstraint> edgeConstraints = new List<EdgeConstraint>();
        [System.NonSerialized] public List<AnchorConstraint> anchorConstraints = new List<AnchorConstraint>();

        private int layerMaskWorld;

        public static void AddPoint(Point point) {
            Instance.points.Add(point);
        }

        public static EdgeConstraint AddEdgeConstraint(Point a, Point b, float distance) {
            EdgeConstraint edgeConstraint = new EdgeConstraint(a, b, distance);
            Instance.edgeConstraints.Add(edgeConstraint);
            return edgeConstraint;
        }

        public static EdgeConstraint AddEdgeConstraint(Point a, Point b, float distance, EdgeConstraintType type) {
            EdgeConstraint edgeConstraint = new EdgeConstraint(a, b, distance, type);
            Instance.edgeConstraints.Add(edgeConstraint);
            return edgeConstraint;
        }

        public static AnchorConstraint AddAnchorConstraint(Point point, Vector3 anchoredPosition) {
            AnchorConstraint anchorConstraint = new AnchorConstraint(point, anchoredPosition);
            Instance.anchorConstraints.Add(anchorConstraint);
            return anchorConstraint;
        }

        public static void RemoveAnchorConstraint(AnchorConstraint anchorConstraint) {
            Instance.anchorConstraints.Remove(anchorConstraint);
        }

        private void Start() {
            layerMaskWorld = Layers.GetMask(Layers.World);
        }


        private void FixedUpdate() {
            float dt2 = Time.deltaTime * Time.deltaTime;
            Vector3 gravity = Vector3.down * 9.82f;
            Vector3 temp, edge, direction, vector, start, end, penetrationVector, projectedVector;
            for (int pointIndex = 0; pointIndex < points.Count; pointIndex++) {
                Point point = points[pointIndex];
                //point.acceleration = gravity + point.force;
                point.acceleration = gravity;
                VectorAdd(ref point.acceleration, ref point.force);

                temp = point.position;
                //point.position += point.position - point.oldPosition + point.acceleration * dt2;

                VectorAdd(ref point.position, ref point.position);
                VectorSub(ref point.position, ref point.oldPosition);
                VectorMul(ref point.acceleration, dt2);
                VectorAdd(ref point.position, ref point.acceleration);

                point.oldPosition = temp;
                point.force = Vector3.zero;
            }

            for (int i = 0; i < iterations; i++) {

                // Edge Constraints
                for (int edgeIndex = 0; edgeIndex < edgeConstraints.Count; edgeIndex++) {
                    EdgeConstraint edgeConstraint = edgeConstraints[edgeIndex];
                    //edge = edgeConstraint.b.position - edgeConstraint.a.position;
                    edge = edgeConstraint.b.position;
                    VectorSub(ref edge, ref edgeConstraint.a.position);

                    float distance = edge.magnitude;

                    if (edgeConstraint.type == EdgeConstraintType.ExactDistance ||
                        edgeConstraint.type == EdgeConstraintType.MaximumDistance && distance > edgeConstraint.length ||
                        edgeConstraint.type == EdgeConstraintType.MinimumDistance && distance < edgeConstraint.length) {

                        //direction = edge / distance;
                        direction = edge;
                        VectorDiv(ref direction, distance);

                        float diff = distance - edgeConstraint.length;
                        if (i == 0) {
                            edgeConstraint.tension = diff;
                        } else {
                            edgeConstraint.tension = 0.5f * edgeConstraint.tension + 0.5f * diff;
                        }
                        //edgeConstraint.a.position += direction * diff * 0.5f;
                        //edgeConstraint.b.position -= direction * diff * 0.5f;
                        VectorMul(ref direction, diff * 0.5f);
                        VectorAdd(ref edgeConstraint.a.position, ref direction);
                        VectorSub(ref edgeConstraint.b.position, ref direction);
                    }
                }

                // Collisions
                for (int pointIndex = 0; pointIndex < points.Count; pointIndex++) {
                    Point point = points[pointIndex];
                    float radius = 0.05f;
                    float frictionCoeff = 0.98f;
                    //vector = point.position - point.oldPosition;
                    vector = point.position;
                    VectorSub(ref vector, ref point.oldPosition);

                    float distance = vector.magnitude;

                    //direction = vector / distance;
                    direction = vector;
                    VectorDiv(ref direction, distance);

                    float maxDistance = distance + radius;
                    //start = point.oldPosition - direction * radius;
                    //end = point.position + direction * radius;

                    VectorMul(ref direction, radius);

                    start = point.oldPosition;
                    VectorSub(ref start, ref direction);

                    end = point.position;
                    VectorAdd(ref end, ref direction);

                    if (Physics.SphereCast(start, radius, end - start, out RaycastHit hitInfo, maxDistance, layerMaskWorld)) {
                        penetrationVector = point.position - hitInfo.point;
                        projectedVector = Vector3.ProjectOnPlane(penetrationVector, hitInfo.normal);

                        point.position = hitInfo.point + hitInfo.normal * radius + projectedVector * frictionCoeff;
                        //Debug.DrawLine(start, end, Color.green);
                    } else {
                        //Debug.DrawLine(start, end, Color.blue);
                    }
                }

                // Anchor Constraints
                for (int anchorIndex = 0; anchorIndex < anchorConstraints.Count; anchorIndex++) {
                    AnchorConstraint anchorConstraint = anchorConstraints[anchorIndex];
                    if (anchorConstraint.enabled) {
                        anchorConstraint.point.position = anchorConstraint.anchoredPosition;
                    }
                }
            }
        }

        private void VectorMul(ref Vector3 modify, float f) {
            modify.x = modify.x * f;
            modify.y = modify.y * f;
            modify.z = modify.z * f;
        }

        private void VectorDiv(ref Vector3 modify, float f) {
            modify.x = modify.x / f;
            modify.y = modify.y / f;
            modify.z = modify.z / f;
        }

        private void VectorAdd(ref Vector3 modify, ref Vector3 b) {
            modify.x = modify.x + b.x;
            modify.y = modify.y + b.y;
            modify.z = modify.z + b.z;
        }

        private void VectorSub(ref Vector3 modify, ref Vector3 b) {
            modify.x = modify.x - b.x;
            modify.y = modify.y - b.y;
            modify.z = modify.z - b.z;
        }

        //private void OnDrawGizmos() {
        //    foreach (Point point in points) {
        //        Gizmos.DrawSphere(point.position, 0.1f);
        //    }
        //    foreach (EdgeConstraint edge in edgeConstraints) {
        //        Gizmos.DrawLine(edge.a.position, edge.b.position);
        //    }
        //}
    }
}
