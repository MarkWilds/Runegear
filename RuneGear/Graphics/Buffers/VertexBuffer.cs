using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace RuneGear.Graphics.Buffers
{
    public class VertexBuffer : IDisposable
    {
        private int mID;

        public VertexBuffer()
        {
            mID = 0;
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, mID);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Dispose of the vertex buffer
        /// </summary>
        public void Dispose()
        {
            if (mID != 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(mID);
            }
        }

        /// <summary>
        /// Create a vertex buffer
        /// </summary>
        /// <param name="size"></param>
        /// <param name="usage"></param>
        public void Create(int size, BufferUsageHint usage)
        {
            mID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, mID);

            // create empty buffer on the videocard of the specified size
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)size, IntPtr.Zero, usage);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Add data to an already created vertex buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        public void SubData<T>(List<T> buffer) where T: struct
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, mID);

            T[] dataArray = buffer.ToArray<T>();
            GL.BufferSubData<T>(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(buffer.Count), dataArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Data<T>(List<T> buffer, int size, BufferUsageHint usage) where T: struct
        {
            T[] dataArray = buffer.ToArray<T>();
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)size, dataArray, usage);
        }
    }
}
