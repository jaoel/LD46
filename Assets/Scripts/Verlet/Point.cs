using UnityEngine;

namespace Verlet {
    public class Point {
        public Point(Vector3 position_) {
            position = position_;
            oldPosition = position_;
            acceleration = Vector3.zero;
            force = Vector3.zero;
        }

        public Vector3 position;
        public Vector3 oldPosition;
        public Vector3 acceleration;
        public Vector3 force;
    }
}
