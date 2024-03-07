
#if ANDROID
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#endif
using R3;

namespace Bug21068
{
    public class MainPageViewModel : IDisposable
    {
        public BindableReactiveProperty<double> Rotation { get; }

        public ReactiveCommand<Unit> Rotate90ClockwiseCommand { get; }

        public ReactiveCommand<Unit> Rotate90CounterClockwiseCommand { get; }

        public MainPageViewModel()
        {
            Rotation = new BindableReactiveProperty<double>(90);

            Rotate90ClockwiseCommand = new ReactiveCommand<Unit>();
            Rotate90ClockwiseCommand.Subscribe(_ =>
            {
                Rotation.Value += 90;
            });

            Rotate90CounterClockwiseCommand = new ReactiveCommand<Unit>();
            Rotate90CounterClockwiseCommand.Subscribe(_ =>
            {
                Rotation.Value -= 90;
            });
        }

        public void Dispose()
        {
            Disposable.Dispose(Rotation, Rotate90ClockwiseCommand, Rotate90CounterClockwiseCommand);
        }
    }

    public class InitalRotationImage : Image
    {
        public InitalRotationImage()
        {
        }

#if ANDROID
#nullable enable
        public static void MapBackPivot(IViewHandler handler, IView view)
        {
            (handler.PlatformView as Android.Views.View)?.ResetPivot(view);
        }

        internal static void RemapForControls()
        {
            ImageHandler.Mapper.AppendToMapping<InitalRotationImage, IViewHandler>(nameof(IView.Height), MapBackPivot);
            ImageHandler.Mapper.AppendToMapping<InitalRotationImage, IViewHandler>(nameof(IView.Width), MapBackPivot);
        }
#endif
    }

#if ANDROID
    public static class ViewExtentions
    {
        public static void ResetPivot(this Android.Views.View platformView, IView view)
        {
            var pivotX = (float)(view.AnchorX * platformView.ToPixels(view.Frame.Width));
            var pivotY = (float)(view.AnchorY * platformView.ToPixels(view.Frame.Height));
            platformView.PivotX = pivotX;
            platformView.PivotY = pivotY;
        }
    }

    public static class ContextExtensions
    {
        public static float ToPixels(this Android.Views.View view, double dp)
        {
            return view.Context.ToPixels(dp);
        }
    }

#endif

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }
    }

}
