#nullable enable

using System;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Yang.Maui.Helper.Skia.SKGpuView
{
    /// <summary>
    /// Windows use SKXamlCanvas, no gpu.
    /// iOS use SKGLView or SKMetalView.
    /// Android use SKGLSurfaceView or SKGLTextureView.
    /// </summary>
    public partial class SKGpuView : View, ISKGpuView
    {
        public static readonly BindableProperty HasRenderLoopProperty =
            BindableProperty.Create(nameof(HasRenderLoop), typeof(bool), typeof(SKGpuView), false);

        public static readonly BindableProperty EnableTouchEventsProperty =
            BindableProperty.Create(nameof(EnableTouchEvents), typeof(bool), typeof(SKGpuView), false);
         
        public static readonly BindableProperty EnableTransparentProperty =
            BindableProperty.Create(nameof(EnableTransparent), typeof(bool), typeof(SKGpuView), false);

        public bool HasRenderLoop
        {
            get { return (bool)GetValue(HasRenderLoopProperty); }
            set { SetValue(HasRenderLoopProperty, value); }
        }

        public bool EnableTouchEvents
        {
            get { return (bool)GetValue(EnableTouchEventsProperty); }
            set { SetValue(EnableTouchEventsProperty, value); }
        }
        
        public bool EnableTransparent
        {
            get { return (bool)GetValue(EnableTransparentProperty); }
            set { SetValue(EnableTransparentProperty, value); }
        }

        // the user can subscribe to repaint
        public event EventHandler<SKPaintGpuSurfaceEventArgs>? PaintSurface;

        // the user can subscribe to touch events
        public event EventHandler<SKTouchEventArgs>? Touch;


        event EventHandler ISKGpuView.SurfaceInvalidated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<GetPropertyValueEventArgs<SKSize>> ISKGpuView.GetCanvasSize
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<GetPropertyValueEventArgs<GRContext>> ISKGpuView.GetGRContext
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        // the user asks the for the size
        public SKSize CanvasSize
        {
            get
            {
                // send a mesage to the native view
                var args = new GetPropertyValueEventArgs<SKSize>();
                Handler?.Invoke(nameof(ISKGpuView.GetCanvasSize), args);
                return args.Value;
            }
        }

        // the user asks the for the current GRContext
        public GRContext GRContext
        {
            get
            {
                // send a mesage to the native view
                var args = new GetPropertyValueEventArgs<GRContext>();
                Handler?.Invoke(nameof(ISKGpuView.GetGRContext), args);
                return args.Value;
            }
        }

        // the user asks to repaint
        public void InvalidateSurface()
        {
            // send a mesage to the native view
            Handler?.Invoke(nameof(ISKGpuView.SurfaceInvalidated), EventArgs.Empty);
        }

        // the native view tells the user to repaint
        protected virtual void OnPaintSurface(SKPaintGpuSurfaceEventArgs e)
        {
            PaintSurface?.Invoke(this, e);
        }

        // the native view responds to a touch
        protected internal virtual void OnTouch(SKTouchEventArgs e)
        {
            Touch?.Invoke(this, e);
        }

        void ISKGpuView.OnPaintSurface(SKPaintGpuSurfaceEventArgs e)
        {
            OnPaintSurface(e);
        }

        void ISKGpuView.OnTouch(SKTouchEventArgs e)
        {
            OnTouch(e);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(40.0, 40.0));
        }

        public virtual Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            return Size.Zero;
        }
    }

    public interface ISKGpuView : IView
    {
        event EventHandler SurfaceInvalidated;
        event EventHandler<GetPropertyValueEventArgs<SKSize>> GetCanvasSize;
        event EventHandler<GetPropertyValueEventArgs<GRContext>> GetGRContext;

        // the native view tells the user to repaint
        void OnPaintSurface(SKPaintGpuSurfaceEventArgs e);

        // the native view responds to a touch
        void OnTouch(SKTouchEventArgs e);
    }
}