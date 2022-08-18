using UnityEngine;

namespace Kits.ClientKit.Handlers.Animation
{
    /// <summary>
    /// Unity 内置类型扩展
    /// </summary>
    public static class AnimationCurveHandler
    {

        /// <summary>
        /// 复制 AnimationCurve 实例
        /// </summary>
        public static AnimationCurve Clone(this AnimationCurve target)
        {
            AnimationCurve newCurve = new AnimationCurve(target.keys);
            newCurve.postWrapMode = target.postWrapMode;
            newCurve.preWrapMode = target.preWrapMode;

            return newCurve;
        }

        /// <summary>
        /// Add a new key to an AnimationCurve.
        /// Ensures the integrity of other key's tangent modes.
        /// </summary>
        /// <param name="curve">The existing AnimationCurve.</param>
        /// <param name="keyframe">The new keyframe</param>
        /// <returns>The index of the newly added key.</returns>
        public static int AddKey(AnimationCurve curve, Keyframe keyframe)
        {
            if (curve.length == 0)
            {
                return curve.AddKey(keyframe);
            }
            else if (curve.length == 1)
            {
                // Save the existing keyframe data. (Unity changes the tangent info).
                Keyframe temp = curve[0];
                int newIndex = curve.AddKey(keyframe);
                if(newIndex == -1)
                {
                    return 0;
                }
                else if(newIndex == 0)
                {
                    curve.MoveKey(1, temp);
                }
                else
                {
                    curve.MoveKey(0, temp);
                }
                return newIndex;
            }
            else
            {
                Keyframe left = new Keyframe();
                Keyframe right = new Keyframe();

                for (int i = 0; i < curve.length - 1; i++)
                {
                    Keyframe l = curve[i];
                    Keyframe r = curve[i + 1];

                    if (l.time < keyframe.time && keyframe.time < r.time)
                    {
                        left = l;
                        right = r;
                    }
                }

                int index = curve.AddKey(keyframe);

                // Handle left neighbour.
                if (index > 0)
                {
                    // Restore the saved data.
                    curve.MoveKey(index - 1, left);

                    // Update tangent data based on tangent mode.
                    int tangentMode = curve[index - 1].tangentMode;
                    if (IsAuto(tangentMode))
                    {
                        curve.SmoothTangents(index - 1, 0);
                    }
                    if (IsBroken(tangentMode))
                    {
                        if (IsRightLinear(tangentMode))
                        {
                            SetKeyRightLinear(curve, index - 1);
                        }
                    }
                }

                // Handle the Right neighbour.
                if (index < curve.length - 1)
                {
                    // Restore the saved data.
                    curve.MoveKey(index + 1, right);

                    // Update tangent data based on tangent mode.
                    int tangentMode = curve[index + 1].tangentMode;
                    if (IsAuto(tangentMode))
                    {
                        curve.SmoothTangents(index + 1, 0);
                    }
                    if (IsBroken(tangentMode))
                    {
                        if (IsLeftLinear(tangentMode))
                        {
                            SetKeyLeftLinear(curve, index + 1);
                        }
                    }
                }

                return index;
            }
        }

        /// <summary>
        /// Move/Change an existing key in an AnimationCurve.
        /// Maintains TangentMode and updates neighbours.
        /// </summary>
        /// <param name="curve">The existing AnimationCurve.</param>
        /// <param name="index">The index of the current Keyframe.</param>
        /// <param name="keyframe">The new Keyframe data.</param>
        /// <returns>The index of the Keyframe.</returns>
        public static int MoveKey(AnimationCurve curve, int index, Keyframe keyframe)
        {
            // Save the tangent mode.
            Keyframe old = curve[index];
            keyframe.tangentMode = old.tangentMode;

            int newIndex = curve.MoveKey(index, keyframe);

            // Respect the tangentMode and update as necessary.
            if (IsAuto(keyframe.tangentMode))
            {
                curve.SmoothTangents(newIndex, 0);
            }
            else if (IsBroken(keyframe.tangentMode))
            {
                if (IsLeftLinear(keyframe.tangentMode))
                {
                    SetKeyLeftLinear(curve, newIndex);
                }
                if (IsRightLinear(keyframe.tangentMode))
                {
                    SetKeyRightLinear(curve, newIndex);
                }
            }

            // update the left neighbour
            if (newIndex > 0)
            {
                // Update tangent data based on tangent mode.
                int tangentMode = curve[newIndex - 1].tangentMode;
                if (IsAuto(tangentMode))
                {
                    curve.SmoothTangents(newIndex - 1, 0);
                }
                if (IsBroken(tangentMode))
                {
                    if (IsRightLinear(tangentMode))
                    {
                        SetKeyRightLinear(curve, newIndex - 1);
                    }
                }
            }

            // update the right neighbour
            if (newIndex < curve.length - 1)
            {
                // Update tangent data based on tangent mode.
                int tangentMode = curve[newIndex + 1].tangentMode;
                if (IsAuto(tangentMode))
                {
                    curve.SmoothTangents(newIndex + 1, 0);
                }
                if (IsBroken(tangentMode))
                {
                    if (IsLeftLinear(tangentMode))
                    {
                        SetKeyLeftLinear(curve, newIndex + 1);
                    }
                }
            }

            return newIndex;
        }

