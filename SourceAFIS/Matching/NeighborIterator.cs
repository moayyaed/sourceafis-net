using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SourceAFIS.General;
using SourceAFIS.Meta;
using SourceAFIS.Extraction.Templates;

namespace SourceAFIS.Matching
{
    public sealed class NeighborIterator
    {
        [DpiAdjusted]
        [Parameter(Lower = 30, Upper = 1500)]
        public int MaxDistance = 150;

        public IEnumerable<int> GetNeighbors(Template template, int minutia)
        {
            Point center = template.Minutiae[minutia].Position;
            for (int i = 0; i < template.Minutiae.Length; ++i)
                if (i != minutia && Calc.DistanceSq(center, template.Minutiae[i].Position) <= Calc.Sq(MaxDistance))
                    yield return i;
        }
    }
}
