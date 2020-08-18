// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;

namespace Hestia.UIRunner
{
    [ExcludeFromCodeCoverage]
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object param)
        {
            // ReSharper disable once PossibleNullReferenceException
            var name = param.GetType()
                            .FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data) => data is ReactiveObject;
    }
}
