using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Vector2Double
{
	//Static Properties
	public static Vector2Double up => new Vector2Double(0, 1);
	public static Vector2Double down => new Vector2Double(0, -1);
	public static Vector2Double left => new Vector2Double(-1, 0);
	public static Vector2Double right => new Vector2Double(1, 0);
	public static Vector2Double one => new Vector2Double(1, 1);
	public static Vector2Double zero => new Vector2Double(0, 0);
	public static Vector2Double negativeInfinity => new Vector2Double(double.NegativeInfinity, double.NegativeInfinity);
	public static Vector2Double infinity => new Vector2Double(double.PositiveInfinity, double.PositiveInfinity);
	
	//Properties
	public double x;
	public double y;
	public double magnitude => Math.Sqrt(x * x + y * y);
	public double sqrMagnitude => x * x + y * y;
	public Vector2Double normalized => this / magnitude;
	public double this[int index]
	{
		get
		{
			if (index == 0) return x;
			if (index == 1) return y;
			throw new ArgumentOutOfRangeException(nameof(index));
		}
	}

	//Constructor
	public Vector2Double(double x, double y)
	{
		this.x = x;
		this.y = y;
	}
	
	//Methods
	public override bool Equals(object obj)
	{
		return obj is Vector2Double other && Equals(other);
	}

	public bool Equals(Vector2Double other)
	{
		return x.Equals(other.x) && y.Equals(other.y);
	}

	public override int GetHashCode()
	{
		unchecked {
			return (x.GetHashCode() * 397) ^ y.GetHashCode();
		}
	}

	public void Set(double newX, double newY)
	{
		this = new Vector2Double(newX, newY);
	}
	
	public override string ToString()
	{
		return $"({x:0.0}, {y:0.0})";
	}

	public string ToString(int decimals)
	{
		if (decimals <= 0) return ToString();
		
		var format = "0";
		format += ".";
		for (var i = 0; i < decimals; i++) format += "0";

		return "(" + x.ToString(format) + ", " + y.ToString(format) + ")";
	}

	public Vector2 ToVector2()
	{
		return new Vector2((float) x, (float) y);
	}
	
	//Static Methods
	//TODO
	
	//Operators
	public static Vector2Double operator +(Vector2Double a, Vector2Double b)
	{
		return new Vector2Double(a.x + b.x, a.y + b.y);
	}
	
	public static Vector2Double operator -(Vector2Double a, Vector2Double b)
	{
		return new Vector2Double(a.x - b.x, a.y - b.y);
	}

	public static Vector2Double operator *(Vector2Double a, double b)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	public static Vector2Double operator *(double b, Vector2Double a)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	
	public static Vector2Double operator /(Vector2Double a, double b)
	{
		return new Vector2Double(a.x / b, a.y / b);
	}
	
	public static Vector2Double operator *(Vector2Double a, float b)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	public static Vector2Double operator *(float b, Vector2Double a)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	
	public static Vector2Double operator /(Vector2Double a, float b)
	{
		return new Vector2Double(a.x / b, a.y / b);
	}
	
	public static Vector2Double operator *(Vector2Double a, int b)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	public static Vector2Double operator *(int b, Vector2Double a)
	{
		return new Vector2Double(a.x * b, a.y * b);
	}
	
	public static Vector2Double operator /(Vector2Double a, int b)
	{
		return new Vector2Double(a.x / b, a.y / b);
	}

	public static bool operator !=(Vector2Double a, Vector2Double b)
	{
		return (float)Math.Abs(a.x - b.x) > Mathf.Epsilon || (float)Math.Abs(a.y - b.y) > Mathf.Epsilon;
	}
	
	public static bool operator ==(Vector2Double a, Vector2Double b)
	{
		return (float)Math.Abs(a.x - b.x) < Mathf.Epsilon && (float)Math.Abs(a.y - b.y) < Mathf.Epsilon;
	}

	public static implicit operator Vector2(Vector2Double source)
	{
		return new Vector2((float)source.x, (float)source.y);
	}
	
	public static implicit operator Vector2Double(Vector2 source)
	{
		return new Vector2Double(source.x, source.y);
	}

}

