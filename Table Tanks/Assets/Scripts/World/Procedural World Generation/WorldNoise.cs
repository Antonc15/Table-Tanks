using System;
using UnityEngine;

public class WorldNoise
{
    // Protected Fields \\
    protected int seed;
    protected int width;
    protected int length;

    protected ConsistantRandom random;

    // Public Methods \\
    public WorldNoise(int _seed, int _width, int _length)
    {
        seed = _seed;
        width = _width;
        length = _length;

        random = new ConsistantRandom(_seed);
    }

    // Public Virtal Methods \\
    public virtual int GetHeight(int _x, int _y)
    {
        return 0;
    }

    public virtual LandType GetLandType(int _x, int _y)
    {
        return LandType.grass;
    }

    public virtual VegitationType GetVegitationType(int _x, int _y)
    {
        return VegitationType.none;
    }
}

public class DefaultWorldNoise : WorldNoise
{
    // Private Fields \\
    private const float heightAmplitude = 24f;

    private const float forestThreshold = 0.5f;

    private const float conniferChance = 0.3f;
    private const float palmChance = 0.06f;
    private const float rockChance = 0.04f;

    #region Noise 1

    private FastNoise noise1 = new FastNoise();

    private const FastNoise.NoiseType noise1Type = FastNoise.NoiseType.SimplexFractal;
    private const float noise1Frequency = 0.05f;
    private const int noise1Octaves = 2;

    #endregion

    #region Noise 2

    private FastNoise noise2 = new FastNoise();

    private const FastNoise.NoiseType noise2Type = FastNoise.NoiseType.SimplexFractal;
    private const float noise2Frequency = 0.045f;
    private const int noise2Octaves = 3;

    #endregion

    #region Noise 3

    private FastNoise noise3 = new FastNoise();

    private const FastNoise.NoiseType noise3Type = FastNoise.NoiseType.SimplexFractal;
    private const float noise3Frequency = 0.05f;
    private const int noise3Octaves = 2;

    #endregion

    // Private Methods \\
    private void Initialize()
    {
        noise1.SetSeed(seed);
        noise1.SetNoiseType(noise1Type);
        noise1.SetFrequency(noise1Frequency);
        noise1.SetFractalOctaves(noise1Octaves);

        noise2.SetSeed(seed);
        noise2.SetNoiseType(noise2Type);
        noise2.SetFrequency(noise2Frequency);
        noise2.SetFractalOctaves(noise2Octaves);

        noise3.SetSeed(seed);
        noise3.SetNoiseType(noise3Type);
        noise3.SetFrequency(noise3Frequency);
        noise3.SetFractalOctaves(noise3Octaves);
    }

    private float FalloffAmount(int _x, int _y)
    {
        float a = 3f;
        float b = 2.2f;

        float x = ((float)_x / (width - 1) * 2) - 1;
        float y = ((float)_y / (length - 1) * 2) - 1;

        float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - (b * value), a));
    }

    private bool TryGenerateConniferTree(LandType _landType, float _noiseValue, float _randomValue)
    {
        if (_landType != LandType.grass) { return false; }

        if (_noiseValue > forestThreshold) { return false; }

        if (_randomValue > conniferChance) { return false; }

        return true;
    }

    private bool TryGeneratePalmTree(LandType _landType, float _noiseValue, float _randomValue)
    {
        if (_landType != LandType.sand) { return false; }

        if (_randomValue > palmChance) { return false; }

        return true;
    }

    private bool TryGenerateRock(LandType _landType, float _noiseValue, float _randomValue)
    {
        if(_landType == LandType.sand) { return false; }

        if (_noiseValue <= forestThreshold) { return false; }

        if (_randomValue > rockChance) { return false; }

        return true;
    }

    // Public Methods \\
    public DefaultWorldNoise(int _seed, int _width, int _length) : base(_seed, _width, _length)
    {
        Initialize();
    }

    // Public Override Methods \\
    public override int GetHeight(int _x, int _y)
    {
        float noise1Value = ((noise1.GetNoise(_x, _y) + 1) / 2f);
        float noise2Value = ((noise2.GetNoise(_x, _y)) / 2f);

        float fHeight = (noise1Value + noise2Value) / 2f;

        fHeight -= FalloffAmount(_x, _y);
        fHeight *= heightAmplitude;

        return Mathf.CeilToInt(fHeight);
    }

    public override LandType GetLandType(int _x, int _y)
    {
        int tileHeight = GetHeight(_x, _y);

        if(tileHeight <= 1)
        {
            return LandType.sand;
        }

        int[] borderTileHeights = new int[4];

        borderTileHeights[0] = GetHeight(_x, _y + 1);
        borderTileHeights[1] = GetHeight(_x + 1, _y);
        borderTileHeights[2] = GetHeight(_x, _y - 1);
        borderTileHeights[3] = GetHeight(_x - 1, _y);

        for (int i = 0; i < borderTileHeights.Length; i++)
        {
            if(borderTileHeights[i] <= 0)
            {
                return LandType.cliff;
            }

        }

        return LandType.grass;
    }

    public override VegitationType GetVegitationType(int _x, int _y)
    {
        int tileHeight = GetHeight(_x, _y);

        if(tileHeight <= 0) { return VegitationType.none; }

        LandType landType = GetLandType(_x, _y);

        float noiseValue = (noise3.GetNoise(_x, _y) + 1) / 2f;
        float randomValue = (float) random.NextDouble();

        if (TryGenerateConniferTree(landType, noiseValue, randomValue))
        {
            return VegitationType.tree1;
        }

        if(TryGeneratePalmTree(landType, noiseValue, randomValue))
        {
            return VegitationType.tree2;
        }

        if (TryGenerateRock(landType, noiseValue, randomValue))
        {
            return VegitationType.rock1;
        }

        return VegitationType.none;
    }
}

public class TestWorldNoise : WorldNoise
{
    // Public Methods \\
    public TestWorldNoise(int _seed, int _width, int _length) : base(_seed, _width, _length)
    {
    }

    // Public Override Methods \\
    public override int GetHeight(int _x, int _y)
    {
        return 1;
    }

    public override LandType GetLandType(int _x, int _y)
    {
        return LandType.grass;
    }

    public override VegitationType GetVegitationType(int _x, int _y)
    {
        return VegitationType.none;
    }
}