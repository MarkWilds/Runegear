using System;
using OpenTK;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Camera
{
    public class PerspCamera : BaseViewportCamera
    {
        private int mWindowWidth, mWindowHeight;

        public float Fov { get; set; } 

        public PerspCamera()
        {
            mWindowWidth = mWindowHeight = 0;
            Fov = 80.0f;
        }

        public void SetWindowDimensions(int width, int height)
        {
            mWindowWidth = width;
            mWindowHeight = height;

            mProjDirty = true;
        }

        public override Matrix4 GetProjMatrix()
        {
            if (!mProjDirty)
            {
                return mProjMatrix;
            }
            mProjDirty = false;
            mProjMatrix = MathExtensions.CreatePerspectiveLH(Fov, mWindowWidth, mWindowHeight, 0.01f, 10000.0f);

            return mProjMatrix;
        }

        public void MoveFPS(Vector3 move, float speed)
        {
            if (move == Vector3.Zero)
                return;

            Vector3 displacement = mForwardVec * move.Y + mRightVec * move.X;
            mPositionVec += (displacement.Normalized() * speed);

            mViewDirty = true;
        }

        public void RotateFPS(double yawDelta, double pitchDelta)
        {
            double twoPi = Math.PI / 2.0;
            double toRadians = Math.PI / 180.0;

            // get te euler pitch and yaw for the rotated offset vector
            double mPitch = Math.Asin(-mForwardVec.Y);
            double mYaw = Math.Atan2(mForwardVec.X, mForwardVec.Z);

            // convert to radians
            pitchDelta = pitchDelta * toRadians;
            yawDelta = yawDelta * toRadians;

            mPitch += pitchDelta;
            mYaw += yawDelta;

            // clamp pitch
            if (mPitch < -twoPi || mPitch > twoPi)
            {
                double difference = 0.0f;
                if (mPitch < 0.0)
                    difference = -twoPi - mPitch;
                else
                    difference = twoPi - mPitch;

                pitchDelta += difference;
            }

            // rotate all 3 axis vectors
            Matrix4 resultMatrix = Matrix4.CreateFromAxisAngle(mRightVec, (float)pitchDelta) * Matrix4.CreateRotationY((float)yawDelta);
            mRightVec = mRightVec.TransformL(resultMatrix);
            mUpVec = mUpVec.TransformL(resultMatrix);
            mForwardVec = mForwardVec.TransformL(resultMatrix);

            mRightVec.Normalized();
            mUpVec.Normalized();
            mForwardVec.Normalized();

            mViewDirty = true;
        }
    }
}
