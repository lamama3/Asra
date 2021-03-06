using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AC
{

	[CustomEditor(typeof(FootstepSounds))]
	public class FootstepSoundsEditor : Editor
	{
		
		public override void OnInspectorGUI ()
		{
			FootstepSounds _target = (FootstepSounds) target;

			EditorGUILayout.Space ();
			_target.footstepSounds = ShowClipsGUI (_target.footstepSounds, "Walking sounds");
			EditorGUILayout.Space ();
			_target.runSounds = ShowClipsGUI (_target.runSounds, "Running sounds (optional)");
			EditorGUILayout.Space ();

			EditorGUILayout.BeginVertical ("Button");
			_target.character = (Char) CustomGUILayout.ObjectField <Char> ("Character:", _target.character, true, "", "The Player or NPC that this component is for");
			if (_target.doGroundedCheck)
			{
				_target.doGroundedCheck = CustomGUILayout.ToggleLeft ("Only play when grounded?", _target.doGroundedCheck, "", "If True, sounds will only play when the character is grounded");
			}
			_target.soundToPlayFrom = (Sound) CustomGUILayout.ObjectField <Sound> ("Sound to play from:", _target.soundToPlayFrom, true, "", "The Sound object to play from");

			_target.footstepPlayMethod = (FootstepSounds.FootstepPlayMethod) CustomGUILayout.EnumPopup ("Play sounds:", _target.footstepPlayMethod, "", "How the sounds are played");
			if (_target.footstepPlayMethod == FootstepSounds.FootstepPlayMethod.Automatically)
			{
				_target.walkSeparationTime = CustomGUILayout.FloatField ("Walking separation time (s):", _target.walkSeparationTime, "", "The separation time between sounds when walking");
				_target.runSeparationTime = CustomGUILayout.FloatField ("Running separation time (s):", _target.runSeparationTime, "", "The separation time between sounds when running");
			}
			else if (_target.footstepPlayMethod == FootstepSounds.FootstepPlayMethod.ViaAnimationEvents)
			{
				EditorGUILayout.HelpBox ("A sound will be played whenever this component's PlayFootstep function is run. This component should be placed on the same GameObject as the Animator.", MessageType.Info);
			}
			EditorGUILayout.EndVertical ();

			UnityVersionHandler.CustomSetDirty (_target);
		}


		private AudioClip[] ShowClipsGUI (AudioClip[] clips, string headerLabel)
		{
			EditorGUILayout.BeginVertical ("Button");
			EditorGUILayout.LabelField (headerLabel, EditorStyles.boldLabel);
			List<AudioClip> clipsList = new List<AudioClip>();

			if (clips != null)
			{
				foreach (AudioClip clip in clips)
				{
					clipsList.Add (clip);
				}
			}

			int numParameters = clipsList.Count;
			numParameters = EditorGUILayout.DelayedIntField ("# of footstep sounds:", numParameters);

			if (numParameters < clipsList.Count)
			{
				clipsList.RemoveRange (numParameters, clipsList.Count - numParameters);
			}
			else if (numParameters > clipsList.Count)
			{
				if (numParameters > clipsList.Capacity)
				{
					clipsList.Capacity = numParameters;
				}
				for (int i=clipsList.Count; i<numParameters; i++)
				{
					clipsList.Add (null);
				}
			}

			for (int i=0; i<clipsList.Count; i++)
			{
				clipsList[i] = (AudioClip) CustomGUILayout.ObjectField <AudioClip> ("Sound #" + (i+1).ToString (), clipsList[i], false, "", headerLabel);
			}
			if (clipsList.Count > 1)
			{
				EditorGUILayout.HelpBox ("Sounds will be chosen at random.", MessageType.Info);
			}
			EditorGUILayout.EndVertical ();

			return clipsList.ToArray ();
		}

	}
}