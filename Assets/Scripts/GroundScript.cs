using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
	public float width = 20;
	public int pointCount = 20;

	public float centerHeight = 1;
	public float edgeHeight = 5;

	void Start() {
		CreateMesh();
	}

	public void CreateMesh() {
		List<Vector2> points = new List<Vector2>();
		points.Add(new Vector2(width / 2, 0));
		points.Add(new Vector2(-width / 2, 0));

		for (int p = 0; p <= pointCount; p++) {
			float x = p / (float)pointCount - .5f;
			points.Add(new Vector2(
				transform.position.x + width * x,
				centerHeight + (edgeHeight - centerHeight) * x * x));
		}

		GetComponent<PolygonCollider2D>().points = points.ToArray();

		Triangulator tr = new Triangulator(points.ToArray());
		int[] indices = tr.Triangulate();

		Vector3[] vertices = new Vector3[points.Count];
		for (int i = 0; i < points.Count; i++) {
			vertices[i] = new Vector3(points[i].x, points[i].y, 0);
		}

		Mesh mesh = new Mesh();
		mesh.name = "ground mesh";
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
