using System;

namespace MVI.Binder.Interfaces {

    public interface BinderConsumer {
        void Consume(object newEvent);
    }

}