//UNITY_SHADER_NO_UPGRADE
#ifndef HLSLCIRCLE_INCLUDED
#define HLSLCIRCLE_INCLUDED

void CalcSymmetricalDistanceFromCenter_float(float A, float L, out float distance)
{
    // 1. Calculate symmetrical distance from the center (0.5).
    // The value 'A' is already centered and rotated
    // A = Angular Distance
    // L = Degrees

    float dist = abs(A - 0.5);

    //2. Adjust distance for wrapping (shortest path on a circle).
    dist = 0.5 - abs(dist - 0.5);

    // 3. Get the Arc Half-Length.
    float halfL = L / 2.0;

    //4. Return 1 (White) if the distance is within the half-length.
    // and 0 (Black) otherwise
    distance = step(dist, halfL);
}
#endif //HLSLCIRCLE_INCLUDED