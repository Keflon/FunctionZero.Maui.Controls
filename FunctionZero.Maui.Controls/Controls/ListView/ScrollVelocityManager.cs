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
        record struct DataPoint(double delta, long elapsedMilliseconds);

        private readonly Queue<DataPoint> _velocityQueue;
        double _totalY = 0;
        private double _startScrollOffset;
        double _lastScrollOffset;
        private long _lastElapsedMilliseconds;

        public ScrollVelocityManager(int qDepth)
        {
            _qDepth = qDepth;

            _sw = new();
            _velocityQueue = new(qDepth);
        }

        internal void Start(double scrollOffset)
        {
            _startScrollOffset = scrollOffset;
            _lastScrollOffset = scrollOffset;
            _lastElapsedMilliseconds = 0;
            Stop();
            _sw.Start();
        }

        internal void StoreDataPoint(double scrollOffset)
        {
            //Debug.WriteLine($"OFFSET:{scrollOffset},DELTA:{scrollOffset - _lastScrollOffset}");

            DataPoint nextDataPoint;
            var elapsedMilliseconds = _sw.ElapsedMilliseconds;

            if (_velocityQueue.Count == _qDepth)
            {
                nextDataPoint = _velocityQueue.Dequeue();
                nextDataPoint.delta = scrollOffset - _lastScrollOffset;
                nextDataPoint.elapsedMilliseconds = elapsedMilliseconds;
            }
            else
            {
                nextDataPoint = new DataPoint(scrollOffset - _lastScrollOffset, elapsedMilliseconds);
            }

            _velocityQueue.Enqueue(nextDataPoint);

            _lastScrollOffset = scrollOffset;
            _lastElapsedMilliseconds = elapsedMilliseconds;
        }

        internal double GetVelocity(uint millisecondRate)
        {
            //double timeDelta = _sw.Elapsed.TotalMilliseconds;
            var totalElapsedMilliseconds = _sw.ElapsedMilliseconds;

            long timeAnchor = 0;

            int validcount = 0;
            double totalDelta = 0;

            foreach (var item in _velocityQueue)
            {
                if (item.elapsedMilliseconds > (totalElapsedMilliseconds - 100))
                {
                    var timeDelta = item.elapsedMilliseconds - timeAnchor;
                    if (timeDelta != 0)
                    {
                        double animationDelta = (item.delta / timeDelta);
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
