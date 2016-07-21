﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public UniverseGenerator universe;
	public CameraController camController;

	void ReachDelegate( Planet p )
	{
		Debug.Log ("ok im ere");
	}

	void reachGlobalDelegate( )
	{
		Debug.Log ("ok im in global");
	}

	// Use this for initialization
	void Start () {
		/*
		int selected = Random.Range (0, this.universe.planets.Count - 1);
		Planet p = this.universe.planets[selected].GetComponent<Planet>();
		this.camController.GoToPlanet (p, ReachDelegate);
		*/
		this.camController.GoToGlobal ( reachGlobalDelegate );
	}

	public void FirstPlanetChosen( Planet p )
	{
		this.camController.GoToPlanet (p, ReachDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
