using System.Collections.Generic;
using MVI.Binder.Interfaces;

namespace MVI.Binder {
    public class BinderMiddleware: BinderInterceptor {

        private static BinderMiddleware instance = null;
        private static readonly List<BinderInterceptor> middlewares = new List<BinderInterceptor>();

        public static BinderMiddleware Shared {
            get {
                if (instance == null) {
                    instance = new BinderMiddleware();
                }
                return instance;
            }
        }

        private BinderMiddleware() {
        }

        public void Register(BinderInterceptor middleware) {
            middlewares.Add(middleware);
        }

        public object Intercept(string name, object newEvent) {
            var interceptedEvent = newEvent;
            foreach (BinderInterceptor middleware in middlewares) {
                var resultEvent = middleware.Intercept(name, newEvent);
                if (resultEvent != null) {
                    interceptedEvent = resultEvent;
                }
            }
            return interceptedEvent;
        }
    }

}