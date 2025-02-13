using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment
{
    internal class WatermarkDecorator : DocumentDecorator
    {
        private string watermark;

        public WatermarkDecorator(Document document, string watermark) : base(document)
        {
            this.watermark = watermark;
        }

        public override void SetHeader(string watermark, User user)
        {
            string watermarkedHeader = "\n" + GenerateWatermark();
            document.SetHeader(watermarkedHeader, user);
            Console.WriteLine("Added Watermark to document header.");
        }

        private string GenerateWatermark()
        {
            string watermarkPattern = $"~~~ {watermark} ~~~";
            string repeatedWatermark = string.Join("\n", Enumerable.Repeat(watermarkPattern, 3));
            return repeatedWatermark;
        }
    }

}
