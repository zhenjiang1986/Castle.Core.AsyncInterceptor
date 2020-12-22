// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITarget
    {
        void VoidSynchronous();

        int ResultSynchronous();

        Task CompletedTaskAsynchronous();

        Task<int> CompletedResultTaskAsynchronous();

        Task IncompleteTaskAsynchronous();

        Task<int> IncompleteResultTaskAsynchronous();
    }

    public class Targets : ITarget
    {
        private static readonly Task<int> CompletedResultTask = Task.FromResult(1);

        public void VoidSynchronous()
        {
        }

        public int ResultSynchronous() => 1;

        public Task CompletedTaskAsynchronous() => Task.CompletedTask;

        public Task<int> CompletedResultTaskAsynchronous() => CompletedResultTask;

        public async Task IncompleteTaskAsynchronous()
        {
            await Task.Yield();
        }

        public async Task<int> IncompleteResultTaskAsynchronous()
        {
            await Task.Yield();
            return 1;
        }
    }

    public class TargetNopWrapper : ITarget
    {
        private readonly ITarget _inner;

        public TargetNopWrapper(ITarget inner) => _inner = inner;

        public void VoidSynchronous() => _inner.VoidSynchronous();

        public int ResultSynchronous() => _inner.ResultSynchronous();

        public Task CompletedTaskAsynchronous() => _inner.CompletedTaskAsynchronous();

        public Task<int> CompletedResultTaskAsynchronous() => _inner.CompletedResultTaskAsynchronous();

        public Task IncompleteTaskAsynchronous() => _inner.IncompleteTaskAsynchronous();

        public Task<int> IncompleteResultTaskAsynchronous() => _inner.IncompleteResultTaskAsynchronous();
    }

    public class TargetAsyncWrapper : ITarget
    {
        private readonly ITarget _inner;

        public TargetAsyncWrapper(ITarget inner) => _inner = inner;

        public void VoidSynchronous()
        {
            Task.Run(async () => await Task.Yield()).GetAwaiter().GetResult();
            _inner.VoidSynchronous();
            Task.Run(async () => await Task.Yield()).GetAwaiter().GetResult();
        }

        public int ResultSynchronous()
        {
            Task.Run(async () => await Task.Yield()).GetAwaiter().GetResult();
            int result = _inner.ResultSynchronous();
            Task.Run(async () => await Task.Yield()).GetAwaiter().GetResult();
            return result;
        }

        public async Task CompletedTaskAsynchronous()
        {
            await Task.Yield();
            await _inner.CompletedTaskAsynchronous().ConfigureAwait(false);
            await Task.Yield();
        }

        public async Task<int> CompletedResultTaskAsynchronous()
        {
            await Task.Yield();
            int result = await _inner.CompletedResultTaskAsynchronous().ConfigureAwait(false);
            await Task.Yield();
            return result;
        }

        public async Task IncompleteTaskAsynchronous()
        {
            await Task.Yield();
            await _inner.IncompleteTaskAsynchronous().ConfigureAwait(false);
            await Task.Yield();
        }

        public async Task<int> IncompleteResultTaskAsynchronous()
        {
            await Task.Yield();
            int result = await _inner.IncompleteResultTaskAsynchronous().ConfigureAwait(false);
            await Task.Yield();
            return result;
        }
    }
}
