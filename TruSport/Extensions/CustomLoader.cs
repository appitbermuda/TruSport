using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TruSport.Extensions
{
    public class CustomLoader : Image
    {
        #region Fields

        private CancellationTokenSource cancellationToken;

        #endregion

        #region Binadables

        public static BindableProperty IsRunningProperty = BindableProperty.Create(
            propertyName: nameof(IsRunning),
            returnType: typeof(bool),
            declaringType: typeof(CustomLoader),
            defaultValue: false);

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }

        public static BindableProperty RotationLengthProperty = BindableProperty.Create(
            propertyName: nameof(RotationLength),
            returnType: typeof(int),
            declaringType: typeof(CustomLoader),
            defaultValue: 2500);

        public int RotationLength
        {
            get { return (int)GetValue(RotationLengthProperty); }
            set { SetValue(RotationLengthProperty, value); }
        }

        public static BindableProperty EasingProperty = BindableProperty.Create(
            propertyName: nameof(Easing),
            returnType: typeof(Easing),
            declaringType: typeof(CustomLoader),
            defaultValue: Easing.CubicInOut);

        public Easing Easing
        {
            get { return (Easing)GetValue(EasingProperty); }
            set { SetValue(EasingProperty, value); }
        }

        #endregion

        #region Constructor(s)

        public CustomLoader()
        {
            Opacity = 0;
        }

        #endregion

        #region Overrides

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsRunningProperty.PropertyName)
            {
                if (IsRunning)
                {
                    this.FadeTo(1);
                    cancellationToken = new CancellationTokenSource();
                    RotateElement(this, cancellationToken.Token);
                }
                else
                {
                    cancellationToken?.Cancel();
                    this.FadeTo(0);
                }
            }
        }

        #endregion

        #region Methods

        private async Task RotateElement(VisualElement element, CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                await element.RotateYTo(90, (uint)RotationLength, this.Easing);
                await element.RotateYTo(270, (uint)RotationLength, this.Easing);
                await element.RotateYTo(360, (uint)RotationLength, this.Easing);
                await element.RotateYTo(0, 0);
            }
        }

        #endregion
    }
}
