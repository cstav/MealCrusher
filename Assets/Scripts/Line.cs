using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {
	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int lengthOfLineRenderer = 2;
	void Start() {
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.SetVertexCount(lengthOfLineRenderer);
		lineRenderer.sortingLayerName = "Line";
		lineRenderer.SetPosition(0, new Vector2(0,0));
		lineRenderer.SetPosition(1, new Vector2(0,6));

	}
	void Update() {
		
	}
}