[Serializable]
public struct Vector3Double
{
	//Static Properties
	public static Vector3Double up => new Vector3Double (0, 1, 0);
	public static Vector3Double down => new Vector3Double (0, -1, 0);
	public static Vector3Double left => new Vector3Double (-1, 0, 0);
	public static Vector3Double right => new Vector3Double (1, 0, 0);
	public static Vector3Double forward => new Vector3Double (0, 0, 1);
	public static Vector3Double back => new Vector3Double (0, 0, -1);
	public static Vector3Double one => new Vector3Double (1, 1, 0);
	public static Vector3Double zero => new Vector3Double (0, 0, 0);
	public static Vector3Double negativeInfinity => new Vector3Double (double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);
	public static Vector3Double infinity => new Vector3Double (double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
	
	//Properties
	public double x;
	public double y;
	public double z;
	public double magnitude => Math.Sqrt(x * x + y * y + z * z);
	public double sqrMagnitude => x * x + y * y + z * z;
	public Vector3Double normalized => this / magnitude;
	public double this[int index]
	{
		get
		{
			if (index == 0) return x;
			if (index == 1) return y;
			if (index == 2) return z;
			throw new ArgumentOutOfRangeException(nameof(index));
		}
	}

	//Constructor
	public Vector3Double(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	
	//Methods
	public override bool Equals(object obj)
	{
		return obj is Vector3Double other && Equals(other);
	}

	public bool Equals(Vector3Double other)
	{
		return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
	}

	public override int GetHashCode()
	{
		unchecked {
			var hashCode = x.GetHashCode();
			hashCode = (hashCode * 397) ^ y.GetHashCode();
			hashCode = (hashCode * 397) ^ z.GetHashCode();
			return hashCode;
		}
	}

	public void Set(double newX, double newY, double newZ)
	{
		this = new Vector3Double(newX, newY, newZ);
	}

	public override string ToString()
	{
		return $"({x:0.0}, {y:0.0}, {z:0.0})";
	}

	public string ToString(int decimals)
	{
		if (decimals <= 0) return ToString();

		var format = "0";
		format += ".";
		for (var i = 0; i < decimals; i++) format += "0";

		return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
	}
	
	public Vector3 ToVector3()
	{
		return new Vector3((float) x, (float) y, (float) z);
	}

	//Static Methods
	//TODO
	
	//Operators
	public static Vector3Double operator +(Vector3Double a, Vector3Double b)
	{
		return new Vector3Double(a.x + b.x, a.y + b.y, a.z + b.z);
	}
	
	public static Vector3Double operator -(Vector3Double a, Vector3Double b)
	{
		return new Vector3Double(a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3Double operator *(Vector3Double a, double b)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	public static Vector3Double operator *(double b, Vector3Double a)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	
	public static Vector3Double operator /(Vector3Double a, double b)
	{
		return new Vector3Double(a.x / b, a.y / b, a.z / b);
	}
	
	public static Vector3Double operator *(Vector3Double a, float b)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	public static Vector3Double operator *(float b, Vector3Double a)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	
	public static Vector3Double operator /(Vector3Double a, float b)
	{
		return new Vector3Double(a.x / b, a.y / b, a.z / b);
	}
	
	public static Vector3Double operator *(Vector3Double a, int b)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	public static Vector3Double operator *(int b, Vector3Double a)
	{
		return new Vector3Double(a.x * b, a.y * b, a.z * b);
	}
	
	public static Vector3Double operator /(Vector3Double a, int b)
	{
		return new Vector3Double(a.x / b, a.y / b, a.z / b);
	}

	public static bool operator !=(Vector3Double a, Vector3Double b)
	{
		return (float) Math.Abs(a.x - b.x) > Mathf.Epsilon || 
		       (float) Math.Abs(a.y - b.y) > Mathf.Epsilon ||
		       (float) Math.Abs(a.z - b.z) > Mathf.Epsilon;
	}
	
	public static bool operator ==(Vector3Double a, Vector3Double b)
	{
		return (float) Math.Abs(a.x - b.x) < Mathf.Epsilon && 
		       (float) Math.Abs(a.y - b.y) < Mathf.Epsilon &&
		       (float) Math.Abs(a.z - b.z) < Mathf.Epsilon;
	}
	
	public static implicit operator Vector3(Vector3Double source)
	{
		return new Vector3((float)source.x, (float)source.y, (float)source.z);
	}
	
	public static implicit operator Vector3Double(Vector3 source)
	{
		return new Vector3Double(source.x, source.y, source.z);
	}

	public static implicit operator Vector2Double(Vector3Double source)
	{
		return new Vector2Double(source.x, source.y);
	}

	public static implicit operator Vector3Double(Vector2Double source)
	{
		return new Vector3Double(source.x, source.y, 0d);
	}

}