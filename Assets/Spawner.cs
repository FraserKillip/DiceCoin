using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Result
{
	public int Count { get; set; }
	public int Up { get; set; }
	public int Down { get; set; }
	public int Side { get; set; }
}
public class Spawner : MonoBehaviour
{
	public int InstanceCount;
	public Rigidbody Prefab;

	public UnityEngine.UI.Text SizeLabel;
	public UnityEngine.UI.Text RatioLabel;
	public UnityEngine.UI.Slider Slider;
	public UnityEngine.UI.Button Button;
	public GameObject Canvas;
	public GameObject BarPrefab;

	public static float Size = 1;
	
	public List<Rigidbody> coins = new List<Rigidbody>();
	public List<Result> results = new List<Result>();
	public List<GameObject> bars = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		// prefill results

		var resultTotal = 20;
		for (int i = 0; i < resultTotal; i++)
		{
			results.Add(new Result());
		}
		
		for (var i = 0; i < InstanceCount; i++)
		{
			var range = 10;
			var position = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
			var diceCoin = Instantiate(Prefab, position, transform.rotation);
			diceCoin.transform.parent = transform;
			diceCoin.GetComponent<DiceCoin>().size = Slider.value;
			coins.Add(diceCoin);
		}

		for (int i = 0; i < resultTotal; i++)
		{
			var bar = Instantiate(BarPrefab, Vector3.zero, Quaternion.identity);
			bar.transform.SetParent(Canvas.transform);
			bar.transform.position = new Vector3(10 * i, 0, 0);
			
			
			SetBarForResult(bar, new Result());
			bars.Add(bar);
		}
		
		
		
		Button.onClick.AddListener(() =>
		{
			coins.ForEach(c =>
			{
				var diceCoin = c.GetComponent<DiceCoin>();
				diceCoin.Reset();
				diceCoin.size = Slider.value;
			});

			DiceCoin.count = 0;
			DiceCoin.down = 0;
			DiceCoin.up = 0;
			DiceCoin.side = 0;

			RatioLabel.text = "Simulating...";
			results.Clear();
			for (int i = 0; i < resultTotal; i++)
			{
				results.Add(new Result());
				SetBarForResult(bars[i], results[i]);
			}
		});
	}

	private static void SetBarForResult(GameObject bar, Result result)
	{
		if (result.Count == 0) result.Count += 1;
		
		var heightUp = 100 * (result.Up / (float) result.Count);
		var heightDown = 100 * (result.Down / (float) result.Count);
		var heightSide = 100 * (result.Side / (float) result.Count);
		
		
		var child0 = bar.transform.GetChild(0);
		child0.transform.localPosition = new Vector3(0, 0, 0);
		child0.GetComponent<RectTransform>().sizeDelta = new Vector2(10, heightUp);
		child0.GetComponent<Image>().color = Color.blue;

		var child1 = bar.transform.GetChild(1);
		child1.transform.localPosition = new Vector3(0, heightUp, 0);
		child1.GetComponent<RectTransform>().sizeDelta = new Vector2(10, heightDown);
		child1.GetComponent<Image>().color = Color.red;

		var child2 = bar.transform.GetChild(2);
		child2.transform.localPosition = new Vector3(0, heightUp + heightDown, 0);
		child2.GetComponent<RectTransform>().sizeDelta = new Vector2(10, heightSide);
		child2.GetComponent<Image>().color = Color.yellow;
	}

	// Update is called once per frame
	void Update ()
	{
		SizeLabel.text = string.Format("Size: {0:0.00}", Slider.value);
		Size = Slider.value;

		var movingCoins = coins.Where(c => c.GetComponent<DiceCoin>().moving).ToList().Count;
		
		if (DiceCoin.count > 0)
		{
			RatioLabel.text = string.Format("{0} - Up: {1:0.00}, Down: {2:0.00}, Side: {3:0.00} -- Moving coins: {4}/{5}", DiceCoin.count,
				DiceCoin.up / (float)DiceCoin.count, DiceCoin.down / (float)DiceCoin.count, DiceCoin.side / (float)DiceCoin.count, movingCoins, InstanceCount);
		}

		if (movingCoins == 0)
		{
			results.RemoveAt(0);
			results.Add(new Result
			{
				Count = DiceCoin.count,
				Down = DiceCoin.down,
				Side = DiceCoin.side,
				Up = DiceCoin.up
			});

			for (var i = 0; i < bars.Count; i++)
			{
				SetBarForResult(bars[i], results[i]);
			}
			
			coins.ForEach(c => c.GetComponent<DiceCoin>().Reset());
		}
	}
}
