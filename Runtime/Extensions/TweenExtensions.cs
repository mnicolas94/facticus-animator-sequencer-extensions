using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

namespace AnimatorSequencerExtensions.Extensions
{
    public static class TweenExtensions
    {
        public static async Task AsyncWaitForCompletion(this Tween t, CancellationToken ct)
        {
            if (!t.active)
            {
                return;
            }

            while (t.active && !t.IsComplete() && !ct.IsCancellationRequested)
            {
                await Task.Yield();
            }

            if (ct.IsCancellationRequested)
            {
                t.Complete(true);
            }
        }
    }
}