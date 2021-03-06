﻿using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class GameController : MonoBehaviour {

	enum GameState {
		StartSelectPlanet,
		OnPlanetView,
		OnUniverseView,
	};

	GameState state;

	public UniverseGenerator universe;
	public CameraController camController;

	GameObject planetView;
	GameObject startGameView;
	GameObject shoppingView;
	GameObject bottomView;

	Planet currentPlanet;
	PlayerStats playerStats;

	// Use this for initialization
	void Start () {
		state = GameState.StartSelectPlanet;
		this.camController.GoToGlobal ( reachGlobalDelegate );
		playerStats = new PlayerStats ();
		planetView = GameObject.Find ("/InGameViews/PlanetView");
		startGameView = GameObject.Find ("/InGameViews/StartGameView");
		shoppingView = GameObject.Find ("/InGameViews/BuidlingShopView");
		bottomView = GameObject.Find ("/InGameViews/BottomView");

		// setup planet view buttons
		bottomView.transform.Find ("Buttons/BackIcon").GetComponent<Button> ().onClick.AddListener (GoToGlobalEventResponse);
		bottomView.transform.Find ("Buttons/BuildingsIcon").GetComponent<Button> ().onClick.AddListener (GoToBuildingsShop);
		planetView.transform.Find ("TransferIcon").GetComponent<Button> ().onClick.AddListener (TransferResourcesToPlayer);

		planetView.transform.Find ("InvisibleGatherIcon").GetComponent<Button> ().onClick.AddListener (GatherResourcesFromCurrentPlanet);

		// turn off all but start game view
		planetView.SetActive (false);
		shoppingView.SetActive (false);
		bottomView.SetActive (false);
	}

	void reachGlobalDelegate( )
	{
		Debug.Log ("ok im in global");
	}

	public void GoToGlobalEventResponse()
	{
		planetView.SetActive (false);
		camController.GoToGlobal (reachGlobalDelegate);
		state = GameState.OnUniverseView;
	}

	public void GoToBuildingsShop()
	{
		shoppingView.SetActive (true);
		planetView.SetActive (false);
	}

	public void TransferResourcesToPlayer()
	{
		currentPlanet.planetStorage.TransferTo (ref playerStats.resourcesStorage);
	}

	public void GatherResourcesFromCurrentPlanet()
	{
		Debug.Assert (state == GameState.OnPlanetView);
		currentPlanet.GatherManualResources ();
	}

	void ReachPlanetDelegate( Planet planet )
	{
		planetView.SetActive (true);
		planetView.transform.Find("PlanetName").GetComponent<Text>().text = planet.settings.name;
	}

	void ReachFirstPlanetDelegate( Planet planet )
	{
		bottomView.SetActive (true);
		ReachPlanetDelegate (planet);
	}

	void ChooseStartingPlanet( Planet starting_planet )
	{
		state = GameState.OnPlanetView;
		startGameView.SetActive (false);
		this.camController.GoToPlanet (starting_planet, ReachFirstPlanetDelegate);
		currentPlanet = starting_planet;
	}

	void Update () {
		switch (state) {
		case GameState.StartSelectPlanet:
			if (true == Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (true == Physics.Raycast (ray, out hit)) {
					Planet starting_planet = hit.collider.gameObject.GetComponent<Planet> ();
					ChooseStartingPlanet (starting_planet);
				}
			}
			break;
		case GameState.OnPlanetView:
			UpdatePlanetResourceMarkers ();
			UpdatePlayerResourceMarkers ();
			break;
		}
	}

	void UpdatePlanetResourceMarkers()
	{
		Text f_text = planetView.transform.Find ("PlanetResourceMarkers/FroncetiteMarker/QuantityText").GetComponent<Text> ();
		Text s_text = planetView.transform.Find ("PlanetResourceMarkers/SandetiteMarker/QuantityText").GetComponent<Text> ();
		Text x_text = planetView.transform.Find ("PlanetResourceMarkers/XargonMarker/QuantityText").GetComponent<Text> ();
		f_text.text = ((int)Mathf.Round (currentPlanet.planetStorage.GetResourceQuantity (ResourceUtils.ResourceType.Froncetite))).ToString();
		s_text.text = ((int)Mathf.Round (currentPlanet.planetStorage.GetResourceQuantity (ResourceUtils.ResourceType.Sandetite))).ToString();
		x_text.text = ((int)Mathf.Round (currentPlanet.planetStorage.GetResourceQuantity (ResourceUtils.ResourceType.Xargon))).ToString();
	}

	void UpdatePlayerResourceMarkers()
	{
		GameObject player_resource_markers = planetView.transform.Find ("PlayerResourceMarkers").gameObject;
		ResourcesStorage storage = playerStats.resourcesStorage;
		Text f_text = player_resource_markers.transform.Find ("FroncetiteQuantity_Text").GetComponent<Text> ();
		Text s_text = player_resource_markers.transform.Find ("SandetiteQuantity_Text").GetComponent<Text> ();
		Text x_text = player_resource_markers.transform.Find ("XargonQuantity_Text").GetComponent<Text> ();
		f_text.text = ((int)Mathf.Round (storage.GetResourceQuantity (ResourceUtils.ResourceType.Froncetite))).ToString();
		s_text.text = ((int)Mathf.Round (storage.GetResourceQuantity (ResourceUtils.ResourceType.Sandetite))).ToString();
		x_text.text = ((int)Mathf.Round (storage.GetResourceQuantity (ResourceUtils.ResourceType.Xargon))).ToString();

	}

}
