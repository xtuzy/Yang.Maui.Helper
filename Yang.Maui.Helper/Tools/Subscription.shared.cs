﻿using System;
using System.Reflection;

// Inspired by AsyncAwaitBestPractices.Subscription: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Yang.Maui.Helper.Tools
{
	struct Subscription
	{
		public WeakReference? Subscriber { get; }

		public MethodInfo Handler { get; }

		public Subscription(WeakReference? subscriber, MethodInfo handler)
		{
			Subscriber = subscriber;
			Handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}
	}
}