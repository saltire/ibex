using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
	public float width = 20;
	public int pointCount = 20;

	public float centerHeight = 1;
	public float edgeHeight = 5;
	public float horizontalNoiseScale = 1;
	public float verticalNoiseScale = .1f;
	public bool relativeNoise;

	void Start() {
		CreateMesh();
	}

	public void CreateMesh() {
		List<Vector2> points = new List<Vector2>();
		points.Add(new Vector2(width / 2, 0));
		points.Add(new Vector2(-width / 2, 0));

		for (int p = 0; p <= pointCount; p++) {
			float x = p / (float)pointCount - .5f;
			float curveHeight = (edgeHeight - centerHeight) * x * x;
			float noiseHeight = (Mathf.PerlinNoise(x * horizontalNoiseScale, 0) - .5f) *
				verticalNoiseScale;

			points.Add(new Vector2(
				transform.position.x + width * x,
				centerHeight + curveHeight +
					(relativeNoise ? curveHeight * (1 + noiseHeight) : noiseHeight)));
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
