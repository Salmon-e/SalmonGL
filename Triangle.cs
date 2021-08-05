using System;
using System.Collections.Generic;
using System.Text;

namespace SalmonGL
{
    public struct Triangle<T>
    {
        public T[] Vertices;
        public Triangle(T v1, T v2, T v3)
        {
            Vertices = new T[]{ v1, v2, v3};
        }
    }
}
