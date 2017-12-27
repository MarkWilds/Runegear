using System;
using OpenTK.Graphics.OpenGL4;

namespace RuneGear.Graphics.Shaders
{
    public abstract class ShaderProgram : IDisposable
    {
        public int Id { get; private set; }

        /// <summary>
        /// Specify locations to attributes into the shader
        /// Needs to happen before linking the shader
        /// </summary>
        protected abstract void BindAttributes();

        /// <summary>
        /// This methods specifies how the data for the input is retrieved from the array
        /// </summary>
        public abstract void SpecifyAttributes(int vertexSizeInBytes);

        public abstract void Create();

        protected ShaderProgram() 
        {
            Id = 0;
        }

        public void Dispose()
        {
            GL.UseProgram(0);
            GL.DeleteProgram(Id);
        }

        public void Start()
        {
            GL.UseProgram(Id);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        /// <summary>
        /// Create the shader program
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        protected bool CreateShaderProgram(string vertex, string fragment)
        {
            // create vertex shader & fragment shader
            int vertexId = CreateShader(ShaderType.VertexShader, vertex);
            int fragmentId = CreateShader(ShaderType.FragmentShader, fragment);

            // check if shader succeeded compiling
            if (fragmentId == 0 || vertexId == 0)
            {
                GL.DeleteShader(vertexId);
                GL.DeleteShader(fragmentId);
                return false;
            }

            // create shader program
            Id = GL.CreateProgram();
            GL.AttachShader(Id, vertexId);
            GL.AttachShader(Id, fragmentId);

            // we can free memory after attachment
            GL.DeleteShader(vertexId);
            GL.DeleteShader(fragmentId);

            // bind the attributes specified by the concrete shader
            BindAttributes();

            GL.LinkProgram(Id);

            // error check
            int statusCode = -1;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out statusCode);

            if (statusCode != 1)
            {
                // get the info log
                string message = GL.GetProgramInfoLog(Id);

                Console.WriteLine(message);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Create a shader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="shaderCode"></param>
        /// <returns></returns>
        private int CreateShader(ShaderType type, string shaderCode)
        {
            int statusCode = -1;

            // create the shader hull
            int id = GL.CreateShader(type);

            // put the shadercode in
            GL.ShaderSource(id, shaderCode);

            // compile the shader
            GL.CompileShader(id);

            // error checking
            GL.GetShader(id, ShaderParameter.CompileStatus, out statusCode);

            if (statusCode != 1)
            {
                // get the info log
                string message = GL.GetShaderInfoLog(id);

                Console.WriteLine(message);

                // show error
                return 0;
            }

            return id;
        }
    }
}
