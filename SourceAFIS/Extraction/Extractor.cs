using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SourceAFIS.General;
using SourceAFIS.Meta;
using SourceAFIS.Visualization;

namespace SourceAFIS.Extraction
{
    public sealed class Extractor
    {
        [DpiAdjusted]
        public int BlockSize = 16;

        public DpiAdjuster DpiAdjuster = new DpiAdjuster();
        [Nested]
        public LocalHistogram Histogram = new LocalHistogram();
        [Nested]
        public SegmentationMask Mask = new SegmentationMask();
        [Nested]
        public Equalizer Equalizer = new Equalizer();
        [Nested]
        public HillOrientation Orientation = new HillOrientation();
        [Nested]
        public OrientedSmoother RidgeSmoother = new OrientedSmoother();

        public void Extract(byte[,] invertedImage, int dpi)
        {
            DpiAdjuster.Adjust(this, dpi, delegate()
            {
                byte[,] image = GrayscaleInverter.GetInverted(invertedImage);

                BlockMap blocks = new BlockMap(new Size(image.GetLength(1), image.GetLength(0)), BlockSize);
                Logger.Log(this, "BlockMap", blocks);

                short[, ,] histogram = Histogram.Analyze(blocks, image);
                short[, ,] smoothHistogram = Histogram.SmoothAroundCorners(blocks, histogram);

                BinaryMap mask = Mask.ComputeMask(blocks, histogram);

                float[,] equalized = Equalizer.Equalize(blocks, image, smoothHistogram, mask);
                byte[,] orientation = Orientation.Detect(equalized, mask, blocks);

                float[,] smoothed = RidgeSmoother.Smooth(equalized, orientation, mask, blocks);
            });
        }
    }
}
