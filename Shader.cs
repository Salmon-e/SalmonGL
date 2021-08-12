using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.IO;
namespace SalmonGL
{
    public class Shader : IDisposable
    {
        
        public int Program;
        public Shader(string vertPath, string fragPath)
        {
            string vertSource;
            string fragSource;
            using (var reader = new StreamReader(vertPath, Encoding.UTF8))
                vertSource = reader.ReadToEnd();
            using (var reader = new StreamReader(fragPath, Encoding.UTF8))
                fragSource = reader.ReadToEnd();

            Compile(vertSource, fragSource);
        }
        protected void Compile(string vertSource, string fragSource)
        {
            int vert = GL.CreateShader(ShaderType.VertexShader);
            int frag = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vert, vertSource);
            GL.ShaderSource(frag, fragSource);

            GL.CompileShader(vert);
            GL.CompileShader(frag);
            Console.WriteLine($"{GL.GetShaderInfoLog(vert)}\n\n{GL.GetShaderInfoLog(frag)}");
            Program = GL.CreateProgram();
            GL.AttachShader(Program, vert);
            GL.AttachShader(Program, frag);

            GL.LinkProgram(Program);

            GL.DetachShader(Program, vert);
            GL.DetachShader(Program, frag);
            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }
        public void SetUniform(string name, object value)
        {
            int location = GL.GetUniformLocation(Program, name);
            if(value is double) GL.ProgramUniform1(Program, location, (double)value);            
            if(value is float) GL.ProgramUniform1(Program, location, (float)value);              
            if (value is int) GL.ProgramUniform1(Program, location, (int)value);
            if (value is short) GL.ProgramUniform1(Program, location, (short)value);
            if (value is byte) GL.ProgramUniform1(Program, location, (byte)value);
            if (value is ushort) GL.ProgramUniform1(Program, location, (ushort)value);
            if (value is uint) GL.ProgramUniform1(Program, location, (uint)value);
            if (value is sbyte) GL.ProgramUniform1(Program, location, (sbyte)value);

            if (value is Vector2) GL.ProgramUniform2(Program, location, (Vector2)value);
            if (value is Vector2i) GL.ProgramUniform2(Program, location, (Vector2i)value);

            if (value is Vector3) GL.ProgramUniform3(Program, location, (Vector3)value);
            if (value is Vector3i) GL.ProgramUniform3(Program, location, (Vector3i)value);

            if (value is Vector4) GL.ProgramUniform4(Program, location, (Vector4)value);
            if (value is Vector4i) GL.ProgramUniform4(Program, location, (Vector4i)value);

            if (value is Matrix2) 
            {
                Matrix2 v = (Matrix2)value;
                GL.ProgramUniformMatrix2(Program, location, false, ref v);                    
            }
            if (value is Matrix3)
            {
                Matrix3 v = (Matrix3)value;
                GL.ProgramUniformMatrix3(Program, location, false, ref v);
            }
            if (value is Matrix4)
            {
                Matrix4 v = (Matrix4)value;
                GL.ProgramUniformMatrix4(Program, location, false, ref v);
            }  
            
            if(value is Texture2D)
            {
                Texture2D tex = (Texture2D)value;
                tex.Enable(tex.currentUnit);
                GL.ProgramUniform1(Program, location, (int)tex.currentUnit - (int) TextureUnit.Texture0);
            }
        }
        public void Enable()
        {
            GL.UseProgram(Program);
        }
        public void Dispose()
        {            
            GL.DeleteProgram(Program);
        }
    }
}
