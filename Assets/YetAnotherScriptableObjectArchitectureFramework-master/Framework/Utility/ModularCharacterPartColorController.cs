using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularCharacterPartColorController : MonoBehaviour
{
    [SerializeField]
    private Color m_PrimaryColor;

    [SerializeField]
    private Color m_SecondaryColor;

    [SerializeField]
    private Color m_LeatherPrimaryColor;

    [SerializeField]
    private Color m_MetalPrimaryColor;

    [SerializeField]
    private Color m_LeatherSecondaryColor;

    [SerializeField]
    private Color m_MetalDarkColor;

    [SerializeField]
    private Color m_MetalSecondaryColor;

    [SerializeField]
    private Color m_BodyArtColor;

    public void SaveColors()
    {
        Renderer rend = GetComponent < Renderer >();
        Material mat = rend.sharedMaterial;

        m_PrimaryColor = mat.GetColor("_Color_Primary");

        m_SecondaryColor = mat.GetColor("_Color_Secondary");

        m_LeatherPrimaryColor = mat.GetColor("_Color_Leather_Primary");

        m_MetalPrimaryColor = mat.GetColor("_Color_Metal_Primary");

        m_LeatherSecondaryColor = mat.GetColor("_Color_Leather_Secondary");

        m_MetalDarkColor = mat.GetColor("_Color_Metal_Dark");

        m_MetalSecondaryColor = mat.GetColor("_Color_Metal_Secondary");

        m_BodyArtColor = mat.GetColor("_Color_BodyArt");
    }

    private void OnEnable()
    {
        Renderer rend = GetComponent<Renderer>();
        Material mat = rend.material;

        mat.SetColor("_Color_Primary", m_PrimaryColor);

        mat.SetColor("_Color_Secondary", m_SecondaryColor);

        mat.SetColor("_Color_Leather_Primary", m_LeatherPrimaryColor);

        mat.SetColor("_Color_Metal_Primary", m_MetalPrimaryColor);

        mat.SetColor("_Color_Leather_Secondary", m_LeatherSecondaryColor);

        mat.SetColor("_Color_Metal_Dark", m_MetalDarkColor);

        mat.SetColor("_Color_Metal_Secondary", m_MetalSecondaryColor);

        mat.SetColor("_Color_BodyArt", m_BodyArtColor);
    }
}
