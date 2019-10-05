using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Serializable]
public class ShaderProperty
{
    public enum PropertyType
    {
        Float,
        Int,
        Vector,
        Bool,
        Matrix,
        Texture,
        Color,
        TextureScale,
        TextureOffset
    }

    public PropertyType Type;
    public string Name;
    public int KernelIndex;
    public int IntValue;
    public float FloatValue;
    public Vector4 VectorValue;
    public bool BoolValue;
    public Matrix4x4 MatrixValue;
    public Texture TextureValue;
    public Color ColorValue;
    public Vector2 TextureScaleValue;
    public Vector2 TextureOffsetValue;

    public ShaderProperty()
    {
        Type = PropertyType.Float;
        Name = "_PropertyNameHere";
        KernelIndex = 0;
        IntValue = 0;
        FloatValue = 0;
        VectorValue = Vector4.zero;
        BoolValue = false;
        MatrixValue = Matrix4x4.identity;
        TextureValue = null;
        ColorValue = Color.white;
        TextureScaleValue = Vector2.one;
        TextureOffsetValue = Vector2.zero;
    }

}

public static class ShaderPropertyExtensions
{
    public static void SetProperty(this ComputeShader shader, ShaderProperty property)
    {
        switch (property.Type) {
            case ShaderProperty.PropertyType.Float:
                shader.SetFloat(property.Name, property.FloatValue);
                break;
            case ShaderProperty.PropertyType.Int:
                shader.SetInt(property.Name, property.IntValue);
                break;
            case ShaderProperty.PropertyType.Vector:
                shader.SetVector(property.Name, property.VectorValue);
                break;
            case ShaderProperty.PropertyType.Bool:
                shader.SetBool(property.Name, property.BoolValue);
                break;
            case ShaderProperty.PropertyType.Matrix:
                shader.SetMatrix(property.Name, property.MatrixValue);
                break;
            case ShaderProperty.PropertyType.Texture:
                shader.SetTexture(property.KernelIndex, property.Name, property.TextureValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void SetProperty(this Material material, ShaderProperty property)
    {
        switch (property.Type) {
            case ShaderProperty.PropertyType.Float:
                material.SetFloat(property.Name, property.FloatValue);
                break;
            case ShaderProperty.PropertyType.Int:
                material.SetInt(property.Name, property.IntValue);
                break;
            case ShaderProperty.PropertyType.Vector:
                material.SetVector(property.Name, property.VectorValue);
                break;
            case ShaderProperty.PropertyType.Color:
                material.SetColor(property.Name, property.ColorValue);
                break;
            case ShaderProperty.PropertyType.Matrix:
                material.SetMatrix(property.Name, property.MatrixValue);
                break;
            case ShaderProperty.PropertyType.Texture:
                material.SetTexture(property.Name, property.TextureValue);
                break;
            case ShaderProperty.PropertyType.TextureScale:
                material.SetTextureScale(property.Name, property.TextureOffsetValue);
                break;
            case ShaderProperty.PropertyType.TextureOffset:
                material.SetTextureOffset(property.Name, property.TextureScaleValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
