using UnityEngine;
using Verlet;

public class Wire : MonoBehaviour {
    private static float segmentLength = 0.01f;
    public float length = 0.2f;

    public WireInput connectedWireInput = null;
    public WireTrigger connectorTrigger = null;
    public GameObject plug = null;

    private TubeRenderer tubeRenderer = null;
    private Point[] points = new Point[0];
    private AnchorConstraint anchorA = null;
    private AnchorConstraint anchorB = null;
    private EdgeConstraint edge = null;
    private Vector3[] tubePositions = new Vector3[0];
    private float thickness = 0.02f;
    private Collider _collider = null;
    private bool _isGrabbed = false;
    private WireInput closestWireInput = null;

    private static bool pickingUpAnyWire = false;

    private void Awake() {
        tubeRenderer = GetComponent<TubeRenderer>();
        _collider = plug.GetComponentInChildren<Collider>();
    }

    private void Start() {
        int segmentCount = (int)(length / segmentLength) + 1;
        points = new Point[segmentCount + 1];
        for (int i = 0; i < points.Length; i++) {
            Vector3 position = Vector3.Lerp(transform.position, plug.transform.position, i / (float)points.Length) + Random.insideUnitSphere * 0.05f;
            points[i] = new Point(position);
            VerletPhysicsManager.AddPoint(points[i]);

            if (i == 0) {
                anchorA = VerletPhysicsManager.AddAnchorConstraint(points[i], transform.position + transform.forward * thickness);
            } else if (i == points.Length - 1) {
                anchorB = VerletPhysicsManager.AddAnchorConstraint(points[i], plug.transform.position + plug.transform.forward * thickness);
            }

            if (i > 0) {
                EdgeConstraint edgeConstraint = VerletPhysicsManager.AddEdgeConstraint(points[i], points[i - 1], segmentLength);
                if (i == points.Length - 1) {
                    edge = edgeConstraint;
                }
            }

            if (i > 1) {
                VerletPhysicsManager.AddEdgeConstraint(points[i], points[i - 2], segmentLength * 2f - 0.01f, EdgeConstraintType.MinimumDistance);
            }

        }

        tubePositions = new Vector3[points.Length + 2];
        tubeRenderer._useWorldSpace = true;
        UpdateTubePositions();
    }

    private void UpdateTubePositions() {
        for (int i = 0; i < tubePositions.Length; i++) {
            if (i == 0) {
                tubePositions[i] = transform.position;
            } else if (i == tubePositions.Length - 1) {
                tubePositions[i] = plug.transform.position;
            } else {
                int index = i - 1;
                tubePositions[i] = points[index].position;
            }
        }

        tubeRenderer.SetPositions(tubePositions);
    }

    private void Update() {
        if (_isGrabbed) {
            connectorTrigger.TryGetClosestInput(plug.transform.position, out closestWireInput);
        } else {
            closestWireInput = null;
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!pickingUpAnyWire && _collider.Raycast(ray, out RaycastHit hit, 100f)) {
                _isGrabbed = true;
                pickingUpAnyWire = true;
                if (connectedWireInput != null) {
                    connectedWireInput.connectedWire = null;
                    connectedWireInput = null;
                }
            }
        } else if (Input.GetMouseButtonUp(0)) {
            pickingUpAnyWire = false;
            _isGrabbed = false;
            if (closestWireInput != null && closestWireInput.connectedWire == null) {
                connectedWireInput = closestWireInput;
                closestWireInput.connectedWire = this;
            }
        }

        UpdateTubePositions();
    }

    private void FixedUpdate() {
        if (_isGrabbed) {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane p = new Plane(transform.forward, transform.position + transform.forward * 0.1f);
            if (p.Raycast(mouseRay, out float enter)) {
                Vector3 point = mouseRay.origin + mouseRay.direction * enter;
                anchorB.anchoredPosition = point;
                anchorB.enabled = true;
                plug.transform.position = point;

                Vector3 toVector = (transform.position + transform.forward * 0.1f) - plug.transform.position;
                Quaternion to = Quaternion.FromToRotation(Vector3.forward, toVector);

                if (closestWireInput != null && closestWireInput.connectedWire == null) {
                    Vector3 toClosestVector = plug.transform.position - closestWireInput.transform.position;
                    Quaternion toClosest = Quaternion.FromToRotation(Vector3.forward, toClosestVector);
                    float distT = 1f - Mathf.Clamp01(Vector3.Distance(plug.transform.position, closestWireInput.transform.position) / 0.75f - 0.5f);
                    to = Quaternion.Lerp(to, toClosest, distT);
                }

                float angle = Quaternion.Angle(plug.transform.rotation, to);
                plug.transform.rotation = Quaternion.RotateTowards(plug.transform.rotation, to, angle * 0.5f);

                if (Vector3.Distance(transform.position, plug.transform.position) > length * 2.5f) {
                    pickingUpAnyWire = false;
                    _isGrabbed = false;
                }
            }
        } else if (connectedWireInput != null) {
            connectedWireInput.connectedWire = this;
            plug.transform.position = connectedWireInput.transform.position + connectedWireInput.transform.forward * 0.058f;
            plug.transform.rotation = connectedWireInput.transform.rotation;
            anchorB.anchoredPosition = plug.transform.position + connectedWireInput.transform.forward * thickness;
            anchorB.enabled = true;
        } else {
            plug.transform.position = anchorB.point.position;
            plug.transform.rotation = Quaternion.FromToRotation(Vector3.forward, edge.b.position - edge.a.position);
            anchorB.enabled = false;
        }
    }
}
