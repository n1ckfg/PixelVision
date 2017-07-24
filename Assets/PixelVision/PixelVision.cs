// based on https://gist.github.com/wtrebella/421e698ff7adb7143bf8

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PixelVision : MonoBehaviour {

	public int PixelSize = 4;
	public FilterMode FilterMode = FilterMode.Point;
	public Camera cam;

	private Material mat;
	private Texture2D tex;

	void Awake() {
		if (cam == null) cam = GetComponent<Camera>();
		mat = new Material(Shader.Find("Hidden/PixelVision"));	
	}

	void Start() {
		int width = Screen.width / PixelSize;
		int height = Screen.height / PixelSize;
		cam.pixelRect = new Rect(0, 0, width, height);

		if (cam.name == "UiCamera") {
			cam.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
		} else {
			cam.pixelRect = new Rect(0, 0, width, height);
		}
	}

	void OnGUI() {
		if (Event.current.type == EventType.Repaint) {
			Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
		}

		if (cam.name == "UiCamera") {
			cam.Render();
		}
	}


	void OnPostRender() { 
		GL.PushMatrix();
		GL.LoadOrtho();
		for (var i = 0; i < mat.passCount; i++) {
			mat.SetPass(i);
			GL.Begin(GL.QUADS);
			GL.Vertex3(0, 0, 0.1f);
			GL.Vertex3(1, 0, 0.1f);
			GL.Vertex3(1, 1, 0.1f);
			GL.Vertex3(0, 1, 0.1f);
			GL.End();
		}
		GL.PopMatrix();

		DestroyImmediate(tex);

		tex = new Texture2D(Mathf.FloorToInt(cam.pixelWidth), Mathf.FloorToInt(cam.pixelHeight));
		tex.filterMode = FilterMode;
		tex.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
		tex.Apply();
	}

}
