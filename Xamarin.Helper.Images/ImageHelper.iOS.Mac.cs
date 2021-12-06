#if __IOS__ || __MACOS__
using CoreGraphics;
using Foundation;
using ImageIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

#if __IOS__
using SkiaSharp.Views.iOS;
using UIKit;
#else
using AppKit;
using UIImage = AppKit.NSImage;
using SkiaSharp.Views.Mac;
#endif



namespace Xamarin.Helper.Images
{

    public static partial class ImageHelper
    {
        /// <summary>
        /// 采样读取网络图片,降低内存<br/>
        /// <see href="https://www.jianshu.com/p/7d8a82115060">参考 iOS性能优化——图片加载和处理</see>
        /// </summary>
        /// <param name="imageUrl">图像http地址</param>
        /// <param name="pointSize"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static UIImage DownSample(string imageUrl, CGSize pointSize, nfloat scale)
        {
            // Go fetch the image from internet.
            byte[] stream = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    stream = webClient.DownloadData(imageUrl);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            var imageData = NSData.FromArray(stream);
            var imageSourceOptions = new CGImageOptions() { ShouldCache = false };
            var imageSource = CGImageSource.FromData(imageData, imageSourceOptions);
            var maxDimensionInPixels = (int)(Math.Max(pointSize.Width, pointSize.Height) * scale);
            var downsampleOptions = new CGImageThumbnailOptions()
            {
                CreateThumbnailFromImageAlways = true,
                ShouldCacheImmediately = true,
                CreateThumbnailWithTransform = true,
                MaxPixelSize = maxDimensionInPixels
            };
            var downsampledImage = imageSource.CreateThumbnail(0, downsampleOptions);
#if __IOS__
            return new UIImage(cgImage: downsampledImage);
#else
            return new UIImage(cgImage: downsampledImage, CGSize.Empty);//Apple:使用CGSIze.Zero会使用原图像大小
#endif
        }

        /// <summary>
        /// 采样读取本地图片,降低内存<br/>
        /// <see href="https://www.jianshu.com/p/7d8a82115060">参考 iOS性能优化——图片加载和处理</see>
        /// </summary>
        /// <param name="imageURL"></param>
        /// <param name="pointSize">?</param>
        /// <param name="scale">?</param>
        /// <returns></returns>
        public static UIImage DownSample(NSUrl imageURL, CGSize pointSize, nfloat scale)
        {
            var imageSourceOptions = new CGImageOptions() { ShouldCache = false };
            var imageSource = CGImageSource.FromUrl(imageURL, imageSourceOptions);
            var maxDimensionInPixels = (int)(Math.Max(pointSize.Width, pointSize.Height) * scale);
            var downsampleOptions = new CGImageThumbnailOptions()
            {
                CreateThumbnailFromImageAlways = true,
                ShouldCacheImmediately = true,
                CreateThumbnailWithTransform = true,
                MaxPixelSize = maxDimensionInPixels
            };
            var downsampledImage = imageSource.CreateThumbnail(0, downsampleOptions);
#if __IOS__
            return new UIImage(cgImage: downsampledImage);
#else
            return new UIImage(cgImage: downsampledImage, CGSize.Empty);//Apple:使用CGSIze.Zero会使用原图像大小
#endif
        }

            /// <summary>
            /// 通过Skiasharp从网络获取图像
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public static UIImage FromUrl(string url)
        {
            using (var image = SKImageHelper.FromUrl(url))
            {
                if (image != null)
                {
#if __IOS__
                    return image.ToUIImage();
#else
                    return image.ToNSImage();
#endif
                }

                return null;
            }
        }

        /// <summary>
        /// 通过Skiasharp从网络获取图像
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestWidth"></param>
        /// <param name="requestHeight"></param>
        /// <returns></returns>
        public static UIImage FromUrl(string url, int requestWidth, int requestHeight)
        {
            using (var image = SKImageHelper.FromUrl(url, requestWidth, requestHeight))
            {
                if (image != null)
                {
#if __IOS__
                    return image.ToUIImage();
#else
                    return image.ToNSImage();
#endif
                }
                return null;
            }
        }