        /// <summary>
        /// Remove a key from an AnimationCurve.
        /// </summary>
        /// <param name="curve">The existing AnimationCurve.</param>
        /// <param name="index">The index of the Key to be removed.</param>
        public static void RemoveKey(AnimationCurve curve, int index)
        {
            curve.RemoveKey(index);

            // Update left neighbour.
            if (index > 0)
            {
                // Update tangent data based on tangent mode.
                int tangentMode = curve[index-1].tangentMode;

                if (IsAuto(tangentMode))
                {
                    curve.SmoothTangents(index - 1, 0);
                }
                if (IsBroken(tangentMode))
                {
                    if (IsRightLinear(tangentMode))
                    {
                        SetKeyRightLinear(curve, index - 1);
                    }
                }
            }

            // Update right neighbour.
            if (index < curve.length)
            {
                // Update tangent data based on tangent mode.
                int tangentMode = curve[index].tangentMode;

                if (IsAuto(tangentMode))
                {
                    curve.SmoothTangents(index, 0);
                }
                if (IsBroken(tangentMode))
                {
                    if (IsLeftLinear(tangentMode))
                    {
                        SetKeyLeftLinear(curve, index);
                    }
                }
            }
        }

        /// <summary>
        /// Set the indexed key of an AnimationCurve to RightLinear.
        /// </summary>
        /// <param name="curve">The curve to change.</param>
        /// <param name="index">The index of the key to set to RightLinear.</param>
        public static void SetKeyRightLinear(AnimationCurve curve, int index)
        {
            Keyframe kf = curve[index];
            float tangentValue = kf.outTangent;

            if (index < curve.length - 1)
            {
                Keyframe next = curve[index + 1];
                tangentValue = (next.value - kf.value) / (next.time - kf.time);
            }

            Keyframe newKeyframe = new Keyframe(kf.time, kf.value, kf.inTangent, tangentValue);

            // Get current tangent mode.
            int leftTangent = (IsAuto(kf.tangentMode) || kf.tangentMode == 0) ? 0 : (kf.tangentMode % 8) - 1;
            newKeyframe.tangentMode = leftTangent + 16 + 1;

            curve.MoveKey(index, newKeyframe);
        }

        /// <summary>
        /// Set the indexed key of an AnimationCurve to LeftLinear.
        /// </summary>
        /// <param name="curve">The curve to change.</param>
        /// <param name="index">The index of the key to set to LeftLinear.</param>
        public static void SetKeyLeftLinear(AnimationCurve curve, int index)
        {
            Keyframe kf = curve[index];
            float tangentValue = kf.inTangent;

            if (index > 0)
            {
                Keyframe prev = curve[index - 1];
                tangentValue = (kf.value - prev.value) / (kf.time - prev.time);
            }

            Keyframe newKeyframe = new Keyframe(kf.time, kf.value, tangentValue, kf.outTangent);

            int rightTangent = kf.tangentMode > 16 ? (kf.tangentMode / 8) * 8 : 0;
            newKeyframe.tangentMode = rightTangent + 1 + 4;

            curve.MoveKey(index, newKeyframe);
        }

        /// <summary>
        /// Is the TangentMode Auto.
        /// </summary>
        /// <param name="tangentMode">The tangentMode value.</param>
        /// <returns>True if set to auto.</returns>
        public static bool IsAuto(int tangentMode)
        {
            return tangentMode == 10;
        }

        /// <summary>
        /// Is the TangentMode Broken.
        /// </summary>
        /// <param name="index">The tangentMode value.</param>
        /// <returns>True if set to Broken.</returns>
        public static bool IsBroken(int tangentMode)
        {
            return (tangentMode % 2) == 1;
        }

        /// <summary>
        /// Is the TangentMode RightLinear.
        /// </summary>
        /// <param name="tangentMode">The tangentMode value.</param>
        /// <returns>True if the right tangent mode is set to linear.</returns>
        public static bool IsRightLinear(int tangentMode)
        {
            return (tangentMode / 8) == 2;
        }

        /// <summary>
        /// Is the TangentMode LeftLinear.
        /// </summary>
        /// <param name="tangentMode">The tangentMode value.</param>
        /// <returns>True if the left tangent mode is set to linear.</returns>
        public static bool IsLeftLinear(int tangentMode)
        {
            return IsBroken(tangentMode) && (tangentMode % 8) == 5;
        }

        /// <summary>
        /// Inverts an animation curve
        /// </summary>
        /// <param name="curve">The curve to invert</param>
        public static void InvertAnimationCurve(ref AnimationCurve curve)
        {
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;

            for (int i = 0; i < curve.keys.Length; i += curve.keys.Length - 1)
            {
                if (curve.keys[i].value > maxValue)
                {
                    maxValue = curve.keys[i].value;
                }
                if (curve.keys[i].value < minValue)
                {
                    minValue = curve.keys[i].value;
                }
            }

            float middleValue = (maxValue + minValue) * 0.5f;

            Keyframe[] keys = curve.keys;
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].value = keys[i].value + 2 * (middleValue - keys[i].value);
                keys[i].inTangent *= -1f;
                keys[i].inWeight *= -1f;
                keys[i].outTangent *= -1f;
                keys[i].outWeight *= -1f;
            }
            curve.keys = keys;
        }
        
        /// <summary>
        /// Checks if all the keys have the same check value
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public static bool CheckAnimationCurveKeys(Keyframe[] keys, float checkValue)
        {
            if (keys == null || keys.Length < 1)
            {
                return true;
            }

            int checkCount = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].value == checkValue)
                {
                    checkCount++;
                }
            }

            if (checkCount == keys.Length)
            {
                return true;
            }

            return false;
        }
    } 

} 