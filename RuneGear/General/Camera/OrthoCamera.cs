using OpenTK;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Camera
{
    public class OrthoCamera : BaseViewportCamera
    {
        private float mZoom;
        public float Zoom 
        { 
            get
            {
                return mZoom;
            }

            set
            {
                mZoom = value;
                mProjDirty = true;
            }
        }

        public OrthoCamera()
        {
            Zoom = 1.0f;
        }

        public override Matrix4 GetProjMatrix()
        {
            if (!mProjDirty)
            {
                return mProjMatrix;
            }
            mProjDirty = false;

            mProjMatrix = MathExtensions.CreateOrthographicOffCenterLH(mLeft * Zoom, mRight * Zoom,
                    mTop * Zoom, mBottom * Zoom, mNear, mFar);

            return mProjMatrix;
        }
    }
}