        /// <summary>
        /// 适合在同步方法中自己定义线程调用,带缓存
        /// </summary>
        /// <returns></returns>
        public static UIImage FromUrlAndCache(string url)
        {
            return ImageCache.PublicCache.Load(new NSUrl(url));
        }

        /// <summary>
        /// 从Assets中读取图片
        /// </summary>
        /// <param name="name">ios无需后缀,mac需要后缀</param>
        /// <returns></returns>
        public static UIImage FromAssets(string name)
        {
#if __IOS__
            return UIImage.FromBundle(name);
#else
            return UIImage.ImageNamed(name);//参考微软文档 in app boundle的都能这样使用,(included in resouces)
#endif
        }

        /// <summary>
        /// 从iOS系统自带的SFSymble符号获取图片
        /// </summary>
        /// <param name="name">例如"line.horizontal.3"</param>
        /// <returns></returns>
        public static UIImage FromSFSymble(string name)
        {
#if __IOS__
            return UIImage.GetSystemImage(name);
#else
            return UIImage.GetSystemSymbol(name,null);
#endif

        }

        /// <summary>
        /// 从Resources获取图片
        /// </summary>
        /// <param name="name">mac需要后缀</param>
        /// <returns></returns>
        public static UIImage FromResources(string name)
        {
#if __IOS__
            return UIImage.FromFile(name);
#else
            return UIImage.ImageNamed(name);//微软文档:自动加载二倍图
#endif
        }
    }

    /// <summary>
    /// <see href="https://developer.apple.com/documentation/uikit/views_and_controls/table_views/asynchronously_loading_images_into_table_and_collection_views">TableView load Image ---Apple</see>
    /// </summary>
    public class ImageCache
    {

        public static ImageCache PublicCache = new ImageCache();
        //UIImage placeholderImage = new UIImage("rectangle");
        private Dictionary<NSUrl, UIImage> cachedImages = new Dictionary<NSUrl, UIImage>();
        //private var loadingResponses = [NSURL: [(Item, UIImage ?)->Swift.Void]]();

        public UIImage TryGetImageFromCache(NSUrl url)
        {
            if (cachedImages.ContainsKey(url))
                return cachedImages[url];//可能存的是null
            else return null;
        }
        /// - Tag: cache
        // Returns the cached image if available, otherwise asynchronously loads and caches it.
        public UIImage Load(NSUrl url)
        {
            // Check for a cached image.
            var cachedImage = TryGetImageFromCache(url);//尝试从缓存获取
            if (cachedImage != null)
            {
                return cachedImage;
            }
            // In case there are more than one requestor for the image, we append their completion block.
            /*if loadingResponses[url] != nil {
                loadingResponses[url]?.append(completion)
                return
            }
            else
            {
                loadingResponses[url] = [completion]
            }*/
            // Go fetch the image from internet.
            //ImageURLProtocol.urlSession().dataTask(with: url as URL) {(data, response, error) in
            byte[] stream = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    stream = webClient.DownloadData(url.AbsoluteString);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            // Check for the error, then data and try to create the image.
            //guard let responseData = data, let image = UIImage(data: responseData),
            var image = new UIImage(NSData.FromArray(stream));
            if (image == null)
                return null;
            // Cache the image.
            if (cachedImages.ContainsKey(url))//这里是不知道怎么会有url存在还运行到这里的Bug
                this.cachedImages[url] = image;
            else this.cachedImages.Add(url, image);
            // Iterate over each requestor for the image and pass it back.
            /*for block in blocks {
                    DispatchQueue.main.async {
                        block(item, image)
                    }
                    return;
            }
            }.resume()*/
            return image;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            this.cachedImages.Clear();
        }
    }


}
#endif