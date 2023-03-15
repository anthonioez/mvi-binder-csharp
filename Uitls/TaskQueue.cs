using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MVI.Utils {

    internal class TaskQueue {
        private readonly Queue<Action> tasks = new Queue<Action>();
        private Task currentTask;

        private TaskFactory runner;

        internal CancellationTokenSource TokenSource;
        internal Action Completion;

        public TaskQueue() {
            TokenSource = new CancellationTokenSource();
            runner = new TaskFactory(TokenSource.Token);
        }

        public Task Execute(Action action) {
            return runner.StartNew(action);
        }

        public void Enqueue(Action action) {
            if (action == null) {
                return;
            }

            lock (tasks) {
                if (currentTask == null) {
                    taskStart(action);
                } else {
                    tasks.Enqueue(action);
                }
            }
        }

        public void Shutdown() {
            Completion = null;
            if (TokenSource != null) {
                TokenSource.Cancel(true);
                TokenSource = null;
            }

            runner = null;
        }


        private void taskStart(Action nextItem) {
            if (runner == null) {
                return;
            }

            currentTask = runner.StartNew(nextItem);
            currentTask.GetAwaiter().OnCompleted(() => {
                taskComplete();
            });
        }

        private void taskComplete() {
            lock (tasks) {
                currentTask = null;
                if (tasks.Count > 0) {
                    taskStart(tasks.Dequeue());
                } else {
                    Completion?.Invoke();
                }
            }
        }

    }

}