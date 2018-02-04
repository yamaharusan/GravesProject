/*汎用性高くて、あると便利な関数群を格納する。
 * 
 *
 *
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//汎用関数放り込んどけ
public class StdLib{
	/// <summary>
	/// 方向ベクトルvをrだけ回転させる
	/// </summary>
	/// <returns>The d.</returns>
	/// <param name="v">V.</param>
	/// <param name="r">The red component.</param>
	public Vector3 rot3D(Vector3 v,Vector3 r){
		
		float s = Mathf.Sin (r.z),c = Mathf.Cos(r.z);
		v.y = v.y*c - v.x*s; 
		v.x = v.x*c + v.y*s;
		
		s = Mathf.Sin(r.x); c = Mathf.Cos(r.x);
		v.z = v.z*c - v.y*s; 
		v.y = v.y*c + v.z*s;
		
		s = Mathf.Sin(r.y); c = Mathf.Cos(r.y);
		v.z = v.z*c - v.x*s; 
		v.x = v.x*c + v.z*s;
		
		return v;
	}
	
	/// <summary>
	/// Limit the specified val, min and max.
	/// </summary>
	/// <param name="val">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public int limit(int val,int min,int max){
		if (val > max) {
			return max;
		} else if (val < min) {
			return min;
		} else {
			return val;
		}
	}
	public double limit(double val,double min,double max){
		if (val > max) {
			return max;
		} else if (val < min) {
			return min;
		} else {
			return val;
		}
	}
	public float limit(float val,float min,float max){
		return Mathf.Max(min, Mathf.Min(max, val));
	}
	
	/// <summary>
	/// 変数aを目標値bにcに比例した滑らかさで漸近
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	/// <param name="c">C.</param>
	public float move(float a,float b,float c){
		return a - ( (a-b) / c );
	}
	public double move(double a,double b,double c){
		return a - ( (a-b) / c );
	}
	public Vector3 move(Vector2 a,Vector2 b,float c){
		return a - ( (a-b) / c );
	}
	public Vector3 move(Vector3 a,Vector3 b,float c){
		return a - ( (a-b) / c );
	}
	
	public float move(float a,float b,float min,float max){
		return a - limit((a - b), min, max);
	}
	public double move(double a,double b,double min,double max){
		return a - limit((a - b), min, max);
	}
}

