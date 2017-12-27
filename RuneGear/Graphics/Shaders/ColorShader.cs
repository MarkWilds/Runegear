using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RuneGear.Graphics.Shaders
{
    public class ColorShader : ShaderProgram
    {
        private const string vertexSource = @"
        #version 330

        layout (location=0) in vec3 inPosition;
        layout (location=1) in vec3 inVColor;

        out vec3 color;
        uniform mat4 vpMat;

        void main()
        {
            color = inVColor;
            gl_Position = vpMat * vec4(inPosition, 1.0);
        }";

        private const string fragmentSource = @"
        #version 330

        in vec3 color;
        out vec4 outColor;

        void main()
        {
            outColor = vec4(color, 1.0);
        }";

        private int viewProjectionMatrixLocation;

        protected override void BindAttributes()
        {
        }

        public override void SpecifyAttributes(int vertexSizeInBytes)
        {
            // set attrib pointers
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSizeInBytes, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSizeInBytes, Vector3.SizeInBytes);
        }

        public override void Create()
        {
            CreateShaderProgram(vertexSource, fragmentSource);
            Start();
            viewProjectionMatrixLocation = GL.GetUniformLocation(Id, "vpMat");
            Stop();
        }

        public void SetViewProjectionMatrix(Matrix4 matrix)
        {
            GL.UniformMatrix4(viewProjectionMatrixLocation, false, ref matrix);
        }
    }
}
