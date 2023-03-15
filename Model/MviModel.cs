using System;
using System.Threading.Tasks;
using MVI.Binder;
using MVI.Binder.Interfaces;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace MVI.Model {

    internal class MviModel<MviIntent, MviEvent>: BinderEvent, BinderConsumer {

        internal Guid uuid;

        public virtual void Consume(object newEvent) {
            Task.Run(async () => {
                var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    ConsumeEvent(newEvent);
                });
            });
        }

        internal virtual void ConsumeEvent(object _) {}

        override public void Destroy() {
            base.Destroy();
            
        }

        protected void Enqueue(Action action) {
            queue.Enqueue(action);
        }

    }

}
