using System;
using System.Collections.Generic;
using MVI.Binder.Interfaces;
using MVI.Utils;

namespace MVI.Binder {

    public class BinderEvent: BinderProducer {

        private readonly List<BinderObserver> listeners = new List<BinderObserver>();

        internal readonly TaskQueue queue = new TaskQueue();

        public BinderEvent() {
        }

        public void Produce(object newEvent) {
            queue.Enqueue(() => {
                callListeners(newEvent);
            });
        }

        public virtual void Destroy() {
            listeners.Clear();
            queue.Shutdown();
        }

        #region BinderProducer
        public void Subscribe(BinderObserver observer) {
            if (!listeners.Contains(observer)) {
                listeners.Add(observer);
            }
        }

        public void Unsubscribe(BinderObserver observer) {
            if (listeners.Contains(observer)) {
                listeners.Remove(observer);
            }
        }
        #endregion

        private void callListeners(object newEvent) {
            var list = new List<BinderObserver>(listeners);
            foreach (BinderObserver listener in list) {
                try {
                    listener.OnEvent(newEvent);
                } catch (Exception) {
                }
            }
        }
    }

}
