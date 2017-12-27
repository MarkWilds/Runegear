using OpenTK;

namespace RuneGear.MapObjects.SolidMapObject
{
    public struct TextureMapping
    {
        public const decimal DefaultScaleValue = 1;
        public const decimal DefaultShiftValue = 0;
        public const decimal DefaultRotationValue = 0;
        public const bool DefaultTextureLockValue = false;

        public Vector3 UAxis;
        public Vector3 VAxis;

        public float UScale;
        public float VScale;

        public float UShift;
        public float VShift;

        public float Rotation;

        public bool TextureLocked;
    }
}
