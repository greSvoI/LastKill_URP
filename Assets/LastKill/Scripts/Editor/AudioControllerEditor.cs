using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LastKill
{
    [CustomEditor(typeof(AudioController))]
    public class AudioControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (target == null) return;
            AudioController controller = (AudioController)target;

            
         


        }
    }
}
