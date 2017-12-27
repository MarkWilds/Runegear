using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace RuneGear.Graphics.Shaders
{
    public class LitColorShader : ShaderProgram
    {
        private const string vertexSource = @"
        #version 330

        layout (location=0) in vec3 inPosition;
        layout (location=1) in vec3 inDiffuseColor;
        layout (location=2) in vec3 inNormal;

        out vec3 color;
        out vec3 normal;
        uniform mat4 vpMat;

        const vec3 sunPosition = vec3(8192, 12192, -16384);
        const float ambientFactor = 0.3;

        void main()
        {
            float diffuseFactor = 1.0 - ambientFactor + ambientFactor * dot(normalize(inNormal), normalize(sunPosition - inPosition));           

            color = inDiffuseColor * diffuseFactor;
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
            GL.EnableVertexAttribArray(2);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSizeInBytes, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSizeInBytes, Vector3.SizeInBytes);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, vertexSizeInBytes, Vector3.SizeInBytes * 2);
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
