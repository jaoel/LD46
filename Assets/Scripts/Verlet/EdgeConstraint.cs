using UnityEngine;

namespace Verlet {
    public enum EdgeConstraintType {
        ExactDistance,
        MinimumDistance,
        MaximumDistance,
    }

    public class EdgeConstraint {
        public Point a;
        public Point b;
        public float length;
        public EdgeConstraintType type = EdgeConstraintType.ExactDistance;
        public float tension = 0f;

        public EdgeConstraint(Point a, Point b, float length, EdgeConstraintType type) {
            this.a = a;
            this.b = b;
            this.length = length;
            this.type = type;
        }

        public EdgeConstraint(Point a, Point b, float length)
            : this(a, b, length, EdgeConstraintType.ExactDistance) { }

        public EdgeConstraint(Point a, Point b)
            : this(a, b, Vector3.Distance(a.position, b.position)) {}
    }
}
