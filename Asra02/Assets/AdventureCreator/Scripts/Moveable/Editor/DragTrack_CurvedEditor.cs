using UnityEngine;
using UnityEditor;

namespace AC
{

	[CustomEditor(typeof(DragTrack_Curved))]
	public class DragTrack_CurvedEditor : DragTrackEditor
	{
		
		public override void OnInspectorGUI ()
		{
			DragTrack_Curved _target = (DragTrack_Curved) target;

			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField ("Track shape:", EditorStyles.boldLabel);
			
			_target.radius = CustomGUILayout.FloatField ("Radius:", _target.radius, "", "The track's radius");
			_target.handleColour = CustomGUILayout.ColorField ("Handles colour:", _target.handleColour, "", "The colour of Scene window Handles");

			_target.doLoop = CustomGUILayout.Toggle ("Is looped?", _target.doLoop, "", "If True, then the track forms a complete loop");
			if (!_target.doLoop)
			{
				_target.maxAngle = CustomGUILayout.FloatField ("Maximum angle:", _target.maxAngle, "", "The angle of the tracks's curve");

				if (_target.maxAngle > 360f)
				{
					_target.maxAngle = 360f;
				}
			}

			EditorGUILayout.EndVertical ();
			
			SharedGUI (true);
		}
		
		
		public void OnSceneGUI ()
		{
			DragTrack_Curved _target = (DragTrack_Curved) target;

			float _angle = _target.maxAngle;
			if (_target.doLoop)
			{
				_angle = 360f;
			}

			Handles.color = new Color (_target.handleColour.r / 2f, _target.handleColour.g / 2f, _target.handleColour.b / 2f, _target.handleColour.a);
			Vector3 startPosition = _target.transform.position + (_target.radius * _target.transform.right);
			Handles.DrawSolidDisc (startPosition, _target.transform.up, _target.discSize);

			Transform t = _target.transform;
			Vector3 originalPosition = _target.transform.position;
			Quaternion originalRotation = _target.transform.rotation;
			t.position = startPosition;
			t.RotateAround (originalPosition, _target.transform.forward, _angle);

			Handles.color = _target.handleColour;
			Handles.DrawSolidDisc (t.position, t.up, _target.discSize);

			_target.transform.position = originalPosition;
			_target.transform.rotation = originalRotation;

			Handles.color = _target.handleColour;
			Handles.DrawWireArc (_target.transform.position, _target.transform.forward, _target.transform.right, _angle, _target.radius);
		}
		
	}

}