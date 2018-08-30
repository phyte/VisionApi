using System;
using System.Collections.Generic;
using System.Linq;
using BMVisionApiClient.VisionAPI.Models.Response;
using BMVisionApiClient.VisionAPI.Models.Response.OCR;

namespace BMVisionApiClient.VisionAPI.Output
{
    public static class ConsoleWriter
    {
        public static void Write(AnalyzeResponseObject result)
        {
            PrintResult(result);
        }

        public static void Write(OCRResponseObject result)
        {
            PrintOCRResult(result);
        }

        private static void PrintOCRResult(OCRResponseObject result)
        {
            PrintOCRMetadata(result);
            PrintWords(result.Regions);
        }

        private static void PrintOCRMetadata(OCRResponseObject result)
        {
            Console.WriteLine($"# Metadata");
            Console.WriteLine($" Language: {result.Language}");
            Console.WriteLine($" Orientation: {result.Orientation}");
            Console.WriteLine($" TextAngle: {result.TextAngle}");
            Console.WriteLine();
        }

        private static void PrintWords(List<Region> regions)
        {
            Console.WriteLine($"# Regions");
            foreach (var region in regions)
            {
                foreach (var line in region.Lines)
                {
                    var lineText = line.Words.Aggregate("", (current, word) => current + word.Text + " ");
                    Console.WriteLine($"{lineText}");
                }
            }
            Console.WriteLine();
        }

        private static void PrintResult(AnalyzeResponseObject result)
        {
            PrintRequestId(result.RequestId);
            PrintMetadata(result.Metadata);
            PrintCategories(result.Categories);
            PrintAdult(result.Adult);
            PrintTags(result.Tags);
            PrintDescription(result.Description);
            PrintFaces(result.Faces);
            PrintColor(result.Color);
            PrintImageType(result.ImageType);
        }

        private static void PrintRequestId(string requestId)
        {
            Console.WriteLine($"# RequestId");
            Console.WriteLine($" {requestId}");
            Console.WriteLine();
        }

        private static void PrintMetadata(Metadata metadata)
        {
            Console.WriteLine($"# Metadata");
            Console.WriteLine($" {metadata.Width}x{metadata.Height} ({metadata.Format})");
            Console.WriteLine();
        }

        private static void PrintCategories(List<Category> categories)
        {
            Console.WriteLine($"# Categories");
            foreach (var category in categories)
            {
                Console.WriteLine($" {category.Name} ({category.Score})");
                if (category.Detail != null)
                {
                    Console.WriteLine($" ## Detail");
                    Console.WriteLine($" ### Celebrities");
                    foreach (var celebrity in category.Detail?.Celebrities)
                    {
                        Console.WriteLine(
                            $"   {celebrity.Name} ({celebrity.Confidence}) [{celebrity.FaceRectangle.Top}|{celebrity.FaceRectangle.Left}|{celebrity.FaceRectangle.Width}|{celebrity.FaceRectangle.Height}]");
                    }
                    Console.WriteLine($" ### Landmarks");
                    foreach (var landmark in category.Detail?.Landmarks)
                    {
                        Console.WriteLine($"   {landmark.Name} ({landmark.Confidence})");
                    }
                }
            }
            Console.WriteLine();
        }

        private static void PrintAdult(Adult adult)
        {
            Console.WriteLine($"# Adult");
            Console.WriteLine($" AdultContent: {adult.IsAdultContent} ({adult.AdultScore})");
            Console.WriteLine($" RacyContent: {adult.IsRacyContent} ({adult.RacyScore})");
            Console.WriteLine();
        }

        private static void PrintTags(List<Tag> tags)
        {
            Console.WriteLine($"# Tags");
            foreach (var tag in tags)
            {
                Console.WriteLine($" {tag.Name} ({tag.Confidence})");
            }
            Console.WriteLine();
        }
        private static void PrintDescription(Description description)
        {
            Console.WriteLine($"# Description");
            Console.WriteLine($" ## Tags");
            foreach (var tag in description.Tags)
            {
                Console.WriteLine($"  {tag}");
            }
            Console.WriteLine($" ## Captions");
            foreach (var caption in description.Captions)
            {
                Console.WriteLine($"  {caption.Text} ({caption.Confidence})");
            }
            Console.WriteLine();
        }
        private static void PrintFaces(List<Face> faces)
        {
            Console.WriteLine($"# Faces");
            foreach (var face in faces)
            {
                Console.WriteLine($" ## Face");
                Console.WriteLine($" Age: {face.Age}");
                Console.WriteLine($" Gender: {face.Gender}");
                Console.WriteLine($" Rectangle: [{face.FaceRectangle.Top}|{face.FaceRectangle.Left}|{face.FaceRectangle.Width}|{face.FaceRectangle.Height}]");
            }
            Console.WriteLine();
        }

        private static void PrintColor(Color color)
        {
            Console.WriteLine($"# Color");
            Console.WriteLine($" DominantColorForeground: {color.DominantColorForeground}");
            Console.WriteLine($" DominantColorBackground: {color.DominantColorBackground}");
            Console.WriteLine($"# Color");
            Console.WriteLine($" ## DominantColors");
            foreach (var domianntColor in color.DominantColors)
            {
                Console.WriteLine($"  {domianntColor}");
            }
            Console.WriteLine($" AccentColor: {color.AccentColor}");
            Console.WriteLine($" IsBWImg: {color.IsBWImg}");
            Console.WriteLine();
        }

        private static void PrintImageType(ImageType imageType)
        {
            Console.WriteLine($"# ImageType");
            Console.WriteLine($" ClipArtType: {imageType.ClipArtType}");
            Console.WriteLine($" LineDrawingType: {imageType.LineDrawingType}");
            Console.WriteLine();
        }
    }
}
