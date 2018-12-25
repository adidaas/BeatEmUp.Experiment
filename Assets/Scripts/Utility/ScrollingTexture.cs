using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingTexture : MonoBehaviour {

	public float speed = 0.5f;

	// Use this for initialization
	void Start () {

	}

	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2(Time.time * speed, 0);

		GetComponent<Image>().material.mainTextureOffset = offset;﻿
	}
}
