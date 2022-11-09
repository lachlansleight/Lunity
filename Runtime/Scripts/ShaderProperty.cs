using System;
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
    
    private int _propertyId;
    private bool _hasPropertyId;

    public int Id
    {
        get
        {
            if (_hasPropertyId) return _propertyId;
            
            _propertyId = Shader.PropertyToID(Name);
            _hasPropertyId = true;
            return _propertyId;
        }
    }

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
                shader.SetFloat(property.Id, property.FloatValue);
                break;
            case ShaderProperty.PropertyType.Int:
                shader.SetInt(property.Id, property.IntValue);
                break;
            case ShaderProperty.PropertyType.Vector:
                shader.SetVector(property.Id, property.VectorValue);
                break;
            case ShaderProperty.PropertyType.Bool:
                shader.SetBool(property.Id, property.BoolValue);
                break;
            case ShaderProperty.PropertyType.Matrix:
                shader.SetMatrix(property.Id, property.MatrixValue);
                break;
            case ShaderProperty.PropertyType.Texture:
                shader.SetTexture(property.KernelIndex, property.Id, property.TextureValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void SetProperty(this Material material, ShaderProperty property)
    {
        switch (property.Type) {
            case ShaderProperty.PropertyType.Float:
                material.SetFloat(property.Id, property.FloatValue);
                break;
            case ShaderProperty.PropertyType.Int:
                material.SetInt(property.Id, property.IntValue);
                break;
            case ShaderProperty.PropertyType.Vector:
                material.SetVector(property.Id, property.VectorValue);
                break;
            case ShaderProperty.PropertyType.Color:
                material.SetColor(property.Id, property.ColorValue);
                break;
            case ShaderProperty.PropertyType.Matrix:
                material.SetMatrix(property.Id, property.MatrixValue);
                break;
            case ShaderProperty.PropertyType.Texture:
                material.SetTexture(property.Id, property.TextureValue);
                break;
            case ShaderProperty.PropertyType.TextureScale:
                material.SetTextureScale(property.Id, property.TextureOffsetValue);
                break;
            case ShaderProperty.PropertyType.TextureOffset:
                material.SetTextureOffset(property.Id, property.TextureScaleValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
