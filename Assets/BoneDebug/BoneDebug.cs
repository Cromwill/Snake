using UnityEngine;
//using System.Collections;

[ExecuteInEditMode]
public class BoneDebug : MonoBehaviour
{
	public Color color = Color.white;
	public float length = 0.01f;
	
	void RenderBonesRecursvely(Transform t)
	{
		foreach(Transform child in t)
		{
			Gizmos.color = color;
			Gizmos.DrawLine(t.position, child.position);
			RenderBonesRecursvely(child.transform);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawLine(t.position, t.position + t.rotation * Vector3.right * length);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(t.position, t.position + t.rotation * Vector3.up * length);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(t.position, t.position + t.rotation * Vector3.forward * length);
	}
	
	void OnDrawGizmos()
	{
		RenderBonesRecursvely(transform);
	}
	
}
