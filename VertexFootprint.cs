using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Linq;
namespace SalmonGL
{
    public class VertexFootprint
    {
        public VertexAttribPointerType[] types;
        public int[] counts;
        public string[] names;
        public int size;
        public static Dictionary<VertexAttribPointerType, int> TypeSizes = new Dictionary<VertexAttribPointerType, int> {
            { VertexAttribPointerType.Byte, sizeof(byte)},
            { VertexAttribPointerType.Double, sizeof(double) },
            { VertexAttribPointerType.Float, sizeof(float) },
            { VertexAttribPointerType.Int, sizeof(int) },
            { VertexAttribPointerType.Short, sizeof(short) },
            { VertexAttribPointerType.UnsignedInt, sizeof(uint) },
            { VertexAttribPointerType.UnsignedShort, sizeof(ushort) }
        };
        private static Dictionary<char, VertexAttribPointerType> _charTypes = new Dictionary<char, VertexAttribPointerType>
        {
            {'b', VertexAttribPointerType.Byte},
            {'d', VertexAttribPointerType.Double},
            {'f', VertexAttribPointerType.Float},
            {'i', VertexAttribPointerType.Int},
            {'s', VertexAttribPointerType.Short},
            {'I', VertexAttribPointerType.UnsignedInt},
            {'S', VertexAttribPointerType.UnsignedShort},
        };
        // Processes a string of the form [type][count]_[location] x n
        public VertexFootprint(string str)
        {
            var types = new List<VertexAttribPointerType>();
            var counts = new List<int>();
            var names = new List<string>();
            
            foreach (string s in str.Split())
            {
                types.Add(_charTypes[s[0]]);
                string[] cl = s.Substring(1).Split('-');
                counts.Add(int.Parse(cl[0]));
                if (cl.Length > 1)
                    names.Add(cl[1]);
                else
                    throw new ArgumentException("Type not followed by attribute name");                                       
            }
            
            for(int i = 0; i < types.Count; i++)
            {
                size += TypeSizes[types[i]] * counts[i];
            }
            this.types = types.ToArray();
            this.counts = counts.ToArray();
            this.names = names.ToArray();
        }
        public VertexFootprint(VertexAttribPointerType[] types, int[] counts, string[] names)
        {            
            if (types.Length != counts.Length || types.Length != names.Length)
                throw new ArgumentException("There must be a matching type for each count and name");
            
            this.types = types;
            this.counts = counts;
            this.names = names;
        }        
    }
}
