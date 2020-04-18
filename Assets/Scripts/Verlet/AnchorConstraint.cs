using UnityEngine;

namespace Verlet {
    public class AnchorConstraint {
        public Point point;
        public Vector3 anchoredPosition;
        public bool enabled = true;

        public AnchorConstraint(Point point, Vector3 anchoredPosition) {
            this.point = point;
            this.anchoredPosition = anchoredPosition;
        }
    }
}
