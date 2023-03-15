using System.Collections.Generic;
using MVI.Binder.Interfaces;

namespace MVI.Binder {

    public class Binder {

        private readonly List<BinderConnection> bindings = new List<BinderConnection>();
        private readonly string name = "";

        public static BinderConnection Connect {
            get {
                return new BinderConnection();
            }
        }

        public static BinderConnection Intercept {
            get {
                return new BinderConnection() {
                    isIntercepted = true
                };
            }
        }

        public Binder(string name = null) {
            this.name = name;

        }

        public void Bind(BinderProducer from, BinderConsumer to) {
            if (from == null || to == null) {
                return;
            }

            BinderConnection connection = new BinderConnection(from, to: to);
            connection.Connect();

            bindings.Add(connection);
        }

        public void Bind(BinderConnection connection) {
            if (connection == null) {
                return;
            }

            connection.name = name;
            connection.Connect();

            bindings.Add(connection);
        }

        public void Destroy() {
            foreach (BinderConnection connection in bindings) {
                connection.Disconnect();
                connection.Destroy();
            }
            bindings.Clear();
        }

    }

}