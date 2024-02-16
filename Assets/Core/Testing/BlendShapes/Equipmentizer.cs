using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentizer : MonoBehaviour
{
    public SkinnedMeshRenderer TargetMeshRenderer;
    public Transform rootBone;
    public Transform newParent;
    public GameObject toRemove;

    [Button("Equip")]
    public void Equip()
    {
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();

        //Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        //foreach (Transform bone in TargetMeshRenderer.bones)
        //    boneMap[bone.gameObject.name] = bone;


        SkinnedMeshRenderer myRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        //myRenderer.rootBone = rootBone;

        //Transform[] newBones = new Transform[myRenderer.bones.Length];
        //for (int i = 0; i < myRenderer.bones.Length; ++i)
        //{
        //    GameObject bone = myRenderer.bones[i].gameObject;
        //    if (!boneMap.TryGetValue(bone.name, out newBones[i]))
        //    {
        //        Debug.Log("Unable to map bone \"" + bone.name + "\" to target skeleton.");
        //        break;
        //    }
        //}
        myRenderer.bones = TargetMeshRenderer.bones;

        transform.SetParent(newParent);
        toRemove.SetActive(false);
        // Destroy(toRemove);

        st.Stop();
        Debug.Log(string.Format("MyMethod took {0} ms to complete", st.ElapsedMilliseconds));

    }

}