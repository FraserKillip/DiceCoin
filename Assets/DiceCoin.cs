using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DiceCoin : MonoBehaviour, IPointerClickHandler
{
	public static int up = 0;
	public static int down = 0;
	public static int side = 0;
	public static int count = 0;
	
	public bool moving = true;

	public float size = 1;
	
	// Use this for initialization
	void Start ()
	{
		Reset();

	}
	
	// Update is called once per frame
	void Update () {
		var rb = GetComponent<Rigidbody>();
		if (rb.velocity.magnitude < 0.001 && moving)
		{
			var dotUp = Vector3.Dot(transform.up, Vector3.up);
			
			var mr = GetComponent<MeshRenderer>();
			if (dotUp < -0.9)
			{
				mr.material.color = Color.blue;
				down++;
			}
			else if (dotUp > 0.9)
			{
				mr.material.color = Color.red;
				up++;
			}
			else
			{
				mr.material.color = Color.yellow;
				side++;
			}

			count++;

			moving = false;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log(eventData.button);
	}

	public void Reset()
	{
		var rb = GetComponent<Rigidbody>();
		moving = true;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		var mr = GetComponent<MeshRenderer>();
		mr.material.color = Color.white;

		
		
		transform.rotation = Random.rotationUniform;
		//transform.rotation = Quaternion.Euler(90, 90, 0);
		rb.AddForceAtPosition(Random.insideUnitSphere * Random.Range(8, 20), Random.insideUnitSphere);
		rb.velocity = Random.onUnitSphere * Random.Range(1, 10);
		
		//transform.localScale = new Vector3(1, (float) (0.5f / (2 * Math.Sqrt(2))), 1);
		transform.localScale = new Vector3(1, (float) (0.5f / (size)), 1);
		var range = 10;
		transform.position = transform.parent.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));

	}
}
