using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SCC_Drivetrain))]
public class SCC_DrivetrainEditor : Editor {

	SCC_Drivetrain driveTrainScript;
	SCC_Drivetrain.SCC_Wheels[] wheels;
	Transform[] wheelModels;
	Color defaultGUIColor;

	[MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add Main Controller To Vehicle", false)]
	static void CreateBehavior(){

		if(!Selection.activeGameObject.GetComponentInParent<SCC_Drivetrain>()){

			GameObject pivot = new GameObject (Selection.activeGameObject.name);
			pivot.transform.position = RCC_GetBounds.GetBoundsCenter (Selection.activeGameObject.transform);
			pivot.transform.rotation = Selection.activeGameObject.transform.rotation;
			pivot.transform.localScale = Vector3.one;

			pivot.AddComponent<SCC_Drivetrain>();
			pivot.AddComponent<SCC_Inputs>();
			pivot.AddComponent<SCC_Audio>();
			pivot.AddComponent<SCC_Particles>();
			pivot.AddComponent<SCC_AntiRoll>();
			pivot.AddComponent<SCC_RigidStabilizer>();

			pivot.GetComponent<Rigidbody>().mass = 1350f;
			pivot.GetComponent<Rigidbody>().drag = .05f;
			pivot.GetComponent<Rigidbody>().angularDrag = .5f;
			pivot.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
			pivot.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.Continuous;

			Selection.activeGameObject.transform.SetParent (pivot.transform);
			Selection.activeGameObject = pivot;

			GameObject COM = new GameObject ("COM");
			COM.transform.SetParent (pivot.transform, false);
			pivot.GetComponent<SCC_Drivetrain> ().COM = COM.transform;

			EditorUtility.DisplayDialog("SCC Initialized", "SCC Initialized. Select all of your wheels and create theirs wheel colliders.", "Ok");

		}else{

			EditorUtility.DisplayDialog("Your Gameobject Already Has Simple Car Controller", "Your Gameobject Already Has Simple Car Controller", "Ok");

		}

	}

	[MenuItem("Tools/BoneCracker Games/Simple Car Controller/Add Main Controller To Vehicle", true)]
	static bool CheckCreateBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	public override void OnInspectorGUI () {

		serializedObject.Update ();
		driveTrainScript = (SCC_Drivetrain)target;
		defaultGUIColor = GUI.color;

		//EditorGUILayout.PropertyField (serializedObject.FindProperty ("wheelModels"), true);

		EditorGUILayout.Space ();

		wheels = driveTrainScript.wheels;

		float defaultLabelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 100f;

		if (wheels != null) {

			for (int i = 0; i < wheels.Length; i++) {

				EditorGUILayout.BeginVertical (GUI.skin.box);

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Wheels " + i.ToString (), EditorStyles.boldLabel);
				GUILayout.FlexibleSpace ();

				GUI.color = Color.red;

				if (GUILayout.Button ("X"))
					RemoveWheel (i);
	
				GUI.color = defaultGUIColor;

				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.Space ();
			 
				EditorGUILayout.BeginHorizontal ();

				wheels [i].wheelTransform = (Transform)EditorGUILayout.ObjectField ("Wheel Model", wheels [i].wheelTransform, typeof(Transform), true);
				wheels [i].wheelCollider = (SCC_Wheel)EditorGUILayout.ObjectField ("Wheel Collider", wheels [i].wheelCollider, typeof(SCC_Wheel), true);

				if (wheels [i].wheelCollider == null) {
					GUI.color = Color.cyan;
					if (GUILayout.Button ("Create WheelCollider")) {
						WheelCollider newWheelCollider = SCC_CreateWheelCollider.CreateWheelCollider (driveTrainScript.gameObject, wheels [i].wheelTransform);
						wheels [i].wheelCollider = newWheelCollider.gameObject.GetComponent<SCC_Wheel> ();
						wheels [i].wheelCollider.wheelModel = wheels [i].wheelTransform;
						Debug.Log ("Created wheelcollider for " + wheels [i].wheelTransform.name);
					}
					GUI.color = defaultGUIColor;
				}


				EditorGUILayout.EndHorizontal ();

				EditorGUIUtility.labelWidth = 20f;

				EditorGUILayout.Space ();

				EditorGUILayout.BeginHorizontal (GUI.skin.button);
				wheels [i].isSteering = EditorGUILayout.ToggleLeft ("Steering", wheels [i].isSteering);
				wheels [i].isTraction = EditorGUILayout.ToggleLeft ("Traction", wheels [i].isTraction);
				wheels [i].isBrake = EditorGUILayout.ToggleLeft ("Brake", wheels [i].isBrake);
				wheels [i].isHandbrake = EditorGUILayout.ToggleLeft ("Handbrake", wheels [i].isHandbrake);
				EditorGUILayout.Space ();
				EditorGUILayout.EndHorizontal ();
				if (wheels [i].isSteering)
					wheels [i].steeringAngle = EditorGUILayout.Slider ("Steer Angle", wheels [i].steeringAngle, -45f, 45f);
				EditorGUILayout.Space ();

				EditorGUILayout.EndVertical ();

				EditorGUIUtility.labelWidth = 100f;

			}

		}

		GUI.color = Color.green;
		EditorGUILayout.Space ();

		if (GUILayout.Button ("Create New Wheel Slot"))
			AddNewWheel ();

		GUI.color = defaultGUIColor;
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField ("Configurations", EditorStyles.boldLabel);
		EditorGUILayout.Space ();

		EditorGUIUtility.labelWidth = defaultLabelWidth;

		EditorGUILayout.PropertyField (serializedObject.FindProperty ("COM"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("engineTorque"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("brakeTorque"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("maximumSpeed"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("finalDriveRatio"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("highSpeedSteerAngle"));
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("direction"));

		if (GUI.changed)
			EditorUtility.SetDirty (driveTrainScript);

		serializedObject.ApplyModifiedProperties ();

	}

	void AddNewWheel(){

		List<SCC_Drivetrain.SCC_Wheels> currentWheels = new List<SCC_Drivetrain.SCC_Wheels>();

		if(wheels != null)
			currentWheels.AddRange(wheels);

		currentWheels.Add (null);

		driveTrainScript.wheels = currentWheels.ToArray ();

	}

	void RemoveWheel(int index){

		List<SCC_Drivetrain.SCC_Wheels> currentWheels = new List<SCC_Drivetrain.SCC_Wheels>();

		if(wheels != null)
			currentWheels.AddRange(wheels);

		if(currentWheels [index].wheelCollider != null)
			DestroyImmediate (currentWheels [index].wheelCollider.gameObject);

		currentWheels.RemoveAt(index);

		driveTrainScript.wheels = currentWheels.ToArray ();

	}

}
