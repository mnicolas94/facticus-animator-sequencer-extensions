using System.Threading;
using System.Threading.Tasks;
using BrunoMikoski.AnimationSequencer;

namespace AnimatorSequencerExtensions.Extensions
{
    public static class AnimationSequencerControllerExtensions
    {
        public static async Task PlayAndWaitAsync(this AnimationSequencerController animation, CancellationToken ct)
        {
            animation.Play();
            await animation.PlayingSequence.AsyncWaitForCompletion(ct);
        }
    }
}