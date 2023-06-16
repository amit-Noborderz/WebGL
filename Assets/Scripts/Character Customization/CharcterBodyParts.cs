using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;

public class CharcterBodyParts : MonoBehaviour
{
    public static CharcterBodyParts instance;
    [Tooltip("Default Texture for pent and shirt")]
    public Texture Shirt_Texture, Pent_Texture, Shoe_Texture, Eye_Texture;
    // For Eye Color Slider Require Some new Textures
    public Texture Eye_Mask_Texture, Eye_Color_Texture;
    private string shirt_TextureName, Pent_TextureName, Shoes_TextureName, 
        Skin_ColorName, Hair_ColorName, Lip_ColorName, Eyebrow_ColorName, Eye_ColorName, // Using For Color Modification through Slider
        eyeLen_TextureName, eyeLashes_TextureName, Makeup_TextureName, GredientColorName, SssIntensity,EyeBrrow_TextureName;
    [SerializeField]
    public Color DefaultSkinColor, DefaultEyebrowColor, DefaultHairColor, DefaultLipColor, DefaultGredientColor;
    public SkinnedMeshRenderer Body;
    [Tooltip("Region for Bones")]
    public GameObject[] BothEyes, EyeIner, EyesOut, BothLips;
    public GameObject Body_Bone, JBone, Head, Nose, Lips, PelvisBone, ForeHead, headAll;

    [Tooltip("Values For Bones Movement as slider take float")]
    [HideInInspector]
    public float scaleEyeX, EyeScaleY, MouthX, MouthY, NoseX, NoseY, ForeHeadX, ForeHeadY, ForeheadZ, JBoneX, JBoneY, JBoneZ, Lipscalex;

    [Header("skinToon Color List")]
    public List<Color> skinColor;
    public List<Color> lipColor;
    public List<Color> skinGredientColor;
    public List<GameObject> _scaleBodyParts;


    //[Header("Testing")]
    //public TextMeshProUGUI boneName;
    //public TextMeshProUGUI xvalue, yvalue;
    //[HideInInspector]
    public List<BoneDataContainer> BonesData = new List<BoneDataContainer>();

    public Texture2D defaultMakeup, defaultEyelashes, defaultEyebrow;
    public Sprite defaultPngForSkinIcon;
    public static Action<Color> OnSkinColorApply;
    public float defaultSssValue;
    public Color PresetGredientColor;
    //public List<BoneDataContainer> DefaultBones = new List<BoneDataContainer>();

    AvatarController avatarController;

    private void Awake()
    {
        instance = this;
        shirt_TextureName = "_Shirt_Mask";
        Pent_TextureName = "_Pant_Mask";
        Shoes_TextureName = "_Shoes_Mask";
        Skin_ColorName = "_BaseColor";
        Hair_ColorName = "_Color";
        Lip_ColorName = "_Lips_Color";
        Eyebrow_ColorName = "_BaseColor";
        Eye_ColorName = "_Mask_Color";
        eyeLen_TextureName = "_Main_Trexture";
        eyeLashes_TextureName = "_BaseMap";
        Makeup_TextureName = "_Base_Texture";
        GredientColorName = "_Color";
        SssIntensity = "_SSS_Intensity";
        EyeBrrow_TextureName = "_BaseMap";
        defaultSssValue = 2.5f;
        IntCharacterBones();
    }

    private void Start()
    {
        blend = BlendShapeImporter.Instance;
        avatarController = GetComponent<AvatarController>();
    }


    //Set Texture For Shirt
    public void TextureForShirt(Texture texture)
    {
        Body.materials[0].SetTexture(shirt_TextureName, texture);
    }


    // Set texture For 
    public void TextureForPant(Texture texture)
    {
        Body.materials[0].SetTexture(Pent_TextureName, texture);
    }

    public void TextureForShoes(Texture texture)
    {
        Body.materials[0].SetTexture(Shoes_TextureName, texture);
    }

