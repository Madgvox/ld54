// All credit: https://blog.s-schoener.com/2018-05-05-animation-curves/

using UnityEngine;

/// <summary>
/// Samples according to a density given by an animation curve.
/// This assumes that the animation curve is non-negative everywhere.
/// </summary>
public class AnimationCurveSampler {
    private readonly AnimationCurve _densityCurve;
    private readonly IntegrateFunc _integratedDensity;

    public AnimationCurveSampler(AnimationCurve curve, int integrationSteps=100) {
        _densityCurve = curve;
        _integratedDensity = new IntegrateFunc(curve.Evaluate,
                                               curve.keys[0].time,
                                               curve.keys[curve.length - 1].time,
                                               integrationSteps);
    }

    /// <summary>
    /// Takes a value s in [0, 1], scales it up to the interval
    /// [0, totalIntegratedValue] and computes its inverse.
    /// </summary>
    private float Invert(float s) {
        s *= _integratedDensity.Total;
        float lower = MinT;
        float upper = MaxT;
        const float precision = 0.00001f;
        while (upper - lower > precision) {
            float mid = (lower + upper) / 2f;
            float d = _integratedDensity.Evaluate(mid);
            if (d > s) {
                upper = mid;
            } else if (d < s) {
                lower = mid;
            } else {
                // unlikely :)
                return mid;
            }
        }

        return (lower + upper) / 2f;
    }

    public float TransformUnit(float unitValue) {
        return Invert(unitValue);
    }

    public float Sample() {
        return Invert(Random.value);
    }

    private float MinT { 
        get { return _densityCurve.keys[0].time; }
    }

    private float MaxT {
        get { return _densityCurve.keys[_densityCurve.length - 1].time; }
    }
}