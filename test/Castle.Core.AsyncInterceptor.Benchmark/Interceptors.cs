// Copyright (c) 2016-2020 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class Interceptors : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }

    public class NopAsyncInterceptor : IAsyncInterceptor
    {
        public void InterceptSynchronous(IInvocation invocation)
        {
            invocation.Proceed();
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.Proceed();
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }

    public class ProcessingAsyncInterceptor : ProcessingAsyncInterceptor<string>
    {
        protected override string StartingInvocation(IInvocation invocation)
        {
            return string.Empty;
        }
    }

    public class NopAsyncTimingInterceptor : AsyncTimingInterceptor
    {
        protected override void StartingTiming(IInvocation invocation)
        {
        }

        protected override void CompletedTiming(IInvocation invocation, Stopwatch stopwatch)
        {
        }
    }

    public class BenchmarkAsyncInterceptorBase : AsyncInterceptorBase
    {
        protected override async Task InterceptAsync(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await Task.Yield();
            await proceed(invocation, proceedInfo).ConfigureAwait(false);
            await Task.Yield();
        }

        protected override async Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            await Task.Yield();
            TResult result = await proceed(invocation, proceedInfo).ConfigureAwait(false);
            await Task.Yield();
            return result;
        }
    }
}