    // Set Default 
    public void DefaultTexture(bool ApplyClothMask= true)
    {
        if (ApplyClothMask)
        {
            Body.materials[0].SetTexture(Pent_TextureName, Pent_Texture);
            Body.materials[0].SetTexture(shirt_TextureName, Shirt_Texture);
            Body.materials[0].SetTexture(Shoes_TextureName, Shoe_Texture);
        }
        Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
        Body.materials[0].SetColor(GredientColorName, DefaultGredientColor);
        Body.materials[0].SetFloat(SssIntensity, defaultSssValue);

        SkinnedMeshRenderer HeadMeshComponent = Head.GetComponent<SkinnedMeshRenderer>();

        //HeadMeshComponent.material.SetColor(Skin_ColorName, DefaultSkinColor);
        //HeadMeshComponent.material.SetColor(Lip_ColorName, DefaultLipColor);
        //HeadMeshComponent.material.SetColor(Eyebrow_ColorName, DefaultEyebrowColor);

        HeadMeshComponent.materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
        HeadMeshComponent.materials[2].SetColor(Lip_ColorName, DefaultLipColor);

        HeadMeshComponent.materials[0].SetTexture(eyeLen_TextureName, Eye_Texture);
        // After EyeShader update need to pass this texture to another property
        HeadMeshComponent.materials[0].SetTexture("_Mask_texture", Eye_Texture);

        HeadMeshComponent.materials[1].SetTexture(EyeBrrow_TextureName, defaultEyebrow);
        HeadMeshComponent.materials[1].SetColor(Eyebrow_ColorName, DefaultEyebrowColor);

        HeadMeshComponent.materials[3].SetTexture(eyeLashes_TextureName, defaultEyelashes);
        HeadMeshComponent.materials[2].SetTexture(Makeup_TextureName, defaultMakeup);
        HeadMeshComponent.materials[2].SetColor(GredientColorName, DefaultGredientColor);
        HeadMeshComponent.materials[2].SetFloat(SssIntensity, defaultSssValue);
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0f);
    }



    BlendShapeImporter blend;
    public void ValuesForSliderXY(List<int> morphsList)
    {
        float min = 0;
        float max = 100;
        float currentValue = 0;
        int blendIndex = 0;

        #region Comment Section 
        //if (BlendShapeController.instance.allBlendShapes[morphsList[0]].boneAvailable)
        //{
        //    print("Yes Bone");
        //    foreach (var item in BlendShapeController.instance.allBlendShapes[morphsList[0]].boneData)
        //    {
        //        print("---- AxesDetails : " + item.workingAxes);


        //        if (item.workingAxes == AxesDetails.z)
        //        {
        //            if (BlendShapeController.instance.allBlendShapes[morphsList[0]].AxisType == AxisType.X_Axis)
        //            {
        //                // X-Slider
        //                if (item.workingAxes == AxesDetails.x)
        //                {
        //                    // Set Min Max Values
        //                    blend.SliderX.maxValue = item.maxValue;
        //                    blend.SliderX.minValue = item.minValue;

        //                    // Setting Slider Value
        //                    Transform bone = BlendShapeController.instance.allBlendShapes[morphsList[0]].boneObj.transform;
        //                    blend.SliderX.value = GetBoneCurrentValue('x', item.workingTransform, bone);
        //                }
        //            }
        //        }

        //        // X-Slider
        //        else if (item.workingAxes == AxesDetails.x)
        //        {
        //            // Set Min Max Values
        //            blend.SliderX.maxValue = item.maxValue;
        //            blend.SliderX.minValue = item.minValue;

        //            // Setting Slider Value
        //            Transform bone = BlendShapeController.instance.allBlendShapes[morphsList[0]].boneObj.transform;
        //            blend.SliderX.value = GetBoneCurrentValue('x', item.workingTransform, bone);

        //            blend.SliderX.onValueChanged.AddListener(delegate { BoneSliderCallBack('x', item.workingTransform, bone, blend.SliderX.value); });
        //            blend.SliderX.gameObject.SetActive(true);
        //        }

        //        //Y-Slider
        //        else if (item.workingAxes == AxesDetails.y)
        //        {
        //            // Set Min Max Values
        //            blend.SliderY.maxValue = item.maxValue;
        //            blend.SliderY.minValue = item.minValue;

        //            // Setting Slider Value
        //            Transform bone = BlendShapeController.instance.allBlendShapes[morphsList[0]].boneObj.transform;
        //            blend.SliderY.value = GetBoneCurrentValue('x', item.workingTransform, bone);

        //            blend.SliderY.onValueChanged.AddListener(delegate { BoneSliderCallBack('y', item.workingTransform, bone, blend.SliderY.value); });
        //            blend.SliderY.gameObject.SetActive(true);
        //        }

        //        // XY Both Axis with single slider
        //        else if (item.workingAxes == AxesDetails.xy)
        //        {
        //            // Get Bone Reference
        //            Transform bone = BlendShapeController.instance.allBlendShapes[morphsList[0]].boneObj.transform;

        //            print("Sub Axis :" + BlendShapeController.instance.allBlendShapes[morphsList[0]].AxisType);
        //            // Check which slider require 
        //            // X Slider or Y Slider
        //            if (BlendShapeController.instance.allBlendShapes[morphsList[0]].AxisType == AxisType.X_Axis)
        //            {
        //                // Set Min Max Values
        //                blend.SliderX.maxValue = item.maxValue;
        //                blend.SliderX.minValue = item.minValue;

        //                // all axis has same value so use anyone of them
        //                blend.SliderX.value = GetBoneCurrentValue('x', item.workingTransform, bone);
        //                blend.SliderX.onValueChanged.AddListener(delegate
        //                {
        //                    // Need to modify values for all axis
        //                    BoneSliderCallBack('a', item.workingTransform, bone, blend.SliderX.value);
        //                });
        //                blend.SliderX.gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                // Set Min Max Values
        //                blend.SliderY.maxValue = item.maxValue;
        //                blend.SliderY.minValue = item.minValue;

        //                // all axis has same value so use anyone of them
        //                blend.SliderY.value = GetBoneCurrentValue('x', item.workingTransform, bone);
        //                print("Assigning Listners : " + BlendShapeController.instance.allBlendShapes[morphsList[0]].itemName);
        //                blend.SliderY.onValueChanged.AddListener(delegate
        //                {
        //                    // Need to modify values for all axis
        //                    BoneSliderCallBack('a', item.workingTransform, bone, blend.SliderY.value);
        //                });
        //                blend.SliderY.gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    //Blend Shape Working
        //    foreach (var item in morphsList)
        //    {
        //        min = BlendShapeController.instance.allBlendShapes[item].minValue;
        //        max = BlendShapeController.instance.allBlendShapes[item].maxValue;
        //        blendIndex = BlendShapeController.instance.allBlendShapes[item].index;

        //        currentValue = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);

        //        // X-Slider
        //        if (BlendShapeController.instance.allBlendShapes[item].AxisType == AxisType.X_Axis)
        //        {
        //            // Set Min Max Values
        //            blend.SliderX.maxValue = max;
        //            blend.SliderX.minValue = min;

        //            // Setting Slider Value
        //            blend.SliderX.value = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);

        //            // Due To some unity Issue need to create local variables
        //            int indexTemp = blendIndex;

        //            // Adding Listener
        //            blend.SliderX.onValueChanged.AddListener(delegate { SliderXCallBack(blend.SliderX.value, indexTemp); });
        //            blend.SliderX.gameObject.SetActive(true);
        //        }
        //        else // Y-Slider
        //        {
        //            // Set Min Max Values
        //            blend.SliderY.maxValue = max;
        //            blend.SliderY.minValue = min;

        //            // Setting Slider Value
        //            blend.SliderY.value = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);

        //            // Due To some unity Issue need to create local variables
        //            int indexTemp = blendIndex;

        //            // Adding Listener
        //            blend.SliderY.onValueChanged.AddListener(delegate { SliderYCallBack(blend.SliderY.value, indexTemp); });
        //            blend.SliderY.gameObject.SetActive(true);
        //        }
        //    }
        //}

        #endregion

        for (int i = 0; i < morphsList.Count; i++)
        {
            if (BlendShapeController.instance.allBlendShapes[morphsList[i]].boneAvailable)
            {
                print("Yes Bone");
                foreach (var item in BlendShapeController.instance.allBlendShapes[morphsList[i]].boneData)
                {
                    print("---- AxesDetails : " + item.workingAxes);

                    Transform bone = BlendShapeController.instance.allBlendShapes[morphsList[i]].boneObj.transform;
                    // Has only 2 slider x & y
                    // If z axis need to modify than use above mention sliders[x,y]
                    if (item.workingAxes == AxesDetails.z)
                    {
                        if (BlendShapeController.instance.allBlendShapes[morphsList[i]].AxisType == AxisType.X_Axis)
                            SetSliderReleatedDataForBone(blend.SliderX, 'z', 'z', item, bone);
                        else
                            SetSliderReleatedDataForBone(blend.SliderY, 'z', 'z', item, bone);
                    }

                    // X-Slider
                    else if (item.workingAxes == AxesDetails.x)
                    {
                        SetSliderReleatedDataForBone(blend.SliderX, 'x', 'x', item, bone);
                    }

                    //Y-Slider
                    else if (item.workingAxes == AxesDetails.y)
                    {
                        //SetSliderReleatedDataForBone(blend.SliderY, 'x', 'y', item, bone);
                        SetSliderReleatedDataForBone(blend.SliderY, 'y', 'y', item, bone);
                    }

                    // XYZ Axis with single slider
                    else if (item.workingAxes == AxesDetails.xy)
                    {
                        // char 'a' stand for All Axis
                        if (BlendShapeController.instance.allBlendShapes[morphsList[i]].AxisType == AxisType.X_Axis)
                            SetSliderReleatedDataForBone(blend.SliderX, 'x', 'a', item, bone);
                        else
                            SetSliderReleatedDataForBone(blend.SliderY, 'x', 'a', item, bone);
                    }
                }
            }
            else
            {
                print("No Bone");
                //Blend Shape Working
                min = BlendShapeController.instance.allBlendShapes[morphsList[i]].minValue;
                max = BlendShapeController.instance.allBlendShapes[morphsList[i]].maxValue;
                blendIndex = BlendShapeController.instance.allBlendShapes[morphsList[i]].index;

                currentValue = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);
                bool localReverse = BlendShapeController.instance.allBlendShapes[morphsList[i]].reverseMyValue;

                // X-Slider
                if (BlendShapeController.instance.allBlendShapes[morphsList[i]].AxisType == AxisType.X_Axis)
                {
                    if (!localReverse)
                    {
                        // Set Min Max Values
                        blend.SliderX.maxValue = max;
                        blend.SliderX.minValue = min;

                        // Setting Slider Value
                        blend.SliderX.value = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);
                    }

                    // Due To some unity Issue need to create local variables
                    int indexTemp = blendIndex;

                    // Adding Listener
                    blend.SliderX.onValueChanged.AddListener(delegate { SliderXCallBack(blend.SliderX.value, indexTemp, localReverse); });
                    blend.SliderX.gameObject.SetActive(true);
                }
                else // Y-Slider
                {
                    if (!localReverse)
                    {
                        // Set Min Max Values
                        blend.SliderY.maxValue = max;
                        blend.SliderY.minValue = min;

                        // Setting Slider Value
                        blend.SliderY.value = Head.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(blendIndex);
                    }

                    // Due To some unity Issue need to create local variables
                    int indexTemp = blendIndex;

                    // Adding Listener
                    blend.SliderY.onValueChanged.AddListener(delegate { SliderYCallBack(blend.SliderY.value, indexTemp, localReverse); });
                    blend.SliderY.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SliderXCallBack(float value, int blendInd, bool reverseMyValue = false)
    {
        if (reverseMyValue)
        {
            value = blend.SliderX.maxValue - value;
        }
        Head.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(blendInd, value);
    }
    public void SliderYCallBack(float value, int blendInd, bool reverseMyValue = false)
    {
        if (reverseMyValue)
        {
            float maxValue = blend.SliderY.maxValue;
            value = maxValue - value;
        }
        Head.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(blendInd, value);
    }


    float GetBoneCurrentValue(char axis, TransformDetails td, Transform bone)
    {
        if (axis == 'x')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    return bone.localScale.x;

                case TransformDetails.rotation:
                    return bone.eulerAngles.x;

                case TransformDetails.postion:
                    return bone.localPosition.x;
            }
        }
        else if (axis == 'y')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    return bone.localScale.y;

                case TransformDetails.rotation:
                    return bone.eulerAngles.y;

                case TransformDetails.postion:
                    return bone.localPosition.y;
            }
        }
        else if (axis == 'z')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    return bone.localScale.z;

                case TransformDetails.rotation:
                    return bone.eulerAngles.z;

                case TransformDetails.postion:
                    return bone.localPosition.z;
            }
        }
        return 0;
    }
    public void BoneSliderCallBack(char axis, TransformDetails td, Transform bone, float value, bool reverseValue = false)
    {
        Vector3 tempVector = Vector3.zero;

        if (reverseValue)
            value = (-1) * value;

        if (axis == 'x')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    bone.localScale = new Vector3(value, bone.localScale.y, bone.localScale.z);
                    break;

                case TransformDetails.rotation:
                    bone.eulerAngles = new Vector3(value, bone.eulerAngles.y, bone.eulerAngles.z);
                    break;

                case TransformDetails.postion:
                    bone.localPosition = new Vector3(value, bone.localPosition.y, bone.localPosition.z);
                    break;
            }
        }
        else if (axis == 'y')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    bone.localScale = new Vector3(bone.localScale.x, value, bone.localScale.z);
                    break;

                case TransformDetails.rotation:
                    bone.eulerAngles = new Vector3(bone.eulerAngles.x, value, bone.eulerAngles.z);
                    break;

                case TransformDetails.postion:
                    bone.localPosition = new Vector3(bone.localPosition.x, value, bone.localPosition.z);
                    break;
            }
        }
        else if (axis == 'z')
        {
            switch (td)
            {
                case TransformDetails.scale:
                    bone.localScale = new Vector3(bone.localScale.x, bone.localScale.y, value);
                    break;

                case TransformDetails.rotation:
                    bone.eulerAngles = new Vector3(bone.eulerAngles.x, bone.eulerAngles.y, value);
                    break;

                case TransformDetails.postion:
                    bone.localPosition = new Vector3(bone.localPosition.x, bone.localPosition.y, value);
                    break;
            }
        }
        else if (axis == 'a') // All Axis
        {
            switch (td)
            {
                case TransformDetails.scale:
                    bone.localScale = Vector3.one * value;
                    break;

                case TransformDetails.rotation:
                    bone.eulerAngles = Vector3.one * value;
                    break;

                case TransformDetails.postion:
                    bone.localPosition = Vector3.one * value;
                    break;
            }
        }
    }
    void SetSliderReleatedDataForBone(UnityEngine.UI.Slider selectedSlider, char effectedAxis, char callBackAxis, BoneData item, Transform bone)
    {
        // Set Min Max Values
        selectedSlider.maxValue = item.maxValue;
        selectedSlider.minValue = item.minValue;

        // Setting Slider Value
        selectedSlider.value = GetBoneCurrentValue(effectedAxis, item.workingTransform, bone);
        selectedSlider.onValueChanged.AddListener(delegate
        {
            BoneSliderCallBack(callBackAxis, item.workingTransform, bone, selectedSlider.value, item.reverseMyvalue);
        });

        selectedSlider.gameObject.SetActive(true);
    }


    public void ApplyGredientColor(Color gredientColor, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, gredientColor);
        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(GredientColorName, gredientColor);
    }
    public void ApplyGredientDefault(GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, DefaultGredientColor);
        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(GredientColorName, DefaultGredientColor);
    }


    public void ChangeSkinColor(int colorInd)
    {
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].color = skinColor[colorInd];
        //Body.materials[0].color = skinColor[colorInd];

        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, skinColor[colorInd]);
        ApplyGredientColor(skinGredientColor[colorInd], this.gameObject);
        Body.materials[0].SetColor(Skin_ColorName, skinColor[colorInd]);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, defaultSssValue);
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0f);

        Body.materials[0].SetFloat(SssIntensity, defaultSssValue);

    }
    public void ChangeSkinColor(Color color)
    {
        print("Change Skin 2 : " + color);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, color);
        Body.materials[0].SetColor(Skin_ColorName, color);

    }
    public void ChangeSkinColorSlider(int colorInd)
    {
        OnSkinColorApply?.Invoke(skinColor[colorInd]);
    }

    public void ChangeLipColor(int colorInd)
    {
        print("Lip Working Here : " + colorInd);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, lipColor[colorInd]);


        //if (colorInd != 0) // is not Deafult lip
        //{
        //    Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, lipColor[colorInd]);
        //}
        //else
        //{
        //    Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, DefaultLipColor);
        //}
    }
    public void ChangeLipColor(Color color)
    {
        print("Lip Working Here : " + color);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, color);
    }
    public void ChangeEyebrowColor(Color color)
    {
        print("Change Eyebrow From Slider : " + color);
        Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Eyebrow_ColorName, color);
    }
    public void ChangeHairColor(Color color)
    {
        print("Change Hair From Slider : " + color);
        if (avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("_Band"))
        {
            // For Band using Eye Shader so variable name is Changed 
            // Variable is equal to eyename
            avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName,color);
        }
        else if (avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials.Length > 1) // In case Of Hat there is 2 material
        {
            if (avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Cap") ||
               avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Hat"))
                 avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Hair_ColorName, color);
            else
                 avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, color);
        }
        else
            avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, color);

        //avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, color);
    }
    public void ChangeEyeColor(Color color)
    {
        print("Change eye Color From Slider : " + color);
        // Change color if default texture selected
        // else do nothing

        //if default texture than update mask so color reflect init
        // _Main_Trexture //Color_Texture 
        // _Mask_texture
        // _Emission_Texture

        print("Eye Texture Name : " + Head.GetComponent<SkinnedMeshRenderer>().materials[0].GetTexture("_Main_Trexture").name);
        string currentTextureName = Head.GetComponent<SkinnedMeshRenderer>().materials[0].GetTexture("_Main_Trexture").name.ToLower();
        if (currentTextureName == "xana_eye_default" ||currentTextureName == "eye_color_texture")
        {
            if(currentTextureName == "xana_eye_default")
            {
                Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetTexture("_Main_Trexture", Eye_Color_Texture);
                Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetTexture("_Mask_texture", Eye_Mask_Texture);
            }
            Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, color);
        }
    }

    //public void SetIntensityDefault() {
    //    Body.materials[0].SetColor(GredientColorName, Color.black);
    //    Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0);
    //   // Body.materials[0].SetFloat(SssIntensity, 0);
    //}

    /// <summary>
    /// Call for have SssIntensity, SkinGredientColor
    /// </summary>
    /// <param name="savedColor"></param>
    /// <param name="skinGredientColor"></param>
    /// <param name="SssValue"></param>
    public void ImplementSavedSkinColor(Color savedColor, /*Color skinGredientColor,*/ float SssValue)
    {
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].color = savedColor;
        //Body.materials[0].color = savedColor;
        print("Change Skin 3 : " + savedColor);

        if (new Vector3(savedColor.r, savedColor.b, savedColor.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
        {
            Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, savedColor);
            Body.materials[0].SetColor(Skin_ColorName, savedColor);
            //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, DefaultGredientColor);
            //Body.materials[0].SetColor(GredientColorName, DefaultGredientColor);
        }
        else
        {
            Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
            Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
            //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, skinGredientColor);
            //Body.materials[0].SetColor(GredientColorName, skinGredientColor);
        }
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, SssValue);
        // Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0);

        // Body.materials[0].SetFloat(SssIntensity, SssValue);
    }

    /// <summary>
    /// Call for not having SsIntensirty, GredientColorName
    /// </summary>
    /// <param name="savedColor"></param>
    /// <param name="skinGredientColor"></param>
    /// <param name="SssValue"></param>
    public void ImplementSavedSkinColor(Color savedColor)
    {
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].color = savedColor;
        //Body.materials[0].color = savedColor;
        print("Change Skin 3 : " + savedColor);

        if (new Vector3(savedColor.r, savedColor.b, savedColor.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
        {
            Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, savedColor);
            Body.materials[0].SetColor(Skin_ColorName, savedColor);
        }
        else
        {
            Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
            Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
        }
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, DefaultGredientColor);
        //Body.materials[0].SetColor(GredientColorName, DefaultGredientColor);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, defaultSssValue);
        //Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0f);

        //Body.materials[0].SetFloat(SssIntensity, defaultSssValue);
    }

    /// <summary>
    ///  Call for not having SsIntensirty, GredientColorName
    /// </summary>
    /// <param name="SkinColor"></param>
    /// <param name="LipColor"></param>
    /// <param name="applyOn"></param>
    /// <returns></returns>
    public IEnumerator ImplementColors(Color SkinColor, Color LipColor, GameObject applyOn)
    {
        print("Change Skin 4 : " + SkinColor);
        yield return new WaitForSeconds(0f);
        if (new Vector3(SkinColor.r, SkinColor.b, SkinColor.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, SkinColor);
            applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, SkinColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
            applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
        }

        if (new Vector3(LipColor.r, LipColor.b, LipColor.g) != new Vector3(0.00f, 0.00f, 0.00f))
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, LipColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, DefaultLipColor);
        }

    }
    public IEnumerator ImplementColors(Color SkinColor, Color LipColor, Color HairColor, Color EyebrowColor, Color EyeColor, GameObject applyOn)
    {
        print("Change Skin 4 : " + SkinColor);
        yield return new WaitForSeconds(0f);
        if (new Vector3(SkinColor.r, SkinColor.b, SkinColor.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, SkinColor);
            applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, SkinColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
            applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
        }

        if (new Vector3(LipColor.r, LipColor.b, LipColor.g) != new Vector3(0.00f, 0.00f, 0.00f))
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, LipColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, DefaultLipColor);
        }

        // Hair
        if (new Vector3(HairColor.r, HairColor.b, HairColor.g) != new Vector3(0.00f, 0.00f, 0.00f))
        {
            if(applyOn.GetComponent<CharcterBodyParts>().avatarController.wornHair != null)
            applyOn.GetComponent<CharcterBodyParts>().avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, HairColor);
        }

        // EyeBrow
        if (new Vector3(EyebrowColor.r, EyebrowColor.b, EyebrowColor.g) != new Vector3(0.00f, 0.00f, 0.00f) && EyebrowColor != Color.white)
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Eyebrow_ColorName, EyebrowColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Eyebrow_ColorName, DefaultEyebrowColor);
        }

        // Eyes
        if (new Vector3(EyeColor.r, EyeColor.b, EyeColor.g) != new Vector3(0.00f, 0.00f, 0.00f))
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, EyeColor);
        }
        else
        {
            applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, Color.white);
        }
    }

    // public enum ObjColor { skinColor,lipColor,hairColor,eyebrowColor,eyeColor}
    public IEnumerator ImplementColors(Color _color, SliderType _objColor, GameObject applyOn)
    {
        yield return new WaitForSeconds(0f);

        switch (_objColor)
        {
            case SliderType.Skin:
                if (new Vector3(_color.r, _color.b, _color.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, _color);
                    applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, _color);
                }
                else
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
                    applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);
                }
                break;

            case SliderType.HairColor:
                if (new Vector3(_color.r, _color.b, _color.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
                {
                    AvatarController ac = applyOn.GetComponent<CharcterBodyParts>().avatarController;
                    if (ac.wornHair != null)
                    {
                        //ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, _color);
                        if (ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("_Band"))
                        {
                            // For Band using Eye Shader so variable name is Changed 
                            // Variable is equal to eyename
                            ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, _color);
                        }
                        else if (ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials.Length > 1) // In case Of Hat there is 2 material
                        {
                            if (ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Cap") ||
                               ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Hat"))
                                ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Hair_ColorName, _color);
                            else
                                ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, _color);
                        }
                        else
                            ac.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Hair_ColorName, _color);
                    }

                }
                break;

            case SliderType.EyeBrowColor:
                if (new Vector3(_color.r, _color.b, _color.g) != new Vector3(0.00f, 0.00f, 0.00f) && _color != Color.white)
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Eyebrow_ColorName, _color);
                }
                else
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor(Eyebrow_ColorName, DefaultEyebrowColor);
                }
                break;

            case SliderType.EyesColor:
                if (new Vector3(_color.r, _color.b, _color.g) != new Vector3(0.00f, 0.00f, 0.00f))
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, _color);
                }
                else
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor(Eye_ColorName, Color.white);
                }
                break;
           
            case SliderType.LipsColor:
                if (new Vector3(_color.r, _color.b, _color.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, _color);
                }
                else
                {
                    applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, DefaultLipColor);
                }
                break;

            default:
                break;
        }
    }


    ///// <summary>
    /////  Call for  having SsIntensirty, GredientColorName
    ///// </summary>
    ///// <param name="SkinColor"></param>
    ///// <param name="LipColor"></param>
    ///// <param name="applyOn"></param>
    ///// <returns></returns>
    //public IEnumerator ImplementColors(Color SkinColor, Color LipColor, Color gredientColor, GameObject applyOn)
    //{
    //    print("Change Skin 4 : " + SkinColor);
    //    yield return new WaitForSeconds(0);
    //    if (new Vector3(SkinColor.r, SkinColor.b, SkinColor.g) != new Vector3(0.00f, 0.00f, 0.00f) /*!SkinColor.Compare(Color.black)*/)
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, SkinColor);
    //        //applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0);
    //        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, SkinColor);

    //    }
    //    else
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Skin_ColorName, DefaultSkinColor);
    //        //applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, 0);
    //        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(Skin_ColorName, DefaultSkinColor);


    //    }

    //    if (new Vector3(LipColor.r, LipColor.b, LipColor.g) != new Vector3(0.00f, 0.00f, 0.00f))
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, LipColor);
    //    }
    //    else
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(Lip_ColorName, DefaultLipColor);
    //    }

    //    if (new Vector3(gredientColor.r, gredientColor.b, gredientColor.g) != new Vector3(0.00f, 0.00f, 0.00f) )
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, gredientColor);
    //        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(GredientColorName, gredientColor);
    //    }
    //    else
    //    {
    //        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, DefaultGredientColor);
    //        applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetColor(GredientColorName, DefaultGredientColor);
    //        //applyOn.GetComponent<CharcterBodyParts>().Body.materials[0].SetFloat(SssIntensity, defaultSssValue);
    //    }

    //}

    /// <summary>
    /// Intializa Character Bones from the character body script for saving.
    /// </summary>
    public void IntCharacterBones()
    {
        // BonesData.Clear();
        // Eye
        foreach (var bone in BothEyes)
        {
            BonesData.Add(new BoneDataContainer(bone.name, bone.gameObject, bone.transform.localPosition, bone.transform.localEulerAngles, bone.transform.localScale));
        }

        // Eye Inner
        foreach (var bone in EyeIner)
        {
            BonesData.Add(new BoneDataContainer(bone.name, bone.gameObject, bone.transform.localPosition, bone.transform.localEulerAngles, bone.transform.localScale));
        }

        // Eye out
        foreach (var bone in EyesOut)
        {
            BonesData.Add(new BoneDataContainer(bone.name, bone.gameObject, bone.transform.localPosition, bone.transform.localEulerAngles, bone.transform.localScale));
        }

        // Lips
        foreach (var bone in BothLips)
        {
            BonesData.Add(new BoneDataContainer(bone.name, bone.gameObject, bone.transform.localPosition, bone.transform.localEulerAngles, bone.transform.localScale));
        }

        Transform singleBone;
        // jaw
        singleBone = JBone.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        // head
        singleBone = Head.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        // nose
        singleBone = Nose.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        // mouth
        singleBone = Lips.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        // Fore head
        singleBone = ForeHead.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        // Head all
        singleBone = headAll.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));
        //[WaqasAhmad] New Bone Add After character Scaling From Design End
        singleBone = headAll.transform.transform.parent.transform;
        BonesData.Add(new BoneDataContainer(singleBone.name, singleBone.gameObject, singleBone.transform.localPosition, singleBone.transform.localEulerAngles, singleBone.transform.localScale));



        // Fat
        foreach (var bone in _scaleBodyParts)
        {
            BonesData.Add(new BoneDataContainer(bone.name, bone.gameObject, bone.transform.localPosition, bone.transform.localEulerAngles, bone.transform.localScale));
        }
        //// saving default Transfrom of bones
        //foreach (var item in BonesData)
        //{
        //    DefaultBones.Add(new  BoneDataContainer(item.Name,item.Obj, item.Obj.transform.position, item.Obj.transform.localScale));
        //}
    }

    public void ApplyTexture(string name, Texture texture, GameObject applyOn)
    {
        if (name.Contains("shirt") || name.Contains("Shirt") || name.Contains("arabic"))
        {
            applyOn.GetComponent<CharcterBodyParts>().TextureForShirt(texture);

            //CharcterBodyParts.instance.TextureForShirt(texture);
        }
        else if (name.Contains("pant") || name.Contains("Pants"))
        {
            applyOn.GetComponent<CharcterBodyParts>().TextureForPant(texture);
        }
        else if (name.Contains("shoes") || name.Contains("Shose", System.StringComparison.CurrentCultureIgnoreCase) || name.Contains("Plain_mask", System.StringComparison.CurrentCultureIgnoreCase))
        {
            applyOn.GetComponent<CharcterBodyParts>().TextureForShoes(texture);
        }
    }
    //public void ApplyTexture(string name, Texture texture)
    //{
    //    if (name.Contains("shirt", System.StringComparison.CurrentCultureIgnoreCase))
    //    {
    //        CharcterBodyParts.instance.TextureForShirt(texture);

    //    }
    //    else if (name.Contains("pant", System.StringComparison.CurrentCultureIgnoreCase))
    //    {
    //        CharcterBodyParts.instance.GetComponent<CharcterBodyParts>().TextureForPant(texture);
    //    }
    //    else if (name.Contains("shoes") || name.Contains("Shoes") || name.Contains("Shose", System.StringComparison.CurrentCultureIgnoreCase))
    //    {
    //        CharcterBodyParts.instance.TextureForShoes(texture);
    //    }
    //}
    public void ApplyEyeLenTexture(Texture texture, GameObject applyOn)
    {
        Material mainMaterial = applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0];
        // _Main_Trexture
        // _Mask_texture
        // _Emission_Texture

        mainMaterial.SetTexture(eyeLen_TextureName, texture);

        // Update Mask Texture As well & reset Its Color

        if (texture.name.ToLower() == "eye_color_texture" && mainMaterial.GetColor(Eye_ColorName) !=  Color.white)
        {
            mainMaterial.SetTexture("_Mask_texture", Eye_Mask_Texture);
        }
        else
        {
            mainMaterial.SetTexture("_Mask_texture", Eye_Color_Texture);
            mainMaterial.SetColor(Eye_ColorName, Color.white);
        }
        // After EyeShader update need to pass this texture to another property
        //applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[0].SetTexture("_Emission_Texture", texture);
    }
    public void ApplyEyeBrowTexture(Texture texture, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetTexture(EyeBrrow_TextureName, texture);
        if(BlendShapeController.instance !=null)
        BlendShapeController.instance.ResetEyeBrowBlendValues();
    }

    public void ApplyEyeBrow(Texture texture, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[1].SetTexture(EyeBrrow_TextureName, texture);
    }
   
    public Color GetEyebrowColor()
    {
        return Head.GetComponent<SkinnedMeshRenderer>().materials[1].GetColor(Eyebrow_ColorName);
    }
    public Color GetBodyColor()
    {
        return Body.materials[0].GetColor(Skin_ColorName);
    }
    public Color GetHairColor()
    {
        if (avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("_Band"))
        {
            // For Band using Eye Shader so variable name is Changed 
            // Variable is equal to eyename
            return avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].GetColor(Eye_ColorName);
        }
        else if (avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials.Length > 1) // In case Of Hat there is 2 material
        {
             if(avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Cap") ||
                avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].name.Contains("Hat"))
                return avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[1].GetColor(Hair_ColorName);
             else
                return avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].GetColor(Hair_ColorName);
        }
        else
            return avatarController.wornHair.GetComponent<SkinnedMeshRenderer>().materials[0].GetColor(Hair_ColorName);
    }
    public Color GetEyeColor()
    {
        return Head.GetComponent<SkinnedMeshRenderer>().materials[0].GetColor(Eye_ColorName);
    }
    public Color GetSkinGredientColor()
    {
        return Body.materials[0].GetColor(GredientColorName);
    }

    public float GetSssIntensity()
    {
        return Head.GetComponent<SkinnedMeshRenderer>().materials[2].GetFloat(SssIntensity);
    }
    public Color GetLipColor()
    {
        return Head.GetComponent<SkinnedMeshRenderer>().materials[2].GetColor(Lip_ColorName);
    }

    public void LoadBlendShapes(SavingCharacterDataClass data, GameObject applyOn)
    {
        SkinnedMeshRenderer effectedHead = applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>();
        //blend shapes

        for (int i = 0; i < effectedHead.sharedMesh.blendShapeCount; i++)
        {
            if (data.FaceBlendsShapes != null && data.FaceBlendsShapes.Length > 0)
            {
                //if (i < data.FaceBlendsShapes.Length)
                //{

                //    if (i == 32)
                //        effectedHead.SetBlendShapeWeight(i, 0);
                //    else
                //        effectedHead.SetBlendShapeWeight(i, data.FaceBlendsShapes[i]);
                //}
                //else
                //    effectedHead.SetBlendShapeWeight(i, 0);

                // Added By WaqasAhmad
                // if BlendCount & Blend In File are Same Then Assign Blend Value
                // Else Set Blend Values to Default

                if (data.FaceBlendsShapes.Length == effectedHead.sharedMesh.blendShapeCount)
                    effectedHead.SetBlendShapeWeight(i, data.FaceBlendsShapes[i]);
                else
                    effectedHead.SetBlendShapeWeight(i, 0);
            }
        }
    }
    public void ApplyBlendShapeEyesValues(GameObject applyOn, List<BlendShapeContainer> data, Vector3 eyesPos, float rotationz)
    {
        SkinnedMeshRenderer blendRender = applyOn.GetComponent<SkinnedMeshRenderer>();

        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                blendRender.SetBlendShapeWeight(data[i].blendShapeind, data[i].blendShapeValue);
            }
        }

        // ***** Reset to Default If Custom chance made
        BothEyes[0].transform.localPosition = new Vector3(-0.052f, 0.1106f, 0.122f);
        BothEyes[1].transform.localPosition = new Vector3(0.052f, 0.1106f, 0.122f);

        EyeIner[0].transform.localPosition = new Vector3(0f, 0f, -0.039f);
        EyeIner[1].transform.localPosition = new Vector3(0f, 0f, 0.039f);

        EyesOut[0].transform.localPosition = new Vector3(0f, 0f, -0.039f);
        EyesOut[1].transform.localPosition = new Vector3(0f, 0f, 0.039f);

        for (int i = 0; i < BothEyes.Length; i++)
        {
            BothEyes[i].transform.localScale = Vector3.one;
            EyeIner[i].transform.localScale = Vector3.one;
            EyesOut[i].transform.localScale = Vector3.one;
        }

        // *****



        // SettingPosition
        BothEyes[1].transform.localPosition = eyesPos;

        // Inverst x Position for other eye
        eyesPos.x *= (-1);
        BothEyes[0].transform.localPosition = eyesPos;


        BothEyes[0].transform.localEulerAngles = new Vector3(0, 0, rotationz);
        BothEyes[1].transform.localEulerAngles = new Vector3(-180, 0, rotationz);
    }
    public void ApplyBlendShapeLipsValues(GameObject applyOn, List<BlendShapeContainer> data)
    {
        SkinnedMeshRenderer blendRender = applyOn.GetComponent<SkinnedMeshRenderer>();

        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                blendRender.SetBlendShapeWeight(data[i].blendShapeind, data[i].blendShapeValue);
            }
        }
    }
    public void ApplyEyeLashes(Texture texture, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[3].SetTexture(eyeLashes_TextureName, texture);
    }

    public void ApplyMakeup(Texture texture, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetTexture(Makeup_TextureName, texture);
    }


    /// <summary>
    /// Set Sss Intentsity for worlds
    /// </summary>
    /// <param name="value"> intensity value for shadder</param>
    /// <param name="applyOn"> player gameobject on which apply value</param>
    public void SetSssIntensity(float value, GameObject applyOn)
    {
        applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetFloat(SssIntensity, value);
        print("HEAD shader SSs for gmaeplay is " + applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[2].GetFloat(SssIntensity));
        applyOn.GetComponent<CharcterBodyParts>().Body.GetComponent<SkinnedMeshRenderer>().materials[0].SetFloat(SssIntensity, value);
        print("BODY shader SSs for gmaeplay is " + applyOn.GetComponent<CharcterBodyParts>().Body.GetComponent<SkinnedMeshRenderer>().materials[0].GetFloat(SssIntensity));

    }

    //public string GetEyeLashesName(GameObject applyOn) {
    //    string name;
    //    name = applyOn.GetComponent<CharcterBodyParts>().Head.GetComponent<SkinnedMeshRenderer>().materials[3].GetTexture(eyeLashes_TextureName);
    //}


    /// <summary>
    /// To Apply Gredient for preset data.
    /// </summary>
    public void ApplyPresiteGredient()
    {
        Body.materials[0].SetColor(GredientColorName, PresetGredientColor);
        Head.GetComponent<SkinnedMeshRenderer>().materials[2].SetColor(GredientColorName, PresetGredientColor);
    }

}

public enum SliderType
{
    Skin, HairColor, EyeBrowColor, EyesColor, LipsColor
}