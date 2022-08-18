using System;
using System.Reflection;
using UnityEngine;

namespace Kits.ClientKit.Handlers.Reflection
{
    public static class FieldInfoHandler
    {
          /// <summary>
        /// Copies all public fields from one object to the other, as long as they are from the same type.
        /// </summary>
        /// <param name="sourceOBJ">The source.</param>
        /// <param name="destOBJ">The destination.</param>
        /// <param name="copyProperties">Copy properties as well</param>
        public static void CopyFields(object sourceOBJ, object destOBJ, bool copyProperties = false)
        {

            if (sourceOBJ == null || destOBJ == null)
            {
                Debug.LogError("Error while copying object properties, source / destination not filled!");
                return;
            }

            Type typeDest = destOBJ.GetType();
            Type typeSrc = sourceOBJ.GetType();

            if (typeSrc != typeDest)
            {
                Debug.LogError("Type mismatch when trying to copy fields");
            }



            if (copyProperties)
            {
                foreach (PropertyInfo sourceProp in typeSrc.GetProperties())
                {
                    PropertyInfo targetProp = typeDest.GetProperty(sourceProp.Name);
                    if (targetProp == null)
                    {
                        continue;
                    }

                    //Check for common unity reference types to create a deep copy of the object whose fields we are copying.
                    //if-else chain for the sake of type safety
                    if (sourceProp.PropertyType == typeof(Gradient))
                    {
                        Gradient newGradient = new Gradient();
                        GradientAlphaKey[] sourceAlphaKeys = ((Gradient)sourceProp.GetValue(sourceOBJ)).alphaKeys;
                        GradientAlphaKey[] targetAlphaKeys = new GradientAlphaKey[sourceAlphaKeys.Length];
                        for (int i = 0; i < sourceAlphaKeys.Length; i++)
                        {
                            targetAlphaKeys[i].alpha = sourceAlphaKeys[i].alpha;
                            targetAlphaKeys[i].time = sourceAlphaKeys[i].time;
                        }
                        GradientColorKey[] sourceColorKeys = ((Gradient)sourceProp.GetValue(sourceOBJ)).colorKeys;
                        GradientColorKey[] targetColorKeys = new GradientColorKey[sourceColorKeys.Length];
                        for (int i = 0; i < sourceColorKeys.Length; i++)
                        {
                            targetColorKeys[i].color = new Color(sourceColorKeys[i].color.r, sourceColorKeys[i].color.g, sourceColorKeys[i].color.b, sourceColorKeys[i].color.a);
                            targetColorKeys[i].time = sourceColorKeys[i].time;
                        }
                        newGradient.mode = ((Gradient)sourceProp.GetValue(sourceOBJ)).mode;
                        newGradient.SetKeys(targetColorKeys, targetAlphaKeys);
                        targetProp.SetValue(destOBJ, newGradient);
                    }
                    else if (sourceProp.PropertyType == typeof(AnimationCurve))
                    {
                        targetProp.SetValue(destOBJ, new AnimationCurve(((AnimationCurve)sourceProp.GetValue(sourceOBJ)).keys));
                    }
                    else if (sourceProp.PropertyType == typeof(Color))
                    {
                        Color sourceColor = ((Color)sourceProp.GetValue(sourceOBJ));
                        targetProp.SetValue(destOBJ, new Color(sourceColor.r, sourceColor.g, sourceColor.b, sourceColor.a));
                    }
                    else
                    {
                        targetProp.SetValue(destOBJ, sourceProp.GetValue(sourceOBJ));
                    }

                }
            }

            FieldInfo[] srcFields = typeSrc.GetFields();
            foreach (FieldInfo srcField in srcFields)
            {
                FieldInfo targetField = typeDest.GetField(srcField.Name);
                if (targetField == null)
                {
                    continue;
                }

                //Check for common unity reference types to create a deep copy of the object whose fields we are copying.
                //if-else chain for the sake of type safety
                if (srcField.FieldType == typeof(Gradient))
                {
                    Gradient newGradient = new Gradient();
                    GradientAlphaKey[] sourceAlphaKeys = ((Gradient)srcField.GetValue(sourceOBJ)).alphaKeys;
                    GradientAlphaKey[] targetAlphaKeys = new GradientAlphaKey[sourceAlphaKeys.Length];
                    for (int i = 0; i < sourceAlphaKeys.Length; i++)
                    {
                        targetAlphaKeys[i].alpha = sourceAlphaKeys[i].alpha;
                        targetAlphaKeys[i].time = sourceAlphaKeys[i].time;
                    }
                    GradientColorKey[] sourceColorKeys= ((Gradient)srcField.GetValue(sourceOBJ)).colorKeys;
                    GradientColorKey[] targetColorKeys = new GradientColorKey[sourceColorKeys.Length];
                    for (int i = 0; i < sourceColorKeys.Length; i++)
                    {
                        targetColorKeys[i].color =  new Color(sourceColorKeys[i].color.r, sourceColorKeys[i].color.g, sourceColorKeys[i].color.b, sourceColorKeys[i].color.a);
                        targetColorKeys[i].time = sourceColorKeys[i].time;
                    }
                    newGradient.mode = ((Gradient)srcField.GetValue(sourceOBJ)).mode;
                    newGradient.SetKeys(targetColorKeys,targetAlphaKeys);
                    targetField.SetValue(destOBJ, newGradient);
                }
                else if(srcField.FieldType == typeof(AnimationCurve))
                {
                    targetField.SetValue(destOBJ, new AnimationCurve(((AnimationCurve)srcField.GetValue(sourceOBJ)).keys));
                }
                else if (srcField.FieldType == typeof(Color))
                {
                    Color sourceColor = ((Color)srcField.GetValue(sourceOBJ));
                    targetField.SetValue(destOBJ, new Color(sourceColor.r, sourceColor.g, sourceColor.b, sourceColor.a));
                }
                else
                {
                    targetField.SetValue(destOBJ, srcField.GetValue(sourceOBJ));
                }
                
            }
        }
    }
}