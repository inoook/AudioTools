using UnityEngine;
using System.Collections;

/// <summary>
/// AddValue で入力したデータのヒストリーグラフ描画
/// </summary>
public class DrawHistory : MonoBehaviour {
	
	// 波形描画のための変数
	private float[] values;

	[SerializeField]　private int value_num = 800;
	private int value_count;

	[SerializeField]　float viewScaleAmp = 2.5f;

	[SerializeField]
	Color color = Color.white;

	// Use this for initialization
	void Awake () {
		values = new float[value_num];
		value_count = 0;
	}

	public void AddValue(float scale)
	{
		values[value_count] = scale;
		Draw(values);

		value_count++;
		if (value_count >= value_num) value_count = 0;
	}

	void Draw(float[] scale)
	{
		Vector3 offsetPos = this.transform.position;
		float offsetX = - value_num / 2.0f;//center offset
		for (int i = 1; i < value_count; i++)
		{
			MeshLine.DrawLine (new Vector3(i - 1 +offsetX, scale[i-1] * viewScaleAmp, 0) + offsetPos , new Vector3(i +offsetX, scale[i] * viewScaleAmp, 0) + offsetPos, color);
		}
	}
}
