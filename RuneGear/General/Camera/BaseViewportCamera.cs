using System;
using OpenTK;
using RuneGear.Utilities;
using RuneGear.Utilities.Extensions;

namespace RuneGear.General.Camera
{
    public abstract class BaseViewportCamera
    {
        #region data members

        protected float mNear, mFar;
        protected float mLeft, mTop, mRight, mBottom;
        protected bool mProjDirty, mViewDirty;

        protected Vector3 mPositionVec;
        protected Vector3 mRightVec;
        protected Vector3 mUpVec;
        protected Vector3 mForwardVec;

        protected Matrix4 mProjMatrix;
        protected Matrix4 mViewMatrix;

        #endregion

        public float Near
        {
            get { return mNear; }
            private set
            {
                mNear = value;
            }
        }

        public float Far
        {
            get { return mFar; }
            private set
            {
                mFar = value;
            }
        }

        protected BaseViewportCamera()
        {
            mNear = 1.0f;
            mFar = 10000.0f;
            Reset();
        }

        public void Reset()
        {
            mProjDirty = true;
            mViewDirty = true;

            mPositionVec = Vector3.Zero;
            mRightVec = Vector3.UnitX;
            mUpVec = Vector3.UnitY;
            mForwardVec = Vector3.UnitZ;
        }

        public void SetProjectionWindow(float left, float right, float top, float bottom)
        {
            mLeft = left;
            mRight = right;
            mTop = top;
            mBottom = bottom;

            mProjDirty = true;
        }

        public void SetClipDistance(float n, float f)
        {
            mNear = n;
            mFar = f;
            mProjDirty = true;
        }

        public void Move(float x, float y, float z)
        {
            mPositionVec.X += x;
            mPositionVec.Y += y;
            mPositionVec.Z += z;

            mViewDirty = true;
        }

        public void Move(Vector3 v)
        {
            Move(v.X, v.Y, v.Z);
        }

        public void Rotate(float x, float y, float z, float angle)
        {
            float angleRad = (float)(angle * Math.PI / 180.0);
            Matrix4 resultMatrix = Matrix4.CreateFromAxisAngle(new Vector3(x, y, z), angleRad).Normalized();

            FixPrecisionDrift(ref resultMatrix);

            // rotate all 3 axis vectors
            mRightVec = mRightVec.TransformL(resultMatrix);
            mUpVec = mUpVec.TransformL(resultMatrix);
            mForwardVec = mForwardVec.TransformL(resultMatrix);
            mPositionVec = Vector3.TransformPosition(mPositionVec, resultMatrix);

            mViewDirty = true;
        }

        public void Rotate(Vector3 v, float angle)
        {
            Rotate(v.X, v.Y, v.Z, angle);
        }

        private void FixPrecisionDrift(ref Matrix4 matrix)
        {
            for (int i = 0; i < 16; i++)
            {
                int row = i%4;
                int col = i/4;

                float value = matrix[row, col];
                if (GeneralUtility.IsCloseEnough(value, 0.0f) || GeneralUtility.IsCloseEnough(value, 1.0f))
                    matrix[row, col] = (float)Math.Round(value, 0);
            }
        }

        #region virtual methods

        public abstract Matrix4 GetProjMatrix();

        public virtual Matrix4 GetViewMatrix()
        {
            if (!mViewDirty)
            {
                return mViewMatrix;
            }
            mViewDirty = false;

            // regenerate the orientation
            mForwardVec.Normalize();

            mUpVec = Vector3.Cross(mForwardVec, mRightVec);
            mUpVec.Normalize();

            mRightVec = Vector3.Cross(mUpVec, mForwardVec);
            mRightVec.Normalize();

            // create the view matrix
            mViewMatrix = Matrix4.Identity;

            mViewMatrix.Row0 = new Vector4(mRightVec.X, mUpVec.X, mForwardVec.X, 0.0f);
            mViewMatrix.Row1 = new Vector4(mRightVec.Y, mUpVec.Y, mForwardVec.Y, 0.0f);
            mViewMatrix.Row2 = new Vector4(mRightVec.Z, mUpVec.Z, mForwardVec.Z, 0.0f);

            mViewMatrix.M41 = -Vector3.Dot(mPositionVec, mRightVec);
            mViewMatrix.M42 = -Vector3.Dot(mPositionVec, mUpVec);
            mViewMatrix.M43 = -Vector3.Dot(mPositionVec, mForwardVec);

            FixPrecisionDrift(ref mViewMatrix);

            return mViewMatrix;
        }

        public virtual Matrix4 GetWorldMatrix()
        {
            Matrix4 world = new Matrix4();

            // first row
            world.M11 = mRightVec.X;
            world.M12 = mRightVec.Y;
            world.M13 = mRightVec.Z;

            // second row
            world.M21 = mUpVec.X;
            world.M22 = mUpVec.Y;
            world.M23 = mUpVec.Z;

            // third row
            world.M31 = mForwardVec.X;
            world.M32 = mForwardVec.Y;
            world.M33 = mForwardVec.Z;

            world.Row3 = new Vector4(mPositionVec, 1.0f);

            FixPrecisionDrift(ref world);

            return world;
        }

        #endregion
    }
}