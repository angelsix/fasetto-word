using Fasetto.Word.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Fasetto.Word
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimations
    {
        #region Slide In / Out

        /// <summary>
        /// Slides an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="direction">The direction of the slide</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="size">The animation width/height to animate to. If not specified the elements size is used</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInAsync(this FrameworkElement element, AnimationSlideInDirection direction, bool firstLoad, float seconds = 0.3f, bool keepMargin = true, int size = 0)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Slide in the correct direction
            switch (direction)
            {
                // Add slide from left animation
                case AnimationSlideInDirection.Left:
                    sb.AddSlideFromLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // Add slide from right animation
                case AnimationSlideInDirection.Right:
                    sb.AddSlideFromRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // Add slide from top animation
                case AnimationSlideInDirection.Top:
                    sb.AddSlideFromTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                // Add slide from bottom animation
                case AnimationSlideInDirection.Bottom:
                    sb.AddSlideFromBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
            }
            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0 || firstLoad)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides an element out
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="direction">The direction of the slide (this is for the reverse slide out action, so Left would slide out to left)</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="size">The animation width/height to animate to. If not specified the elements size is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutAsync(this FrameworkElement element, AnimationSlideInDirection direction, float seconds = 0.3f, bool keepMargin = true, int size = 0)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Slide in the correct direction
            switch (direction)
            {
                // Add slide to left animation
                case AnimationSlideInDirection.Left:
                    sb.AddSlideToLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // Add slide to right animation
                case AnimationSlideInDirection.Right:
                    sb.AddSlideToRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // Add slide to top animation
                case AnimationSlideInDirection.Top:
                    sb.AddSlideToTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                // Add slide to bottom animation
                case AnimationSlideInDirection.Bottom:
                    sb.AddSlideToBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
            }

            // Add fade in animation
            sb.AddFadeOut(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));

            // Make element invisible
            if (element.Opacity == 0)
                element.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Fade In / Out

        /// <summary>
        /// Fades an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task FadeInAsync(this FrameworkElement element, bool firstLoad, float seconds = 0.3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add fade in animation
            sb.AddFadeIn(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0 || firstLoad)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Fades out an element
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="firstLoad">Indicates if this is the first load</param>
        /// <returns></returns>
        public static async Task FadeOutAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Add fade in animation
            sb.AddFadeOut(seconds);

            // Start animating
            sb.Begin(element);

            // Make page visible only if we are animating or its the first load
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));

            // Fully hide the element
            element.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Marquee

        /// <summary>
        /// Animates a marquee style element
        /// The structure should be:
        /// [Border ClipToBounds="True"]
        ///   [Border local:AnimateMarqueeProperty.Value="True"]
        ///      [Content HorizontalAlignment="Left"]
        ///   [/Border]
        /// [/Border]
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static void MarqueeAsync(this FrameworkElement element, float seconds = 3f)
        {
            // Create the storyboard
            var sb = new Storyboard();

            // Run until element is unloaded
            var unloaded = false;

            // Monitor for element unloading
            element.Unloaded += (s, e) => unloaded = true;

            // Run a loop off the caller thread
            IoC.Task.Run(async () =>
            {
                // While the element is still available, recheck the size
                // after every loop in case the container was resized
                while (element != null && !unloaded)
                {
                    // Create width variables
                    var width = 0d;
                    var innerWidth = 0d;

                    try
                    {
                        // Check if element is still loaded
                        if (element == null || unloaded)
                            break;

                        // Try and get current width
                        width = element.ActualWidth;
                        innerWidth = ((element as Border).Child as FrameworkElement).ActualWidth;
                    }
                    catch
                    {
                        // Any issues then stop animating (presume element destroyed)
                        break;
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Add marquee animation
                        sb.AddMarquee(seconds, width, innerWidth);

                        // Start animating
                        sb.Begin(element);

                        // Make page visible
                        element.Visibility = Visibility.Visible;
                    });

                    // Wait for it to finish animating
                    await Task.Delay((int)seconds * 1000);

                    // If this is from first load or zero seconds of animation, do not repeat
                    if (seconds == 0)
                        break;
                }
            });
        }

        #endregion
    }
}
