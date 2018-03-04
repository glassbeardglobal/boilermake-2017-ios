/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
==============================================================================*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class RocksTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		//private ArrayList foundModels;
		//private ArrayList foundModelsC;
		private bool found;
		private bool tapped;
		private bool tracked;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			//foundModels = new ArrayList();
			//foundModelsC = new ArrayList();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
//			Dictionary<string, string> param = new Dictionary<string, string>();
//			param.Add("flag[claimed]", "0");
//			param.Add("_method", "patch");
			Server.GET("");
			found = false;
			tracked = false;
		}

		void Update() {
			// Enable rendering:
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && tracked) {
				Debug.Log ("tap");
				tapped = true;
				OnTrackingFound ();
			}
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
				newStatus == TrackableBehaviour.Status.TRACKED ||
				newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e) {
			
		}

		private void OnTrackingFound()
		{
			tracked = true;

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			int res = -1;
			Debug.Log(int.TryParse(Server.GET("api/goals/fulfilled/5883fbd786bb264f736f37d4"), out res));


			if (!tapped) {
				foreach (Renderer component in rendererComponents)
				{
					component.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
					component.enabled = true;
				}
			} else {
				foreach (Renderer component in rendererComponents)
				{
					component.material.color = new Color (1.0f, 0.0f, 0.0f, 0.1f);
					component.enabled = true;
				}
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				if (res == 0) {
					component.enabled = true;
				}
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			tapped = false;
		}


		private void OnTrackingLost()
		{
			tracked = false;
			//WWW url = new WWW("http://scavar.herokuapp.com/flags.json");

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			if (found) {
				Dictionary<string, string> param = new Dictionary<string, string>();
				param.Add("flag[claimed]", "1");
				param.Add("_method", "patch");
				Server.POST("flags/0", param);
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			tapped = false;
		}

		#endregion // PRIVATE_METHODS
	}
}
