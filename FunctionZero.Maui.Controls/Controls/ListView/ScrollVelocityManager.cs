using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class ScrollVelocityManager
    {
        private readonly int _qDepth;
        Stopwatch _sw;

        record DataPoint(float delta, long elapsedMilliseconds);

        private readonly Queue<DataPoint> _velocityQueue;
        double _totalY = 0;
        private float _startScrollOffset;
        float _lastScrollOffset;
        private long _lastElapsedMilliseconds;

        public ScrollVelocityManager(int qDepth)
        {
            _qDepth = qDepth;

            _sw = new();
            _velocityQueue = new(qDepth);
        }

        internal void Start(float scrollOffset)
        {
            _startScrollOffset = scrollOffset;
            _lastScrollOffset = scrollOffset;
            _lastElapsedMilliseconds = 0;
            Stop();
            _sw.Start();
        }

        internal void StoreDataPoint(float scrollOffset)
        {
            //Debug.WriteLine($"OFFSET:{scrollOffset},DELTA:{scrollOffset - _lastScrollOffset}");

            if (_velocityQueue.Count == _qDepth)
                _velocityQueue.Dequeue();

            var elapsedMilliseconds = _sw.ElapsedMilliseconds;

            // TODO: Pre-allocate all DataPoints and re-use!
            _velocityQueue.Enqueue(
                new DataPoint(
                    scrollOffset - _lastScrollOffset,
                    elapsedMilliseconds// - _lastElapsedMilliseconds
                )
            );

            _lastScrollOffset = scrollOffset;
            _lastElapsedMilliseconds = elapsedMilliseconds;
        }

        internal double GetVelocity(uint millisecondRate)
        {
            //double timeDelta = _sw.Elapsed.TotalMilliseconds;
            var totalElapsedMilliseconds = _sw.ElapsedMilliseconds;

            long timeAnchor = 0;

            int validcount = 0;
            float totalDelta = 0;

            foreach (var item in _velocityQueue)
            {
                //if (item.elapsedMilliseconds > (totalElapsedMilliseconds - 150))
                {
                    var timeDelta = item.elapsedMilliseconds - timeAnchor;
                    if (timeDelta != 0)
                    {
                        float animationDelta = (item.delta / timeDelta);
                        var positiveDelta = Math.Abs(animationDelta);

                        validcount++;
                        totalDelta += positiveDelta;

                        Debug.WriteLine($"{validcount} - {totalDelta} - {animationDelta} {positiveDelta}");
                    }
				}

				timeAnchor = item.elapsedMilliseconds;

            }
            if (validcount >= 2)
            {
                var result = totalDelta / validcount * millisecondRate;
                if (_startScrollOffset > _lastScrollOffset)
                    return -result;

                return result;
            }

            return 0;
        }

        internal void Stop()
        {
            _sw.Reset();
            _velocityQueue.Clear();
        }
    }
}
