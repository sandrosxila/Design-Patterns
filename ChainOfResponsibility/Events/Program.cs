using System;
using System.Threading;

namespace Events
{
    public class Video
    {
        public string Title { get; set; }
    }

    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }

    public class VideoEncoder
    {
        // public delegate void VideoEncodedEventHandler(object source, VideoEventArgs args);
        // public event VideoEncodedEventHandler VideoEncoded;

        public string Name { get; }

        public VideoEncoder()
        {
            Name = "Video Encoder";
        }
        
        public event EventHandler<VideoEventArgs> VideoEncoded;

        public void Encode(Video video)
        {
            Console.WriteLine($"Encoding {video.Title}");
            Thread.Sleep(3000);
            OnVideoRecorded(video);
        }

        protected virtual void OnVideoRecorded(Video video)
        {
            if (VideoEncoded != null)
                VideoEncoded(this, new VideoEventArgs() {Video = video});

            // that's same: VideoEncoded?.Invoke(this, new VideoEventArgs(){Video = video});
        }
    }

    public class MailService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine($"{((dynamic)source).Name} has ended encoding.");
            Console.WriteLine($"Mail Service is Sending an E-mail... ({args.Video.Title})");
        }
    }

    public class MessageService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine($"{((VideoEncoder)source).Name} has ended encoding.");
            Console.WriteLine($"Message Service is Sending a text message... ({args.Video.Title})");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var video = new Video() {Title = "Video 1"};
            var videoEncoder = new VideoEncoder(); //publisher
            var mailService = new MailService(); // subscriber
            var messageService = new MessageService(); // subscriber

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded;

            videoEncoder.Encode(video);
        }
    }
}