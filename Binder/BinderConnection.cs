using MVI.Binder.Interfaces;

namespace MVI.Binder {

    public class BinderConnection: BinderObserver {
        internal string name;
        internal bool isIntercepted = false;

        internal BinderProducer producer = null;
        internal BinderConsumer consumer = null;

        internal BinderTransformer transformer = null;
        internal BinderInterceptor interceptor = null;

        //statics
        public BinderConnection TransformWith(BinderTransformer tx) {
            transformer = tx;
            return this;
        }

        public BinderConnection InterceptWith(BinderInterceptor itc) {
            interceptor = itc;
            isIntercepted = true;
            return this;
        }

        public static BinderConnection Intercepted() {
            return new BinderConnection() {
                isIntercepted = true
            };
        }

        public static BinderConnection InterceptUsing(BinderTransformer transformer) {
            return new BinderConnection() {
                transformer = transformer,
                isIntercepted = true
            };
        }

        // publics
        public BinderConnection() {
        }

        public BinderConnection(BinderProducer from, BinderConsumer to) {
            producer = from;
            consumer = to;
        }

        public BinderConnection WithIntercept() {
            isIntercepted = true;
            return this;
        }

        public BinderConnection Intercept() {
            isIntercepted = true;
            return this;
        }

        public BinderConnection From(BinderProducer producer) {
            this.producer = producer;
            return this;
        }

        public BinderConnection To(BinderConsumer consumer) {
            this.consumer = consumer;
            return this;
        }

        public void Connect() {
            producer?.Subscribe(this);
        }

        public void Disconnect() {
            producer?.Unsubscribe(this);
        }

        public void Destroy() {
            producer = null;
            consumer = null;
            transformer = null;
            interceptor = null;
        }

        public void OnEvent(object newEvent) {
            if (newEvent != null) {
                object interceptedEvent = isIntercepted ? BinderMiddleware.Shared.Intercept(name, newEvent) : newEvent;
                consumer?.Consume(transformer != null ? transformer.Tranform(interceptedEvent) : interceptedEvent);
            }
        }

        public static BinderConnection operator +(BinderConnection lhs, BinderProducer rhs) {
            lhs.producer = rhs;
            return lhs;
        }

        public static BinderConnection operator -(BinderConnection lhs, BinderConsumer rhs) {
            lhs.consumer = rhs;
            return lhs;
        }

        public static BinderConnection operator |(BinderConnection lhs, BinderConsumer rhs) {
            lhs.consumer = rhs;
            return lhs;
        }

    }


}