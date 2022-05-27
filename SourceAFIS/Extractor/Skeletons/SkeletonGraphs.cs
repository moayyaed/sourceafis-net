// Part of SourceAFIS for .NET: https://sourceafis.machinezoo.com/net
using SourceAFIS.Features;
using SourceAFIS.Primitives;

namespace SourceAFIS.Extractor.Skeletons
{
    static class SkeletonGraphs
    {
        public static Skeleton Create(BooleanMatrix binary, SkeletonType type)
        {
            // https://sourceafis.machinezoo.com/transparency/binarized-skeleton
            FingerprintTransparency.Current.Log(type.Prefix() + "binarized-skeleton", binary);
            var thinned = BinaryThinning.Thin(binary, type);
            var skeleton = SkeletonTracing.Trace(thinned, type);
            SkeletonFilters.Apply(skeleton);
            return skeleton;
        }
    }
}
